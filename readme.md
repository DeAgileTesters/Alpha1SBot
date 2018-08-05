# Control Alpha 1S Robot from commandline

Program to control the Alpha 1S Robot from the command tool

## Howto
- Compile with Visual Studio 2017 Community Edition
- Pair the robot with the bluetooth connection
- Adjust App.config and set the correct serialPort (for example: COM3")
        See: 'Alpha 1S' bluetooth devices and tab 'Hardware'
        If you see two COM ports, experiment whitch one works
- Turn off energy management on Bluetooth adapter and paired device
   See: https://helpdeskgeek.com/how-to/prevent-windows-from-powering-off-usb-device/

## Options
Run console application and give options by using parameters:

ListActions - Lists the current actions loaded on the bot firmware
BatteryStatus - Display status of the battery
PlayAction - Plays a action. Give actionname as parameter and playtime as optional parameter
Reset - Resets the robot
StartStop - Starts or stops the robot (depending on current action)

### Prerequisites
.Net Framework 4.6.2

## Dependancies (NuGet)
- ManyConsole
- Mono.Options

## Powerpoint

You can use Macro's in Powerpoint together with VBA code to run the console application so that you can control the robot from Powerpoint.
