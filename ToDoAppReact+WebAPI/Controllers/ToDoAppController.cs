using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace ToDoAppReact_WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoAppController : Controller
    {
        private IConfiguration _configuration;
        public ToDoAppController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        /// <summary>
        /// This method is used to get the notes present in the table
        /// </summary>
        /// <returns>will return the JSON format of the content present in the table</returns>
        [HttpGet]
        [Route("getnotes")]
        public JsonResult GetNotes()
        {
            string query = "select * from notes";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppConn");
            SqlDataReader myReader;
            using (SqlConnection mycon = new SqlConnection(sqlDataSource))
            {
                mycon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, mycon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    mycon.Close();
                }
            }
            return new JsonResult(table);
        }
        /// <summary>
        /// This mehtod is for adding the notes i.e., HttpPost
        /// </summary>
        /// <param name="newNotes"></param>
        /// <returns>Will returns the string i.e., Added Successfully</returns>
        [HttpPost]
        [Route("addnotes")]
        public JsonResult AddNotes([FromForm]string newNotes) 
        {
            string query = "Insert Into notes values(@newNotes)";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppConn");
            SqlDataReader myReader;
            using(SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query,myCon))
                {
                    myCommand.Parameters.AddWithValue("@newNotes", newNotes);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Succesfully");
        }

        /// <summary>
        /// This method is for the deletion of the added notes
        /// </summary>
        /// <param name="id"> id will be passed in the method parameter</param>
        /// <returns>Will return the simple string i.e., deleted succesfully</returns>
        [HttpDelete]
        [Route("deletenotes")]
        public JsonResult DeleteNotes(int id)
        {
            string query = "Delete from notes where id = @id";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("todoAppConn");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@id", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Succesfully");
        }
    }
}
