using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PIABD.Models;
using System.Data;
using System.Data.SqlClient;

namespace PIABD.Controllers
{
    [Route("roles")]
    [ApiController]

    public class RolesController : ControllerBase
        {
            private IConfiguration _configuration;
            public RolesController(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            [HttpGet]

            public JsonResult get()
            {
                string query = "select * from Roles";
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

