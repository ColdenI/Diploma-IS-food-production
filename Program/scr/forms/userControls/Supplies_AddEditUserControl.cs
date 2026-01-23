using Program.scr.core;
using Program.scr.core.dbt;

namespace Program.scr.forms.userControls
{
    public partial class Supplies_AddEditUserControl : UserControl
    {
        DBT_Supplies Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        ComboBox comboBox_SupplierID;
        ComboBox comboBox_RawMaterialID;
        TextBox textBox_Quantity;

        List<DBT_Suppliers> Suppliers = new List<DBT_Suppliers>();
        List<DBT_RawMaterials> RawMaterial = new List<DBT_RawMaterials>();

        public Supplies_AddEditUserControl()
        {
            InitializeComponent();
            Init();
        }
        /*
        public Supplies_AddEditUserControl(DBT_Supplies obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_SupplierID.Text = obj.SupplierID.ToString();
            textBox_RawMaterialID.Text = obj.RawMaterialID.ToString();
            textBox_EmployeeID.Text = obj.EmployeeID.ToString();
            textBox_Quantity.Text = obj.Quantity.ToString();
            dateTimePicker_SupplyDate.Value = (DateTime)((obj.SupplyDate == null) ? DateTime.Now : obj.SupplyDate);
        }
        */
        private void Init()
        {
            this.Size = new Size(600, 500);
            this.Text = "Поставки - " + (Object == null ? "Добавить" : "Изменить").ToString();
            this.MinimumSize = new Size(400, 400);
            button_apply = new Button()
            {
                Height = 30,
                Dock = DockStyle.Bottom
            };
            button_apply.Click += Button_apply_Click;
            button_apply.Text = Object == null ? "Добавить" : "Изменить";
            this.Controls.Add(button_apply);

            tableLayout = new TableLayoutPanel()
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 5
            };

            Label label_SupplierID = new Label();
            SetLabel(ref label_SupplierID, "Поставщик");
            tableLayout.Controls.Add(label_SupplierID, 0, 0);
            comboBox_SupplierID = new ComboBox();
            comboBox_SupplierID.Dock = DockStyle.Fill;
            comboBox_SupplierID.MaxLength = 1000;
            tableLayout.Controls.Add(comboBox_SupplierID, 1, 0);

            Label label_RawMaterialID = new Label();
            SetLabel(ref label_RawMaterialID, "Сырьё");
            tableLayout.Controls.Add(label_RawMaterialID, 0, 1);
            comboBox_RawMaterialID = new ComboBox();
            comboBox_RawMaterialID.Dock = DockStyle.Fill;
            comboBox_RawMaterialID.MaxLength = 1000;
            tableLayout.Controls.Add(comboBox_RawMaterialID, 1, 1);

            Label label_Quantity = new Label();
            SetLabel(ref label_Quantity, "Количество");
            tableLayout.Controls.Add(label_Quantity, 0, 3);
            textBox_Quantity = new TextBox();
            textBox_Quantity.Dock = DockStyle.Fill;
            textBox_Quantity.MaxLength = 1000;
            tableLayout.Controls.Add(textBox_Quantity, 1, 3);

            this.Controls.Add(tableLayout);

            LoadComboBox_Supplier();
            LoadComboBox_RawMaterial();
        }

        private void LoadComboBox_Supplier()
        {
            Suppliers = DBT_Suppliers.GetAll();

            comboBox_SupplierID.Items.Clear();
            foreach (var i in Suppliers)
                comboBox_SupplierID.Items.Add($"{i.CompanyName}");
        }
        private void LoadComboBox_RawMaterial()
        {
            RawMaterial = DBT_RawMaterials.GetAll();

            comboBox_RawMaterialID.Items.Clear();
            foreach (var i in RawMaterial)
                comboBox_RawMaterialID.Items.Add($"{i.Name}");
        }

        private void SetLabel(ref Label label, string text = "")
        {
            label.Font = new Font(Font.FontFamily, 12);
            label.TextAlign = ContentAlignment.TopLeft;
            label.Dock = DockStyle.Fill;
            label.AutoSize = false;
            label.Width = 200;
            label.Text = text;
        }

        private void Button_apply_Click(object? sender, EventArgs e)
        {
            if (comboBox_SupplierID.SelectedIndex == -1) { MessageBox.Show("Поле 'Поставщик' имеет некорректное значение!"); return; }
            if (comboBox_RawMaterialID.SelectedIndex == -1) { MessageBox.Show("Поле 'Сырьё' имеет некорректное значение!"); return; }
            if (!decimal.TryParse(textBox_Quantity.Text, out decimal tp_Quantity)) { MessageBox.Show("Поле 'Количество' имеет некорректное значение!"); return; }
            if (tp_Quantity <= 0) { MessageBox.Show("Поле 'Количество' имеет некорректное значение!"); return; }

            int res = 0;


            res = DBT_Supplies.Create(
                new DBT_Supplies()
                {
                    SupplierID = Suppliers[comboBox_SupplierID.SelectedIndex].ID,
                    RawMaterialID = RawMaterial[comboBox_RawMaterialID.SelectedIndex].ID,
                    EmployeeID = Core.ThisUser_ID,
                    Quantity = decimal.Parse(textBox_Quantity.Text),
                    SupplyDate = DateTime.Now
                }
            );
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
            if (res == -1) return;

            var raw = DBT_RawMaterials.GetById(RawMaterial[comboBox_RawMaterialID.SelectedIndex].ID);
            raw.QuantityInStock += decimal.Parse(textBox_Quantity.Text);
            DBT_RawMaterials.Edit(raw);

            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");

            (this.Parent as Form).Close();
        }
    }
}
