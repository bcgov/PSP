﻿using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedPagination : PageObjectBase
    {
        //Search Acquisition File Pagination

        private By searchListResetButton = By.Id("reset-button");

        private By searchTableEntriesSpan = By.XPath("//span[contains(text(),'Entries')]");
        private By searchTablePagination5 = By.CssSelector("div[title='menu-item-5']");
        private By searchTablePagination10 = By.CssSelector("div[title='menu-item-10']");
        private By searchTablePagination20 = By.CssSelector("div[title='menu-item-20']");
        private By searchTablePagination50 = By.CssSelector("div[title='menu-item-50']");
        private By searchTablePagination100 = By.CssSelector("div[title='menu-item-100']");

        private By searchTableNextPageButton = By.CssSelector("ul[class='pagination'] li:last-child");
        private By searchTable1stPageButton = By.CssSelector("ul[class='pagination'] li:nth-child(2)");


        public SharedPagination(IWebDriver webDriver) : base(webDriver)
        {}

        public void ChoosePaginationOption(int pagination)
        {
            Wait();

            WaitUntilVisible(searchTableEntriesSpan);
            FocusAndClick(searchTableEntriesSpan);

            switch (pagination)
            {
                case 5:
                    WaitUntilVisible(searchTablePagination5);
                    FocusAndClick(searchTablePagination5);
                    break;
                case 10:
                    WaitUntilVisible(searchTablePagination10);
                    FocusAndClick(searchTablePagination10);
                    break;
                case 20:
                    WaitUntilClickable(searchTablePagination20);
                    FocusAndClick(searchTablePagination20);
                    break;
                case 50:
                    WaitUntilClickable(searchTablePagination50);
                    FocusAndClick(searchTablePagination50);
                    break;
                case 100:
                    WaitUntilClickable(searchTablePagination100);
                    FocusAndClick(searchTablePagination100);
                    break;
            }
        }

        public void GoNextPage()
        {
            webDriver.FindElement(searchTableNextPageButton).Click();
        }

        public void Go1stPage()
        {
            webDriver.FindElement(searchTable1stPageButton).Click();

        }

        public void ResetSearch()
        {
            WaitUntilClickable(searchListResetButton);
            webDriver.FindElement(searchListResetButton).Click();
        }
    }
}
