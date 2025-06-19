using System;
using System.Configuration;
using System.Data.SqlClient;

namespace DataAccess
{
    public class clsBackups
    {

        /// <summary>
        /// This for backup the database with differential
        /// </summary>
        /// <param name="ToDiskPath">This is the path to save a backup</param>
        /// <returns>result of backup and the message if there is an error.</returns>
        public static (bool Result,string MessageError) BackUp(string ToDiskPath)
        {
            (bool Result, string MessageError) Data = (true, null);
            try
            {
                using(SqlConnection conn = new SqlConnection(ConfigurationManager.AppSettings["ConnectionString"]))
                {

                    string query = @"exec SP_BackUp
                                    @Path";
                    using(SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        conn.Open();
                        cmd.Parameters.AddWithValue("@Path", ToDiskPath);
                        cmd.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                Data.Result = false;
                Data.MessageError = ex.Message;
            }

            return Data;
        }
    }
}
