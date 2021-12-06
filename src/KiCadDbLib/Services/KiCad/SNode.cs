using System;
using System.Collections.Generic;
using System.Linq;

namespace KiCadDbLib.Services.KiCad
{
    public partial class SNode
    {
        private List<SNode>? _childs;

        public SNode()
        {
        }

        public SNode(string name, bool isString = false)
        {
            Name = name;
            IsString = isString;
        }

        public SNode(string name, params SNode[] childs)
            : this(childs)
        {
            Name = name;
        }

        public SNode(params SNode[] childs)
        {
            _childs = new(childs);
        }

        public SNode(IEnumerable<SNode>? childs)
        {
            if (childs != null)
            {
                _childs = new(childs);
            }
        }

        public string? Name { get; set; }

        public IReadOnlyList<SNode> Childs
            => (IReadOnlyList<SNode>?)_childs ?? Array.Empty<SNode>();

        public bool IsString { get; set; }

        public void Add(SNode node)
        {
            _childs ??= new();
            _childs.Add(node);
        }

        public void Insert(int index, SNode node)
        {
            _childs ??= new();
            _childs.Insert(index, node);
        }

        public void Remove(SNode node)
        {
            _childs?.Remove(node);
        }

        public SNode Clone()
        {
            var childs = _childs
                ?.Select(child => child.Clone());

            return new SNode(childs)
            {
                Name = Name,
                IsString = IsString,
            };
        }
    }
}