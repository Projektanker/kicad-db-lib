using System.Text;

namespace KiCad.UnitTest
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
                if (Name.IndexOfAny(new char[] { ' ', '"', '(', ')' }) != -1 || Name.Length == 0)
                {
                    sb.Append('"').Append(GetEscapcedName()).Append('"');
                }
                else
                {
                    sb.Append(Name);
                }
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

        private string GetEscapcedName()
        {
            return Name.Replace("\"", "\\\"");
        }
    }
}