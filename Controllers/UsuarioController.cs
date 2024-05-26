using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIABD.Models;
using System.Data;
using System.Data.SqlClient;


namespace PIABD.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IConfiguration _configuration;
        public UsuarioController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult get()
        {
            string query = "select * from Usuarios";
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

        [HttpGet("{correo}")]
        public JsonResult get(string correo)
        {
            string query = @"SELECT * FROM Usuarios Where Correo=@Correo ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Correo", correo);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            if (table.Rows.Count == 0)
            {
                Console.WriteLine("NOAHY");
                return new JsonResult(new { message = "Usuario no encontrado" }) { StatusCode = 404 };
            }
            DataRow row = table.Rows[0];
            Usuario usuario  = new Usuario
            {
                usuarioID = Convert.ToInt32(row["UsuarioID"]),
                rolID = Convert.ToInt32(row["RolID"]),
                dependenciaID = Convert.ToInt32(row["DependenciaID"]),
                nombre = row["Nombre"].ToString(),
                apellidoPaterno = row["ApellidoPaterno"].ToString(),
                apellidoMaterno = row["ApellidoMaterno"].ToString(),
                constraseña = row["Contraseña"].ToString(),
                correo = row["Correo"].ToString(),
            };
            return new JsonResult(usuario);
        }


        [HttpPost]
        public JsonResult Post(Usuario usuario)
        {
            string query = "EXEC sp_AgregarUsuario @Nombre, @ApellidoPaterno, @ApellidoMaterno, @Correo, @Constraseña, @RolID, @DependenciaID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@Nombre", usuario.nombre);
                    myCommand.Parameters.AddWithValue("@ApellidoPaterno", usuario.apellidoPaterno);
                    myCommand.Parameters.AddWithValue("@ApellidoMaterno", usuario.apellidoPaterno);
                    myCommand.Parameters.AddWithValue("@Correo", usuario.correo);
                    myCommand.Parameters.AddWithValue("@Constraseña", usuario.constraseña);
                    myCommand.Parameters.AddWithValue("@RolID", usuario.rolID);
                    myCommand.Parameters.AddWithValue("@DependenciaID", usuario.dependenciaID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Agrego exito el Usuario!");
        }

        [HttpGet("mostrarUsuarios()")]
        public JsonResult mostrarEventos()
        {
            string query = @"EXEC sp_MotrarUsuarios";
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

        [HttpDelete("{ID}")]
        public JsonResult Delete(int ID)
        {
            string query = "EXEC sp_EliminarUsuario @UsuarioID";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@UsuarioID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader); ;
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Se Elimino con Exito el Usuario");
        }


    }
}
