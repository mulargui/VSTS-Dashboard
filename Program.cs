using System;

namespace VSTS_Dashboard
{
    class Program
    {
        static void Main(string[] args)
        {
            VsoData vsodata = new VsoData();
            try
            {
                using (VsoMessage vsomessage = new VsoMessage())
                {
                    vsomessage.GetData(vsodata);
                }
            }
            catch (Exception ex)
            {
                Log.Write(ex);
            }
            //vsodata.Dump();
            vsodata.SaveToCSV();
        }
    }
}
