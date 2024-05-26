using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PIABD.Models;
using System.Data;
using System.Data.SqlClient;

namespace PIABD.Controllers
{
    [Route("/costos")]
    [ApiController]
    public class CostosConstroller : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public CostosConstroller(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"SELECT * FROM Costos";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpGet("{ID}")]
        public JsonResult GetbyEventoID(int ID)
        {
            string query = @"SELECT * FROM Costos WHERE EventoID=@EventoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EventoID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
                return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Costo costo)
        {
            string query = "EXEC sp_AgregarCosto  @EventoID, @Descripcion, @Costo";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Descripcion", costo.descripcion);
                    myCommand.Parameters.AddWithValue("@Costo", costo.costo);
                    myCommand.Parameters.AddWithValue("@EventoID", costo.eventoID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            
            return new JsonResult("Se Agrego con Exito el Costo");
        }

        [HttpDelete("{ID}")]
        public JsonResult Delete(int ID)
        {
            string query = @"DELETE FROM Costos WHERE CostoID = @CostoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CostoID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Elimino con Exito el Costo");
        }

        [HttpPut]
        public JsonResult Put(Costo costo)
        {
            string query = "EXEC sp_ModificarCosto @Descripcion,@Costo,@CostoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@CostoID", costo.costoID);
                    myCommand.Parameters.AddWithValue("@Descripcion", costo.descripcion);
                    myCommand.Parameters.AddWithValue("@Costo", costo.costo);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }

            }
            return new JsonResult("Se Modifico con Exito el Precio");
        }

    }
}
