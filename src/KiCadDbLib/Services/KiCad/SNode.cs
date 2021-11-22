using System.Collections.Generic;
using System.Linq;

namespace KiCadDbLib.Services.KiCad
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

        private SNode(string? name, bool isPrimitive, IEnumerable<SNode> childs)
        {
            Name = name;
            IsPrimitive = isPrimitive;
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

        public void Insert(int index, SNode node)
        {
            _childs.Insert(index, node);
            IsPrimitive = false;
        }

        public void Remove(SNode node)
        {
            _childs.Remove(node);
        }

        public SNode Clone()
        {
            var childs = _childs
                .Select(child => child.Clone());

            return new SNode(Name, IsPrimitive, childs);
        }
    }
}