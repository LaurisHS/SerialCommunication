# Copilot session instructions for SerialCommunication

Purpose: give future Copilot sessions repository-specific context so suggestions are accurate.

---

Build, test, and lint commands

- Primary solution: SerialCommunication.slnx (Visual Studio .sln). Recommended: open in Visual Studio 2019/2022.
- CLI build (Windows):
  - Build whole solution: msbuild "SerialCommunication.slnx" /p:Configuration=Debug
  - Build single project: msbuild "SerialCommunication\SerialCommunication.csproj" /p:Configuration=Debug
  - Note: project targets .NET Framework 4.7.2 — use MSBuild/Visual Studio (dotnet build is not guaranteed to work).

- Arduino sketch (SerialCommunication.ino):
  - Using arduino-cli: arduino-cli compile --fqbn <board_fqbn> SerialCommunication.ino
  - Upload example: arduino-cli upload -p <COMx> --fqbn <board_fqbn> SerialCommunication.ino
  - Or use the Arduino IDE.

- Tests / Lint
  - No automated unit tests or linting configuration present in the repo.

---

High-level architecture (big picture)

- Desktop host: a Windows Forms application (SerialCommunication\) — entrypoint: Program.Main -> Form1.
  - Form1 enumerates serial ports and provides UI to connect/send commands (see SerialCommunication\Form1.cs).
- Microcontroller/firmware side:
  - SerialCommunication.ino implements a small serial command protocol using the bundled SerialCommand library (SerialCommand.h / SerialCommand.cpp).
  - analog.c provides analog read helper code used by the sketch.
- Protocol:
  - Text-based commands, tokenized by spaces (delim = " "). Terminator is newline ('\n').
  - SerialCommand buffer size = 32 bytes, MAXSERIALCOMMANDS = 10. Handlers are registered with addCommand().
  - Built-in command handlers in the sketch: set, toggle, get, ping, help, debug; behavior in SerialCommunication.ino.
- Resources: images in SerialCommunication\Resources used by the UI.

---

Key conventions and patterns specific to this repo

- Serial protocol conventions (important when modifying either side):
  - Prefixes: digital pins use "d" (e.g., "d2"), analog pins use "a" (e.g., "a0"), PWM uses prefix "pwm" (e.g., "pwm9").
  - Responses are human-readable ASCII lines (e.g., "pong", "set done", "unknown command \"x\"").
  - Default baudrate used in the sketch: 115200.
  - Commands are parsed with strtok_r semantics; use SerialCommand.next() to iterate arguments.
  - The sketch expects printable characters only and uses a small fixed-size buffer — avoid sending very long tokens.

- C# UI conventions:
  - Combo box names: comboBoxPoort for serial port, comboBoxBaudrate for baudrate selection. Default baudrate selection is "115200".
  - Form1 is the main UI surface; Program.Run(new Form1()) is the app entry point.

- Build artifacts and editor state present in the repo (do not commit):
  - .vs/, SerialCommunication\obj\, and generated temporary files. .gitignore exists but reviewers should confirm they are up-to-date.

---

Other repository notes for Copilot sessions

- No CONTRIBUTING.md or README.md to pull workflow conventions from; LICENSE.txt is present.
- No AI assistant-specific config files detected (CLAUDE.md, .cursorrules, AGENTS.md, etc.).

---

If editing both host and device code, keep the serial protocol details in sync (terminator, delim, buffer limits, and command names). When suggesting protocol changes, prefer explicit migration steps (e.g., bump buffer size, add versioning token) and update both sides in the same PR.

---

If this file should incorporate specific workflow steps (CI, test harnesses, preferred board fqbn for arduino-cli, or Visual Studio target), add that data to repo root (README or CONTRIBUTING) and request an update to this file.
