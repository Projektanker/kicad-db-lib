using Avalonia.Controls;

namespace KiCadDbLib
{
    public interface IHosted
    {
        Window Host { get; set; }
    }
}