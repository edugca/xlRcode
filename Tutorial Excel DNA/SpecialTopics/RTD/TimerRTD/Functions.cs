using ExcelDna.Integration;

namespace TimerRTD
{
    public static class Functions
    {
        public static object TimerNow()
        {
            return XlCall.RTD("TimerRTD.RtdServer", null, "NOW");
        }

        public static object TimerWave(double amplitude, double frequency)
        {
            return XlCall.RTD("TimerRTD.RtdServer", null, "WAVE", amplitude.ToString(), frequency.ToString());
        }
    }
}
