using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using DynamicData.Binding;

namespace KiCadDbLib.Models
{
    public class ColumnInfo : IEquatable<ColumnInfo>
    {
        public ColumnInfo(string headerAndPath)
            : this(headerAndPath, headerAndPath)
        {
        }

        public ColumnInfo(string header, string path)
        {
            Header = header;
            Path = path;
        }

        public string Header { get; }

        public string Path { get; }

        public SortDirection? SortDirection { get; set; }

        public static bool operator !=(ColumnInfo left, ColumnInfo right)
        {
            return !(left == right);
        }

        public static bool operator ==(ColumnInfo left, ColumnInfo right)
        {
            return EqualityComparer<ColumnInfo>.Default.Equals(left, right);
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return Equals(obj as ColumnInfo);
        }

        /// <inheritdoc/>
        public bool Equals(ColumnInfo other)
        {
            return other != null &&
                   Header == other.Header &&
                   Path == other.Path;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Path);
        }
    }
}
