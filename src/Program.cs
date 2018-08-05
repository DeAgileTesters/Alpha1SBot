using System;
using System.Collections.Generic;
using System.Threading;
using ManyConsole;

namespace DATBot
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("");
            Console.WriteLine("DAT Robot - De Agile Testers");
            Console.WriteLine("Controlling Alpha 1S Robot");
            Console.WriteLine("");
            Console.WriteLine("How To:");
            Console.WriteLine("- Pair the robot with the bluetooth connection");
            Console.WriteLine("- Adjust App.config and set the correct serialPort (for example: COM3");
            Console.WriteLine("    See: 'Alpha 1S' bluetooth devices and tab 'Hardware'");
            Console.WriteLine("- Turn off energy management on Bluetooth adapter and paired device");
            Console.WriteLine("    See: https://helpdeskgeek.com/how-to/prevent-windows-from-powering-off-usb-device/");
            Console.WriteLine("");

            // Get commands from commandline
            var commands = GetCommands();

            // Return exit code
            return ConsoleCommandDispatcher.DispatchCommand(commands, args, Console.Out);
        }

        public static IEnumerable<ConsoleCommand> GetCommands()
        {
            return ConsoleCommandDispatcher.FindCommandsInSameAssemblyAs(typeof(Program));
        }
    }
}
