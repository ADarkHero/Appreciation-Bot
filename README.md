# Ist-Halt-Kein-Bot
Generates random Tweets with the Steam API.

The tweets normally look like the following: "{Game1} ist halt kein {Game2}.", but you can change them to your will of course.



## How to run the program
To get the program going, download the latest release and change your "CONSUMER_KEY", "CONSUMER_SECRET", "ACCESS_TOKEN" and "ACCESS_TOKEN_SECRET" in the source code. You can get them at [the Twitter-apps page](https://apps.twitter.com/).

You'll also need [Json.NET / Newtonsoft.Json](https://www.newtonsoft.com/json) and [Tweetinvi](https://github.com/linvi/tweetinvi). You can install both via NuGet in Visual Studio.

You can use the task scheduler from Windows, to generate tweets on a regular basis.
