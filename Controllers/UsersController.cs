using System.Data;
using Microsoft.AspNetCore.Mvc;
using Npgsql;
using RESTAPIs.Models;

namespace RESTAPIs.Controllers;

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
    public JsonResult Get()
    {
        string query = @"SELECT * FROM users ORDER BY id ASC";
        DataTable table = new DataTable();
        string? psqlDataSource = _configuration.GetConnectionString("UsersCon");
        NpgsqlDataReader myReader;
        using (var conn = new NpgsqlConnection(psqlDataSource))
        {
            conn.Open();
            using (NpgsqlCommand command = new NpgsqlCommand(query, conn))
            {
                myReader = command.ExecuteReader();
                table.Load(myReader);
                conn.Close();
            }
        }
        return new JsonResult(table);
    }

    [HttpPost]
    [Route("/addUsers")]
    public JsonResult Post(UsersModel user)
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
        return new JsonResult("Added Successfully");
    }

    [HttpPut]
    [Route("/updateUsers")]
    public JsonResult Put(UsersModel user)
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
        return new JsonResult("Updated Successfully");
    }

    [HttpDelete]
    [Route("/deleteUsers")]
    public JsonResult Delete(UsersModel user)
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
        return new JsonResult("Deleted Successfully");
    }
}