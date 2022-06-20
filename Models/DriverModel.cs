using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDrivers.Models
{
    public class DriverModel
    {
        // Variables
        string ConnectionString = "Server=tcp:sqlserverjalg.database.windows.net,1433;Initial Catalog=sqldriversjalg;Persist Security Info=False;User ID=sqljalg;Password={your_password};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";

        //Propiedades
        public int ID { get; set; }
        public string? Name { get; set; }
        public string? Picture { get; set; }
        public string? Status { get; set; }
        public PositionModel ActualPosition { get; set; }
        public List<PositionModel> RouteLastTravel { get; set; }

        //Métodos
        public ApiResponse GetAll()
        {
            List<DriverModel> list = new List<DriverModel>();
            try
            {
                using (SqlConnection conn = new SqlConnection(ConnectionString))
                {
                    conn.Open();
                    string tsql = "SELECT * FROM Driver " +
                                  "INNER JOIN Position On Driver.IDActualPosition = Position.IDPosition";
                    using(SqlCommand cmd = new SqlCommand(tsql, conn))
                    {
                        using(SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                list.Add(new DriverModel
                                {
                                    ID = (int)reader["IDDriver"],
                                    Name = reader["Name"].ToString(),
                                    Picture = reader["Picture"].ToString(),
                                    Status = reader["Status"].ToString(),
                                    ActualPosition = new PositionModel
                                    {
                                        ID = (int)reader["IDPosition"],
                                        Latitude = (double)reader["Latitude"],
                                        Longitude = (double)reader["Longitude"]
                                    }
                                });
                            }
                        }
                    }
                }
                return new ApiResponse
                {
                    IsSuccess = true,
                    Message = "Los conductores fueron obtenidos correctamente",
                    Result = list
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = $"Se generó un error al obtener los conductores: {ex.Message}",
                    Result = null
                };
            }
        }

    }
}
