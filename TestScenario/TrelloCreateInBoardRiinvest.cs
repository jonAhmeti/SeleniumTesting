using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace TestScenarios_JonAhmeti.TestScenario
{
    class TrelloCreateInBoardRiinvest
    {
        private Driver driver;

        [SetUp]
        public void InitialSetup()
        {
            driver = new Driver("chrome");
            driver.WebDriver.Navigate().GoToUrl(ConfigData.Trello.BaseUrl + "/login");
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);

            driver.WebDriver.FindElement(By.Id("googleButton")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);

            driver.WebDriver.FindElement(By.Id("identifierId")).SendKeys(ConfigData.Trello.GoogleCredentials.Email);
            driver.WebDriver.FindElement(By.Id("identifierNext")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(2000);

            driver.WebDriver.FindElement(By.CssSelector("#password input")).SendKeys(ConfigData.Trello.GoogleCredentials.Password);
            driver.WebDriver.FindElement(By.Id("passwordNext")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(10000);

            driver.WebDriver.FindElement(By.CssSelector(@"ul.boards-page-board-section-list a[href='/b/7qCasqZc/riinvest']")).Click();
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);
        }

        [TearDown]
        public void EndTest()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                Actions.TakeScreenshot(driver.WebDriver, TestContext.CurrentContext.Test.MethodName, ConfigData.Trello.ScreenshotDir);
            }
            else
            {
                Actions.TakeScreenshot(driver.WebDriver,"Failed_" + TestContext.CurrentContext.Test.MethodName, ConfigData.Trello.ScreenshotDir);
            }
        }

        [Test]
        public void CreateList([Values(ConfigData.Trello.List.ListName)] string listName)
        {
            driver.WebDriver.FindElement(By.ClassName("open-add-list")).Click();
            Thread.Sleep(100);
            driver.WebDriver.FindElement(By.CssSelector(".js-add-list input.list-name-input")).SendKeys(listName);
            driver.WebDriver.FindElement(By.ClassName("mod-list-add-button")).Click();
            Thread.Sleep(100);
            var listTextarea = driver.WebDriver.FindElement(By.CssSelector($"textarea[aria-label='{listName}']"));
            
            Assert.IsTrue(listTextarea.Text == listName);
            ConfigData.ScrollTo(listTextarea, driver.WebDriver);
        }

        [Test]
        public void AddCardToList([Values(ConfigData.Trello.List.ListName)]
            string listName, [Values(ConfigData.Trello.List.CardTitle)]
            string cardTitle)
        {
            var lists = driver.WebDriver.FindElements(By.CssSelector(".js-list.list-wrapper"));
            foreach (var list in lists)
            {
                if (list.Text.Split("\r\n")[0] == listName)
                {
                    list.FindElement(By.CssSelector(".open-card-composer.js-open-card-composer")).Click();
                    var cardComposerElement = list.FindElement(By.CssSelector("div.card-composer"));
                    cardComposerElement.FindElement(By.TagName("textarea")).SendKeys(cardTitle);
                    cardComposerElement.FindElement(By.CssSelector(".cc-controls input")).Click();
                }
            }
        }

        [Test]
        public void AddDescriptionToCard([Values(ConfigData.Trello.List.ListName)] string listName, [Values(ConfigData.Trello.List.CardTitle)]
            string cardTitle, [Values(ConfigData.Trello.List.CardDetails)]
            string cardDetails)
        {
            var lists = driver.WebDriver.FindElements(By.CssSelector(".js-list.list-wrapper"));
            foreach (var list in lists)
            {
                if (list.Text.Split("\r\n")[0] != "TS5") continue;

                var cards = list.FindElements(By.CssSelector("div.list-cards.js-list-cards a"));
                foreach (var card in cards)
                {
                    if (card.Text != cardTitle) continue;

                    card.Click();
                    Thread.Sleep(1000);
                    driver.WebDriver.FindElement(By.CssSelector(".u-gutter .editable")).Click();
                        
                    var descriptionText = driver.WebDriver.FindElement(By.CssSelector(".description-edit textarea"));
                    descriptionText.SendKeys(cardDetails);
                    driver.WebDriver.FindElement(By.CssSelector(".edit-controls input")).Click();

                    Thread.Sleep(1000);
                    var descriptionAfterSave = driver.WebDriver.FindElement(By.CssSelector(".description-content p"));
                    var x = descriptionAfterSave.Text;
                    Assert.IsTrue(descriptionAfterSave.Text == cardDetails);
                }
            }
        }
    }
}
