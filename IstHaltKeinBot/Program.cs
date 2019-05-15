using System;
using System.Collections.Generic;
using Tweetinvi;
using System.Net;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using System.Text.RegularExpressions;
using System.Text;
using Tweetinvi.Models;

namespace AppreciationBot
{
    class Program
    {
        static void Main(string[] args)
        {
            string usersfile = "users.csv";
            string tweetsfile = "tweets.csv";

            //Read current line number from settings
            int curLine = Properties.Settings.Default.Line;
            curLine = 0; //Debug stuff
            Properties.Settings.Default.Line = curLine;
            Properties.Settings.Default.Save();
            if (curLine == 0)
            {
                writeUserList();
            }


            //Read current user the bot should tweet to
            string user = File.ReadLines(usersfile).Skip(curLine).Take(1).First();

            //Read random tweet message
            int messageLineCount = File.ReadLines(tweetsfile).Count();
            Random random = new Random();
            int randomLine = random.Next(0, messageLineCount-1);
            string message = File.ReadLines(tweetsfile, Encoding.GetEncoding("UTF-8")).Skip(randomLine).Take(1).First();

            //Writes a new tweet
            Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            string tweet = "@" + user + " " + message;
            Tweet.PublishTweet(" ");

            //After the tweet was successfully published, increase the line number by one
            curLine++;
            Properties.Settings.Default.Line = curLine;
            Properties.Settings.Default.Save();
        }

        private static void writeUserList()
        {
            //Download user list
            var userCredentials = Auth.SetUserCredentials("CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN", "ACCESS_TOKEN_SECRET");
            var authenticatedUser = User.GetAuthenticatedUser(userCredentials);
            var followers = authenticatedUser.GetFollowerIds();

            //Write userlist to string
            string userlist = null;
            for (int i = 0; i < followers.Count(); i++)
            {
                try
                {
                    userlist += User.GetUserFromId(followers.ElementAt(i)).ScreenName + "\r\n";
                }
                //Exception occures, if you make to many api calls. So let's wait for a bit.
                catch(NullReferenceException ex)
                {
                    Console.Write(ex.ToString());
                    System.Threading.Thread.Sleep(60000);
                    i--;
                }             
            }

            var followings = authenticatedUser.GetFriendIds();
            for (int i = 0; i < followings.Count(); i++)
            {
                try
                {
                    userlist += User.GetUserFromId(followings.ElementAt(i)).ScreenName + "\r\n";
                }
                //Exception occures, if you make to many api calls. So let's wait for a bit.
                catch (NullReferenceException ex)
                {
                    Console.Write(ex.ToString());
                    System.Threading.Thread.Sleep(60000);
                    i--;
                } 
            }

            //Write userlist to file
            System.IO.File.WriteAllText(@"users.csv", userlist);
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
