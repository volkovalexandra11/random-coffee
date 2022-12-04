using System.Text;

namespace RandomCoffee.QueryBuilder;

public static class CaseConverter
{
    public static string CamelToSnake(string s)
    {
        var res = new StringBuilder();
        var sPos = 0;
        while (sPos < s.Length)
        {
            if (!char.IsUpper(s[sPos]) || sPos == 0)
            {
                res.Append(s[sPos]);
            }
            else
            {
                res.Append('_');
                res.Append(char.ToLower(s[sPos]));
            }
            sPos++;
        }
        return res.ToString();
    }

    public static string SnakeToCanel(string s)
    {
        var res = new StringBuilder();
        var sPos = 0;
        var nextIsUpper = true;
        while (sPos < s.Length)
        {
            if (s[sPos] == '_')
            {
                nextIsUpper = true;
            }
            else if (nextIsUpper)
            {
                res.Append(char.ToUpper(s[sPos]));
                nextIsUpper = false;
            }
            else
            {
                res.Append(s[sPos]);
            }
        }
        return res.ToString();
    }
}