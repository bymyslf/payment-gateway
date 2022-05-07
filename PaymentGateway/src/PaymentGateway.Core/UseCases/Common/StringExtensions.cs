namespace PaymentGateway.Core.UseCases.Common;

internal static class StringExtensions
{
    public static T ToEnum<T>(this string value)
        where T : Enum
        => (T)Enum.Parse(typeof(T), value, true);

    public static string Mask(this string value)
    {
        Span<char> temp = stackalloc char[value.Length];

        value.CopyTo(temp);
        
        for (var i = 4; i < temp.Length; i++)
            temp[i] = '#';

        return new string(temp);
    }
}