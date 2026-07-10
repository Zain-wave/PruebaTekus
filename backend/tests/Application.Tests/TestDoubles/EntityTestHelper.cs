namespace PruebaTekus.Application.Tests.TestDoubles;

internal static class EntityTestHelper
{
    public static void SetProperty(object target, string propertyName, object? value)
    {
        var property = target.GetType().GetProperty(propertyName)
            ?? throw new InvalidOperationException($"Property '{propertyName}' not found on {target.GetType().Name}.");

        property.SetValue(target, value);
    }
}
