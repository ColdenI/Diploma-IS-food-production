using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Program.scr.forms
{
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) => new TopPageForm(new userControls.Clients_ViewUserControl(), "Клиенты").ShowDialog();
        private void button2_Click(object sender, EventArgs e) => new TopPageForm(new userControls.Suppliers_ViewUserControl(), "Поставщики").ShowDialog();
        private void button3_Click(object sender, EventArgs e) => new TopPageForm(new userControls.RawMaterials_ViewUserControl(), "Сырьё").ShowDialog();
        private void button4_Click(object sender, EventArgs e) => new TopPageForm(new userControls.Supplies_ViewUserControl(), "Поставки").ShowDialog();
        private void button5_Click(object sender, EventArgs e) => new TopPageForm(new userControls.Employees_ViewUserControl(), "Сотрудники").ShowDialog();
        private void button6_Click(object sender, EventArgs e) => new TopPageForm(new userControls.Auth_ViewUserControl(), "Авторизация").ShowDialog();
        private void button7_Click(object sender, EventArgs e) => new TopPageForm(new userControls.Products_ViewUserControl(), "Продукты").ShowDialog();
        private void button8_Click(object sender, EventArgs e) => new TopPageForm(new userControls.SalesOrders_ViewUserControl(), "Заказы").ShowDialog();

        private void button9_Click(object sender, EventArgs e)
        {
            new AnalyticsForm().ShowDialog();
        }
    }
}
