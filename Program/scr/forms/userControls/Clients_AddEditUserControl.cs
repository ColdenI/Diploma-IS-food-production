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
    public partial class Clients_AddEditUserControl : UserControl
    {
        DBT_Clients Object;

        Button button_apply;
        TableLayoutPanel tableLayout;

        TextBox textBox_FullName;
        TextBox textBox_ContactPerson;
        TextBox textBox_Phone;
        TextBox textBox_Email;

        public Clients_AddEditUserControl()
        {
            InitializeComponent();
            Init();
        }
        public Clients_AddEditUserControl(DBT_Clients obj)
        {
            InitializeComponent();
            Object = obj;
            Init();

            textBox_FullName.Text = obj.FullName.ToString();
            textBox_ContactPerson.Text = obj.ContactPerson.ToString();
            textBox_Phone.Text = obj.Phone.ToString();
            textBox_Email.Text = obj.Email.ToString();

            textBox_Email.Enabled = false;
        }

        private void Init()
        {
            this.Size = new Size(600, 500);
            this.Text = "Клиенты - " + (Object == null ? "Добавить" : "Изменить").ToString();
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
                RowCount = 4
            };

            Label label_FullName = new Label();
            SetLabel(ref label_FullName, "ФИО");
            tableLayout.Controls.Add(label_FullName, 0, 0);
            textBox_FullName = new TextBox();
            textBox_FullName.Dock = DockStyle.Fill;
            textBox_FullName.MaxLength = 255;
            tableLayout.Controls.Add(textBox_FullName, 1, 0);

            Label label_ContactPerson = new Label();
            SetLabel(ref label_ContactPerson, "Контактное лицо");
            tableLayout.Controls.Add(label_ContactPerson, 0, 1);
            textBox_ContactPerson = new TextBox();
            textBox_ContactPerson.Dock = DockStyle.Fill;
            textBox_ContactPerson.MaxLength = 255;
            tableLayout.Controls.Add(textBox_ContactPerson, 1, 1);

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

            int res = 0;

            if (Object == null)
            {
                res = DBT_Clients.Create(
                    new DBT_Clients()
                    {
                        FullName = textBox_FullName.Text,
                        ContactPerson = textBox_ContactPerson.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text
                    }
                );
            }
            else
            {
                res = DBT_Clients.Edit(
                    new DBT_Clients()
                    {
                        ID = Object.ID,
                        FullName = textBox_FullName.Text,
                        ContactPerson = textBox_ContactPerson.Text,
                        Phone = textBox_Phone.Text,
                        Email = textBox_Email.Text
                    }
                );
            }
            if (res == -1) MessageBox.Show("Ошибка! Один из ID не ссылается на запись в БД!");

            (this.Parent as Form).Close();
        }
    }
}
