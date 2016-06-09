using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace OPGGReplay
{
    public class API
    {
        public string apiKey { get; set; }//	
        public Matches GetMatchListBySummonerName(string name)
        {
            string id = GetIdByName(name);
            return GetMatchListBySummonerId(id);
        }
        public string GetIdByName(string name)
        {
            try
            {
                string url = string.Format("https://kr.api.pvp.net/api/lol/kr/v1.4/summoner/by-name/{0}?api_key={1}", name, apiKey);
                using (WebClient wc = new WebClient())
                {
                    byte[] result = wc.DownloadData(url);
                    string r = Encoding.UTF8.GetString(result);
                    JObject obj = JsonConvert.DeserializeObject<JObject>(r);
                    return obj[name.Replace(" ", "").Trim().ToLower()]["id"].ToString(); 

                }
            }
            catch
            {
                return "";
            }
        }
        public Matches GetMatchListBySummonerId(string id)
        {
            try
            {
                string url = string.Format("https://kr.api.pvp.net/api/lol/kr/v2.2/matchlist/by-summoner/{0}?api_key={1}", id, apiKey);
                using (WebClient wc = new WebClient())
                {
                    byte[] result = wc.DownloadData(url);
                    string r = Encoding.UTF8.GetString(result);
                    Matches obj = JsonConvert.DeserializeObject<Matches>(r);
                    return obj;
                }
            }
            catch
            {
                return null;
            }
        }
        public void DownloadBat(string matchId)
        {
            if (!Directory.Exists(System.Environment.CurrentDirectory + "\\Replay\\"))
            {
                Directory.CreateDirectory(System.Environment.CurrentDirectory + "\\Replay\\");
            }
            using (WebClient wc = new WebClient())
            {
                string url = string.Format("http://www.op.gg/match/new/id={0}", matchId);
                wc.DownloadFile(url, System.Environment.CurrentDirectory + "\\Replay\\" + matchId + ".bat");
            }
        }
    }
    public class Matches
    {
        public Match[] matches { get; set; }
    }
    public class Match
    {
        public string region { get; set; }
        public string platformId { get; set; }
        public long matchId { get; set; }
        public int champion { get; set; }
        public string queue { get; set; }
        public string season { get; set; }
        public long timestamp { get; set; }
        public string lane { get; set; }
        public string role { get; set; }
    }
}
