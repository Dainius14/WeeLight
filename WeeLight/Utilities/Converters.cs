using System;
using System.Globalization;

namespace WeeLight.Utilities
{
    public static class Converters
    {
        public static byte[] TemperatureToRGBBytes(int temperature)
        {

            double dTemperature = (int)temperature / (double)100;

            double dRed, dGreen, dBlue;
            // Red
            if (dTemperature <= 66)
            {
                dRed = 255;
            }
            else
            {
                dRed = dTemperature - 60;
                dRed = 329.698727446 * Math.Pow(dRed, -0.1332047592);
                dRed = Math.Round(dRed);
            }

            // Green
            if (dTemperature <= 66)
            {
                dGreen = dTemperature;
                dGreen = 99.4708025861 * Math.Log(dGreen) - 161.1195681661;
                dGreen = Math.Round(dGreen);
            }
            else
            {
                dGreen = dTemperature - 60;
                dGreen = 288.1221695283 * Math.Pow(dGreen, -0.0755148492);
                dGreen = Math.Round(dGreen);
            }

            // Blue
            if (dTemperature >= 66)
            {
                dBlue = 255;
            }
            else
            {
                if (dTemperature <= 19)
                {
                    dBlue = 0;
                }
                else
                {
                    dBlue = dTemperature - 10;
                    dBlue = 138.5177312231 * Math.Log(dBlue) - 305.0447927307;
                    dBlue = Math.Round(dBlue);

                }
            }

            int red = Math.Clamp(System.Convert.ToInt32(dRed), 0, 255);
            int green = Math.Clamp(System.Convert.ToInt32(dGreen), 0, 255);
            int blue = Math.Clamp(System.Convert.ToInt32(dBlue), 0, 255);

            return new byte[] { (byte)red, (byte)green, (byte)blue };
        }

        public static int RGBBytesToInt(byte[] rgb)
        {
            return rgb[0] << 16 | rgb[1] << 8 | rgb[2];
        }
        public static byte[] RGBIntToBytes(int rgb)
        {
            byte red = (byte)(rgb >> 16 & 0xFF);
            byte green = (byte)(rgb >> 8 & 0xFF);
            byte blue = (byte)(rgb & 0xFF);
            return new byte[] { red, green, blue };
        }

        public static string RGBIntToString(int rgb)
        {
            byte[] bytes = RGBIntToBytes(rgb);
            return bytes[0].ToString("X") + bytes[1].ToString("X") + bytes[2].ToString("X");
        }

        public static byte[]RGBStringToBytes(string rgb)
        {
            byte red = byte.Parse(rgb.Substring(0, 2), NumberStyles.HexNumber);
            byte green = byte.Parse(rgb.Substring(2, 2), NumberStyles.HexNumber);
            byte blue = byte.Parse(rgb.Substring(4, 2), NumberStyles.HexNumber);
            return new byte[] { red, green, blue };
        }

    }
}
