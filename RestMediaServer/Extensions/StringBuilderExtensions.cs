using System;
using System.Text;

namespace RestMediaServer.Extensions
{
    public static class StringBuilderExtensions
    {
        public static StringBuilder AppendMessage(this StringBuilder sb, string text, Func<string, bool> predicate = null)
        {
            if (predicate != null)
            {
                if (predicate(text))
                {
                    sb.Append(" ");
                    sb.Append(text);
                }
            }
            else
            {
                sb.Append(" ");
                sb.Append(text);
            }

            return sb;
        }
    }
}