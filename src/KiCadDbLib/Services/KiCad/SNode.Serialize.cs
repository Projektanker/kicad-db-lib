using System;
using System.Text;

namespace KiCadDbLib.Services.KiCad
{
    public partial class SNode
    {
        public override string ToString()
        {
            var sb = new StringBuilder();
            ToString(sb);
            return sb.ToString();
        }

        private void ToString(StringBuilder sb)
        {
            if (!IsPrimitive)
            {
                sb.Append('(');
            }

            var hasName = Name is not null;
            if (hasName)
            {
                AppendName(sb);
            }

            for (var i = 0; i < Childs.Count; i++)
            {
                if (hasName || i > 0)
                {
                    sb.Append(' ');
                }

                Childs[i].ToString(sb);
            }

            if (!IsPrimitive)
            {
                sb.Append(')');
            }
        }

        private void AppendName(StringBuilder sb)
        {
            var quotes = Name!.IndexOfAny(new char[] { ' ', '"', '(', ')' }) != -1 || Name.Length == 0 || IsString
                ? "\""
                : string.Empty;

            sb.Append(quotes);
            sb.Append(Name.Replace("\"", "\\\"", StringComparison.Ordinal));
            sb.Append(quotes);
        }
    }
}