using System;

namespace VSTS_Dashboard
{
    class Log
    {
        public static void Write(Exception ex)
        {
            Log.Write(ex.ToString());
        }

        public static void Write(int i)
        {
            Log.Write(i.ToString());
        }

        public static void Write(DateTime d)
        {
            Log.Write(d.ToString());
        }

        public static void Write(string s)
        {
            Console.WriteLine("----------------------------------------------------------------------------------------");
            Console.WriteLine(s);
        }
    }
}
