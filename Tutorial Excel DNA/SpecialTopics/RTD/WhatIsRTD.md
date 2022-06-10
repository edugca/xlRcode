# What is RTD?

Excel has a 'Real-Time Data' (RTD) feature lets us push data updates into an Excel sheet.
A typical example for RTD is to provide a real-time stock ticker feed, but this powerful feature makes various interactions with the Excel sheet and calculation engine possible.

You might already have noticed the `=RTD(...)` function on the list of worksheet functions, or seen me describe some Excel-DNA feature as 'RTD-based'. 
In this tutorial I'll explain what RTD is, how works under the covers and give an example of adding it to an Excel-DNA add-in.

I can think of some reasons why the RTD feature of Excel is not so well known:
* There are no built-in RTD data sources that ship with Office. There are only available from as part of third-party add-ins.
* RTD Servers cannot be created in VBA, so they need another environment like C++ or .NET to create.
* The COM-based nature of RTD means there are a few things to learn and take care with when making RTD Servers.
* Registration of RTD server normally requires administrator permissions

Despite the challenges in getting to know RTD, it is a very powerful feature of Excel that is closely integrated into the Excel calculation engine. Hence, it provides a foundation on which various high-level features can be built. But let me not run ahead of myself.

## What is an RTD Server?

An RTD data source defined by code in a COM(\*) library that implements an 'RTD Server'. An RTD Server supports the interaction between Excel and the data source by implementing the `IRtdServer` interface. An RTD Server will then expose its data as one or more 'Topics', each of which is defined by an array of strings passed to the RTD Server when connecting to a new topic, and in return provides a stream of values back to Excel.

> (*) What is COM?
>
> This is separate essay question... In brief, the Component Object Model (COM) is a standard that describes how software components can interact. The Excel COM object model is the set of interfaces that VBA uses to interact with Excel - this includes objects like `Application` and `Workbook`. Excel-DNA add-ins can also use the COM object model to interact with Excel. COM libraries are .dll libraries that work according to the COM standard. So in the context of RTD, it means that an RTD Server must follow these standard rules, so that Excel knows how to interact with it.

Being a COM class, an RTD Server is identified by its COM 'ProgId'. These strings normally have a dotted form like 'MyCompany.RtdServer' or 'MyCompany.DataLink'. Behind the scenes there is also a Guid (a 'globally unique identifier') called the COM 'ClsId' for the RTD server, which normally looks like a long hexadecimal number 'B73B68BD-9DD0-4E9D-82A1-E9B2798AF8E5'.

The combination of the `IRtdServer` interface and the `IRTDUpdateEvent` callback interface form the COM-based specification for how RTD Servers will iteract with Excel.

The `IRtdServer` interface has these members:
* `ServerStart` - create a new connection to the server (before any topics are connected)
* `ServerTerminate` - end the connection to the server (after all topics are disconnected)
* `ConnectData`- create a new topic according to the given topic strings
* `DisconnectData` - notify the server that a topic is no longer connected to Excel
* `RefreshData` - called to fetch updates for all topics
* `Heartbeat` - check that the server is still running.

The helper interface to notify Excel of any updates is called `IRTDUpdateEvent`, and is passed to the server in the `ServerStart` call.
The server then notifies Excel that new data is ready, with a call to `IRTDUpdateEvent.UpdateNotify`.

## How does Excel interact with an RTD Server?

We can now trace the interaction sequence between Excel and an RTD Server.

A basic call sequence might look like this
![RTD Call Sequence](https://user-images.githubusercontent.com/414659/105495810-4623dc00-5cc5-11eb-8b44-03dc73caa6eb.png)

Some notes:
* The RTD mechanism is started by a cell formula that makes an `RTD` function call.
* Excel then locates the RTD server with the COM activation. While COM allows the RTD Server to be out-of-process or even on another machine, I recommend, and Excel-DNA only supports, RTD servers that run in-process as part of the Excel add-in.
* Excel calls `ServerStart` when an RTD Server is first activated, and `ConnectData` for each topic subscribed to.
* The RTD Server might in turn connect to a data service that is local or remote, and which is polled by the RTD server or provides streaming updates.
* The `UpdateNotify` call tell Excel that there is some data available from the RTD Server, but does not provide the individual data items. The means the RTD Server does not have to notify Excel for each individual data item.
* When Excel is ready (the exact timing of this can be configured with the `ThrottleInterval` setting) Excel will fetch all updated values from the RTD Server in a single call.
* Some care needs to be taken with the `UpdateNotify` callbacks to Excel. (This is the shaded part in the diagram.) Callbacks must be made on the main Excel thread, and must not be too frequent. Excel-DNA provides a base implementation of an RTD Server that assists in correct working of this part of the process.
* Finally, Excel also provides `DisconnectData` and `ServerTerminate` calls to allow clean-up for a topic and the server respectively.

These are some of the features of RTD that make it a uniquely powerful update feature for Excel.
* High throughput to support many data items, and high update rates
* Updates do not interfere with the user's interaction with Excel
* Reliable tracking of formula locations, with notification when a data item is no longer referenced

Alternative approaches like using the COM object model to update a workbook are far more limited.

## ExcelRtdServer helper class in Excel-DNA

The Excel-DNA library contains a base class that can ease the implementation of an RTD server, in particular helping with the following:

* It allows RTD servers to be used without prior admin registration of the COM library. This is done by making the COM registrations at runtime, and using the user keys in the registry if access to the machine keys are not available.

* It provides internal data management for the updated topic values for the duration between the update call and the point where Excel reads the values with the `RefreshData` call. This includes the conversion of data types if need be, allowing long strings, errors and also (in Dynamic-Arrays versions of Excel) large arrays to be used in the RTD server as in normal UDFs.

* It allowing the RTD Server topics to be updated topics from any thread, at any frequency. The callbacks to Excel are managed and called on the correct thread internally.

In addition, the add-in can wrap the `=RTD(...)` call inside a user-friendly function, so instead of having the formula `=RTD("MyAddIn.RTD",,"MSFT")` the formula can be something like `=GetLiveData("MSFT")`.

## Build a basic RTD Server

When implementing an RTD Server there are additional helpers in Excel-DNA and .NET libraries like the 'Reactive Extensions' to make it easier. But for this introduction I will show a simple implementation where the data source is just an object that has a timer to update the data values.

We build an RTD server that provides two data topics:
* A real-time ticking clock
* A 'wave' of values changing in a cycle

We've arrange the sample into three parts:
* **DataSources** - does not depend on Excel or Excel-DNA, but encapsulates the data provider or, connection to back-end services.
* **RTDServer** - implments the RTD Server code which interprets manages topic information and links the add-in to the data sources.
* **Functions** - wrapper functions which make the RTD calls and present a friendly interface to the user.

We'll look at each one in turn.

### The data source

Our simple data source is implemented with a timer which will fire an event with the new value.
An actual implementation might set up a link to a remote data source, and when an incoming message is received it fires an update event. Or it might make an asynchronous call, and notify when the call is complete.

For the example we have two similar data sources - I'll show the `NowDataSource`

```cs

    // Represents a streaming source of 'current time' updates, 
    // implementd by a simple timer
    public class NowDataSource: IDisposable
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

```

### The RTD Server

The Timer example's `RtdServer` class derives from the Excel-DNA base class for RTD servers, called `ExcelRtdServer`.
The bulk of the implementation is in the `ConnectData` method, which handles the request to connect a new RTD topic.

```cs
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
                double amplitude = 1;
                double frequency = 1;
                if (topicInfo.Count >= 3)
                {
                    double.TryParse(topicInfo[1], out amplitude);
                    double.TryParse(topicInfo[2], out frequency);
                }
                var source = new WaveDataSource(amplitude, frequency);
                source.NewValue += val => topic.UpdateValue(val);
                _dataSources[topic] = source;
                return null;
            }

            // This is unexpected and an error in our code - the RTD Server is used with our own wrapper functions
            throw new ArgumentOutOfRangeException(nameof(topicInfo), $"Unknown topic {sourceName}");
        }
```

Note that we don't need any registration step for the RTD server - Excel-DNA will take care of this at runtime.

### The wrapper functions

Finally we define two user-defined functions that wrap the internal RTD call. The call the Excel-DNA RTD wrapper (which handles the runtime registration) by calling `XlCall.RTD`. These functions could also be marked up with the usual `ExcelFunction` and `ExcelArgument` attributes to provide descriptions etc.

```cs
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
```

We can now call these functions from an Excel sheet as

```
=TimerNow()
=TimerWave(1, 0.5)
```

### Application.RTD.ThrottelInterval

Excel has a setting that affects the rate at which values are refreshed from an RTD server.
The default value is 2000 ms, which means available data from an RTD Server will be fetched at this interval.
This setting can most easily be updated by opening the VBA interface, and entering the following into the immediate window:

```vba
Application.RTD.ThrottleInterval = 500
```

The will set the interval to 500 ms, allowing a good balance between update performance and system resources (a too short interval can lead to instability if Excel cannot recalulate and process the updates fast enough.


## References

#### Excel-DNA samples

The **RtdClocks** sample shows a number of implementation mechanisms 
https://github.com/Excel-DNA/Samples/tree/master/RtdClocks

There is also a Visual Basic version of one of the RtdClocks projects at https://github.com/Excel-DNA/Samples/tree/master/RtdClocksVB/RtdClockVB-ExcelRtdServer

#### Microsoft references

For Excel 365 versions there were important [performance improvements](https://docs.microsoft.com/en-us/office/vba/excel/concepts/excel-performance/excel-performance-and-limit-improvements) made in early 2020. These include supporting RTD as a thread-safe function and improving the internal update performance in Excel. This will allow future versions of Excel-DNA to support thread-safe RTD wrapper functions.

Some older information can be found here:
* https://docs.microsoft.com/en-us/office/troubleshoot/excel/set-up-realtimedata-function
* [Real-Time Data: Frequently asked questions](https://docs.microsoft.com/en-us/previous-versions/office/developer/office-xp/aa140060(v=office.10)) (from 2001!)


