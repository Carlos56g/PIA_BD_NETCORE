using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PIABD.Models;
using System.Data;
using System.Data.SqlClient;

namespace PIABD.Controllers
{
    [ApiController]
    [Route("recintos")]
    public class RecintosController : ControllerBase
    {

        private IConfiguration _configuration;
        public RecintosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        
        [HttpGet]


        public JsonResult get()
        {
            string query = "select * from Recintos";
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
        public JsonResult Post(Recinto recinto)
        {
            string query = "EXEC sp_AgregarRecinto @Recinto, @Direccion, @Capacidad";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Recinto", recinto.recinto);
                    myCommand.Parameters.AddWithValue("@Direccion", recinto.direccion);
                    myCommand.Parameters.AddWithValue("@Capacidad", recinto.capacidad);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Se Agrego el Recinto Con Exito!");
        }

        [HttpPut]
        public JsonResult Put(Recinto recinto)
        {
            string query = "EXEC sp_ModificarRecinto @RecintoID, @Recinto, @Direccion, @Capacidad";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Direccion", recinto.direccion);
                    myCommand.Parameters.AddWithValue("@Recinto", recinto.recinto);
                    myCommand.Parameters.AddWithValue("@Capacidad", recinto.capacidad);
                    myCommand.Parameters.AddWithValue("@RecintoID", recinto.recintoID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Se Modifico con Exito el Recinto");
        }

        [HttpDelete("{ID}")]
        public JsonResult Delete(int ID)
        {
            string query = "EXEC sp_EliminarRecinto @RecintoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@RecintoID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Elimino con Exito el Recinto");
        }

        [HttpGet("{ID}")]
        public JsonResult GetRecintobyID(int ID)
        {
            string query = @"SELECT * FROM Recintos WHERE RecintoID=@RecintoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@RecintoID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count == 0)
            {
                return new JsonResult(new { message = "Recinto no encontrado" }) { StatusCode = 404 };
            }
            DataRow row = table.Rows[0];
            Recinto recinto = new Recinto
            {
                recintoID = Convert.ToInt32(row["RecintoID"]),
                recinto = row["Recinto"].ToString(),
                direccion = row["Direccion"].ToString(),
                capacidad = Convert.ToInt32(row["Capacidad"])
            };
            return new JsonResult(recinto);
        }

    }


}
