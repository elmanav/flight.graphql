using System.Threading.Tasks;
using Flight.Contracts;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Gateway.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly IRequestClient<IGraphQLRequest> _client;
        
        public GraphQLController(IRequestClient<IGraphQLRequest> client)
        {
            _client = client;
        }
        
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQueryDto query)
        {

            var response = await _client.GetResponse<IGraphQLResponse>(new { Query = query.Query });
           return Ok(response.Message.QueryResult);
        }
    }
}