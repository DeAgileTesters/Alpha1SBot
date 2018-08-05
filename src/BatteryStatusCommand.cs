using ManyConsole;
using System;

namespace DATBot
{
    public class BatteryStatusCommand : ConsoleCommand
    {
        public BatteryStatusCommand()
        {
            IsCommand("BatteryStatus", "Report the battery status of the robot.");
        }

        public override int Run(string[] remainingArguments)
        {
            byte[] command = CreateSerialCommand();

            SerialConnection.StartConnection(true);
            SerialConnection.SendSerialMessageToRobot(command);

            return 0;
        }

        public static void ReportData(byte[] serialMessage)
        {
            string chargeStatus = "";

            switch (serialMessage[6])
            {
                case 0X00:
                    chargeStatus = "Not charging";
                    break;
                case 0X01:
                    chargeStatus = "Charging";
                    break;
                case 0X02:
                    chargeStatus = "No battery";
                    break;
            }

            Console.WriteLine($"   Battery status is: Charge status: {chargeStatus}, capacity: {serialMessage[7]}%");
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
            command[3] = Helpers.Constants.BATTERYSTATIS;

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