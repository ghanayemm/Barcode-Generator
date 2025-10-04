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

using ClosedXML.Excel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.IO;
using System.Data.OleDb;
using iTextSharp.text;
using iTextSharp.text.pdf;


namespace Barcode_Generator
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        //********************************
        private void frmMain_Load(object sender, EventArgs e)
        {
            txtNewBarcode.Enabled = false;
            _LoadData();
            //FillComboBox();
            _RefreshData();
            txtNumber.Enabled = false;
        }

        clsBarcode NewBarcode = new clsBarcode();

        private bool CheckDuplicate(string potentialBarcode)
        {
            // Skip header row if present
            foreach (DataGridViewRow row in dgvBarcodes.Rows)
            {
                // Assuming barcode is in first column
                if (row.Cells[1].Value?.ToString() == potentialBarcode)
                {
                    return true;
                }

            }
            return false;
            
            

        }

        // تعديل عدد الخانات الى 11 رقم بدلا من 13
        private string GenerateBarcode()
        {
            Random random = new Random();
            string result = null;

            for(int i=0;i<11;i++)
            {
                result += random.Next(0, 10);
            }
            return result;
        }    

        //اختصار اسم المورد
        private string GetNameAbbreviation(string SupplierName)
        {
            // تحميل جدول الموردين من الكلاس الخاص بالموردين
            DataTable suppliers = clsSupplier.LoadSuppliers();

            // البحث عن الصفوف التي تطابق اسم المورد المدخل
            DataRow[] rows = suppliers.Select("Supplier = '" + SupplierName.Replace("'", "''") + "'");

            if(rows.Length >0)
            {
                // إعادة قيمة العمود الثالث (الاختصار) من أول صف مطابق
                return rows[0][2].ToString();
            }
            MessageBox.Show("المورد غير موجود: " + SupplierName);
            // إعادة قيمة فارغة إذا لم يتم العثور على المورد
            return string.Empty;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            

            try
            {
                //الحصول على اختصار اسم المورد من combobox
                string Abbreviation = GetNameAbbreviation(cmbSuppliers.Text);

                // دمج الاختصار مع الباركود المولد من الدالة GenerateBarcode
                string MergeBarcode = Abbreviation+ GenerateBarcode();
                // إزالة أي مسافات زائدة من الباركود النهائي
                string Barcode = MergeBarcode.Trim();


                txtNewBarcode.Text = Barcode;
     
                //اذا كان العدد اكبر او يساوي 1 ننفذ هذا الشرط 


                if (Barcode.Length != 13)
                {
                    MessageBox.Show("الباركود يجب أن يكون 13 خانة فقط.");
                    return;
                }


                if (CheckDuplicate(Barcode))
                {
                    MessageBox.Show("⚠️ هذا الباركود موجود مسبقًا، لا يمكن إضافته.");
                    return;
                }

                int NewID = NewBarcode.CreateBarcode(Barcode);

                if (NewID > 0)
                    MessageBox.Show("تمت إضافة الباركود بنجاح. رقم ID = " + NewID);
                else
                    MessageBox.Show("فشل في إضافة الباركود.");

            }
            catch(Exception ex)
            {
                MessageBox.Show($"حدث خطأ: {ex.Message}", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            txtNewBarcode.Clear();
            _RefreshData();
        }


        private static DataTable _AllBarcodes = clsBarcode.DisplayBarcodes();
        private DataTable _Barcodes= _AllBarcodes.DefaultView.ToTable(false, "ID", "Barcode");

        private void _LoadData()
        {
            dgvBarcodes.DataSource = _Barcodes;

            dgvBarcodes.Columns[0].HeaderText = "ID";
            dgvBarcodes.Columns[0].Width = 40;

            dgvBarcodes.Columns[1].HeaderText = "Barcode";
            dgvBarcodes.Columns[1].Width = 200;

            //lblCount.Text = dgvAccounts.RowCount.ToString().Trim();
        }

        private void _RefreshData()
        {
            _AllBarcodes = clsBarcode.DisplayBarcodes();
            _Barcodes = _AllBarcodes.DefaultView.ToTable(false, "ID", "Barcode");

            dgvBarcodes.DataSource = _Barcodes;

            //تحديث معلومات  combo box
            FillComboBox();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if(dgvBarcodes.CurrentRow == null)
                {
                    return;
                }

                int ID = Convert.ToInt32(dgvBarcodes.CurrentRow.Cells[0].Value);

                NewBarcode.DeleteBarcode(ID);
                _RefreshData();

            }
            catch(Exception ex)
            {
                throw new Exception("حدث خطأ غير متوقع أثناء حذف الباركود", ex);
            }
            
        }

        private void btnSuppliers_Click(object sender, EventArgs e)
        {
            Form frm = new frmSuppliers();
            frm.ShowDialog();
        }

       private void FillComboBox()
       {
            DataTable suppliers = clsSupplier.LoadSuppliers();

            cmbSuppliers.DataSource = suppliers;
            cmbSuppliers.DisplayMember = "Supplier";

            cmbSuppliers.ValueMember = "ID";
       }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            _RefreshData();
        }


        //**************************************

        //import data from data grid view to excel
        private void btnImportToExcel_Click(object sender, EventArgs e) 
        {
            // إذا ما في بيانات بالـ DataGridView اظهر رسالة للمستخدم وارجع
            if (dgvBarcodes.Rows.Count == 0)
            {
                MessageBox.Show("لا توجد بيانات للتصدير.", "ملاحظة", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // تحويل DataGridView إلى DataTable
            DataTable dt = ImportData(dgvBarcodes);

            // إنشاء SaveFileDialog لاختيار مكان حفظ الملف واسم الملف
            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "Excel Workbook|*.xlsx",// تحديد صيغة الملف
                FileName = "Barcodes.xlsx"  // الاسم الافتراضي للملف
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        // إنشاء كائن Workbook من مكتبة ClosedXML
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            // إضافة ورقة جديدة ووضع البيانات
                            var ws = wb.Worksheets.Add(dt, "Barcodes");

                            // تنسيقات بسيطة
                            ws.Columns().AdjustToContents(); // ضبط عرض الأعمدة
                            ws.Row(1).Style.Font.Bold = true; // العناوين بالخط العريض

                            // حفظ الملف بالمكان اللي حدده المستخدم
                            wb.SaveAs(sfd.FileName);
                        }

                        MessageBox.Show("✅ تم تصدير البيانات بنجاح إلى ملف Excel.",
                                        "نجاح", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("❌ خطأ أثناء التصدير: " + ex.Message,
                                        "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }


        }

        // دالة بتحول بيانات DataGridView إلى DataTable
        private DataTable ImportData(DataGridView dgvBarcodes)
        {
            DataTable dt = new DataTable();

            // إضافة أعمدة للـ DataTable من أعمدة DataGridView
            foreach (DataGridViewColumn col in dgvBarcodes.Columns)
            {
                if(col.Visible) // تجنب الاعمدة المخفية
                {
                    // إضافة العمود مع استخدام اسم العنوان إذا موجود، وإلا استخدام اسم العمود
                    dt.Columns.Add(string.IsNullOrWhiteSpace(col.HeaderText)?col.Name: col.HeaderText);
                }
            }

            // إضافة صفوف للـ DataTable
            foreach (DataGridViewRow row in dgvBarcodes.Rows)
            {
                if (row.IsNewRow) continue;

                DataRow dr = dt.NewRow();

                int colIndex = 0;

                // المرور على أعمدة DataGridView
                foreach (DataGridViewColumn col in dgvBarcodes.Columns)
                {
                    // تجاهل الأعمدة المخفية
                    if (!col.Visible) continue;
                    // أخذ القيمة من الخلية
                    object val = row.Cells[col.Index].Value;
                    // إذا القيمة null نحط DBNull بدلها
                    dr[colIndex++] = val ?? DBNull.Value;
                }
                dt.Rows.Add(dr);

            }

            return dt;
        }


        //***************************************

        //import data from data grid view to pdf
        public static void ExportDataGridViewToPDF(DataGridView dgv, string filePath)
        {
            try
            {
                // إنشاء مستند PDF
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 20f);
                PdfWriter.GetInstance(pdfDoc, new FileStream(filePath, FileMode.Create));
                pdfDoc.Open();

                // خط عربي أو إنجليزي (لو عندك عربي لازم تحدد خط يدعم العربية)
                BaseFont bf = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
                iTextSharp.text.Font textFont = new iTextSharp.text.Font(bf, 10, iTextSharp.text.Font.NORMAL);

                // إنشاء جدول PDF بنفس عدد أعمدة DataGridView
                PdfPTable pdfTable = new PdfPTable(dgv.Columns.Count);
                pdfTable.WidthPercentage = 100;

                // إضافة العناوين
                foreach (DataGridViewColumn column in dgv.Columns)
                {
                    PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText, textFont));
                    cell.BackgroundColor = BaseColor.LIGHT_GRAY; // لون خلفية للعناوين
                    pdfTable.AddCell(cell);
                }

                // إضافة الصفوف
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    if (row.IsNewRow) continue; // تجاهل الصف الأخير الفارغ
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        string value = cell.Value?.ToString() ?? "";
                        pdfTable.AddCell(new Phrase(value, textFont));
                    }
                }

                // إضافة الجدول إلى الملف
                pdfDoc.Add(pdfTable);

                pdfDoc.Close();

                MessageBox.Show("✅ تم حفظ البيانات في ملف PDF بنجاح!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("❌ خطأ أثناء إنشاء PDF: " + ex.Message);
            }
        }

        private void btnNumber_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog sfd = new SaveFileDialog()
            {
                Filter = "PDF Files|*.pdf",
                FileName = "Barcode.pdf"
            })
            {
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    ExportDataGridViewToPDF(dgvBarcodes, sfd.FileName);
                }
            }
        }
    }


    //********* end of lines ***************
}

