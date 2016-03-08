using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSTS_Dashboard
{

    class VsoData
    {
        public List<Header> listheader;
        public List<Field> listfields;
        public List<int> listitemID;

        private List<Column> listcolumns;
        private List<WorkItemSize> listitemsizes;
        List<WorkItemType> listitemtypes;

        public VsoData()
        {
            listheader = new List<Header>();
            listfields = new List<Field>();
            listitemID = new List<int>();
            listcolumns = new List<Column>();
            listitemsizes = new List<WorkItemSize>();
            listitemtypes = new List<WorkItemType>();
        }

        public void Dump()
        {
            foreach (var p in listheader)
                p.Dump();
            foreach (var q in listfields)
                q.Dump();
        }

        public void SaveToCSV()
        {
            //Creating additional data to make easier to Power BI to digest it

            //generate a columns table
            GenerateColumnsTable();

            //generate a sizes table (1,2,3 -> small, medium, large)
            GenerateSizesTable();

            //generate a workitemtype table (user story, bug)
            GenerateItemTypeTable();

            //save everything in files .csv
            string d = DateTime.Now.ToString("MM-dd-yyyy");

            using (System.IO.StreamWriter hfile =
                new System.IO.StreamWriter(".\\ItemIDs " + d + ".csv", true))
            {
                hfile.WriteLine("WorkItemID");
                foreach (var p in listitemID)
                    hfile.WriteLine(p.ToString());
            }

            using (System.IO.StreamWriter hfile =
                new System.IO.StreamWriter(".\\Headers " + d + ".csv", true))
            {
                Header.CSVHeader(hfile);
                foreach (var p in listheader)
                    p.SaveToCSV(hfile);
            }

            using (System.IO.StreamWriter ffile =
                new System.IO.StreamWriter(".\\Fields " + d + ".csv", true))
            {
                Field.CSVHeader(ffile);
                foreach (var p in listfields)
                    p.SaveToCSV(ffile);
            }

            using (System.IO.StreamWriter cfile =
                new System.IO.StreamWriter(".\\Columns " + d + ".csv", true))
            {
                Column.CSVHeader(cfile);
                foreach (var p in listcolumns)
                    p.SaveToCSV(cfile);
            }

            using (System.IO.StreamWriter sfile =
                new System.IO.StreamWriter(".\\Sizes " + d + ".csv", true))
            {
                WorkItemSize.CSVHeader(sfile);
                foreach (var p in listitemsizes)
                    p.SaveToCSV(sfile);
            }

            using (System.IO.StreamWriter tfile =
                new System.IO.StreamWriter(".\\ItemTypes " + d + ".csv", true))
            {
                WorkItemType.CSVHeader(tfile);
                foreach (var p in listitemtypes)
                    p.SaveToCSV(tfile);
            }
        }

        private void GenerateSizesTable()
        {
            foreach (var q in listfields)
                if (q.tag == "Microsoft.VSTS.Scheduling.StoryPoints")
                {
                    //lets see if this is the last time it has been changed
                    bool laterchanged = false;
                    foreach (var s in listfields)
                        if (s.workitemid == q.workitemid &&
                            s.update > q.update &&
                            s.tag == "Microsoft.VSTS.Scheduling.StoryPoints")
                        {
                            laterchanged = true;
                            break;
                        }
                    if (laterchanged) continue;

                    foreach (var r in listfields)
                        if (r.workitemid == q.workitemid &&
                            r.update == q.update &&
                            r.tag == "System.ChangedDate")
                        {
                            WorkItemSize c = new WorkItemSize();
                            c.workitemid = r.workitemid;
                            c.update = r.update;
                            c.revision = r.revision;
                            try
                            {
                                c.size = Int32.Parse(q.newvalue);
                            }
                            catch (Exception ex)
                            {
                                Log.Write(ex);
                            }
                            c.datechanged = Convert.ToDateTime(r.newvalue);
                            listitemsizes.Add(c);
                        }
                }

        }

        private void GenerateItemTypeTable()
        {
            foreach (var q in listfields)
                if (q.tag == "System.WorkItemType")
                    foreach (var r in listfields)
                        if (r.workitemid == q.workitemid &&
                            r.update == q.update &&
                            r.revision == q.revision &&
                            r.tag == "System.ChangedDate")
                        {
                            WorkItemType c = new WorkItemType();
                            c.workitemid = r.workitemid;
                            c.update = r.update;
                            c.revision = r.revision;
                            c.type = q.newvalue;
                            c.datechanged = Convert.ToDateTime(r.newvalue);
                            listitemtypes.Add(c);
                        }
        }

        private void GenerateColumnsTable()
        {
            foreach (var q in listfields)
                if (q.tag == "WEF_E6F256BE561143D7BF059484652EDA43_Kanban.Column")
                    foreach (var r in listfields)
                        if (r.workitemid == q.workitemid &&
                            r.update == q.update &&
                            r.revision == q.revision &&
                            r.tag == "System.ChangedDate")
                        {
                            Column c = new Column();
                            c.workitemid = r.workitemid;
                            c.update = r.update;
                            c.revision = r.revision;
                            c.columnname = q.newvalue;
                            c.datechanged = Convert.ToDateTime(r.newvalue);
                            listcolumns.Add(c);
                        }

            // curation of values of columns
            CurateColumnsTable();
        }

        private void CurateColumnsTable()
        {
            //old names/states
            foreach (var q in listcolumns)
            {
                if (q.columnname == "Active")
                    q.columnname = "Analysis";
                if (q.columnname == "Development")
                    q.columnname = "Dev In Progress";
                if (q.columnname == "Inbox2")
                    q.columnname = "Inbox";
                if (q.columnname == "MyInbox")
                    q.columnname = "Inbox";
                if (q.columnname == "Ready for Live Test")
                    q.columnname = "Consumer";
                if (q.columnname == "Resolved")
                    q.columnname = "Consumer";
                if (q.columnname == null)
                    q.columnname = "Removed";
            }
            
            //eliminate repeated states
            int workitemid = 0;
            String columnname = "";
            
            for (int i = 0; i < listcolumns.Count; i++)
            {
                var q = listcolumns[i];
                if (q.workitemid == workitemid &&
                    q.columnname == columnname)
                {
                    listcolumns.RemoveAt(i);
                    i--;
                }
                else
                {
                    workitemid = q.workitemid;
                    columnname = q.columnname;
                }
            }

            //calculate durations
            workitemid = 0;
            DateTime last = DateTime.Now;

            for (int i = listcolumns.Count -1; i >= 0 ; i--)
            {
                var q = listcolumns[i];
                if (q.workitemid == workitemid)
                {
                    listcolumns[i].duration = last - listcolumns[i].datechanged;
                }
                else
                {
                    workitemid = listcolumns[i].workitemid;
                    if (q.columnname == "Closed" ||
                        q.columnname == "Removed")
                        listcolumns[i].duration = TimeSpan.FromSeconds(0);
                    else
                        listcolumns[i].duration = DateTime.Now - listcolumns[i].datechanged;
                }
                last = listcolumns[i].datechanged;
            }
        }
    }
}
