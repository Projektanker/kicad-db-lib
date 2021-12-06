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
            if (_childs != null)
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

            if (_childs != null)
            {
                sb.Append(')');
            }
        }

        private void AppendName(StringBuilder sb)
        {
            var quotes = IsString || Name!.Length == 0 || Name!.IndexOfAny(new char[] { ' ', '"', '(', ')' }) != -1
                ? "\""
                : string.Empty;

            sb.Append(quotes);
            sb.Append(Name!.Replace("\"", "\\\"", StringComparison.Ordinal));
            sb.Append(quotes);
        }
    }
}