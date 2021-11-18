using System.Collections.Generic;
using System.Text;

namespace KiCad.UnitTest
{
    public partial class SNode
    {
        private readonly List<SNode> _childs = new();

        public SNode(bool isPrimitive)
        {
            IsPrimitive = isPrimitive;
        }

        public SNode(string name, bool isPrimitive = true)
        {
            Name = name;
            IsPrimitive = isPrimitive;
        }

        public SNode(string name, params SNode[] childs)
            : this(childs)
        {
            Name = name;
        }

        public SNode(params SNode[] childs)
        {
            _childs.AddRange(childs);
        }

        public string? Name { get; set; }

        public IReadOnlyList<SNode> Childs => _childs.AsReadOnly();
        public bool IsPrimitive { get; private set; }

        public void Add(SNode node)
        {
            _childs.Add(node);
            IsPrimitive = false;
        }
    }
}