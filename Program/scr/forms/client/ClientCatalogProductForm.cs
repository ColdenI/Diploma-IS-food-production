namespace Program.scr.forms.client
{
    public partial class ClientCatalogProductForm : Form
    {
        private Dictionary<int, NumericUpDown> productQuantities;

        public ClientCatalogProductForm()
        {
            InitializeComponent();
        }

        private void ClientCatalogProductForm_Load(object sender, EventArgs e)
        {
            LoadProducts();

            this.Size = new Size(1000, 500);
            this.Size = new Size(700, 500);
        }

        private void LoadProducts()
        {
            var products = scr.core.dbt.DBT_Products.GetAll();
            if (products == null || products.Count == 0)
            {
                MessageBox.Show("Не удалось загрузить товары.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            productQuantities = new Dictionary<int, NumericUpDown>();

            tableLayoutPanel.ColumnStyles.Clear(); // очищаем, если уже были
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 70F)); // Название товара — 70%
            tableLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));      // Количество — авто

            // Очищаем предыдущие строки
            tableLayoutPanel.Controls.Clear();
            tableLayoutPanel.RowStyles.Clear();

            // Добавляем строки
            foreach (var product in products)
            {
                int rowIndex = tableLayoutPanel.RowCount;
                tableLayoutPanel.RowCount++;
                tableLayoutPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                var label = new Label
                {
                    Text = $"{product.Name} ({product.PricePerUnit:C})",
                    Anchor = AnchorStyles.Left | AnchorStyles.Right,
                    Margin = new Padding(5),
                    TextAlign = ContentAlignment.MiddleLeft
                };

                var numericUpDown = new NumericUpDown
                {
                    Minimum = 0,
                    Maximum = 1000,
                    Width = 80,
                    Anchor = AnchorStyles.Left,
                    Margin = new Padding(5),
                    Value = 0
                };

                var button = new Button
                {
                    Text = "Состав",
                    BackColor = Color.White,
                    Height = 35,
                    Width = 90,
                    Anchor = AnchorStyles.Right,
                    Margin = new Padding(5)
                };
                button.Click += (s, e) => { new ProductInfoForm(product.ID).ShowDialog(); };

                tableLayoutPanel.Controls.Add(label, 0, rowIndex);
                tableLayoutPanel.Controls.Add(numericUpDown, 1, rowIndex);
                tableLayoutPanel.Controls.Add(button, 2, rowIndex);

                productQuantities[product.ID] = numericUpDown;
            }
        }

        private void button_order_Click(object sender, EventArgs e)
        {
            var orderDict = new Dictionary<int, int>();

            foreach (var kvp in productQuantities)
            {
                int productId = kvp.Key;
                int quantity = (int)kvp.Value.Value;

                if (quantity > 0)
                {
                    orderDict[productId] = quantity;
                }
            }

            if (orderDict.Count == 0)
            {
                MessageBox.Show("Выберите хотя бы один товар с количеством > 0.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Пример: вывести результат в консоль или передать дальше
            Console.WriteLine("Сформированный заказ:");
            foreach (var item in orderDict)
            {
                Console.WriteLine($"ID: {item.Key}, Количество: {item.Value}");
            }


            new ClientDoOrderForm(orderDict).ShowDialog();
            if (ClientDoOrderForm.isClose) Close();
        }
    }
}
