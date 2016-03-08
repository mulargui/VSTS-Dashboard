using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Newtonsoft.Json.Linq;

namespace VSTS_Dashboard
{
    class VsoMessage : VsoConnection
    {
        public async void TestMethods()
        {
            //const string workitem = "12";
            const string workitemid = "41018";
            string responseBody;

            responseBody = await GetStatus(workitemid);
            Log.Write(responseBody);
            responseBody = await GetUpdates(workitemid);
            Log.Write(responseBody);
            responseBody = await GetRevisions(workitemid);
            Log.Write(responseBody);
            responseBody = await GetIDs();
            Log.Write(responseBody);
        }

        public async void GetData(VsoData vsodata)
        {
            //get the IDs of the workitems
            vsodata.listitemID = await GetListofIDs();

            //get the updates of each workitem
            foreach (int i in vsodata.listitemID)
                await GetListofUpdates(i, vsodata);
        }

        private async Task<int> GetListofUpdates(int i, VsoData vsodata)
        {
            string responseBody = await GetUpdates(i.ToString());

            JObject results = JObject.Parse(responseBody);
            //int count = (int) results["count"];
            JArray updates = (JArray)results["value"];

            foreach (JObject jo in updates.Children<JObject>())
            {
                int workitemid = (int)jo["workItemId"];
                int update = (int)jo["id"];
                int revision = (int)jo["rev"];
                DateTime reviseddate = (DateTime)jo["revisedDate"];

                Header h = new Header();
                h.workitemid = workitemid;
                h.update = update;
                h.revision = revision;
                h.reviseddate = reviseddate;
                h.revisedbyid = (string)jo["revisedBy"]["id"];
                h.revisedbyname = (string)jo["revisedBy"]["name"];
                h.revisedbyurl = (string)jo["revisedBy"]["url"];
                vsodata.listheader.Add(h);

                JObject jo2 = (JObject)jo["fields"];
                try
                {
                    foreach (JProperty jProperty in jo2.Properties())
                    {
                        Field f = new Field();
                        f.workitemid = workitemid;
                        f.update = update;
                        f.revision = revision;
                        f.reviseddate = reviseddate;
                        f.tag = (string)jProperty.Name.Trim();
                        JObject jo3 = (JObject)jProperty.Value;
                        f.newvalue = (string)jo3["newValue"];
                        f.oldvalue = (string)jo3["oldValue"];
                        vsodata.listfields.Add(f);
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex);
                    continue;
                }
            }
            return 1;
        }

        internal class header
            {
                public string referenceName { get; set; }
                public string name { get; set; }
                public string url { get; set; }
            }

        internal class workitem
            {
                public int id { get; set; }
                public string url { get; set; }
            }

        internal class JSONDoc
            {
                public string queryType { get; set; }
                public string queryResultType { get; set; }
                public string asOf { get; set; }
                public List<header> columns;
                public List<workitem> workItems;
            }

            private async Task<List<int>> GetListofIDs()
            {
                string responseBody = await GetIDs();

                JavaScriptSerializer ser = new JavaScriptSerializer();
                JSONDoc items = ser.Deserialize<JSONDoc>(responseBody);

                List<int> ids = new List<int>();
                foreach (workitem w in items.workItems)
                    ids.Add(w.id);
                return ids;
            }
    }
}