using Program.scr.core;
using Program.scr.core.dbt;

namespace Program.scr.forms.client
{
    public partial class ClientDoOrderForm : Form
    {

        private Dictionary<int, int> orderDict;
        private Dictionary<int, NumericUpDown> productQuantities;
        private int? clientId = null; // ID найденного клиента или null, если новый
        public static bool isClose = false;
        private bool isManager = false;

        public ClientDoOrderForm(Dictionary<int, int> order, bool isManager = false)
        {
            isClose = false;
            this.isManager = isManager;

            orderDict = order ?? throw new ArgumentNullException(nameof(order));

            InitializeComponent();

            btnCheck.Click += BtnCheck_Click;
            btnOrder.Click += BtnOrder_Click;

            dgvOrderItems.AllowUserToAddRows = false;
            dgvOrderItems.ReadOnly = true;


            // Subscribe to text change events to enable/disable order button
            txtFullName.TextChanged += OnInputChanged;
            txtContactPerson.TextChanged += OnInputChanged;
            txtPhone.TextChanged += OnInputChanged;

            LoadOrderItemsGridFromDict();
        }


        private void LoadOrderItemsGridFromDict()
        {
            dgvOrderItems.Columns.Clear();

            dgvOrderItems.Columns.Add("Name", "Название");
            dgvOrderItems.Columns.Add("Price", "Цена");
            dgvOrderItems.Columns.Add("Quantity", "Количество");
            dgvOrderItems.Columns.Add("Quantityt", "Итого");

            decimal total = 0;

            foreach (var item in orderDict)
            {
                int productId = item.Key;
                int quantity = item.Value;

                var product = scr.core.dbt.DBT_Products.GetById(productId);
                if (product == null) continue; // пропускаем, если продукт не найден

                var row = new DataGridViewRow();
                row.CreateCells(dgvOrderItems);
                row.Cells[0].Value = product.Name;
                row.Cells[1].Value = product.PricePerUnit.ToString("C");
                row.Cells[3].Value = (quantity * product.PricePerUnit).ToString("C");
                row.Cells[2].Value = quantity.ToString();

                total += (quantity * product.PricePerUnit);

                dgvOrderItems.Rows.Add(row);
            }

            label5.Text = "Итого: " + total.ToString("C");
        }

        private void BtnCheck_Click(object sender, EventArgs e)
        {
            var email = txtEmail.Text.Trim();

            if (string.IsNullOrEmpty(email))
            {
                MessageBox.Show("Введите Email.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Проверяем, есть ли клиент с таким email
            var client = scr.core.dbt.DBT_Clients.GetByEmail(email);

            if (client != null)
            {
                clientId = client.ID;
                MessageBox.Show("Клиент найден!", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                btnOrder.Enabled = true;

                var _client = DBT_Clients.GetById((int)clientId);
                txtContactPerson.Text = _client.ContactPerson;
                txtFullName.Text = _client.FullName;
                txtPhone.Text = _client.Phone;
                txtEmail.Enabled = false;
                btnCheck.Enabled = false;
            }
            else
            {
                clientId = null;
                txtFullName.Enabled = true;
                txtContactPerson.Enabled = true;
                txtPhone.Enabled = true;
                MessageBox.Show("Клиент не найден. Заполните данные.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void OnInputChanged(object sender, EventArgs e)
        {
            if (!btnOrder.Enabled && IsNewClientDataValid())
            {
                btnOrder.Enabled = true;
            }
        }

        private bool IsNewClientDataValid()
        {
            return !string.IsNullOrWhiteSpace(txtFullName.Text) &&
                   !string.IsNullOrWhiteSpace(txtContactPerson.Text) &&
                   !string.IsNullOrWhiteSpace(txtPhone.Text);
        }

        private void BtnOrder_Click(object sender, EventArgs e)
        {
            if (clientId == null && !IsNewClientDataValid())
            {
                MessageBox.Show("Заполните все поля клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Создаём клиента, если новый
            if (clientId == null)
            {
                var newClient = new scr.core.dbt.DBT_Clients
                {
                    FullName = txtFullName.Text,
                    ContactPerson = txtContactPerson.Text,
                    Phone = txtPhone.Text,
                    Email = txtEmail.Text
                };
                clientId = scr.core.dbt.DBT_Clients.Create(newClient);
                if (clientId == -1)
                {
                    MessageBox.Show("Ошибка при создании клиента.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            int orderId = Core.SaveOrder((int)clientId, orderDict, isManager);

            MessageBox.Show($"Заказ успешно оформлен!\nНомер заказа: {orderId}", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            isClose = true;
            Close();
        }

    }
}
