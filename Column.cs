using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTS_Dashboard
{
    class Column
    {
        public int workitemid;
        public int update;
        public int revision;
        public String columnname;
        public DateTime datechanged;
        public TimeSpan duration;

        public void Dump()
        {
            Log.Write(workitemid.ToString() + "#" + update.ToString() + "#" + revision.ToString() + "#"
                + columnname + "#" + datechanged.ToString() + "#"
                + duration.ToString(@"d\.hh\:mm\:ss"));
                //+String.Format("{0,4}:{1,2}:{2,2}", duration.Days * 24 + duration.Hours, duration.Minutes, duration.Seconds));
        }

        static public void CSVHeader(System.IO.StreamWriter file)
        {
            file.WriteLine("UniqueID, WorkItemID, Update, Revision, ColumnName, DateChanged, Duration");
        }

        public void SaveToCSV(System.IO.StreamWriter file)
        {
            //Adding a unique ID as Power BI does not support a multicolumn key
            file.WriteLine(workitemid.ToString() + "-" + update.ToString("D2") + "-" + revision.ToString("D2") + ","
                + workitemid.ToString() + "," + update.ToString() + "," + revision.ToString() + ","
                + columnname + "," + datechanged.ToString() + ","
                + duration.ToString(@"d\.hh\:mm\:ss"));
                //+String.Format("{0}:{1}:{2}", duration.Days * 24 + duration.Hours, duration.Minutes, duration.Seconds));
        }
    }
}
