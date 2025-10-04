using System;
using System.Data;
using System.Data.SqlClient;

namespace Data_Layer
{
    public class clsSupplierData
    {

        public static int CreateNewCategory(string Supplier,string Abbreviation)
        {
            int ID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    string query = @"INSERT INTO Supplier
                                            (Supplier,Abbreviation)
                                            VALUES
                                           (@Supplier,@Abbreviation);
                                            SELECT SCOPE_IDENTITY();";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@Supplier", SqlDbType.VarChar, 200).Value = Supplier;
                        command.Parameters.Add("@Abbreviation", SqlDbType.VarChar, 3).Value = Abbreviation;

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if(result != null && int.TryParse(result.ToString(),  out int InsertedID))
                        {
                            ID = InsertedID;
                        }


                    }

                }

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
            return ID;
        }

        public static bool DeleteCategory(int ID)
        {
            int RowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    string query = @"DELETE FROM Supplier WHERE ID=@ID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.Add("@ID", SqlDbType.Int).Value = ID;

                        connection.Open();

                        RowsAffected = command.ExecuteNonQuery();

                    }

                }

            }
            catch(Exception ex)
            {
                throw new Exception("حدث خطأ غير متوقع أثناء حذف الباركود", ex);
            }

            return (RowsAffected > 0);
        }

        public static DataTable GetAllSuppliers()
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataSettings.ConnectionString))
                {
                    connection.Open();

                    string query = "select * from supplier";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                       

                        using(SqlDataAdapter adapter= new SqlDataAdapter(query,connection))
                        {
                            adapter.Fill(dt);
                        }

                    }

                }

            }
            catch(Exception ex)
            {
                throw new Exception("حدث خطأ غير متوقع أثناء عرض البيانات", ex);
            }
            return dt;
        }


        //***** end of text*****
    }
}
