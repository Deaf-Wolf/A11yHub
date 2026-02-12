using NAudio.CoreAudioApi;
using NAudio.Wave;

namespace A11yHub;

public class AudioSplitterUtil
{
    public class OutputDevice
    {
        public MMDevice Device { get; set; }
        public WasapiOut Player { get; set; }

        public OutputDevice(MMDevice device)
        {
            Device = device;
            Player = new WasapiOut(device, AudioClientShareMode.Shared, false, 200);
        }
    }

    private WasapiLoopbackCapture capture;
    private List<BufferedWaveProvider> buffers = new List<BufferedWaveProvider>();


    public string GetDefaultOutputDevice()
    {
        var enumerator = new MMDeviceEnumerator();
        var defaultDevice = enumerator.GetDefaultAudioEndpoint(DataFlow.Render, Role.Multimedia);

        return defaultDevice.FriendlyName;
    }


    /// <summary>
    /// Gets all avaiable Output Devices
    /// </summary>
    /// <returns>List<OutputDevice></returns>
    public List<OutputDevice> GetOutputDevices()
    {
        var enumerator = new MMDeviceEnumerator();
        var outputs = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active);
        var outputDevices = new List<OutputDevice>();



        foreach (var output in outputs)
        {
            outputDevices.Add(new OutputDevice(output));
        }


        return outputDevices;
    }

    /// <summary>
    /// Routes Audio of Source to Selected Outputs.
    /// </summary>
    /// <param name="sourceDevice"></param>
    /// <param name="outputDevices"></param>
    public void RouteAudioToDevices(MMDevice sourceDevice, List<OutputDevice> outputDevices)
    {
        StopRouting();
        if (outputDevices.Count == 0) return;

        capture = new WasapiLoopbackCapture(sourceDevice);
        buffers.Clear();

        foreach (var outputDevice in outputDevices)
        {
            if (outputDevice.Device.ID == sourceDevice.ID) continue;

            var buffer = new BufferedWaveProvider(capture.WaveFormat)
            {
                DiscardOnBufferOverflow = true
            };
            buffers.Add(buffer);

            outputDevice.Player?.Dispose();
            outputDevice.Player = new WasapiOut(outputDevice.Device, AudioClientShareMode.Shared, false, 200);
            outputDevice.Player.Init(buffer);
            outputDevice.Player.Play();
        }

        capture.DataAvailable += (s, e) =>
        {
            foreach (var buffer in buffers)
            {
                buffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
            }
        };

        capture.StartRecording();
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void StopRouting()
    {
        if (capture != null)
        {
            capture.StopRecording();
            capture.Dispose();
            capture = null;
        }

        foreach (var buffer in buffers)
        {
            buffer.ClearBuffer();
        }
        buffers.Clear();
    }
}