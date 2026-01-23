using Program.scr.core.dbt;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.scr.forms.userControls
{
    public partial class Auth_AddEditUserControl : UserControl
    {
        DBT_Auth Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_Login;
        TextBox textBox_PasswordHash;
        ComboBox comboBox_AccessLevel;

        public Auth_AddEditUserControl()
        {
            InitializeComponent();
            Init();
        }
        public Auth_AddEditUserControl(DBT_Auth obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_Login.Text = obj.Login.ToString();
            textBox_PasswordHash.Text = obj.PasswordHash.ToString();
            comboBox_AccessLevel.Text = obj.AccessLevel.ToString();
        }

        private void Init()
        {
            this.Size = new Size(600, 500);
            this.Text = "Авторизация - " + (Object == null ? "Добавить" : "Изменить").ToString();
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
                RowCount = 3
            };

            Label label_Login = new Label();
            SetLabel(ref label_Login, "Логин");
            tableLayout.Controls.Add(label_Login, 0, 0);
            textBox_Login = new TextBox();
            textBox_Login.Dock = DockStyle.Fill;
            textBox_Login.MaxLength = 100;
            tableLayout.Controls.Add(textBox_Login, 1, 0);

            Label label_PasswordHash = new Label();
            SetLabel(ref label_PasswordHash, "Пароль");
            tableLayout.Controls.Add(label_PasswordHash, 0, 1);
            textBox_PasswordHash = new TextBox();
            textBox_PasswordHash.Dock = DockStyle.Fill;
            textBox_PasswordHash.MaxLength = 255;
            tableLayout.Controls.Add(textBox_PasswordHash, 1, 1);

            Label label_AccessLevel = new Label();
            SetLabel(ref label_AccessLevel, "Уровень доступа");
            tableLayout.Controls.Add(label_AccessLevel, 0, 2);
            comboBox_AccessLevel = new ComboBox();
            comboBox_AccessLevel.Dock = DockStyle.Fill;
            comboBox_AccessLevel.MaxLength = 1000;
            tableLayout.Controls.Add(comboBox_AccessLevel, 1, 2);

            comboBox_AccessLevel.Items.AddRange(core.Core.ACs);

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
            if (string.IsNullOrWhiteSpace(textBox_Login.Text)) { MessageBox.Show("Поле 'Логин' имеет некорректное значение!"); return; }
            if (string.IsNullOrWhiteSpace(textBox_PasswordHash.Text)) { MessageBox.Show("Поле 'Пароль' имеет некорректное значение!"); return; }
            if (comboBox_AccessLevel.SelectedIndex == -1) { MessageBox.Show("Поле 'Уровень доступа' имеет некорректное значение!"); return; }

            int res = 0;

            if (Object == null)
            {
                res = DBT_Auth.Create(
                    new DBT_Auth()
                    {
                        Login = textBox_Login.Text,
                        PasswordHash = textBox_PasswordHash.Text,
                        AccessLevel = int.Parse(comboBox_AccessLevel.Text)
                    }
                );
            }
            else
            {
                res = DBT_Auth.Edit(
                    new DBT_Auth()
                    {
                        EmployeeID = Object.EmployeeID,
                        Login = textBox_Login.Text,
                        PasswordHash = textBox_PasswordHash.Text,
                        AccessLevel = comboBox_AccessLevel.SelectedIndex - 1
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");

            (this.Parent as Form).Close();
        }
    }
}
