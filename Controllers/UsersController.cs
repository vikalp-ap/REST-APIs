using System.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RESTAPIs.Models;

namespace RESTAPIs.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly string? _connectionString;

        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("UsersCon");
        }

        [HttpGet]
        [Route("/users")]
        public async Task<IActionResult> Get()
        {
            try
            {
                string query = @"SELECT * FROM users ORDER BY id ASC";
                DataTable table = new DataTable();
                using (var conn = new NpgsqlConnection(_connectionString))
                {
                    await conn.OpenAsync();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                    {
                        NpgsqlDataReader myReader = await command.ExecuteReaderAsync();
                        table.Load(myReader);
                    }
                }
                return Ok(table);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("/addUsers")]
        public async Task<IActionResult> Post(UsersModel user)
        {
            try
            {
                string query = @"INSERT INTO users (name, email, age) VALUES (@Name, @Email, @Age);";
                using (NpgsqlConnection myCon = new NpgsqlConnection(_connectionString))
                {
                    await myCon.OpenAsync();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                        myCommand.Parameters.AddWithValue("@Email", user.Email ?? (object)DBNull.Value);
                        myCommand.Parameters.AddWithValue("@Age", user.Age);
                        await myCommand.ExecuteNonQueryAsync();
                    }
                }
                return Ok("Added Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("/updateUsers")]
        public async Task<IActionResult> Put(UsersModel user)
        {
            try
            {
                string query = @"UPDATE users SET name = @Name WHERE id = @Id;";
                using (NpgsqlConnection myCon = new NpgsqlConnection(_connectionString))
                {
                    await myCon.OpenAsync();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Name", user.Name ?? (object)DBNull.Value);
                        myCommand.Parameters.AddWithValue("@Id", user.Id);
                        await myCommand.ExecuteNonQueryAsync();
                    }
                }
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("/deleteUsers")]
        public async Task<IActionResult> Delete(UsersModel user)
        {
            try
            {
                string query = @"DELETE FROM users WHERE id = @Id;";
                using (NpgsqlConnection myCon = new NpgsqlConnection(_connectionString))
                {
                    await myCon.OpenAsync();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Id", user.Id);
                        await myCommand.ExecuteNonQueryAsync();
                    }
                }
                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}