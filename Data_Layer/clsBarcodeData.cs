using System;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;

namespace Data_Layer
{
    public class clsBarcodeData
    {
        
        public static int CreateBarcode(string barcodeValue)
        {
            int ID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Barcode (Barcode) VALUES(@Barcode); SELECT SCOPE_IDENTITY();";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Barcode", SqlDbType.VarChar, 13).Value = barcodeValue;

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            ID = insertedID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }

            return ID;
        }

        public static bool DeleteBarcode(int ID)
        {
            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"DELETE FROM Barcode WHERE ID=@ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                        

                        rowsAffected = command.ExecuteNonQuery();

                    }

                }

            }
            catch(Exception ex)
            {
                throw new Exception("حدث خطأ غير متوقع أثناء حذف الباركود", ex);
            }
            return (rowsAffected > 0);
        }

        public static DataTable GetAllBarcodes()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "select * from Barcode";

                    using(SqlDataAdapter adapter=new SqlDataAdapter(query,connection))
                    {
                        adapter.Fill(dt);
                    }


                    //using (SqlCommand command = new SqlCommand(query, connection))
                    //{
                    //    using(SqlDataReader reader=command.ExecuteReader())
                    //    {
                    //        if(reader.Read())
                    //        {
                    //            dt.Load(reader);
                    //        }
                    //    }
                    //}

                   
                }

            }
            catch(Exception ex)
            {
                throw new Exception("حدث خطأ غير متوقع أثناء عرض البيانات", ex);
            }
            return dt;
        }

     


        //***** end of lines******
    }
}
