using System;
using System.Data;
using Data_Layer;

namespace Business_Layer
{
    public class clsSupplier
    {
        public int ID { get; set; }
        public string Supplier { get; set; }
        public string Abbreviation { get; set; }

        enum enMode { AddNew = 0, Update = 1 }
        enMode Mode = enMode.AddNew;

        public clsSupplier()
        {
            this.ID = -1;
            this.Supplier = null;
            this.Abbreviation = null;

            Mode = enMode.AddNew;
        }

        private clsSupplier(int ID, string Supplier, string Abbreviation)
        {
            this.ID = ID;
            this.Supplier = Supplier;
            this.Abbreviation = Abbreviation;

            Mode = enMode.Update;
        }

        //********************************

        public int CeateNewCategory(string Supplier, string Abbreviation)
        {
            return clsSupplierData.CreateNewCategory(Supplier, Abbreviation);
        }

        public bool DeleteCategory(int ID)
        {
            return clsSupplierData.DeleteCategory(ID);
        }

        public static DataTable LoadSuppliers()
        {
            return clsSupplierData.GetAllSuppliers();
        }


        //********* end of text __________
    }
}
