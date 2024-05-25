using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace PIABD.Controllers
{
    [ApiController]
    public class RecintosController : ControllerBase
    {

        private IConfiguration _configuration;
        public RecintosController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [Route("recintos")]
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
    }
}
