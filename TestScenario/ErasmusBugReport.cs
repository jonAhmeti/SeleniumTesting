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
    class ErasmusBugReport
    {
        private Driver driver;

        [SetUp]
        public void InitialSetup()
        {
            driver = new Driver("chrome");
            driver.WebDriver.Navigate().GoToUrl(ConfigData.Erasmus.BaseUrl + "help/report-bug_en");
            if (driver.WebDriver.GetType().Name == "ChromeDriver")
                Thread.Sleep(3000);
        }

        [TearDown]
        public void EndTest()
        {
            if (TestContext.CurrentContext.Result.Outcome == ResultState.Success)
            {
                Actions.TakeScreenshot(driver.WebDriver, TestContext.CurrentContext.Test.MethodName, ConfigData.Erasmus.ScreenshotDir);
            }
            else
            {
                Actions.TakeScreenshot(driver.WebDriver,"Failed_" + TestContext.CurrentContext.Test.MethodName, ConfigData.Erasmus.ScreenshotDir);
            }
        }

        [Test]
        public void WithValidValues([Values(ConfigData.Erasmus.BugInfo.BugType)]
            string bugType, [Values(ConfigData.Erasmus.BugInfo.BugFoundUrl)]
            string bugFoundUrl, [Values(ConfigData.Erasmus.BugInfo.BugDetails)]
            string bugDetails)
        {
            var bugCategory =
                driver.WebDriver.FindElement(By.Id("edit-submitted-what-kind-of-bug-problem-have-you-found"));
            SelectElement bugCategoryDropdown = new SelectElement(bugCategory);
            bugCategoryDropdown.SelectByText(bugType);

            driver.WebDriver.FindElement(By.Id("edit-submitted-please-copy-paste-the-link")).SendKeys(bugFoundUrl);
            driver.WebDriver.FindElement(By.Id("edit-submitted-please-describe-the-bug")).SendKeys(bugDetails);
            //((IJavaScriptExecutor) driver.WebDriver).ExecuteScript("document.getElementsByName('op')[0].removeAttribute('disabled')");

            //submit button
            driver.WebDriver.FindElement(By.CssSelector(
                    "#webform-client-form-2138 > div > div.form-item.webform-component.webform-component-markup.webform-component--enabledsudo.form-group > p > button"))
                .Click();

            Thread.Sleep(5000);
            Assert.IsTrue(driver.WebDriver.PageSource.Contains("Bug Reported"));
            ConfigData.ScrollTo(driver.WebDriver.FindElement(By.XPath("//span[text()='Bug Reported']")), driver.WebDriver);
        }

        [Test]
        public void WithNoCategory([Values(ConfigData.Erasmus.BugInfo.BugFoundUrl)]
            string bugFoundUrl, [Values(ConfigData.Erasmus.BugInfo.BugDetails)]
            string bugDetails)
        {
            driver.WebDriver.FindElement(By.Id("edit-submitted-please-copy-paste-the-link")).SendKeys(bugFoundUrl);
            driver.WebDriver.FindElement(By.Id("edit-submitted-please-describe-the-bug")).SendKeys(bugDetails);

            var beforeSubmitPageSrc = driver.WebDriver.PageSource;
            //submit button
            driver.WebDriver.FindElement(By.CssSelector(
                    "#webform-client-form-2138 > div > div.form-item.webform-component.webform-component-markup.webform-component--enabledsudo.form-group > p > button"))
                .Click();
            Thread.Sleep(1000);
            var afterSubmitPageSrc = driver.WebDriver.PageSource;
            Assert.AreEqual(beforeSubmitPageSrc,afterSubmitPageSrc);
        }

        [Test]
        public void WithInvalidUrl([Values(ConfigData.Erasmus.BugInfo.BugType)]
            string bugType, [Values(ConfigData.Erasmus.BugInfo.BugDetails)]
            string bugDetails)
        {
            var bugCategory =
                driver.WebDriver.FindElement(By.Id("edit-submitted-what-kind-of-bug-problem-have-you-found"));
            SelectElement bugCategoryDropdown = new SelectElement(bugCategory);
            bugCategoryDropdown.SelectByText(bugType);

            driver.WebDriver.FindElement(By.Id("edit-submitted-please-copy-paste-the-link")).SendKeys("https://google.com/Invalid/Url");
            driver.WebDriver.FindElement(By.Id("edit-submitted-please-describe-the-bug")).SendKeys(bugDetails);
            //((IJavaScriptExecutor) driver.WebDriver).ExecuteScript("document.getElementsByName('op')[0].removeAttribute('disabled')");

            //submit button
            var submitButton = driver.WebDriver.FindElement(By.CssSelector(
                "#webform-client-form-2138 > div > div.form-item.webform-component.webform-component-markup.webform-component--enabledsudo.form-group > p > button"));

            Assert.IsFalse(submitButton.Enabled);
            ConfigData.ScrollTo(driver.WebDriver.FindElement(By.Id("edit-submitted-please-copy-paste-the-link")), driver.WebDriver);
        }
    }
}
