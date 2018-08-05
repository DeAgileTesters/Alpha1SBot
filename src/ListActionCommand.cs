using ManyConsole;
using System;
using System.Collections.Generic;

namespace DATBot
{
    public class ListActionCommand : ConsoleCommand
    {
        public ListActionCommand()
        {
            IsCommand("ListActions", "Lists the actions of the robot");
            HasLongDescription("List the actions that are in the current firmware of the Robot");
        }

        public override int Run(string[] remainingArguments)
        {
            byte[] command = CreateSerialCommand();
            SerialConnection.StartConnection(true);
            SerialConnection.SendSerialMessageToRobot(command);
            Console.WriteLine($"    Available actions on Robot:");

            return 0;
        }

        public static void ReportData(byte[] serialMessage)
        {
            List<byte[]> actionList = Helpers.HelperMethods.SplitByteArrayByDelimiter(serialMessage, Helpers.Constants.ENDCHARACTER);
            foreach (var action in actionList)
            {
                byte[] actionName = new byte[action.Length - 6];

                for (int i = 4; i < action.Length - 2; i++)
                {
                    actionName[i - 4] = action[i];
                }

                Console.WriteLine($"        {System.Text.Encoding.UTF8.GetString(actionName)}");
            }
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
            command[3] = Helpers.Constants.LISTACTIONS;

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