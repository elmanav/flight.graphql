using System;

namespace Flight.Contracts
{
    public interface IGraphQLRequest
    {
        string Query { get; }
    }
}