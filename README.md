# Beepmouse (Week 1: Signal)

A minimal Windows background app that beeps when the mouse cursor crosses from one monitor to another.

## Behavior

- Trigger only when monitor changes.
- Default tones by monitor order:
  - Monitor 1: 523 Hz
  - Monitor 2: 659 Hz
  - Monitor 3: 784 Hz
  - Monitor 4: 988 Hz
- Beep duration: 90 ms
- Cooldown: 250 ms to avoid edge double-fires

## Run

```powershell
dotnet run -c Release
```

The app runs headless and logs transitions in the console.
Press `Ctrl+C` to stop.
