using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIABD.Models;
using System.Data;
using System.Data.SqlClient;

namespace PIABD.Controllers
{
    [Route("dependencias")]
    [ApiController]
    public class DependenciasController : ControllerBase
    {
        private IConfiguration _configuration;
        public DependenciasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult get()
        {
            string query = "select * from Dependencias";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
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

        [HttpPost]
        public JsonResult Post(Dependencia dependencia)
        {
            string query = "EXEC sp_AgregarDependencia  @Dependencia";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Dependencia", dependencia.dependencia);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Agrego con Exito la Dependencia");
        }

        [HttpDelete("{ID}")]
        public JsonResult Delete(int ID)
        {
            string query = "EXEC sp_EliminarDependencia @DependenciaID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DependenciaID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Elimino con Exito la dependencia");
        }


        [HttpPut]
        public JsonResult Put(Dependencia dependencia)
        {
            string query = "EXEC sp_ModificarDependencia @Dependencia, @DependenciaID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Dependencia", dependencia.dependencia);
                    myCommand.Parameters.AddWithValue("@DependenciaID", dependencia.dependenciaID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Se Modifico con Exito la Dependencia");
        }

        [HttpGet("{ID}")]
        public JsonResult GetDependenciabyID(int ID)
        {
            string query = @"SELECT * FROM Dependencias WHERE DependenciaID=@DependenciaID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@DependenciaID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count == 0)
            {
                return new JsonResult(new { message = "Dependencia no encontrado" }) { StatusCode = 404 };
            }
            DataRow row = table.Rows[0];
            Dependencia dependencia = new Dependencia
            {
                dependenciaID = Convert.ToInt32(row["DependenciaID"]),
                dependencia = row["Dependencia"].ToString(),
            };
            return new JsonResult(dependencia);
        }

    }

}
