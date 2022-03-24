using System.Collections.Generic;
using System.Linq;
using Flight.Gateway.Middleware;
using Flight.Gateway.Stitching;
using Flight.Gateway.Types;
using HotChocolate.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types.Descriptors;
using HotChocolate.Types.Descriptors.Definitions;

namespace Flight.Gateway.Configuration
{
    public class DisableIfNullInterceptor : TypeInterceptor
    {
        /// <inheritdoc />
        public override void OnAfterInitialize(
            ITypeDiscoveryContext discoveryContext,
            DefinitionBase? definition,
            IDictionary<string, object?> contextData)
        {
            if (definition is not ObjectTypeDefinition objectTypeDef)
            {
                return;
            }

            foreach (var objectField in objectTypeDef.Fields)
            {
                if (!objectField.Directives.Any(x =>
                        x.Reference is NameDirectiveReference directiveRef &&
                        directiveRef.Name.Value == DirectiveNames.DisableIfNull))
                {
                    continue;
                }

                var middleware = FieldClassMiddlewareFactory.Create<DisableIfNullMiddleware>();
                objectField.MiddlewareDefinitions.Insert(0, new FieldMiddlewareDefinition(middleware));
            }
        }
    }
}