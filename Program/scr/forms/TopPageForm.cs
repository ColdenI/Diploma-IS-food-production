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
    public partial class TopPageForm : Form
    {
        public TopPageForm(UserControl uc, string title = "")
        {
            InitializeComponent();
            ShowControl(uc);
            this.Text = title;
        }

        public void ShowControl(UserControl control)
        {
            UserControl UControl = control;
            UControl.Dock = DockStyle.Fill;
            this.Controls.Add(UControl);
        }
    }
}
