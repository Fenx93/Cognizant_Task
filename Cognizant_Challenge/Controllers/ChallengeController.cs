using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Cognizant_Challenge.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChallengeController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;

        static readonly HttpClient client = new HttpClient();

        public ChallengeController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        private JsonResult DoDBQuery(string query)
        {
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("CognizantAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);

                    myReader.Close();
                    myCon.Close();
                }
            }
            var test = new JsonResult(JsonConvert.SerializeObject(table));
            return test;
        }

         [HttpGet]
         public JsonResult Get()
         {
            string query = @"
                    select ChallengeName from dbo.Challenge";
            return DoDBQuery(query);
         }
        

        [HttpPost]
        public JsonResult Post(Challenge challenge)
        {
            Uri u = new Uri("https://api.jdoodle.com/v1/execute");
            var payload = new Dictionary<string, string>
            {
              {"clientId", "eabfd046983ec926926e5028d153babe"},
              {"clientSecret", "8c3ee8365f8a30f973f215df7481e4e1931c3cb300d7c2ef97042d75ead93931"},
              {"script", challenge.SolutionCode},
              {"language", "java"},
              {"versionIndex", "3"},
            };

            string strPayload = JsonConvert.SerializeObject(payload);
            HttpContent c = new StringContent(strPayload, Encoding.UTF8, "application/json");
            var t = Task.Run(() => SendURI(u, c));
            t.Wait();
            if (t.Result != null)
            {
                var k = t.Result.ReadAsStringAsync().Result;
                char[] delimeterChars = { ',', ':' };
                var words = k.Split(delimeterChars);

                string query = @"
                    insert into dbo.Challenge values
                ('"+challenge.Name+@"','"
                +challenge.TaskName+@"','"
                +challenge.ParticipantName+@"','"
                +challenge.SolutionCode+@"','"
                +words[1] +  @"')";
                DoDBQuery(query);
                return new JsonResult("Submited successfully!");
            }
            return new JsonResult("Failed!");

        }

        static async Task<HttpContent> SendURI(Uri u, HttpContent c)
        {
            HttpContent response = null;
            using (var client = new HttpClient())
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = u,
                    Content = c
                };

                HttpResponseMessage result = await client.SendAsync(request);
                if (result.IsSuccessStatusCode)
                {
                    response = result.Content;
                }
            }
            return response;
        }
    }

}
