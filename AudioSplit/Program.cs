using NAudio.Wave;
using NAudio.CoreAudioApi;

var enumerator = new MMDeviceEnumerator();

var capture = new WasapiLoopbackCapture(); // Uses default audio out automatically

// Get output devices
var outputs = enumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active).ToList();
var output1 = outputs[1]; // Kopfhörer Laptop
var output2 = outputs[2]; // Kopfhörer Docking

Console.WriteLine($"System audio Redirected to: \n - {output1.DeviceFriendlyName} \n - {output2.DeviceFriendlyName} "   );

// Capture audio
var player1 = new WasapiOut(output1, AudioClientShareMode.Shared, false, 200);
var player2 = new WasapiOut(output2, AudioClientShareMode.Shared, false, 200);

var buffer = new BufferedWaveProvider(capture.WaveFormat);

capture.DataAvailable += (s, e) => {
    buffer.ClearBuffer(); // Clear old data first
    buffer.AddSamples(e.Buffer, 0, e.BytesRecorded);
};


player1.Init(buffer);
player2.Init(buffer);

capture.StartRecording();
player1.Play();
player2.Play();

Console.WriteLine("Routing... Press key to stop");
Console.ReadKey();

capture.StopRecording();
player1.Stop();
player2.Stop();