using System.Text;
using System.Text.Json;

namespace PaymentGateway.Api.Json;

//From here: https://github.com/benfoster/problem-details-demo/blob/eee86ee286e8db09612ca15a4c39e436763fa64e/src/ProblemDetailsDemo/SnakeCaseNamingPolicy.cs#L6
public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static SnakeCaseNamingPolicy Instance { get; } = new();

    public override string ConvertName(string name) => name.ToSnakeCase();
}

internal static class StringExtensions
{
    private enum SnakeCaseState
    {
        Start,
        Lower,
        Upper,
        NewWord
    }

    public static string ToSnakeCase(this string s)
    {
        const char separator = '_';
        if (string.IsNullOrWhiteSpace(s))
        {
            return s;
        }

        var sb = new StringBuilder();
        var state = SnakeCaseState.Start;

        for (var i = 0; i < s.Length; i++)
        {
            if (s[i] == ' ')
            {
                if (state != SnakeCaseState.Start)
                {
                    state = SnakeCaseState.NewWord;
                }
            }
            else if (char.IsUpper(s[i]))
            {
                switch (state)
                {
                    case SnakeCaseState.Upper:
                        var hasNext = (i + 1 < s.Length);
                        if (i > 0 && hasNext)
                        {
                            var nextChar = s[i + 1];
                            if (!char.IsUpper(nextChar) && nextChar != separator)
                            {
                                sb.Append(separator);
                            }
                        }
                        break;
                    case SnakeCaseState.Lower:
                    case SnakeCaseState.NewWord:
                        sb.Append(separator);
                        break;
                }

                sb.Append(char.ToLowerInvariant(s[i]));
                state = SnakeCaseState.Upper;
            }
            else if (s[i] == separator)
            {
                sb.Append(separator);
                state = SnakeCaseState.Start;
            }
            else
            {
                if (state == SnakeCaseState.NewWord)
                {
                    sb.Append(separator);
                }

                sb.Append(s[i]);
                state = SnakeCaseState.Lower;
            }
        }

        return sb.ToString();
    }
}