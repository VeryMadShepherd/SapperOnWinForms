using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SapperOnWinForms
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            int rows = int.Parse(RowsBox.Text);
            int columns = int.Parse(ColumnsBox.Text);
            int mines = int.Parse(MinesBox.Text);
            var sapperForm = new SapperForm(rows, columns, mines);
            sapperForm.Show();
        }

        public static void AllowOnlyInteger(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }
    }
}
