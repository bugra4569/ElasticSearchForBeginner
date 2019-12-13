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
    public class TheathersController : ControllerBase
    {
        private static readonly ConnectionSettings connSettings =
         new ConnectionSettings(new Uri("http://localhost:9200/"))
                         .DefaultIndex("theather_ind")
            .DefaultMappingFor(typeof(Theathers), m => m.IndexName("theather_ind"));
        private static readonly ElasticClient elasticClient = new ElasticClient(connSettings);
        [HttpPost]
        public void InsertTheathers([FromBody]Theathers theather)
        {
            //elasticClient.DeleteIndex("theather_ind");

            if (!elasticClient.Indices.Exists("theather_ind").Exists)
            {
                var indexSettings = new IndexSettings();
                indexSettings.NumberOfReplicas = 1;
                indexSettings.NumberOfShards = 3;


                var createIndexDescriptor = new CreateIndexDescriptor("theather_ind")
               .Mappings(ms => ms
                               .Map<Theathers>(m => m.AutoMap())
                        )
                .InitializeUsing(new IndexState() { Settings = indexSettings })
                .Aliases(a => a.Alias("TheatherAlias"));

                var response = elasticClient.Indices.Create(createIndexDescriptor);
            }
            //Insert Data           

            elasticClient.Index<Theathers>(theather, idx => idx.Index("theather_ind"));
        }
        [HttpGet("{id}")]
        public async Task<List<Theathers>> Get(int id)
        {
            var response = await elasticClient.SearchAsync<Theathers>(p => p
                .Query(q => q
                    .Match(m => m
                        .Field(f => f.ID)
                            .Query(id.ToString())
                            .Operator(Operator.And)
                            )
                    )
                    .Sort(s => s.Descending(f => f.ID))
            );

            var result = new List<Theathers>();
            foreach (var document in response.Documents)
            {
                result.Add(document);
            }
            return result;

        }
        
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] Theathers theather)
        {
            elasticClient.UpdateAsync<Theathers>(id, u =>u.Doc(theather));
        }
        [HttpGet]
        public async Task<List<string>> GetAllTheathers()
        {
            var response = await elasticClient.SearchAsync<Theathers>(p => p
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
            elasticClient.DeleteAsync<Theathers>(id);
        }
    }
}