using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Week9Day5Demo.Models;
using Week9Day5Demo.Models.Settings;

namespace Week9Day5Demo.Services.Students
{
    public class StudentsService : IStudentsService
    {
        private readonly ConnectionStrings _connectionStrings;
        //private const string ConnectionString =
        //    @"Data Source=.;Initial Catalog=WorldLineDatabase;User ID=worldline;Password=Test@123";

        public StudentsService(IOptions<ConnectionStrings> connectionStringsAccessor)
        {
            _connectionStrings = connectionStringsAccessor.Value;
        }


        #region Synchronous Methods

        public List<Student> GetAllStudents()
        {
            var students = new List<Student>();

            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("select * from students", connection))
                {
                    connection.Open();
                    var reader = command.ExecuteReader();

                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            var student = new Student
                            {
                                RollNo = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2),
                                Percentage = reader.GetDouble(3)
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            return students;
        }

        public void Insert(Student student)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                // Terrible programmer to write query inside of aspx source code
                var query = $"insert into Students values({student.RollNo}, {student.Name}, {student.DateOfBirth}, {student.Percentage})";
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Update(Student student)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RollNo", student.RollNo);
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Percentage", student.Percentage);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        public void Delete(Student student)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RollNo", student.RollNo);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Asynchronous Methods

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            var students = new List<Student>();

            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("select * from students", connection))
                {
                    await connection.OpenAsync();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        while (await reader.ReadAsync())
                        {
                            var student = new Student
                            {
                                RollNo = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2),
                                Percentage = reader.GetDouble(3)
                            };

                            students.Add(student);
                        }
                    }
                }
            }

            return students;
        }

        public async Task InsertAsync(Student student)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                // Terrible programmer to write query inside of aspx source code
                //var query = $"insert into Students values({student.RollNo}, '{student.Name}', '{student.DateOfBirth}', {student.Percentage})";

                using (var command = new SqlCommand("InsertStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RollNo", student.RollNo);
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Percentage", student.Percentage);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task UpdateAsync(Student student)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("UpdateStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RollNo", student.RollNo);
                    command.Parameters.AddWithValue("@Name", student.Name);
                    command.Parameters.AddWithValue("@DateOfBirth", student.DateOfBirth);
                    command.Parameters.AddWithValue("@Percentage", student.Percentage);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task DeleteAsync(int rollNo)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("DeleteStudent", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RollNo", rollNo);

                    await connection.OpenAsync();
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task<Student> Find(int rollNo)
        {
            using (var connection = new SqlConnection(_connectionStrings.DefaultConnectionString))
            {
                using (var command = new SqlCommand("GetStudentByRollNo", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@RollNo", rollNo);

                    await connection.OpenAsync();
                    var reader = await command.ExecuteReaderAsync();

                    if (reader.HasRows)
                    {
                        if (await reader.ReadAsync())
                        {
                            var student = new Student
                            {
                                RollNo = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                DateOfBirth = reader.GetDateTime(2),
                                Percentage = reader.GetDouble(3)
                            };

                            return student;
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}