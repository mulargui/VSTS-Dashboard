using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTS_Dashboard
{
    class WorkItemSize
    {
        public int workitemid;
        public int update;
        public int revision;
        public int size;
        public DateTime datechanged;

        public void Dump()
        {
            Log.Write(workitemid.ToString() + "#" + update.ToString() + "#" + revision.ToString() + "#"
                + size.ToString() + "#" + datechanged.ToString());
        }

        static public void CSVHeader(System.IO.StreamWriter file)
        {
            file.WriteLine("UniqueID, WorkItemID, Update, Revision, StoryPoints, DateChanged");
        }

        public void SaveToCSV(System.IO.StreamWriter file)
        {
            //Adding a unique ID as Power BI does not support a multicolumn key
            file.WriteLine(workitemid.ToString() + "-" + update.ToString("D2") + "-" + revision.ToString("D2") + ","
                + workitemid.ToString() + "," + update.ToString() + "," + revision.ToString() + ","
                + size.ToString() + "," + datechanged.ToString());
        }
    }
}
