﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTS_Dashboard
{
    class Field
    {
        public int workitemid;
        public int update;
        public int revision;
        public DateTime reviseddate;
        public string tag;
        public string newvalue;
        public string oldvalue;

        public void Dump()
        {
            Log.Write("Field: " + workitemid.ToString() + "#" + update.ToString() + "#" + revision.ToString() + "#"
                + reviseddate.ToString() + "#" + tag.Replace(',', '#').Replace('\n', ' ') + "#"
                + (newvalue != null ? newvalue.Replace('#', ',').Replace('\n', ' ') : newvalue) + "#"
                + (oldvalue != null ? oldvalue.Replace('#', ',').Replace('\n', ' ') : oldvalue) + "#");
        }

        static public void CSVHeader(System.IO.StreamWriter file)
        {
            file.WriteLine("UniqueID, WorkItemID, Update, Revision, RevisedDate, Field, NewValue, OldValue");
        }

        public void SaveToCSV(System.IO.StreamWriter file)
        {
            //Adding a unique ID as Power BI does not support a multicolumn key
            file.WriteLine(workitemid.ToString() + "-" + update.ToString("D2") + "-" + revision.ToString("D2") + ","
                + workitemid.ToString() + "," + update.ToString() + "," + revision.ToString() + ","
                + reviseddate.ToString() + "," + tag.Replace(',', '#').Replace('\n', ' ') + ","
                + (newvalue != null ? newvalue.Replace(',', '#').Replace('\n', ' ') : newvalue) + ","
                + (oldvalue != null ? oldvalue.Replace(',', '#').Replace('\n', ' ') : oldvalue));
        }

        public void SaveColumns(List<Column> listcolumns)
        {

        }
    }
}
