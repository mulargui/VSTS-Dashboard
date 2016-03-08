using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;

namespace VSTS_Dashboard
{
    public class VsoConnection: HttpClient
    {
        const string accesstoken = "ygjuj44oq3ylz2oo7c2zjjfxxnxku6bjm72hmhavevv7bdtlfoqq";
        const string instance = "mulargui";
        const string project = "Explorer";
        const string user = "mulargui@hotmail.com";

        public VsoConnection()
        {
            DefaultRequestHeaders.Accept.Add(
                        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

            DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                        Convert.ToBase64String(System.Text.ASCIIEncoding.ASCII.GetBytes(string.Format("{0}:{1}", user, accesstoken))));
        }

        public async Task<string> GetStatus(string id)
        {
                    //workitem details
                    using (HttpResponseMessage response = GetAsync(
                        "https://" + instance + ".visualstudio.com/DefaultCollection/_apis/wit/workitems?ids=" + id + "&api-version=1.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
        }

        public async Task<string> GetUpdates(string id)
        {
                    //get updates of a workitem (includes individual changes with new and old values)
                    using (HttpResponseMessage response = GetAsync(
                        "https://" + instance + ".visualstudio.com/DefaultCollection/_apis/wit/workitems/" + id + "/updates?&api-version=1.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
        }

        public async Task<string> GetRevisions(string id)
        {
                     //get revisions of a workitem (includes snapshots of each revision of changes)
                    using (HttpResponseMessage response = GetAsync(
                        "https://" + instance + ".visualstudio.com/DefaultCollection/_apis/wit/workitems/" + id + "/revisions?&api-version=1.0").Result)
                    {
                        response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                    }
        }

        public async Task<string> GetIDs()
        {
            //get the list of IDs of worktitems that are bugs or user stories
            var body = "{ \"query\": \"Select [ID] FROM WorkItems WHERE"
                + " [Work Item Type] = 'Bug'"
                + " OR [Work Item Type] = 'Product Backlog Item'"
                + "\" }";

            using (HttpResponseMessage response = PostAsync(
                       "https://" + instance + ".visualstudio.com/DefaultCollection/" + project + "/_apis/wit/wiql?api-version=1.0",
                        new StringContent(body, Encoding.UTF8, "application/json")).Result)
                {
                    response.EnsureSuccessStatusCode();
                        return await response.Content.ReadAsStringAsync();
                }
        }
    }
}
