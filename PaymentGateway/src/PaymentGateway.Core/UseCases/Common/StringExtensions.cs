namespace PaymentGateway.Core.UseCases.Common;

internal static class StringExtensions
{
    public static T ToEnum<T>(this string value)
        where T : Enum
        => (T)Enum.Parse(typeof(T), value, true);
}