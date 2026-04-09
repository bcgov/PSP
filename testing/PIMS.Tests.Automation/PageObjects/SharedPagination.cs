using OpenQA.Selenium;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedPagination : PageObjectBase
    {
        private readonly By searchListResetButton = By.Id("reset-button");

        private readonly By searchTableEntriesSpan = By.CssSelector("input[data-testid='input-page-size']");
        private readonly By searchTablePagination5 = By.CssSelector("div[class='Menu-options scrollable list-group'] div[title='menu-item-5']");
        private readonly By searchTablePagination10 = By.CssSelector("div[title='menu-item-10']");
        private readonly By searchTablePagination20 = By.CssSelector("div[title='menu-item-20']");
        private readonly By searchTablePagination50 = By.CssSelector("div[title='menu-item-50']");
        private readonly By searchTablePagination100 = By.CssSelector("div[title='menu-item-100']");

        private readonly By searchTableNextPageButton = By.CssSelector("ul[class='pagination'] li:last-child");
        private readonly By searchTable1stPageButton = By.CssSelector("ul[class='pagination'] li:nth-child(2)");

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
                    //WaitUntilClickable(searchTablePagination5);
                    FocusAndClick(searchTablePagination5);
                    break;
                case 10:
                    //WaitUntilVisible(searchTablePagination10);
                    FocusAndClick(searchTablePagination10);
                    break;
                case 20:
                    //WaitUntilClickable(searchTablePagination20);
                    FocusAndClick(searchTablePagination20);
                    break;
                case 50:
                    //WaitUntilClickable(searchTablePagination50);
                    FocusAndClick(searchTablePagination50);
                    break;
                case 100:
                    //WaitUntilClickable(searchTablePagination100);
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
