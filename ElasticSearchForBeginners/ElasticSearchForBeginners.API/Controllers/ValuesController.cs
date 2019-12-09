using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elasticsearch.Net;
using ElasticSearchForBeginners.API.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;

namespace ElasticSearchForBeginners.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {

            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {

        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private void CreateNewIndex() {
            var createIndexDescriptor = new CreateIndexDescriptor("BugraBooksHistory").Map(m => m.AutoMap<Books>());
            createIndexDescriptor.Aliases(a => a.Alias("Books"));
            Uri node = new Uri(@"http://localhost:9200/");
            ConnectionSettings connectionSettings = new ConnectionSettings(node);
            ElasticClient client = new ElasticClient(connectionSettings);
        var request=     client.Indices.Create(createIndexDescriptor);


        }
    }
}
