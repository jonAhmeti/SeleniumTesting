using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestScenarios_JonAhmeti
{
    public static class Actions
    {
        public static void TakeScreenshot(IWebDriver webDriver, string fileName, string location = "NotDefined")
        {
            Screenshot screenshot = ((ITakesScreenshot) webDriver).GetScreenshot();
            screenshot.SaveAsFile($@"{ConfigData.ScreenshotDir}\{location}\{fileName}.png", ScreenshotImageFormat.Png);
        }
    }
}
