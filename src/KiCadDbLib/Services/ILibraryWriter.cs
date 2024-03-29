﻿using System;
using System.Threading.Tasks;
using KiCadDbLib.Models;

namespace KiCadDbLib.Services
{
    public interface ILibraryWriter : IDisposable, IAsyncDisposable
    {
        Task WriteStartLibrary();

        Task WriteEndLibrary();

        Task WritePartAsync(Part part);
    }
}