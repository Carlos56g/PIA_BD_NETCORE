using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace PIABD.Controllers
{
    [Route("butacas")]
    [ApiController]
    public class ButacasController : ControllerBase
    {
        private IConfiguration _configuration;
        public ButacasController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("{ID}")]
        public JsonResult get(int ID)
        {
            string query = "select * from Butacas WHERE RecintoID=@RecintoID";
            DataTable table = new DataTable();
            string SqlDatasource = _configuration.GetConnectionString("eventosUanl_bd");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(SqlDatasource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@RecintoID", ID);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                }
            }
            return new JsonResult(table);
        }

    }
}
