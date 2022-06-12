using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace xlRcode
{
    // Represents a streaming source of 'current time' updates, 
    // implementd by a simple timer
    public class NowDataSource : IDisposable
    {
        // This event 'publishes' new values of 'Now'
        public event Action<DateTime> NewValue;

        // The backing Timer object - it fires from a ThreadPool thread
        readonly Timer _timer;

        public NowDataSource()
        {
            _timer = new Timer(100);
            _timer.Elapsed += (s, e) => NewValue(DateTime.Now);
            _timer.Start();
        }

        // We use the IDisposable pattern to clean up any 'data source' resources
        // It's not really needed in this case, but shows where a clean-up step might appear
        public void Dispose()
        {
            _timer.Dispose();
        }
    }

    public class WaveDataSource : IDisposable
    {
        // Publish an event that 'publishes' new random values
        public event Action<double> NewValue;

        double _amplitude;
        double _frequency;
        DateTime _start;
        Timer _timer;

        public WaveDataSource(double amplitude, double frequency)
        {
            _amplitude = amplitude;
            _frequency = frequency;
            _start = DateTime.UtcNow;
            _timer = new Timer(100);
            _timer.Elapsed += (s, e) => NewValue?.Invoke(CurrentValue());
            _timer.Start();
        }

        double CurrentValue()
        {
            double t = (DateTime.UtcNow - _start).TotalSeconds;
            return _amplitude * Math.Sin(t * 2 * Math.PI * _frequency);
        }

        public void Dispose()
        {
            _timer.Dispose();
        }
    }
}
