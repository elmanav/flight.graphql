using HotChocolate.Data;
using HotChocolate.Data.Filters;
using HotChocolate.Data.Filters.Expressions;

namespace Flight.AircraftHangar.Filters
{
    public class CustomFilteringConvention : FilterConvention
    {
        protected override void Configure(IFilterConventionDescriptor descriptor)
        {
            descriptor.AddDefaults();
            descriptor.Operation(DefaultFilterOperations.Contains).Name("contains");
            descriptor.AddProviderExtension(
                new QueryableFilterProviderExtension(
                    x => x.AddFieldHandler<QueryableStringInvariantEqualsHandler>()));
        }
    }
}