using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;

namespace TestScenarios_JonAhmeti.TestScenario
{
    class TwitterMessaging
    {
        private Driver driver;

        [SetUp]
        public void InitialSetup()
        {
            driver = new Driver("chrome");
            driver.WebDriver.Navigate().GoToUrl(ConfigData.Twitter.Messaging.MessagesUrl);
            Thread.Sleep(1000);
            if (!driver.WebDriver.Url.Contains("message"))
                Thread.Sleep(2000);

            Login(ConfigData.Twitter.Credentials.Valid.Username, ConfigData.Twitter.Credentials.Valid.Password);
            Thread.Sleep(2000);
        }

        [TearDown]
        public void EndTest()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                Actions.TakeScreenshot(driver.WebDriver, TestContext.CurrentContext.Test.MethodName,
                    ConfigData.Twitter.ScreenshotDir);
            }
            else
            {
                Actions.TakeScreenshot(driver.WebDriver, "Failed_" + TestContext.CurrentContext.Test.MethodName,
                    ConfigData.Twitter.ScreenshotDir);
            }
        }

        public void Login(string username, string password)
        {
            driver.WebDriver.FindElement(By.CssSelector("input[name='session[username_or_email]']")).SendKeys(username);
            driver.WebDriver.FindElement(By.CssSelector("input[name='session[password]']")).SendKeys(password);
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='LoginForm_Login_Button']")).Click();
        }

        public void FindUser(string user)
        {
            driver.WebDriver.FindElement(By.CssSelector("a[data-testid='NewDM_Button']")).Click();
            Thread.Sleep(500);
            var searchbar = driver.WebDriver.FindElement(By.CssSelector("input[data-testid='searchPeople']"));
            searchbar.SendKeys(user);
            Thread.Sleep(500);
            var usersList = driver.WebDriver.FindElements(By.CssSelector("form div[role='checkbox']"));

            //Might need to increase this
            Thread.Sleep(1000);

            foreach (var checkbox in usersList)
            {
                var spanElements = checkbox.FindElements(By.CssSelector("span"));
                foreach (var span in spanElements)
                {
                    if (span.Text == user)
                    {
                        checkbox.Click();
                    }
                }
            }

            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='nextButton']")).Click();
            Thread.Sleep(1000);
        }


        [Test]
        public void NewChatMessage([Values(ConfigData.Twitter.Messaging.User)]
            string user, [Values(ConfigData.Twitter.Messaging.Text)]
            string text)
        {
            FindUser(user);
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='dmComposerTextInput']")).SendKeys(text);
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='dmComposerSendButton']")).Click();
            Thread.Sleep(1000);

            var messages = driver.WebDriver.FindElements(By.CssSelector("div[data-testid='messageEntry']"));

            foreach (var msg in messages)
            {
                var spans = msg.FindElements(By.TagName("span"));
                var lastSpan = spans[^1]; //index from end
                if (lastSpan.Text != text) continue;

                Assert.True(lastSpan.Text == text);
                ConfigData.ScrollTo(lastSpan, driver.WebDriver);
            }
        }

        [Test]
        public void SendEmojis([Values(ConfigData.Twitter.Messaging.User)]
            string user)
        {
            FindUser(user);
            driver.WebDriver.FindElement(By.CssSelector("div[aria-label='Add emoji']")).Click();
            Thread.Sleep(2000);
            var emojiMenu = driver.WebDriver.FindElement(By.Id("emoji_picker_categories_dom_id"));
            var rowsHolder = emojiMenu.FindElements(By.CssSelector(
                "div:first-child > div:nth-child(3) > div:first-child > div:nth-child(3) > div:first-child > div:nth-child(2) > div:first-child"));

            byte rowNum = 0;
            foreach (var row in rowsHolder)
            {
                foreach (var div in row.FindElements(By.TagName("div")))
                {
                    if (rowNum == 10) break;
                    div.Click();
                    rowNum++;
                }
            }

            driver.WebDriver.FindElement(By.XPath("//*[@id='layers']/div[2]/div/div/div[1]")).Click();
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='dmComposerSendButton']")).Click();
        }
    }
}
