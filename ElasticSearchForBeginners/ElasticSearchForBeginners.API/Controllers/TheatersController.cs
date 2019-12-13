using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ElasticSearchForBeginners.API.Models;
using Microsoft.AspNetCore.Mvc;
using Nest;


namespace ElasticSearchForBeginners.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TheatersController : ControllerBase
    {
        private static readonly ConnectionSettings connSettings =
         new ConnectionSettings(new Uri("http://localhost:9200/"))
                         .DefaultIndex("theater_ind")
            .DefaultMappingFor(typeof(Theaters), m => m.IndexName("theater_ind"));
        private static readonly ElasticClient elasticClient = new ElasticClient(connSettings);
        [HttpPost]
        public void Inserttheaters([FromBody]Theaters theater)
        {
            //elasticClient.DeleteIndex("theater_ind");

            if (!elasticClient.Indices.Exists("theater_ind").Exists)
            {
                var indexSettings = new IndexSettings();
                indexSettings.NumberOfReplicas = 1;
                indexSettings.NumberOfShards = 3;


                var createIndexDescriptor = new CreateIndexDescriptor("theater_ind")
               .Mappings(ms => ms
                               .Map<Theaters>(m => m.AutoMap())
                        )
                .InitializeUsing(new IndexState() { Settings = indexSettings })
                .Aliases(a => a.Alias("theaterAlias"));

                var response = elasticClient.Indices.Create(createIndexDescriptor);
            }
            //Insert Data           

            elasticClient.Index<Theaters>(theater, idx => idx.Index("theater_ind"));
        }
        [HttpGet("{id}")]
        public async Task<List<Theaters>> Get(int id)
        {
            var response = await elasticClient.SearchAsync<Theaters>(p => p
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.ID)
                            .Query(id.ToString())
                            .Operator(Operator.And)
                            )
                    )
                    .Sort(s => s.Descending(f => f.ID))
            );

            var result = new List<Theaters>();
            foreach (var document in response.Documents)
            {
                result.Add(document);
            }
            return result;

        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Theaters theater)
        {
            elasticClient.UpdateAsync<Theaters>(id, u =>u.Doc(theater));
        }
        [HttpGet]
        public async Task<List<string>> GetAlltheaters()
        {
            var response = await elasticClient.SearchAsync<Theaters>(p => p
                 //.Source(f=>f.Includes(p2=>p2.Field(f2=>f2.message)))                  
                 .Query(q => q
                 .MatchAll()
                 )
            );
            var result = new List<string>();
            foreach (var document in response.Documents)
            {
                result.Add(document.Name);
                result.Add(document.ID.ToString());
            }
            return result.Distinct().ToList();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            elasticClient.DeleteAsync<Theaters>(id);
        }
    }
}