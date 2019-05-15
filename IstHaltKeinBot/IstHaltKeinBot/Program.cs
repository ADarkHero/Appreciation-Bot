using System;
using System.Collections.Generic;
using Tweetinvi;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace IstHaltKeinBot
{
    class Program
    {
        static void Main(string[] args)
        {
            String jsonfile = "steam.json";

            //If the json file is nonexistend or older than 7 days, it should be downloaded
            var threshold = DateTime.Now.AddDays(-7);
            var fileage = System.IO.File.GetLastWriteTime(jsonfile); //Age of the json file
            if (!File.Exists(jsonfile) || fileage  < threshold){
                //Downloads the latest json file of all games that exist on steam.
                WebClient myWebClient = new WebClient();
                myWebClient.DownloadFile("http://api.steampowered.com/ISteamApps/GetAppList/v0002/", jsonfile);
            }

            //Reads the data from the steam.json
            StreamReader sr = new StreamReader(jsonfile);
            String jsonData = sr.ReadToEnd();
            var outObject = JsonConvert.DeserializeObject<RootObject>(jsonData);    //Serializes the JSon String
            sr.Close();



            //Generates random numbers
            int appNumber = outObject.applist.apps.Count;   //How many Steam-Apps?
            Random Rnd = new Random();
            int random1 = Rnd.Next(appNumber);  //First game
            int random2 = Rnd.Next(appNumber);  //Second game

            //Writes a new tweet
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            Tweet.PublishTweet(outObject.applist.apps[random1].name + " ist halt kein " + outObject.applist.apps[random2].name + ".");
        }
    }

    public class App
    {
        public int appid { get; set; }
        public string name { get; set; }
    }

    public class Applist
    {
        public List<App> apps { get; set; }

    }

    public class RootObject
    {
        public Applist applist { get; set; }
    }

}
