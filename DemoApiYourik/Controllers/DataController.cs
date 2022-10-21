using DemoApiYourik.Models;
using DemoApiYourik.Models.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace DemoApiYourik.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(AddDataForm form)
        {
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestPagination;Integrated Security=True";

                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "Insert Into TestData ([Value]) VALUES (@Value);";
                    command.Parameters.AddWithValue("Value", form.Value);

                    connection.Open();
                    int rows = command.ExecuteNonQuery();
                    
                    if (rows != 1)
                        return BadRequest();

                    return NoContent();
                }
            }
        }

        [HttpGet("{pageId}")]
        public IActionResult Get(int? pageId)
        {
            List<DataDto> datas = new List<DataDto>();
            //"SP_GetHistory"
            using (SqlConnection connection = new SqlConnection())
            {
                connection.ConnectionString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=TestPagination;Integrated Security=True";

                using(SqlCommand command = connection.CreateCommand())
                {
                    command.CommandText = "SP_GetHistory";
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter pPageCount = command.CreateParameter();
                    pPageCount.ParameterName = "PageCount";
                    pPageCount.Value = -1;
                    pPageCount.Direction = ParameterDirection.InputOutput;
                    command.Parameters.Add(pPageCount);

                    if (pageId.HasValue)
                        command.Parameters.AddWithValue("Page", pageId.Value);
                    connection.Open();

                    using (SqlDataReader dataReader = command.ExecuteReader())
                    {
                        while(dataReader.Read())
                        {
                            datas.Add(new DataDto() { Id = (int)dataReader["Id"], Value = (string)dataReader["Value"], CreatedDate = (DateTime)dataReader["CreatedDate"] });
                        }
                    }

                    int pageCount = (int)command.Parameters["PageCount"].Value;

                    return Ok(new DataResult() { PageCount = pageCount, Datas = datas });
                }
            }
        }
    }
}
