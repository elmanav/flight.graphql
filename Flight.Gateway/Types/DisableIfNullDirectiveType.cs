using Flight.Gateway.Stitching;
using HotChocolate.Types;

namespace Flight.Gateway.Types
{
    public class DisableIfNullDirectiveType : DirectiveType<DisableIfNullDirective>
    {
        protected override void Configure(
            IDirectiveTypeDescriptor<DisableIfNullDirective> descriptor)
        {
            descriptor.Name(DirectiveNames.DisableIfNull).Description("Use for disable next middleware if field is null (e.g. delegate directive).");
            descriptor.Location(DirectiveLocation.FieldDefinition);
            descriptor.Repeatable();
            descriptor.Argument(t => t.Field)
                .Description("The field that is being tested for null.");
        }
    }
}