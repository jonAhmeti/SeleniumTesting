using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NuGet.Frameworks;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.Extensions;

namespace TestScenarios_JonAhmeti.TestScenario
{
    class TweetCreation
    {
        private Driver driver;

        [SetUp]
        public void InitialSetup()
        {
            driver = new Driver("chrome");
            driver.WebDriver.Navigate().GoToUrl(ConfigData.Twitter.BaseUrl);
            Thread.Sleep(3000);
            Login(ConfigData.Twitter.Credentials.Valid.Username, ConfigData.Twitter.Credentials.Valid.Password);
        }

        [TearDown]
        public void EndTest()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                Actions.TakeScreenshot(driver.WebDriver, TestContext.CurrentContext.Test.MethodName, ConfigData.Twitter.ScreenshotDir);
            }
            else
            {
                Actions.TakeScreenshot(driver.WebDriver, "Failed_" + TestContext.CurrentContext.Test.MethodName, ConfigData.Twitter.ScreenshotDir);
            }
        }

        public void Login(string username, string password)
        {
            //login button from homepage
            driver.WebDriver.FindElement(By.CssSelector("a[data-testid='loginButton']")).Click();
            //username input
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);
            driver.WebDriver.FindElement(By.CssSelector("input[name='session[username_or_email]']")).SendKeys(username);
            driver.WebDriver.FindElement(By.CssSelector("input[name='session[password]']")).SendKeys(password);
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='LoginForm_Login_Button']")).Click();
            Thread.Sleep(3000);
        }

        public void ScrollTo(IWebElement element)
        {
            ConfigData.ScrollTo(element, driver.WebDriver);
            ((IJavaScriptExecutor)driver.WebDriver).ExecuteScript("window.scrollBy(0,-65);");
        }

        [Test]
        public void TextTweet([Values(ConfigData.Twitter.TweetText)]string text)
        {
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='tweetTextarea_0']")).SendKeys(text);
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='tweetButtonInline']")).Click();
            driver.WebDriver.FindElement(By.CssSelector("a[aria-label='Profile']")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);
            var tweetedText = driver.WebDriver.FindElement(By.XPath($"//span[text()='{ConfigData.Twitter.TweetText}']"));
            Assert.IsTrue(tweetedText.Text == ConfigData.Twitter.TweetText);
            ScrollTo(tweetedText);
        }

        [Test]
        public void GifTweet()
        {
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);

            driver.WebDriver.FindElement(By.CssSelector("div[aria-label='Add a GIF']")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);

            string gifModal =
                "div.css-1dbjc4n.r-1867qdf.r-1wbh5a2.r-kwpbio.r-rsyp9y.r-1pjcn9w.r-1279nm1.r-htvplk.r-1udh08x.r-1potc6q";
            driver.WebDriver.FindElement(By.CssSelector($"{gifModal} input")).SendKeys("Testing");
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(5000);

            driver.WebDriver.FindElement(By.CssSelector($"{gifModal} img")).Click();
            driver.WebDriver.FindElement(By.CssSelector("div[data-testid='tweetButtonInline']")).Click();


            driver.WebDriver.FindElement(By.CssSelector("a[aria-label='Profile']")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(5000);
            var tweetGif = driver.WebDriver.FindElement(By.CssSelector("div[data-testid='videoPlayer'] video"));
            var sourceAttribute = tweetGif.GetAttribute("src");
            string gifSourceSupposedly = "https://video.twimg.com/tweet_video/";
            Assert.IsTrue(sourceAttribute.StartsWith(gifSourceSupposedly));
            ScrollTo(tweetGif);
        }

        [Test]
        public void CorruptMediaTweet([Values(ConfigData.Twitter.CorruptMediaPath)]
            string fileUploadPath, [Values(ConfigData.Twitter.CorruptMediaResult)]
            string toastResult)
        {
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);

            IWebElement uploadMediaElement =
                driver.WebDriver.FindElement(By.CssSelector("input[data-testid='fileInput']"));
            uploadMediaElement.SendKeys(fileUploadPath);
            IWebElement alertElement =
                driver.WebDriver.FindElement(By.CssSelector("#layers div[data-testid='toast'] span"));
            Assert.AreEqual(toastResult, alertElement.Text);
            ScrollTo(alertElement);
        }
    }
}
