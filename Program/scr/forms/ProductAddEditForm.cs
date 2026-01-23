using Microsoft.Data.SqlClient;

namespace Program.scr.forms
{
    public partial class ProductAddEditForm : Form
    {
        private int? productId = null; // null = создание, иначе — редактирование

        // Конструктор: передаём ID продукта, если редактируем
        public ProductAddEditForm(int productId = 0)
        {
            this.productId = productId == 0 ? null : productId;
            InitializeComponent();
            InitializeComponent_();

            if (this.productId.HasValue)
            {
                this.Text = "Изменение продукта";
                LoadProductData();
            }
            else
            {
                this.Text = "Создание продукта";
            }
        }

        private void InitializeComponent_()
        {
            this.StartPosition = FormStartPosition.CenterScreen;

            numShelfLifeDays.Minimum = 0;
            numShelfLifeDays.Maximum = 3650;
            numPricePerUnit.DecimalPlaces = 2;
            numPricePerUnit.Increment = 1;
            numPricePerUnit.Minimum = 0;
            numPricePerUnit.Maximum = 999999;

            /* Таблица для состава (ингредиенты)
            dgvIngredients = new DataGridView
            {
                Location = new Point(20, 220),
                Size = new Size(850, 250),

            };*/

            dgvIngredients.AllowUserToAddRows = false;
            dgvIngredients.AllowUserToDeleteRows = false; // Запрещаем стандартное удаление
            dgvIngredients.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvIngredients.MultiSelect = true;

            dgvIngredients.Columns.Add("RawMaterialID", "ID Сырья"); dgvIngredients.Columns[0].Visible = false;
            dgvIngredients.Columns.Add("RawMaterialName", "Сырьё");
            dgvIngredients.Columns.Add("Quantity", "Количество");

            // Контекстное меню для удаления строки
            var ctxMenu = new ContextMenuStrip();
            var removeItem = new ToolStripMenuItem("Удалить ингредиент");
            removeItem.Click += (s, e) =>
            {
                if (dgvIngredients.SelectedRows.Count > 0)
                {
                    var row = dgvIngredients.SelectedRows[0];
                    dgvIngredients.Rows.RemoveAt(row.Index);
                }
            };
            ctxMenu.Items.Add(removeItem);
            dgvIngredients.ContextMenuStrip = ctxMenu;

            // Кнопки
            btnSave.Click += BtnSave_Click;
            btnAddIngredient.Click += BtnAddIngredient_Click;
            btnRemoveSelected.Click += BtnRemoveSelected_Click;
        }

        private void LoadProductData()
        {
            var product = scr.core.dbt.DBT_Products.GetById(productId.Value);
            if (product == null)
            {
                MessageBox.Show("Продукт не найден.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            txtName.Text = product.Name;
            txtCategory.Text = product.Category;
            txtUnitOfMeasure.Text = product.UnitOfMeasure;
            numShelfLifeDays.Value = product.ShelfLifeDays;
            numPricePerUnit.Value = product.PricePerUnit;

            // Загружаем состав
            LoadRecipeData();
        }

        private void LoadRecipeData()
        {
            dgvIngredients.Rows.Clear();

            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand(@"
                    SELECT rm.ID, rm.Name, rec.Quantity
                    FROM Recipes rec
                    INNER JOIN RawMaterials rm ON rec.RawMaterialID = rm.ID
                    WHERE rec.ProductID = @ProductID", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId.Value);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var row = new DataGridViewRow();
                            row.CreateCells(dgvIngredients);
                            row.Cells[0].Value = reader["ID"];
                            row.Cells[1].Value = reader["Name"];
                            row.Cells[2].Value = reader["Quantity"];

                            dgvIngredients.Rows.Add(row);
                        }
                    }
                }
            }
        }

        private void BtnAddIngredient_Click(object sender, EventArgs e)
        {
            var frmSelect = new frmSelectRawMaterial();
            if (frmSelect.ShowDialog() == DialogResult.OK)
            {
                var selected = frmSelect.SelectedRawMaterial;
                var quantity = frmSelect.SelectedQuantity;

                // Проверяем, нет ли уже такого ингредиента
                foreach (DataGridViewRow row in dgvIngredients.Rows)
                {
                    if (row.Cells[0].Value != null && (int)row.Cells[0].Value == selected.ID)
                    {
                        MessageBox.Show("Этот ингредиент уже добавлен.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }

                var newRow = new DataGridViewRow();
                newRow.CreateCells(dgvIngredients);
                newRow.Cells[0].Value = selected.ID;
                newRow.Cells[1].Value = selected.Name;
                newRow.Cells[2].Value = quantity;

                dgvIngredients.Rows.Add(newRow);
            }
        }

        private void BtnRemoveSelected_Click(object sender, EventArgs e)
        {
            if (dgvIngredients.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите ингредиенты для удаления.", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var rowsToRemove = new List<DataGridViewRow>();
            foreach (DataGridViewRow row in dgvIngredients.SelectedRows)
            {
                rowsToRemove.Add(row);
            }

            foreach (var row in rowsToRemove)
            {
                dgvIngredients.Rows.Remove(row);
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show("Введите название продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var product = new scr.core.dbt.DBT_Products
            {
                Name = txtName.Text,
                Category = txtCategory.Text,
                UnitOfMeasure = txtUnitOfMeasure.Text,
                ShelfLifeDays = (int)numShelfLifeDays.Value,
                PricePerUnit = numPricePerUnit.Value
            };

            if (productId.HasValue)
            {
                // Обновляем продукт
                product.ID = productId.Value;
                scr.core.dbt.DBT_Products.Edit(product);

                // Удаляем старые ингредиенты
                DeleteRecipe(productId.Value);

                // Добавляем новые
                InsertRecipe(productId.Value);
            }
            else
            {
                // Создаём продукт
                int newProductId = scr.core.dbt.DBT_Products.Create(product);
                if (newProductId == -1)
                {
                    MessageBox.Show("Ошибка при создании продукта.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Добавляем состав
                InsertRecipe(newProductId);
            }

            MessageBox.Show("Продукт успешно сохранён.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void DeleteRecipe(int productId)
        {
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("DELETE FROM Recipes WHERE ProductID = @ProductID", conn))
                {
                    cmd.Parameters.AddWithValue("@ProductID", productId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void InsertRecipe(int productId)
        {
            using (SqlConnection conn = new SqlConnection(scr.core.SQL._sqlConnectStr))
            {
                conn.Open();
                using (var cmd = new SqlCommand("INSERT INTO Recipes (ProductID, RawMaterialID, Quantity) VALUES (@ProductID, @RawMaterialID, @Quantity)", conn))
                {
                    foreach (DataGridViewRow row in dgvIngredients.Rows)
                    {
                        if (row.Cells[0].Value == null || row.Cells[2].Value == null) continue;

                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@ProductID", productId);
                        cmd.Parameters.AddWithValue("@RawMaterialID", (int)row.Cells[0].Value);
                        cmd.Parameters.AddWithValue("@Quantity", Convert.ToDecimal(row.Cells[2].Value));
                        cmd.ExecuteNonQuery();
                    }
                }
            }
        }
    }

    // Вспомогательная форма для выбора сырья
    public partial class frmSelectRawMaterial : Form
    {
        public class RawMaterial
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

        public RawMaterial SelectedRawMaterial { get; private set; }
        public decimal SelectedQuantity { get; private set; }

        private DataGridView dgvRawMaterials;
        private NumericUpDown numQuantity;
        private Button btnOK;
        private Button btnCancel;

        public frmSelectRawMaterial()
        {
            //InitializeComponent();
            InitializeComponent_();
        }

        private void InitializeComponent_()
        {
            this.Size = new Size(500, 400);
            this.Text = "Выберите сырьё";
            this.StartPosition = FormStartPosition.CenterParent;

            dgvRawMaterials = new DataGridView
            {
                Location = new Point(20, 20),
                Size = new Size(460, 260),
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect
            };

            dgvRawMaterials.Columns.Add("ID", "ID"); dgvRawMaterials.Columns[0].Visible = false;
            dgvRawMaterials.Columns.Add("Name", "Название");

            // Загружаем список сырья
            LoadRawMaterials();

            numQuantity = new NumericUpDown
            {
                Location = new Point(20, 290),
                Size = new Size(100, 25),
                DecimalPlaces = 3,
                Increment = 0.1m,
                Minimum = 0.001m,
                Maximum = 999999,
                Value = 1
            };

            var lblQuantity = new Label { Text = "Количество:", Location = new Point(130, 290) };

            btnOK = new Button { Text = "ОК", Location = new Point(20, 330), Size = new Size(80, 30) };
            btnOK.Click += BtnOK_Click;

            btnCancel = new Button { Text = "Отмена", Location = new Point(110, 330), Size = new Size(80, 30) };
            btnCancel.Click += (s, e) => this.DialogResult = DialogResult.Cancel;

            this.Controls.AddRange(new Control[] { dgvRawMaterials, numQuantity, lblQuantity, btnOK, btnCancel });
        }

        private void LoadRawMaterials()
        {
            dgvRawMaterials.Rows.Clear();

            var rawMaterials = scr.core.dbt.DBT_RawMaterials.GetAll();
            if (rawMaterials != null)
            {
                foreach (var rm in rawMaterials)
                {
                    var row = new DataGridViewRow();
                    row.CreateCells(dgvRawMaterials);
                    row.Cells[0].Value = rm.ID;
                    row.Cells[1].Value = rm.Name;
                    dgvRawMaterials.Rows.Add(row);
                }
            }
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            if (dgvRawMaterials.SelectedRows.Count == 0)
            {
                MessageBox.Show("Выберите сырьё.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var selectedRow = dgvRawMaterials.SelectedRows[0];
            SelectedRawMaterial = new RawMaterial
            {
                ID = (int)selectedRow.Cells[0].Value,
                Name = (string)selectedRow.Cells[1].Value
            };

            SelectedQuantity = numQuantity.Value;
            this.DialogResult = DialogResult.OK;
        }
    }
}
