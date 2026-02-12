using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

var monitorToneMap = BuildToneMap();
var lastMonitorName = string.Empty;
var lastBeepUtc = DateTime.MinValue;
const int beepDurationMs = 90;
const int cooldownMs = 250;

Console.WriteLine("Beepmouse started. Press Ctrl+C to stop.");
foreach (var screen in Screen.AllScreens)
{
    var frequency = monitorToneMap[screen.DeviceName];
    Console.WriteLine($"{screen.DeviceName} -> {frequency}Hz");
}

var shouldRun = true;
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    shouldRun = false;
};

while (shouldRun)
{
    if (GetCursorPos(out var cursorPoint))
    {
        var point = new Point(cursorPoint.X, cursorPoint.Y);
        var currentScreen = Screen.FromPoint(point);
        var currentMonitorName = currentScreen.DeviceName;

        if (!string.Equals(currentMonitorName, lastMonitorName, StringComparison.Ordinal))
        {
            var nowUtc = DateTime.UtcNow;
            if ((nowUtc - lastBeepUtc).TotalMilliseconds >= cooldownMs)
            {
                var frequency = monitorToneMap[currentMonitorName];
                Console.Beep(frequency, beepDurationMs);
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss.fff}] Monitor changed to {currentMonitorName} ({frequency}Hz)");
                lastBeepUtc = nowUtc;
                lastMonitorName = currentMonitorName;
            }
        }
    }

    Thread.Sleep(15);
}

Console.WriteLine("Beepmouse stopped.");

static Dictionary<string, int> BuildToneMap()
{
    var tones = new[] { 523, 659, 784, 988 };
    var map = new Dictionary<string, int>(StringComparer.Ordinal);

    var allScreens = Screen.AllScreens;
    for (var index = 0; index < allScreens.Length; index++)
    {
        var frequency = tones[index % tones.Length];
        map[allScreens[index].DeviceName] = frequency;
    }

    return map;
}

[DllImport("user32.dll")]
static extern bool GetCursorPos(out Win32Point lpPoint);

[StructLayout(LayoutKind.Sequential)]
readonly struct Win32Point
{
    public readonly int X;
    public readonly int Y;
}
