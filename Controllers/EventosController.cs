using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using PIABD.Models;
using System.Data.SqlTypes;


namespace PIABD.Controllers
{
    [Route("eventos")]
    [ApiController]
    public class EventosController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EventosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]

        public JsonResult Get()
        {
            string query = @"SELECT * from Eventos";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon=new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand=new SqlCommand(query, myCon))
                {
                    myReader=myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);

        }

        [HttpPost]
        public JsonResult Post(Evento evento)
        {
            string query = "EXEC sp_AgregarEvento @Titulo, @Descripcion, @Fecha, @RecintoID, @DependenciaID, @CategoriaID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Titulo", evento.titulo);
                    myCommand.Parameters.AddWithValue("@Descripcion", evento.descripcion);
                    myCommand.Parameters.AddWithValue("@Fecha", evento.fecha);
                    myCommand.Parameters.AddWithValue("@RecintoID", evento.recintoID);
                    myCommand.Parameters.AddWithValue("@DependenciaID", evento.dependenciaID);
                    myCommand.Parameters.AddWithValue("@CategoriaID", evento.categoriaID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
                
            }
            return new JsonResult("Se Agrego el Evento Con Exito!");
        }
            /*string query2 = "EXEC sp_AgregarCosto  @EventoID, @Descripcion, @Costo";
            DataTable table2 = new DataTable();
            SqlDataReader myReader2;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                for (int i = 0; i < n; i++)
                {
                    myCon.Open();
                    using (SqlCommand myCommand = new SqlCommand(query2, myCon))
                    {
                        myCommand.Parameters.AddWithValue("@EventoID", costo[i].eventoID);
                        myCommand.Parameters.AddWithValue("@Descripcion", costo[i].descripcion);
                        myCommand.Parameters.AddWithValue("@Costo", costo[i].costo);
                        myReader2 = myCommand.ExecuteReader();
                        table2.Load(myReader2); ;
                        myReader2.Close();
                        myCon.Close();
                    }
                }*/



            [HttpDelete("{ID}")]
        public JsonResult Delete(int ID)
        {
            string query = "EXEC sp_EliminarEvento @EventoID";
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
            return new JsonResult("Se Elimino con Exito");
        }

        [HttpPut]
        public JsonResult put(Evento evento)
        {
            string query = "EXEC sp_ModificarEvento @Titulo, @Descripcion, @Fecha, @RecintoID, @DependenciaID, @CategoriaID, @EventoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Titulo", evento.titulo);
                    myCommand.Parameters.AddWithValue("@Descripcion", evento.descripcion);
                    myCommand.Parameters.AddWithValue("@Fecha", evento.fecha);
                    myCommand.Parameters.AddWithValue("@RecintoID", evento.recintoID);
                    myCommand.Parameters.AddWithValue("@DependenciaID", evento.dependenciaID);
                    myCommand.Parameters.AddWithValue("@CategoriaID", evento.categoriaID);
                    myCommand.Parameters.AddWithValue("@EventoID", evento.eventoID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Modifico con Exito");
        }


        [HttpGet("mostrarEventos()")]
        public JsonResult mostrarEventos()
        {
            string query = @"EXEC sp_MotrarEventos";
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
        public JsonResult getEvento(int ID)
        {
            string query = @"SELECT * from Eventos WHERE EventoID=@EventoID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@EventoID",ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count == 0)
            {
                return new JsonResult(new { message = "Evento no encontrado" }) { StatusCode = 404 };
            }
            DataRow row = table.Rows[0];
            Evento evento = new Evento
            {
                eventoID = Convert.ToInt32(row["EventoID"]),
                titulo = row["Titulo"].ToString(),
                descripcion = row["Descripcion"].ToString(),
                fecha = Convert.ToDateTime(row["Fecha"]),
                recintoID = Convert.ToInt32(row["RecintoID"]),
                dependenciaID = Convert.ToInt32(row["DependenciaID"]),
                categoriaID = Convert.ToInt32(row["CategoriaID"])
            };
            return new JsonResult(evento);


        }

        

    }
}
