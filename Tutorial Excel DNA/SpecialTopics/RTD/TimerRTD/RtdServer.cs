using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using ExcelDna.Integration;
using ExcelDna.Integration.Rtd;

namespace TimerRTD
{
    [ComVisible(true)]
    public class RtdServer : ExcelRtdServer
    {
        Dictionary<Topic, IDisposable> _dataSources;

        protected override bool ServerStart()
        {
            _dataSources = new Dictionary<Topic, IDisposable>();
            return true;
        }

        protected override object ConnectData(Topic topic, IList<string> topicInfo, ref bool newValues)
        {
            var sourceName = topicInfo.FirstOrDefault();
            if (sourceName == "NOW")
            {
                var source = new NowDataSource();
                source.NewValue += dateTime => topic.UpdateValue($"{dateTime:HH:mm:ss}");
                _dataSources[topic] = source;
                return null; // We could also return an initial value
            }

            if (sourceName == "WAVE")
            {
                // Figure out the other topic parameters, which are strings in the topicInfo list.
                // No error - assume we have two extra topic strings and they are numbers
                double.TryParse(topicInfo[1], out double amplitude);
                double.TryParse(topicInfo[2], out double frequency);
                var source = new WaveDataSource(amplitude, frequency);
                source.NewValue += val => topic.UpdateValue(val);
                _dataSources[topic] = source;
                return null;
            }

            // This is unexpected and an error in our code - the RTD Server is used with our own wrapper functions
            throw new ArgumentOutOfRangeException(nameof(topicInfo), $"Unknown topic {sourceName}");
        }

        protected override void DisconnectData(Topic topic)
        {
            _dataSources[topic].Dispose();
            _dataSources.Remove(topic);
        }

        protected override void ServerTerminate()
        {
            // All topics will already have been Disconnected
            // We can do final clean-up here
        }

    }
}
