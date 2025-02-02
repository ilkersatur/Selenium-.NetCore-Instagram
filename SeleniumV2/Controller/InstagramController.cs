using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Interactions;
using SeleniumV2.Model;
using SeleniumV2.MouseMethod;
using System.Collections.ObjectModel;

namespace SeleniumV2.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstagramController : ControllerBase
    {
        private readonly IWebDriver _webDriver;

        public InstagramController()
        {
            _webDriver = new ChromeDriver();
        }

        [HttpGet]
        public IActionResult Instagram()
        {
            _webDriver.Manage().Window.Maximize();

            #region Prop

            ProfileInfo profileInfo = new ProfileInfo();
            Followers followers = new() { Follower = new List<string>() };

            #endregion

            if (string.IsNullOrEmpty(username) && string.IsNullOrEmpty(password))
            {
                throw new Exception("invalid username and password");
            }
            _webDriver.Navigate().GoToUrl("https://www.instagram.com/");

            IWebElement usernameField = _webDriver.FindElement(By.Name("username"));
            IWebElement passwordField = _webDriver.FindElement(By.Name("password"));

            usernameField.SendKeys(username);
            passwordField.SendKeys(password);

            IWebElement loginButton = _webDriver.FindElement(By.XPath("//*[@id=\"loginForm\"]/div[1]/div[3]/button"));

            loginButton.Click();

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

            x = true;

            username = "umiit.koc";

            _webDriver.Navigate().GoToUrl("https://www.instagram.com/" + username);

            while (x)
            {
                ReadOnlyCollection<IWebElement> span_elements = _webDriver.FindElements(By.CssSelector("span.html-span.xdj266r.x11i5rnm.xat24cr.x1mh8g0r.xexx8yu.x4uap5.x18d9i69.xkhd6sd.x1hl2dhg.x16tdsg8.x1vvkbs"));

                if (span_elements.Count > 0)
                {
                    profileInfo.Posts = span_elements[0].Text;
                    profileInfo.Followers = span_elements[1].Text;
                    profileInfo.Following = span_elements[2].Text;
                    x = false;
                }
            }

            x = true;

            IWebElement followingButton = _webDriver.FindElement(By.XPath("//a[@href='/" + username + "/following/']"));
            followingButton.Click();

            Thread.Sleep(3500);

            //ScrollDown Start

            MiddleClick middleClick = new MiddleClick();

            middleClick.MiddleClickAndBottomAction();

            //ScrollDown End

            Thread.Sleep(10000);

            //Takipçi Listeleme Start

            int sayac = 1;

            IWebElement follwersDiv = _webDriver.FindElement(By.XPath("/html/body/div[5]/div[2]/div/div/div[1]/div/div[2]/div/div/div/div/div[2]/div/div/div[3]/div[1]"));

            IReadOnlyCollection<IWebElement> follwers = follwersDiv.FindElements(By.CssSelector("span._ap3a._aaco._aacw._aacx._aad7._aade"));

            foreach (IWebElement follower in follwers)
            {
                string text = ((WebElement)follower).Text;

                followers.Follower.Add(text);
                sayac++;
            }

            Console.WriteLine(followers);
            Console.WriteLine(sayac);

            _webDriver.Quit();
            return Ok();
        }

    }
}
