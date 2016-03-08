using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTS_Dashboard
{
    class Header
    {
        public int workitemid;
        public int update;
        public int revision;
        public DateTime reviseddate;
        public string revisedbyid;
        public string revisedbyname;
        public string revisedbyurl;

        public void Dump()
        {
            Log.Write("Header: " + workitemid.ToString() + "#" + update.ToString() + "#" + revision.ToString() + "#"
                + "#" + reviseddate.ToString() + revisedbyid + "#" + revisedbyname + "#" + revisedbyurl + "#");
        }

        static public void CSVHeader(System.IO.StreamWriter file)
        {
            file.WriteLine("UniqueID, WorkItemID, Update, Revision, RevisedDate, RevisedByID, RevisedByName, RevisedByURL");
        }

        public void SaveToCSV(System.IO.StreamWriter file)
        {
            //Adding a unique ID as Power BI does not support a multicolumn key
            file.WriteLine(workitemid.ToString() + "-" + update.ToString("D2") + "-" + revision.ToString("D2") + ","
                + workitemid.ToString() + "," + update.ToString() + "," + revision.ToString() + ","
                + reviseddate.ToString() + "," + revisedbyid + "," + revisedbyname + "," + revisedbyurl);
        }
    }
}
