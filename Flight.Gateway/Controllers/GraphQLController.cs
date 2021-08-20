using System.Threading.Tasks;
using Flight.Contracts;
using HotChocolate;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace Flight.Gateway.Controllers
{
    [Route("graphql")]
    public class GraphQLController : Controller
    {
        private readonly IRequestClient<IGraphQLRequest> _client;
        private readonly ISchema _schema;

        public GraphQLController(IRequestClient<IGraphQLRequest> client, ISchema schema)
        {
            _client = client;
            _schema = schema;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] GraphQLQueryDto query)
        {
            if (query.Query.ToLower().Contains("schema"))
            {
                return Ok(_schema.Print());
            }
            var response = await _client.GetResponse<IGraphQLResponse>(new { query.Query });
            return Ok(response.Message.QueryResult);
        }
    }
}