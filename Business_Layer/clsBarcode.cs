using System;
using System.Data;
using Data_Layer;


namespace Business_Layer
{
    public class clsBarcode
    {

         enum Mode { AddNew=0, Update=1}
        Mode enMode = Mode.AddNew;

        public int ID { get; set; }
        public string Barcode { get; set; }

        public clsBarcode()
        {
            this.ID = -1;
            this.Barcode = null;

            enMode = Mode.AddNew;
        }

        private clsBarcode(int ID, string Barcode)
        {
            this.ID = ID;
            this.Barcode = Barcode;

            enMode = Mode.Update;
        }


        //------- create new Barcode-------

        public int CreateBarcode(string Barcode)
        {
            return clsBarcodeData.CreateBarcode(Barcode);
        }

        //------- Delete Barcode-------
        public bool DeleteBarcode(int ID)
        {
            return clsBarcodeData.DeleteBarcode(ID);
        }

        //------- Gett all Barcodes-------

        public static DataTable DisplayBarcodes()
        {
            return clsBarcodeData.GetAllBarcodes();
        }



        //end of lines*****
    }
}
