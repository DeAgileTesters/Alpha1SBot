using ManyConsole;
using System;

namespace DATBot
{
    public class ResetCommand : ConsoleCommand
    {
        public ResetCommand()
        {
            IsCommand("Reset", "Resets the robot.");

            HasLongDescription("Resets the robot to the default position.");
        }

        public override int Run(string[] remainingArguments)
        {
            byte[] command = CreateSerialCommand();
            SerialConnection.StartConnection(false);
            Console.WriteLine($"   Sending Reset command to the robot...");
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
            command[3] = Helpers.Constants.RESETCOMMAND;

            //Parameter 
            command[4] = 0x00;

            //Check 
            command[5] = Helpers.HelperMethods.CalculateByteHash(command); //0x08 

            //End 
            command[6] = Helpers.Constants.ENDCHARACTER;
            return command;
        }
    }
}