using Program.scr.core;
using Program.scr.forms;
using Program.scr.forms.client;

namespace Program
{
    public partial class AuthForm : Form
    {
        public AuthForm()
        {
            InitializeComponent();
            panel_client.Visible = false;
            panel_empl.Visible = false;

            button_client.Click += (s, e) => panel_client.Visible = true;
            button_empl.Click += (s, e) => panel_empl.Visible = true;

            button_c_catalog.Click += (s, e) => { this.Hide(); new ClientCatalogProductForm().ShowDialog(); this.Show(); };
            button_c_login.Click += (s, e) => {
                if (string.IsNullOrWhiteSpace(textBox_c_order.Text) || string.IsNullOrWhiteSpace(textBox_c_email.Text)) 
                {
                    MessageBox.Show("Введите данные для проверки заказа!");
                    return;
                }
                if (!int.TryParse(textBox_c_order.Text, out int _Id)) 
                {
                    MessageBox.Show("Введите данные для проверки заказа!");
                    return;
                }
                this.Hide();
                try
                {
                    new ClientOrderViewForm(textBox_c_email.Text, _Id).ShowDialog();
                }
                catch { }
                this.Show(); 
            };

            button_e_login.Click += (s, e) =>
            {
                string login = textBox_e_login.Text.Trim();
                string password = textBox_e_password.Text.Trim();

                if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Введите логин и пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var authResult = Core.Auth.ValidateCredentials(login, password);

                if (authResult == null)
                {
                    MessageBox.Show("Неверный логин или пароль.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (((Core.Auth.AuthResult)authResult).AccessLevel == -1)
                {
                    MessageBox.Show("У вас нет доступа к системе.", "Доступ запрещён", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (((Core.Auth.AuthResult)authResult).HireDate.Year == 1900)
                {
                    MessageBox.Show("Сотрудник уволен. Доступ запрещён.", "Доступ запрещён", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                Core.ThisUser_AC = ((Core.Auth.AuthResult)authResult).AccessLevel;
                Core.ThisUser_ID = ((Core.Auth.AuthResult)authResult).EmployeeID;

                this.Hide();

                // Открываем форму в зависимости от AccessLevel
                switch (((Core.Auth.AuthResult)authResult).AccessLevel)
                {
                    case 0: // Админ
                        new AdminForm().ShowDialog();
                        break;
                    case 1: // Менеджер
                        new TopPageForm(new scr.forms.userControls.SalesOrders_ViewUserControl(), "ПП - Менеджер").ShowDialog();
                        break;
                    case 2: // Повар
                        new TopPageForm(new scr.forms.userControls.SalesOrders_ViewUserControl(), "ПП - Повар").ShowDialog();
                        break;
                    default:
                        MessageBox.Show("Неизвестный уровень доступа.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                }


                Core.ThisUser_ID = -1;
                Core.ThisUser_AC = -1;

                this.Show();

            };
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            panel_client.Visible = false;
            panel_empl.Visible = false;
        }


    }
}
