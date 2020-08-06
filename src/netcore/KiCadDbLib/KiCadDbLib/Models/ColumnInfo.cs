﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace KiCadDbLib.Models
{
    public class ColumnInfo : IEquatable<ColumnInfo>
    {
        public ColumnInfo(string header, string path)
        {
            Header = header;
            Path = path;
        }

        public string Header { get; }

        public string Path { get; }

        public ListSortDirection? SortDirection { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as ColumnInfo);
        }

        public bool Equals(ColumnInfo other)
        {
            return other != null &&
                   Header == other.Header &&
                   Path == other.Path;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Header, Path);
        }

        public static bool operator ==(ColumnInfo left, ColumnInfo right)
        {
            return EqualityComparer<ColumnInfo>.Default.Equals(left, right);
        }

        public static bool operator !=(ColumnInfo left, ColumnInfo right)
        {
            return !(left == right);
        }
    }
}
