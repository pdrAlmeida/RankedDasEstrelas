using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;

namespace RankedDasEstrelas.Selenium.WebDriver
{
    public static class SeleniumWebDriver
    {
        public static IWebDriver WebDriver { get; set; }

        public static void InitializeWebDriver()
        {
            try
            {
                if (WebDriver != null) return;

                var chromeOptions = new ChromeOptions
                {
                    PageLoadStrategy = PageLoadStrategy.Eager,
                    
                };
                chromeOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--disable-gpu");

                WebDriver = new ChromeDriver($"{AppDomain.CurrentDomain.BaseDirectory}driver", chromeOptions, TimeSpan.FromMinutes(5));
                WebDriver.Manage().Timeouts().PageLoad = TimeSpan.FromMinutes(10);
                WebDriver.Manage().Window.Maximize();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro: " + ex.ToString());
                QuitWebDriver();
            }
        }

        public static void QuitWebDriver()
        {
            if (WebDriver != null)
            {
                WebDriver.Close();
                WebDriver.Quit();
                WebDriver.Dispose();
                WebDriver = null;
            }
        }
    }
}