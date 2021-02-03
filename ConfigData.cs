using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;

namespace TestScenarios_JonAhmeti
{
    public static class ConfigData
    {

        public static void ScrollTo(IWebElement element, IWebDriver driver)
        {
            ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView();", element);
        }

        //You need \{Folder Name} added at the end of the directory for each Test Scenario.
        public const string ScreenshotDir = @"C:\Users\Jon\Desktop\TestScreenshots";
        public static class Twitter
        {
            public const string ScreenshotDir = "Twitter";
            public const string BaseUrl = @"https://twitter.com";

            public const string TweetText = "This is an automated test using NUnit, creation of a text tweet";
            public const string CorruptMediaPath = @"C:\Users\Jon\Desktop\TestScreenshots\Twitter\CorruptImage.PNG";
            public const string CorruptMediaResult = "Some of your media failed to load.";

            public static class Credentials
            {
                public static class Valid
                {
                    public const string Username = "notmyusername";
                    public const string Password = "notmypassword";
                }
            }

            public static class Messaging
            {
                public const string MessagesUrl = "https://twitter.com/messages/";
                public const string User = "@TwitterSupport";
                public const string Text = "Messaging " + User + " for automated testing purposes.";
            }
        }

        public static class Erasmus
        {
            public const string ScreenshotDir = "Erasmus";
            public const string BaseUrl = @"https://ec.europa.eu/programmes/erasmus-plus/";

            public static class BugInfo
            {
                public const string BugType = "Other";
                public const string BugFoundUrl = "https://ec.europa.eu/programmes/erasmus-plus/help/report-bug_en";

                public const string BugDetails =
                    "Sorry, this is NOT a bug.\nJust using NUnit testing for one of my university's projects.\nApologies for the flood.";
            }
        }

        public static class Trello
        {
            public const string ScreenshotDir = "Trello";
            public const string BaseUrl = @"https://trello.com";
            public const string BoardUrl = @"https://trello.com/b/7qCasqZc/riinvest";

            public static class GoogleCredentials
            {
                public const string Email = "jon.ahmeti@riinvest.net";
                public const string Password = "password";
            }
            public static class List
            {
                public const string ListName = "NUnit Test List";
                public const string CardTitle = "NUnit Test Card Title";

                //the "\r" and "\n" are used in Trello's Card's Description, we need it for each new line if we want to compare the text
                public const string CardDetails = "This is an automated test case - using:" +
                                                  "\r\n.NET Core" +
                                                  "\r\nNUnit v3.13.0" +
                                                  "\r\nNUnit3TestAdapter v3.17.0" +
                                                  "\r\nSelenium WebDriver" +
                                                  "\r\nSelenium Support";
            }
        }
    }
}
