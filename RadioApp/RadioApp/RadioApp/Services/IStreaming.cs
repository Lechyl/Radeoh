using System;
using System.Collections.Generic;
using System.Text;

namespace RadioApp.Services
{
    public interface IStreaming
    {
        void Play();
        void Stop();
        string DataSource { get; set; }

    }
}
