using Program.scr.core.dbt;

namespace Program.scr.forms.userControls
{
    public partial class Employees_AddEditUserControl : UserControl
    {
        DBT_Employees Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_FullName;
        TextBox textBox_Position;
        TextBox textBox_Phone;
        TextBox textBox_Email;

        TextBox textBox_login;
        TextBox textBox_pwd;
        ComboBox comboBox_role;

        public Employees_AddEditUserControl()
        {
            InitializeComponent();
            Init();
        }
        public Employees_AddEditUserControl(DBT_Employees obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_FullName.Text = obj.FullName.ToString();
            textBox_Position.Text = obj.Position.ToString();
            textBox_Phone.Text = obj.Phone.ToString();
            textBox_Email.Text = obj.Email.ToString();
        }

        private void Init()
        {
            this.Size = new Size(600, 500);
            this.Text = "Сотрудники - " + (Object == null ? "Добавить" : "Изменить").ToString();
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
                RowCount = 7
            };

            Label label_FullName = new Label();
            SetLabel(ref label_FullName, "ФИО");
            tableLayout.Controls.Add(label_FullName, 0, 0);
            textBox_FullName = new TextBox();
            textBox_FullName.Dock = DockStyle.Fill;
            textBox_FullName.MaxLength = 255;
            tableLayout.Controls.Add(textBox_FullName, 1, 0);

            Label label_Position = new Label();
            SetLabel(ref label_Position, "Должность");
            tableLayout.Controls.Add(label_Position, 0, 1);
            textBox_Position = new TextBox();
            textBox_Position.Dock = DockStyle.Fill;
            textBox_Position.MaxLength = 100;
            tableLayout.Controls.Add(textBox_Position, 1, 1);

            Label label_Phone = new Label();
            SetLabel(ref label_Phone, "Номер телефона");
            tableLayout.Controls.Add(label_Phone, 0, 2);
            textBox_Phone = new TextBox();
            textBox_Phone.Dock = DockStyle.Fill;
            textBox_Phone.MaxLength = 50;
            tableLayout.Controls.Add(textBox_Phone, 1, 2);

            Label label_Email = new Label();
            SetLabel(ref label_Email, "Электронная почта");
            tableLayout.Controls.Add(label_Email, 0, 3);
            textBox_Email = new TextBox();
            textBox_Email.Dock = DockStyle.Fill;
            textBox_Email.MaxLength = 255;
            tableLayout.Controls.Add(textBox_Email, 1, 3);

            if (Object == null)
            {
                Label label_log = new Label();
                SetLabel(ref label_log, "Логин");
                tableLayout.Controls.Add(label_log, 0, 4);
                textBox_login = new TextBox();
                textBox_login.Dock = DockStyle.Fill;
                textBox_login.MaxLength = 50;
                tableLayout.Controls.Add(textBox_login, 1, 4);

                Label label_pwd = new Label();
                SetLabel(ref label_pwd, "Пароль");
                tableLayout.Controls.Add(label_pwd, 0, 5);
                textBox_pwd = new TextBox();
                textBox_pwd.Dock = DockStyle.Fill;
                textBox_pwd.MaxLength = 50;
                tableLayout.Controls.Add(textBox_pwd, 1, 5);

                Label label_role = new Label();
                SetLabel(ref label_role, "Роль");
                tableLayout.Controls.Add(label_role, 0, 6);
                comboBox_role = new ComboBox();
                comboBox_role.Dock = DockStyle.Fill;
                comboBox_role.MaxLength = 50;
                tableLayout.Controls.Add(comboBox_role, 1, 6);
                comboBox_role.Items.AddRange(core.Core.ACs);

                textBox_pwd.PasswordChar = '*';
            }

            this.Controls.Add(tableLayout);
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
            if (string.IsNullOrWhiteSpace(textBox_FullName.Text)) { MessageBox.Show("Поле 'ФИО' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_Position.Text)) { MessageBox.Show("Поле 'Должность' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                if (string.IsNullOrWhiteSpace(textBox_login.Text)) { MessageBox.Show("Поле 'Логин' имеет некорректное значение!"); return; }
                if (string.IsNullOrWhiteSpace(textBox_login.Text)) { MessageBox.Show("Поле 'Пароль' имеет некорректное значение!"); return; }
                if (comboBox_role.SelectedIndex == -1) { MessageBox.Show("Поле 'Роль' имеет некорректное значение!"); return; }
                if(DBT_Auth.GetByLogin(textBox_login.Text) != null)
                if (DBT_Auth.GetByLogin(textBox_login.Text).Login != null) { MessageBox.Show("Логин занят!"); return; }

                res = DBT_Employees.Create(
                    new DBT_Employees()
                    {
                        FullName = textBox_FullName.Text,
                        Position = textBox_Position.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text,
                        HireDate = DateTime.Now
                    }
                );
                if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");
                if (res == -1) return;

                res = DBT_Auth.Create(
                    new DBT_Auth()
                    {
                        Login = textBox_login.Text,
                        PasswordHash = textBox_pwd.Text,
                        AccessLevel = comboBox_role.SelectedIndex - 1,
                        EmployeeID = res
                    }
                );
            }
            else
            {
                res = DBT_Employees.Edit(
                    new DBT_Employees()
                    {
                        ID = Object.ID,
                        FullName = textBox_FullName.Text,
                        Position = textBox_Position.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text,
                        HireDate = Object.HireDate
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");

            (this.Parent as Form).Close();
        }
    }
}
