using Microsoft.AspNetCore.Mvc;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V129.ServiceWorker;
using OpenQA.Selenium.Support.UI;
using SeleniumV2.Model;
using SeleniumV2.MouseMethod;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SeleniumV2.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class Instagram2Controller : ControllerBase
    {
        private readonly IWebDriver _webDriver;

        private readonly WebDriverWait _wait;

        public Instagram2Controller()
        {
            _webDriver = new ChromeDriver();

            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(15));
        }

        [HttpGet]
        public Followers Instagram(string username, string password)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            stopwatch.Start();

            _webDriver.Manage().Window.Maximize();

            _webDriver.Navigate().GoToUrl("https://www.instagram.com/");

            _wait.Until(driver => driver.FindElement(By.Name("username"))).SendKeys(username);
            _wait.Until(driver => driver.FindElement(By.Name("password"))).SendKeys(password);

            _wait.Until(driver => driver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[1]/div[3]/button"))).Click();

            //_wait.Until(driver => driver.FindElements(By.XPath("//div[text()='Not now']")))[0].Click();

            bool x = true;

            while (x)
            {
                ReadOnlyCollection<IWebElement> elements = _webDriver.FindElements(By.XPath("//div[text()='Not now']"));

                if (elements.Count > 0)
                {
                    elements[0].Click();
                    x = false;
                }
            }

            username = "****";

            //26 sekme aç
            for (int i = 0; i < 26; i++)
            {
                ((IJavaScriptExecutor)_webDriver).ExecuteScript("window.open('https://www.instagram.com/" + username + "');");
            }

            //Tüm sekmeleri listeye dönüştür
            IReadOnlyCollection<string> windowHandles = _webDriver.WindowHandles.ToList();

            bool isFirstHandle = true;

            foreach (var handle in windowHandles)
            {

                _webDriver.SwitchTo().Window(handle);

                if (isFirstHandle)
                {
                    _webDriver.Close();
                    isFirstHandle = false;
                    continue;
                }

                _wait.Until(driver => driver.FindElement(By.XPath("//a[@href='/" + username + "/following/']"))).Click();

            }

            //Tüm sekmeleri listeye dönüştür
            IReadOnlyCollection<string> followingWindowHandles = _webDriver.WindowHandles.ToList();

            string alphabet = "abcdefghijklmnopqrstuvwxyz";

            int index = 0;

            foreach (var handle in followingWindowHandles)
            {
                _webDriver.SwitchTo().Window(handle);
                _wait.Until(driver => driver.FindElement(By.CssSelector("input[placeholder='Search']"))).SendKeys(alphabet[index].ToString());
                index++;
            }

            HashSet<string> followingList = new HashSet<string>();


            foreach (var handle in followingWindowHandles)
            {
                _webDriver.SwitchTo().Window(handle);

                IWebElement follwersDiv = _wait.Until(driver => driver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div/div[1]/div/div[2]/div/div/div/div/div[2]/div/div/div[3]/div[1]")));

                IReadOnlyCollection<IWebElement> follwers = follwersDiv.FindElements(By.CssSelector("span._ap3a._aaco._aacw._aacx._aad7._aade"));

                foreach (IWebElement follower in follwers)
                {
                    string text = ((WebElement)follower).Text;

                    followingList.Add(text);
                }

            }

            _webDriver.Quit();

            Followers followers = new() { Follower = new List<string>() };

            followers.Follower.AddRange(followingList);

            followers.FollowerCount = followingList.Count;

            stopwatch.Stop();

            followers.Duration = stopwatch.Elapsed;

            return followers;
        }

    }
}
