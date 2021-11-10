using System.Linq;
using System.Threading.Tasks;
using Flight.AircraftHangar.Models;
using HotChocolate;
using HotChocolate.Types;
using JetBrains.Annotations;

namespace Flight.AircraftHangar
{
    public class Query
    {
        #region Public

        public IQueryable<Aircraft> GetAircrafts([Service] AircraftDbContext context)
        {
            return context.Aircrafts;
        }

        public async Task<Aircraft?> GetAircraft(string regNumber, AircraftBatchDataLoader dataLoader)
        {
            return await dataLoader.LoadAsync(regNumber);
        }

        // test input any parameter
        public Aircraft GetAircraftByAny(ValueInput value)
        {
            return new Aircraft(){Id = 1, ICAOCode = "Tst"};
        }
        // public Aircraft GetAircraftByAny(TextValueInput data)
        // {
        //     return new Aircraft(){Id = 1, ICAOCode = "Tst"};
        // }

        #endregion

        public class ValueInput
        {
            public string TypeName { get; init; }
            [GraphQLType(typeof(NonNullType<AnyType>))]
            public object Value { get; init; }
        }
    }

    class ValueAnyType : AnyType
    {
        /// <inheritdoc />
        public ValueAnyType() : base("AnyValue")
        {
        }
    }
}