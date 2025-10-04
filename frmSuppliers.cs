using Business_Layer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Business_Layer;

namespace Barcode_Generator
{
    public partial class frmSuppliers : Form
    {
        public frmSuppliers()
        {
            InitializeComponent();
        }

        //**************************

        clsSupplier NewSupplier = new clsSupplier();

        private void frmSuppliers_Load(object sender, EventArgs e)
        {
            _LoadData();
           
        }


        private static DataTable _AllSuppliers = clsSupplier.LoadSuppliers();
        private DataTable _Supllier = _AllSuppliers.DefaultView.ToTable(false, "ID", "Supplier", "Abbreviation");

        private void _LoadData()
        {
            dgvSuppliers.DataSource = _Supllier;

            dgvSuppliers.Columns[0].HeaderText = "ID";
            dgvSuppliers.Columns[0].Width = 30;

            dgvSuppliers.Columns[1].HeaderText = "Supplier";
            dgvSuppliers.Columns[1].Width = 100;

            dgvSuppliers.Columns[2].HeaderText = "Abbreviation";
            dgvSuppliers.Columns[2].Width = 70;

            //lblCount.Text = dgvAccounts.RowCount.ToString().Trim();
        }

        private void _RefreshData()
        {
            _AllSuppliers = clsSupplier.LoadSuppliers(); // تحديث الجدول الأساسي
            _Supllier = _AllSuppliers.DefaultView.ToTable(false, "ID", "Supplier", "Abbreviation");

            dgvSuppliers.DataSource = _Supllier;
        }

        private void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                if(string.IsNullOrEmpty(txtNewSupplier.Text))
                {
                    MessageBox.Show("ٍSupplier Name is Required");
                    txtNewSupplier.Focus();
                    return;
                }

                if(string.IsNullOrEmpty(txtAbber.Text))
                {
                    MessageBox.Show("ٍAbbreviation is Required");
                    txtAbber.Focus();
                    return;
                }

                if(txtAbber.Text.Length > 3)
                {
                    MessageBox.Show("ٍAbbreviation Must less than 3 characters");
                    txtAbber.Focus();
                    return; 
                }


                string SupplierName = txtNewSupplier.Text.Trim();
                string Abbreviation = txtAbber.Text.Trim();

                NewSupplier.CeateNewCategory(SupplierName, Abbreviation);
                MessageBox.Show("Supplier added successfully");

            }
            catch(Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            ClearData();
            _RefreshData();
        }


        private void ClearData()
        {
            txtNewSupplier.Clear();
            txtAbber.Clear();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvSuppliers.CurrentRow == null)
                {
                    return;
                }

                int ID = Convert.ToInt32(dgvSuppliers.CurrentRow.Cells[0].Value);

                NewSupplier.DeleteCategory(ID);
                _RefreshData();

            }
            catch(Exception ex)
            {
                throw new Exception("حدث خطأ غير متوقع أثناء حذف المورد", ex);
            }
        
        }



        //******* end of text*****
    }
}
