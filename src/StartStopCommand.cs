using ManyConsole;
using System;

namespace DATBot
{
    public class StartStopCommand : ConsoleCommand
    {
        public StartStopCommand()
        {
            IsCommand("StartStop", "Start or Stop the robot with playing the current action.");

            HasLongDescription("(Re) Start or Stop the Robot with the current action it is performing");
        }

        public override int Run(string[] remainingArguments)
        {
            byte[] command = CreateSerialCommand();
            SerialConnection.StartConnection(false);
            Console.WriteLine($"   Sending Start/Stop command to the robot...");
            SerialConnection.SendSerialMessageToRobot(command);
            SerialConnection.CloseConnection();
            return 0;
        }

        public static byte[] CreateSerialCommand()
        {
            byte[] command = new byte[7];
            //Header 
            command[0] = Helpers.Constants.FIRSTBEGINCHARACTER;
            command[1] = Helpers.Constants.SECONDBEGINCHARACTER;

            //Lenght 
            command[2] = Convert.ToByte(command.Length - 1);

            //Command 
            command[3] = Helpers.Constants.STARTSTOP;

            //Parameter 
            command[4] = 0x00;

            //Check 
            command[5] = Helpers.HelperMethods.CalculateByteHash(command);

            //End 
            command[6] = Helpers.Constants.ENDCHARACTER;
            return command;
        }
    }
}