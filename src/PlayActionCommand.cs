using ManyConsole;
using System;
using System.Text;
using System.Threading;

namespace DATBot
{
    public class PlayActionCommand : ConsoleCommand
    {
        public string ActionName { get; set; }
        public int Time { get; set; }

        public PlayActionCommand()
        {
            IsCommand("PlayAction", "Play an action.");
            HasLongDescription("Play an action that is already in the firmware of the robot.");

            HasRequiredOption("a|ActionName=", "The name of the action.", p => ActionName = p);
            HasOption("t|Time=", "The time the action will be played in seconds.", p => Time = Convert.ToInt32(p));
        }

        public override int Run(string[] remainingArguments)
        {
            byte[] actionCommmand = CreateSerialCommand(ActionName);
            SerialConnection.StartConnection(false);
            Console.WriteLine($"   Playing action {ActionName} for {Time} seconds.");
            SerialConnection.SendSerialMessageToRobot(actionCommmand);

            if (Time > 0)
            {
                Thread.Sleep(Time * 1000);
                byte[] stopCommand = StartStopCommand.CreateSerialCommand();
                SerialConnection.SendSerialMessageToRobot(stopCommand);

            }
            SerialConnection.CloseConnection();
            return 0;
        }

        public static byte[] CreateSerialCommand(string actionName)
        {
            byte[] actionNameInBytes = Encoding.ASCII.GetBytes(actionName);
            byte[] command = new byte[6 + actionName.Length];

            //Header 
            command[0] = Helpers.Constants.FIRSTBEGINCHARACTER;
            command[1] = Helpers.Constants.SECONDBEGINCHARACTER;

            //Lenght 
            command[2] = Convert.ToByte(command.Length - 1);

            //Command 
            command[3] = Helpers.Constants.PLAYACTION;

            //Parameter 
            for (int i = 0; i < actionName.Length; i++)
            {
                command[4 + i] = actionNameInBytes[i];
            }

            //Check 
            command[command.Length - 2] = Helpers.HelperMethods.CalculateByteHash(command);

            //End 
            command[command.Length - 1] = Helpers.Constants.ENDCHARACTER;
            return command;
        }
    }
}