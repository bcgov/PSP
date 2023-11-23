﻿using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedPagination : PageObjectBase
    {
        //Search Acquisition File Pagination

        private By searchTableEntriesSpan = By.XPath("//span[contains(text(),'Entries')]");
        private By searchTablePagination5 = By.CssSelector("div[title='menu-item-5']");
        private By searchTablePagination10 = By.CssSelector("div[title='menu-item-10']");
        private By searchTablePagination20 = By.CssSelector("div[title='menu-item-20']");
        private By searchTablePagination50 = By.CssSelector("div[title='menu-item-50']");
        private By searchTablePagination100 = By.CssSelector("div[title='menu-item-100']");

        public SharedPagination(IWebDriver webDriver) : base(webDriver)
        {}

        public void ChoosePaginationOption(int pagination)
        {
            Wait(2000);
            webDriver.FindElement(searchTableEntriesSpan).Click();

            switch (pagination)
            {
                case 5:
                    WaitUntilVisible(searchTablePagination5);
                    webDriver.FindElement(searchTablePagination5).Click();
                    break;
                case 10:
                    WaitUntilVisible(searchTablePagination10);
                    webDriver.FindElement(searchTablePagination10).Click();
                    break;
                case 20:
                    WaitUntilClickable(searchTablePagination20);
                    webDriver.FindElement(searchTablePagination20).Click();
                    break;
                case 50:
                    WaitUntilClickable(searchTablePagination50);
                    webDriver.FindElement(searchTablePagination50).Click();
                    break;
                case 100:
                    WaitUntilClickable(searchTablePagination100);
                    webDriver.FindElement(searchTablePagination100).Click();
                    break;
            }
        }
    }
}
