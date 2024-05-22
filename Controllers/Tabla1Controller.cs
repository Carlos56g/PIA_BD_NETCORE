using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace PIABD.Controllers
{

    [ApiController]
    public class Tabla1Controller : ControllerBase
    {

        private IConfiguration _configuration;
        public Tabla1Controller(IConfiguration configuration)
        {
            _configuration= configuration;
        }

        [HttpGet("get_tasks()")]

        public JsonResult get_task()
        {
            string query = "select * from todo";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("myDB");
            SqlDataReader myReader;
            using (SqlConnection myCon=new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult(table);
        }

        [HttpPost("add_task")]

        public JsonResult add_task([FromForm] string task)
        {
            string query = "insert into todo values (@task)";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("myDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@task", task);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult("Se Agrego con Exito");
        }


        [HttpPost("delete_task()")]
        public JsonResult delete_task([FromForm] string id)
        {
            string query = "delete from todo where id=@id";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("myDB");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {

                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult("Se Elimino con Exito");
        }
    }
}
