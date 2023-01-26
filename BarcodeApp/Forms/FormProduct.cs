using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BarcodeApp.Forms
{
    public partial class FormProduct : Form
    {

        public string ProductName
        {
            get;

            set;
        }

        public string ProductCode
        {
            get; set;
        }


        public FormProduct()
        {
            InitializeComponent();
        }

        private void productForm_Load(object sender, EventArgs e)
        {
            this.productCodeTextBox.Text = this.ProductCode;
            this.productNameTextBox.Text = this.ProductName;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.ProductName = this.productNameTextBox.Text;
            this.DialogResult = DialogResult.OK;
        }
    }
}
