using System;
using System.Collections.Generic;
using System.Configuration;

namespace DATBot.Helpers
{
    public class HelperMethods
    {
        public static List<byte[]> SplitByteArrayByDelimiter(byte[] array, byte delimiter)
        {
            List<byte[]> byteArray = new List<byte[]>();

            int begin = 0;

            // Not the best solution, but it works for now. Needs refactoring
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == delimiter)
                {
                    byte[] tmpArray = new byte[i - begin + 1];
                    for (int j = 0; j < i - begin + 1; j++)
                    {
                        tmpArray[j] = array[begin+j];
                    }

                    byteArray.Add(tmpArray);
                    begin = i + 1;
                }
            }

            return byteArray;
        }

        public static byte CalculateByteHash(byte[] command)
        {
            int intResult = 0;
            for (int i = 2; i < command.Length - 1; i++)
            {
                intResult = intResult + command[i];
            }

            return Convert.ToByte((intResult % 256));
        }

        public static string ReadSettingFromConfigFile(string key)
        {
            try
            {
                var appSettings = ConfigurationManager.AppSettings;
                string result = appSettings[key];
                if (result != null) return result;
                throw new Exception($"Error reading app settings. Key {key} not found.");
            }
            catch (ConfigurationErrorsException e)
            {
                throw new Exception($"Error reading app settings. {e.Message}");
            }
        }
    }
}