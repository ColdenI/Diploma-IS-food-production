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
                    MessageBox.Show("¬ведите данные дл€ проверки заказа!");
                    return;
                }
                if (!int.TryParse(textBox_c_order.Text, out int _Id)) 
                {
                    MessageBox.Show("¬ведите данные дл€ проверки заказа!");
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
        }

        private void button_close_Click(object sender, EventArgs e)
        {
            panel_client.Visible = false;
            panel_empl.Visible = false;
        }


    }
}
