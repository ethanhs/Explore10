using System;
using System.Windows;
using System.Runtime.InteropServices;


namespace Explore10
{
    public static class Helpers
    {
        //this is used to prettify the output of getting the # of bytes on the drive
        static readonly string[] Suffixes =
        { "bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        public static string PrettyByte(long value)
        {
            if (value == 0) { return "0.0 bytes"; }

            var mag = (int)Math.Log(value, 1024);
            var adjustedSize = (decimal)value / (1L << (mag * 10));

            return $"{adjustedSize:n1} {Suffixes[mag]}";
        }

        //this is used to get the current mouse position
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetCursorPos(out POINT lpPoint);

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int X;
            public int Y;

            public POINT(int x, int y)
            {
                this.X = x;
                this.Y = y;
            }

            
        }
        public static System.Drawing.Point GetMousePosition()
        {
            POINT MousePOINT = new POINT();
            GetCursorPos(out MousePOINT);
            return new System.Drawing.Point(MousePOINT.X, MousePOINT.Y);
        }





    }



}
