using Microsoft.AppCenter.Crashes;
using System;
using System.Collections.Generic;
using System.Text;

namespace MarvelApp.Model
{
    public static class ExceptionExtension
    {
        public static void LogException(this Exception exception)
        {
            Crashes.TrackError(exception);
        }
    }
}
