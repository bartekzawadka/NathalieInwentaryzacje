using System;
using NathalieInwentaryzacje.ViewModels.Common;

namespace NathalieInwentaryzacje.ViewModels.Main
{
    public class ErrorViewModel : ScreenBase
    {
        public string Message { get; }

        public ErrorViewModel(string message)
        {
            Message = message;
        }

        public ErrorViewModel(Exception ex) : this(ex?.Message)
        {
        }
    }
}
