using System;
using System.Collections.Generic;
using System.Text;

namespace VBloodKills.Utils
{
    public class ChatColors
    {
        public static string ColorText(string hexColor, string text)
        {
            return $"<color={hexColor}>{text}</color>";
        }
        public static string Green(string text)
        {
            return ColorText("#7FE030", text);
        }
        public static string Yellow(string text)
        {
            return ColorText("#FBC01E", text);
        }
        public static string Red(string text)
        {
            return ColorText("#E90000", text);
        }
    }
}
