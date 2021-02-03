using System;
using System.Collections.Generic;
using System.Text;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;

namespace TestScenarios_JonAhmeti
{
    public class Driver
    {
        public Driver(string driverType)
        {
            switch (driverType)
            {
                case "Chrome":
                case "chrome":
                    WebDriver = new ChromeDriver();
                    break;
                case "Firefox":
                case "firefox":
                    WebDriver = new FirefoxDriver();
                    break;
                default:
                    WebDriver = new FirefoxDriver();
                    break;
            }
        }
        public IWebDriver WebDriver { get; set; }
    }
}
