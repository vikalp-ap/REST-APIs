using System;
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
        public UsersController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        [Route("/users")]
        public IActionResult Get()
        {
            try
            {
                string query = @"SELECT * FROM users ORDER BY id ASC";
                DataTable table = new DataTable();
                string? sqlDataSource = _configuration.GetConnectionString("UsersCon");
                NpgsqlDataReader myReader;
                using (var conn = new NpgsqlConnection(sqlDataSource))
                {
                    conn.Open();
                    using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
                    {
                        myReader = command.ExecuteReader();
                        table.Load(myReader);
                        conn.Close();
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
        public IActionResult Post(UsersModel user)
        {
            try
            {
                string query = @"INSERT INTO users (name, email, age) VALUES (@Name, @Email, @Age);";

                DataTable table = new DataTable();
                string? sqlDataSource = _configuration.GetConnectionString("UsersCon");
                NpgsqlDataReader myReader;
                using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Name", user.Name);
                        myCommand.Parameters.AddWithValue("@Email", user.Email);
                        myCommand.Parameters.AddWithValue("@Age", user.Age);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);

                        myReader.Close();
                        myCon.Close();
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
        public IActionResult Put(UsersModel user)
        {
            try
            {
                string query = @"UPDATE users SET name = @Name WHERE id = @Id;";
                DataTable table = new DataTable();
                string? sqlDataSource = _configuration.GetConnectionString("UsersCon");
                NpgsqlDataReader myReader;
                using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Name", user.Name);
                        myCommand.Parameters.AddWithValue("@Id", user.Id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();
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
        public IActionResult Delete(UsersModel user)
        {
            try
            {
                string query = @"DELETE FROM users WHERE id = @Id;";
                DataTable table = new DataTable();
                string? sqlDataSource = _configuration.GetConnectionString("UsersCon");
                NpgsqlDataReader myReader;
                using (NpgsqlConnection myCon = new NpgsqlConnection(sqlDataSource))
                {
                    myCon.Open();
                    using (NpgsqlCommand myCommand = new NpgsqlCommand(query, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@Id", user.Id);
                        myReader = myCommand.ExecuteReader();
                        table.Load(myReader);
                        myReader.Close();
                        myCon.Close();

                    }
                }
                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}