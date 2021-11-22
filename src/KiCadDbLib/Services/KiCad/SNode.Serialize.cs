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
            var isQuoted = Name!.StartsWith('"') && Name.Length >= 2;

            var name = isQuoted
                ? Name[1..^1]
                : Name;

            if (name!.IndexOfAny(new char[] { ' ', '"', '(', ')' }) != -1 || name.Length == 0)
            {
                sb.Append('"')
                    .Append(name.Replace("\"", "\\\"", StringComparison.Ordinal))
                    .Append('"');
            }
            else
            {
                sb.Append(isQuoted ? "\"" : string.Empty)
                    .Append(name)
                    .Append(isQuoted ? "\"" : string.Empty);
            }
        }
    }
}