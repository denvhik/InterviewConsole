using EmployeeService.Dto;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace EmployeeService.Data
{
    public class EmployeeRepository
    {
        private readonly string _connectionString;

        public EmployeeRepository()
        {
            _connectionString =
                ConfigurationManager.ConnectionStrings["EmployeeDb"].ConnectionString;
        }

        public List<EmployeeDto> GetEmployeesById(int Id)
        {
            List<EmployeeDto> list = new List<EmployeeDto>();

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(Constants.SqlGetEmployeeSubtree, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@id", SqlDbType.Int).Value = Id;

                conn.Open();
                using (var r = cmd.ExecuteReader())
                {
                    while (r.Read())
                    {
                        list.Add(new EmployeeDto
                        {
                            Id = r.GetInt32(0),
                            Name = r.IsDBNull(1) ? null : r.GetString(1),
                            ManagerId = r.IsDBNull(2) ? (int?)null : r.GetInt32(2)
                        });
                    }
                }
            }

            return list;
        }

        public bool SetEmployeeEnabled(int id, bool enabled)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(Constants.SqlEmployeeEnabled, conn))
            {
                cmd.CommandType = CommandType.Text;

                cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                cmd.Parameters.Add("@enable", SqlDbType.Bit).Value = enabled; 

                conn.Open();

                int rowsAffected = cmd.ExecuteNonQuery();

                return rowsAffected > 0;
            }
        }
    }
}
