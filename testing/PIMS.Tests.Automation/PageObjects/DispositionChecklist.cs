using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;


namespace PIMS.Tests.Automation.PageObjects
{
    public class DispositionChecklist : PageObjectBase
    {
        private By checklistLinkTab = By.XPath("//a[contains(text(),'Checklist')]");

        private By checklistEditBttn = By.CssSelector("button[title='Edit checklist']");
        private By checklistInfo = By.XPath("//div/em[contains(text(),'This checklist was last updated')]");

        //File Initiation Section View Elements
        private By checklistFileInitiationTitle = By.XPath("//h2/div/div[contains(text(),'File Initiation')]");
        private By checklistFileInitiation1Label = By.CssSelector("div[data-testid='File Initiation'] div:first-child div:nth-child(1) label");
        private By checklistFileInitiation1Content = By.CssSelector("div[data-testid='File Initiation'] div:first-child div:nth-child(1) div:nth-child(2) span");
        private By checklistFileInitiation2Label = By.CssSelector("div[data-testid='File Initiation'] div div:nth-child(2) label");
        private By checklistFileInitiation2Content = By.CssSelector("div[data-testid='File Initiation'] div:first-child div:nth-child(2) div:nth-child(2) span");
        private By checklistFileInitiation3Label = By.CssSelector("div[data-testid='File Initiation'] div div:nth-child(3) label");
        private By checklistFileInitiation3Content = By.CssSelector("div[data-testid='File Initiation'] div div:nth-child(3) div:nth-child(2) div:nth-child(2) span");
        private By checklistFileInitiation4Label = By.CssSelector("div[data-testid='File Initiation'] div div:nth-child(4) label");
        private By checklistFileInitiation4Content = By.CssSelector("div[data-testid='File Initiation'] div div:nth-child(4) div:nth-child(2) div:nth-child(2) span");

        //Disposition Preparation section View Elements
        private By checklistDispositionPreparationTitle = By.XPath("//h2/div/div[contains(text(),'Disposition Preparation')]");
        private By checklistDispositionPreparation1Label = By.CssSelector("div[data-testid='Disposition Preparation'] div div:nth-child(1) label");
        private By checklistDispositionPreparation1Content = By.CssSelector("div[data-testid='Disposition Preparation'] div:first-child div:nth-child(1) div:nth-child(2) span");
        private By checklistDispositionPreparation2Label = By.CssSelector("div[data-testid='Disposition Preparation'] div div:nth-child(2) label");
        private By checklistDispositionPreparation2Content = By.CssSelector("div[data-testid='Disposition Preparation'] div:first-child div:nth-child(2) div:nth-child(2) span");
        private By checklistDispositionPreparation3Label = By.CssSelector("div[data-testid='Disposition Preparation'] div div:nth-child(3) label");
        private By checklistDispositionPreparation3Content = By.CssSelector("div[data-testid='Disposition Preparation'] div div:nth-child(3) div:nth-child(2) div:nth-child(2) span");
        private By checklistDispositionPreparation4Label = By.CssSelector("div[data-testid='Disposition Preparation'] div div:nth-child(4) label");
        private By checklistDispositionPreparation4Content = By.CssSelector("div[data-testid='Disposition Preparation'] div div:nth-child(4) div:nth-child(2) div:nth-child(2) span");

        //Referrals and Consultations section View Elements
        private By checklistReferralsAndConsultationsTitle = By.XPath("//h2/div/div[contains(text(),'Referrals and Consultations')]");
        private By checklistReferralsAndConsultations1lLabel = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(1) label");
        private By checklistReferralsAndConsultations1Content = By.CssSelector("div[data-testid='Referrals and Consultations'] div:first-child div:nth-child(1) div:nth-child(2) span");
        private By checklistReferralsAndConsultations2Label = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(2) label");
        private By checklistReferralsAndConsultations2Content = By.CssSelector("div[data-testid='Referrals and Consultations'] div:first-child div:nth-child(2) div:nth-child(2) span");
        private By checklistReferralsAndConsultations3Label = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(3) label");
        private By checklistReferralsAndConsultations3Content = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(3) div:nth-child(2) div:nth-child(2) span");
        private By checklistReferralsAndConsultations4Label = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(4) label");
        private By checklistReferralsAndConsultations4Content = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(4) div:nth-child(2) div:nth-child(2) span");
        private By checklistReferralsAndConsultations5Label = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(5) label");
        private By checklistReferralsAndConsultations5Content = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(5) div:nth-child(2) div:nth-child(2) span");
        private By checklistReferralsAndConsultations6Label = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(6) label");
        private By checklistReferralsAndConsultations6Content = By.CssSelector("div[data-testid='Referrals and Consultations'] div div:nth-child(6) div:nth-child(2) div:nth-child(2) span");

        //Direct Sale or Road Closure section View Elements
        private By checklistDirectSaleRoadClosureTitle = By.XPath("//h2/div/div[contains(text(),'Road Closure')]");
        private By checklistDirectSaleRoadClosure1Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(1) label");
        private By checklistDirectSaleRoadClosure1Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div:first-child div:nth-child(1) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure2Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(2) label");
        private By checklistDirectSaleRoadClosure2Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div:first-child div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure3Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(3) label");
        private By checklistDirectSaleRoadClosure3Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(3) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure4Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(4) label");
        private By checklistDirectSaleRoadClosure4Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(4) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure5Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(5) label");
        private By checklistDirectSaleRoadClosure5Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(5) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure6Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(6) label");
        private By checklistDirectSaleRoadClosure6Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(6) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure7Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(7) label");
        private By checklistDirectSaleRoadClosure7Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(7) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure8Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(8) label");
        private By checklistDirectSaleRoadClosure8Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(8) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure9Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(9) label");
        private By checklistDirectSaleRoadClosure9Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(9) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure10Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(10) label");
        private By checklistDirectSaleRoadClosure10Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(10) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure11Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(11) label");
        private By checklistDirectSaleRoadClosure11Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(11) div:nth-child(2) div:nth-child(2) span");
        private By checklistDirectSaleRoadClosure12Label = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(12) label");
        private By checklistDirectSaleRoadClosure12Content = By.CssSelector("div[data-testid='Direct Sale or Road Closure or SRW'] div div:nth-child(12) div:nth-child(2) div:nth-child(2) span");

        //Sale Information section View Elements
        private By checklistSaleInformationTitle = By.XPath("//h2/div/div[contains(text(),'Sale Information')]");
        private By checklistSaleInformation1Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(1) label");
        private By checklistSaleInformation1Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(1) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation2Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(2) label");
        private By checklistSaleInformation2Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(2) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation3Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(3) label");
        private By checklistSaleInformation3Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(3) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation4Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(4) label");
        private By checklistSaleInformation4Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(4) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation5Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(5) label");
        private By checklistSaleInformation5Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(5) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation6Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(6) label");
        private By checklistSaleInformation6Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(6) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation7Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(7) label");
        private By checklistSaleInformation7Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(7) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation8Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(8) label");
        private By checklistSaleInformation8Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(8) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation9Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(9) label");
        private By checklistSaleInformation9Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(9) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation10Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(10) label");
        private By checklistSaleInformation10Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(10) div:nth-child(2) div:nth-child(2) span");
        private By checklistSaleInformation11Label = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(11) label");
        private By checklistSaleInformation11Content = By.CssSelector("div[data-testid='Sale Information'] div div:nth-child(11) div:nth-child(2) div:nth-child(2) span");

        //Checklist Edit Mode Elements
        private By checklistFileInitiationItem1Select = By.Id("input-checklistSections[0].items[0].statusType");
        private By checklistFileInitiationItem2Select = By.Id("input-checklistSections[0].items[1].statusType");
        private By checklistFileInitiationItem3Select = By.Id("input-checklistSections[0].items[2].statusType");
        private By checklistFileInitiationItem4Select = By.Id("input-checklistSections[0].items[3].statusType");

        private By checklistDispositionPreparationItem1Select = By.Id("input-checklistSections[1].items[0].statusType");
        private By checklistDispositionPreparationItem2Select = By.Id("input-checklistSections[1].items[1].statusType");
        private By checklistDispositionPreparationItem3Select = By.Id("input-checklistSections[1].items[2].statusType");
        private By checklistDispositionPreparationItem4Select = By.Id("input-checklistSections[1].items[3].statusType");

        private By checklistReferralsAndConsultationsItem1Select = By.Id("input-checklistSections[2].items[0].statusType");
        private By checklistReferralsAndConsultationsItem2Select = By.Id("input-checklistSections[2].items[1].statusType");
        private By checklistReferralsAndConsultationsItem3Select = By.Id("input-checklistSections[2].items[2].statusType");
        private By checklistReferralsAndConsultationsItem4Select = By.Id("input-checklistSections[2].items[3].statusType");
        private By checklistReferralsAndConsultationsItem5Select = By.Id("input-checklistSections[2].items[4].statusType");
        private By checklistReferralsAndConsultationsItem6Select = By.Id("input-checklistSections[2].items[5].statusType");

        private By checklistDirectSaleRoadClosureItem1Select = By.Id("input-checklistSections[3].items[0].statusType");
        private By checklistDirectSaleRoadClosureItem2Select = By.Id("input-checklistSections[3].items[1].statusType");
        private By checklistDirectSaleRoadClosureItem3Select = By.Id("input-checklistSections[3].items[2].statusType");
        private By checklistDirectSaleRoadClosureItem4Select = By.Id("input-checklistSections[3].items[3].statusType");
        private By checklistDirectSaleRoadClosureItem5Select = By.Id("input-checklistSections[3].items[4].statusType");
        private By checklistDirectSaleRoadClosureItem6Select = By.Id("input-checklistSections[3].items[5].statusType");
        private By checklistDirectSaleRoadClosureItem7Select = By.Id("input-checklistSections[3].items[6].statusType");
        private By checklistDirectSaleRoadClosureItem8Select = By.Id("input-checklistSections[3].items[7].statusType");
        private By checklistDirectSaleRoadClosureItem9Select = By.Id("input-checklistSections[3].items[8].statusType");
        private By checklistDirectSaleRoadClosureItem10Select = By.Id("input-checklistSections[3].items[9].statusType");
        private By checklistDirectSaleRoadClosureItem11Select = By.Id("input-checklistSections[3].items[10].statusType");
        private By checklistDirectSaleRoadClosureItem12Select = By.Id("input-checklistSections[3].items[11].statusType");

        private By checklistSaleInformationItem1Select = By.Id("input-checklistSections[4].items[0].statusType");
        private By checklistSaleInformationItem2Select = By.Id("input-checklistSections[4].items[1].statusType");
        private By checklistSaleInformationItem3Select = By.Id("input-checklistSections[4].items[2].statusType");
        private By checklistSaleInformationItem4Select = By.Id("input-checklistSections[4].items[3].statusType");
        private By checklistSaleInformationItem5Select = By.Id("input-checklistSections[4].items[4].statusType");
        private By checklistSaleInformationItem6Select = By.Id("input-checklistSections[4].items[5].statusType");
        private By checklistSaleInformationItem7Select = By.Id("input-checklistSections[4].items[6].statusType");
        private By checklistSaleInformationItem8Select = By.Id("input-checklistSections[4].items[7].statusType");
        private By checklistSaleInformationItem9Select = By.Id("input-checklistSections[4].items[8].statusType");
        private By checklistSaleInformationItem10Select = By.Id("input-checklistSections[4].items[9].statusType");
        private By checklistSaleInformationItem11Select = By.Id("input-checklistSections[4].items[10].statusType");

        private SharedModals sharedModals;

        public DispositionChecklist(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
        }

        public void NavigateChecklistTab()
        {
            WaitUntilClickable(checklistLinkTab);
            webDriver.FindElement(checklistLinkTab).Click();
        }

        public void EditChecklistButton()
        {
            WaitUntilClickable(checklistEditBttn);
            webDriver.FindElement(checklistEditBttn).Click();
        }

        public void SaveDispositionFileChecklist()
        {
            Wait();
            ButtonElement("Save");

            AssertTrueIsDisplayed(checklistEditBttn);
        }

        public void CancelDispositionFileChecklist()
        {
            Wait();
            ButtonElement("Cancel");

            sharedModals.CancelActionModal();
        }

        public void VerifyChecklistInitViewForm()
        {
            Wait(2000);

            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistFileInitiation1Label);
            AssertTrueIsDisplayed(checklistFileInitiation1Content);
            AssertTrueIsDisplayed(checklistFileInitiation2Label);
            AssertTrueIsDisplayed(checklistFileInitiation2Content);
            AssertTrueIsDisplayed(checklistFileInitiation3Label);
            AssertTrueIsDisplayed(checklistFileInitiation3Content);
            AssertTrueIsDisplayed(checklistFileInitiation4Label);
            AssertTrueIsDisplayed(checklistFileInitiation4Content);

            AssertTrueIsDisplayed(checklistDispositionPreparationTitle);
            AssertTrueIsDisplayed(checklistDispositionPreparation1Label);
            AssertTrueIsDisplayed(checklistDispositionPreparation1Content);
            AssertTrueIsDisplayed(checklistDispositionPreparation2Label);
            AssertTrueIsDisplayed(checklistDispositionPreparation2Content);
            AssertTrueIsDisplayed(checklistDispositionPreparation3Label);
            AssertTrueIsDisplayed(checklistDispositionPreparation3Content);
            AssertTrueIsDisplayed(checklistDispositionPreparation4Label);
            AssertTrueIsDisplayed(checklistDispositionPreparation4Content);

            AssertTrueIsDisplayed(checklistReferralsAndConsultationsTitle);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations1lLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations1Content);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations2Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations2Content);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations3Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations3Content);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations4Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations4Content);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations5Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations5Content);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations6Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations6Content);

            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureTitle);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure1Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure1Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure2Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure2Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure3Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure3Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure4Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure4Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure5Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure5Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure6Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure6Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure7Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure7Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure8Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure8Content);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure9Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure9Content);

            AssertTrueIsDisplayed(checklistSaleInformationTitle);
            AssertTrueIsDisplayed(checklistSaleInformation1Label);
            AssertTrueIsDisplayed(checklistSaleInformation1Content);
            AssertTrueIsDisplayed(checklistSaleInformation2Label);
            AssertTrueIsDisplayed(checklistSaleInformation2Content);
            AssertTrueIsDisplayed(checklistSaleInformation3Label);
            AssertTrueIsDisplayed(checklistSaleInformation3Content);
            AssertTrueIsDisplayed(checklistSaleInformation4Label);
            AssertTrueIsDisplayed(checklistSaleInformation4Content);
            AssertTrueIsDisplayed(checklistSaleInformation5Label);
            AssertTrueIsDisplayed(checklistSaleInformation5Content);
            AssertTrueIsDisplayed(checklistSaleInformation6Label);
            AssertTrueIsDisplayed(checklistSaleInformation6Content);
            AssertTrueIsDisplayed(checklistSaleInformation7Label);
            AssertTrueIsDisplayed(checklistSaleInformation7Content);
            AssertTrueIsDisplayed(checklistSaleInformation8Label);
            AssertTrueIsDisplayed(checklistSaleInformation8Content);
            AssertTrueIsDisplayed(checklistSaleInformation9Label);
            AssertTrueIsDisplayed(checklistSaleInformation9Content);
            AssertTrueIsDisplayed(checklistSaleInformation10Label);
            AssertTrueIsDisplayed(checklistSaleInformation10Content);
            AssertTrueIsDisplayed(checklistSaleInformation11Label);
            AssertTrueIsDisplayed(checklistSaleInformation11Content);
        }

        public void VerifyChecklistEditForm()
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistFileInitiation1Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem1Select);
            AssertTrueIsDisplayed(checklistFileInitiation2Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem2Select);
            AssertTrueIsDisplayed(checklistFileInitiation3Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem3Select);
            AssertTrueIsDisplayed(checklistFileInitiation4Label);
            AssertTrueIsDisplayed(checklistFileInitiationItem4Select);


            AssertTrueIsDisplayed(checklistDispositionPreparationTitle);
            AssertTrueIsDisplayed(checklistDispositionPreparation1Label);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem1Select);
            AssertTrueIsDisplayed(checklistDispositionPreparation2Label);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem2Select);
            AssertTrueIsDisplayed(checklistDispositionPreparation3Label);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem3Select);
            AssertTrueIsDisplayed(checklistDispositionPreparation4Label);
            AssertTrueIsDisplayed(checklistDispositionPreparationItem4Select);


            AssertTrueIsDisplayed(checklistReferralsAndConsultationsTitle);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations1lLabel);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem1Select);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem2Select);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations2Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem3Select);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations3Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem4Select);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations4Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem5Select);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations5Label);
            AssertTrueIsDisplayed(checklistReferralsAndConsultationsItem6Select);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations6Label);

            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureTitle);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure1Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem1Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure2Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem2Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure3Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem3Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure4Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem4Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure5Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem5Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure6Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem6Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure7Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem7Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure8Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem8Select);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure9Label);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureItem9Select);

            AssertTrueIsDisplayed(checklistSaleInformationTitle);
            AssertTrueIsDisplayed(checklistSaleInformation1Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem1Select);
            AssertTrueIsDisplayed(checklistSaleInformation2Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem2Select);
            AssertTrueIsDisplayed(checklistSaleInformation3Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem3Select);
            AssertTrueIsDisplayed(checklistSaleInformation4Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem4Select);
            AssertTrueIsDisplayed(checklistSaleInformation5Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem5Select);
            AssertTrueIsDisplayed(checklistSaleInformation6Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem6Select);
            AssertTrueIsDisplayed(checklistSaleInformation7Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem7Select);
            AssertTrueIsDisplayed(checklistSaleInformation8Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem8Select);
            AssertTrueIsDisplayed(checklistSaleInformation9Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem9Select);
            AssertTrueIsDisplayed(checklistSaleInformation10Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem10Select);
            AssertTrueIsDisplayed(checklistSaleInformation11Label);
            AssertTrueIsDisplayed(checklistSaleInformationItem11Select);
        }

        public void VerifyChecklistViewForm(DispositionFileChecklist checklist)
        {
            AssertTrueIsDisplayed(checklistFileInitiationTitle);
            AssertTrueIsDisplayed(checklistFileInitiation1Label);
            AssertTrueContentEquals(checklistFileInitiation1Content, checklist.FileInitiationSelect1);
            AssertTrueIsDisplayed(checklistFileInitiation2Label);
            AssertTrueContentEquals(checklistFileInitiation2Content, checklist.FileInitiationSelect2);
            AssertTrueIsDisplayed(checklistFileInitiation3Label);
            AssertTrueContentEquals(checklistFileInitiation3Content, checklist.FileInitiationSelect3);
            AssertTrueIsDisplayed(checklistFileInitiation4Label);
            AssertTrueContentEquals(checklistFileInitiation4Content, checklist.FileInitiationSelect4);

            AssertTrueIsDisplayed(checklistDispositionPreparationTitle);
            AssertTrueIsDisplayed(checklistDispositionPreparation1Label);
            AssertTrueContentEquals(checklistDispositionPreparation1Content, checklist.DispositionPreparationSelect1);
            AssertTrueIsDisplayed(checklistDispositionPreparation2Label);
            AssertTrueContentEquals(checklistDispositionPreparation2Content, checklist.DispositionPreparationSelect2);
            AssertTrueIsDisplayed(checklistDispositionPreparation3Label);
            AssertTrueContentEquals(checklistDispositionPreparation3Content, checklist.DispositionPreparationSelect3);
            AssertTrueIsDisplayed(checklistDispositionPreparation4Label);
            AssertTrueContentEquals(checklistDispositionPreparation4Content, checklist.DispositionPreparationSelect4);

            AssertTrueIsDisplayed(checklistReferralsAndConsultationsTitle);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations1lLabel);
            AssertTrueContentEquals(checklistReferralsAndConsultations1Content, checklist.ReferralsAndConsultationsSelect1);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations2Label);
            AssertTrueContentEquals(checklistReferralsAndConsultations2Content, checklist.ReferralsAndConsultationsSelect2);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations3Label);
            AssertTrueContentEquals(checklistReferralsAndConsultations3Content, checklist.ReferralsAndConsultationsSelect3);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations4Label);
            AssertTrueContentEquals(checklistReferralsAndConsultations4Content, checklist.ReferralsAndConsultationsSelect4);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations5Label);
            AssertTrueContentEquals(checklistReferralsAndConsultations5Content, checklist.ReferralsAndConsultationsSelect5);
            AssertTrueIsDisplayed(checklistReferralsAndConsultations6Label);
            AssertTrueContentEquals(checklistReferralsAndConsultations6Content, checklist.ReferralsAndConsultationsSelect6);

            AssertTrueIsDisplayed(checklistDirectSaleRoadClosureTitle);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure1Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure1Content, checklist.DirectSaleRoadClosureSelect1);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure2Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure2Content, checklist.DirectSaleRoadClosureSelect2);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure3Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure3Content, checklist.DirectSaleRoadClosureSelect3);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure4Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure4Content, checklist.DirectSaleRoadClosureSelect4);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure5Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure5Content, checklist.DirectSaleRoadClosureSelect5);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure6Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure6Content, checklist.DirectSaleRoadClosureSelect6);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure7Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure7Content, checklist.DirectSaleRoadClosureSelect7);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure8Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure8Content, checklist.DirectSaleRoadClosureSelect8);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure9Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure9Content, checklist.DirectSaleRoadClosureSelect9);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure10Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure10Content, checklist.DirectSaleRoadClosureSelect10);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure11Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure11Content, checklist.DirectSaleRoadClosureSelect11);
            AssertTrueIsDisplayed(checklistDirectSaleRoadClosure12Label);
            AssertTrueContentEquals(checklistDirectSaleRoadClosure12Content, checklist.DirectSaleRoadClosureSelect12);

            AssertTrueIsDisplayed(checklistSaleInformationTitle);
            AssertTrueIsDisplayed(checklistSaleInformation1Label);
            AssertTrueContentEquals(checklistSaleInformation1Content, checklist.SaleInformationSelect1);
            AssertTrueIsDisplayed(checklistSaleInformation2Label);
            AssertTrueContentEquals(checklistSaleInformation2Content, checklist.SaleInformationSelect2);
            AssertTrueIsDisplayed(checklistSaleInformation3Label);
            AssertTrueContentEquals(checklistSaleInformation3Content, checklist.SaleInformationSelect3);
            AssertTrueIsDisplayed(checklistSaleInformation4Label);
            AssertTrueContentEquals(checklistSaleInformation4Content, checklist.SaleInformationSelect4);
            AssertTrueIsDisplayed(checklistSaleInformation5Label);
            AssertTrueContentEquals(checklistSaleInformation5Content, checklist.SaleInformationSelect5);
            AssertTrueIsDisplayed(checklistSaleInformation6Label);
            AssertTrueContentEquals(checklistSaleInformation6Content, checklist.SaleInformationSelect6);
            AssertTrueIsDisplayed(checklistSaleInformation7Label);
            AssertTrueContentEquals(checklistSaleInformation7Content, checklist.SaleInformationSelect7);
            AssertTrueIsDisplayed(checklistSaleInformation8Label);
            AssertTrueContentEquals(checklistSaleInformation8Content, checklist.SaleInformationSelect8);
            AssertTrueIsDisplayed(checklistSaleInformation9Label);
            AssertTrueContentEquals(checklistSaleInformation9Content, checklist.SaleInformationSelect9);
            AssertTrueIsDisplayed(checklistSaleInformation10Label);
            AssertTrueContentEquals(checklistSaleInformation10Content, checklist.SaleInformationSelect10);
            AssertTrueIsDisplayed(checklistSaleInformation11Label);
            AssertTrueContentEquals(checklistSaleInformation11Content, checklist.SaleInformationSelect11);
        }

        public void UpdateChecklist(DispositionFileChecklist checklist)
        {
            Wait();

            if (checklist.FileInitiationSelect1 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem1Select, checklist.FileInitiationSelect1);
            if (checklist.FileInitiationSelect2 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem2Select, checklist.FileInitiationSelect2);
            if (checklist.FileInitiationSelect3 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem3Select, checklist.FileInitiationSelect3);
            if (checklist.FileInitiationSelect4 != "")
                ChooseSpecificSelectOption(checklistFileInitiationItem4Select, checklist.FileInitiationSelect4);

            if (checklist.DispositionPreparationSelect1 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem1Select, checklist.DispositionPreparationSelect1);
            if (checklist.DispositionPreparationSelect2 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem2Select, checklist.DispositionPreparationSelect2);
            if (checklist.DispositionPreparationSelect3 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem3Select, checklist.DispositionPreparationSelect3);
            if (checklist.DispositionPreparationSelect4 != "")
                ChooseSpecificSelectOption(checklistDispositionPreparationItem4Select, checklist.DispositionPreparationSelect4);

            if (checklist.ReferralsAndConsultationsSelect1 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem1Select, checklist.ReferralsAndConsultationsSelect1);
            if (checklist.ReferralsAndConsultationsSelect2 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem2Select, checklist.ReferralsAndConsultationsSelect2);
            if (checklist.ReferralsAndConsultationsSelect3 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem3Select, checklist.ReferralsAndConsultationsSelect3);
            if (checklist.ReferralsAndConsultationsSelect4 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem4Select, checklist.ReferralsAndConsultationsSelect4);
            if (checklist.ReferralsAndConsultationsSelect5 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem5Select, checklist.ReferralsAndConsultationsSelect5);
            if (checklist.ReferralsAndConsultationsSelect6 != "")
                ChooseSpecificSelectOption(checklistReferralsAndConsultationsItem6Select, checklist.ReferralsAndConsultationsSelect6);

            if (checklist.DirectSaleRoadClosureSelect1 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem1Select, checklist.DirectSaleRoadClosureSelect1);
            if (checklist.DirectSaleRoadClosureSelect2 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem2Select, checklist.DirectSaleRoadClosureSelect2);
            if (checklist.DirectSaleRoadClosureSelect3 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem3Select, checklist.DirectSaleRoadClosureSelect3);
            if (checklist.DirectSaleRoadClosureSelect4 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem4Select, checklist.DirectSaleRoadClosureSelect4);
            if (checklist.DirectSaleRoadClosureSelect5 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem5Select, checklist.DirectSaleRoadClosureSelect5);
            if (checklist.DirectSaleRoadClosureSelect6 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem6Select, checklist.DirectSaleRoadClosureSelect6);
            if (checklist.DirectSaleRoadClosureSelect7 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem7Select, checklist.DirectSaleRoadClosureSelect7);
            if (checklist.DirectSaleRoadClosureSelect8 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem8Select, checklist.DirectSaleRoadClosureSelect8);
            if (checklist.DirectSaleRoadClosureSelect9 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem9Select, checklist.DirectSaleRoadClosureSelect9);
            if (checklist.DirectSaleRoadClosureSelect10 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem10Select, checklist.DirectSaleRoadClosureSelect10);
            if (checklist.DirectSaleRoadClosureSelect11 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem11Select, checklist.DirectSaleRoadClosureSelect11);
            if (checklist.DirectSaleRoadClosureSelect12 != "")
                ChooseSpecificSelectOption(checklistDirectSaleRoadClosureItem12Select, checklist.DirectSaleRoadClosureSelect12);

            if (checklist.SaleInformationSelect1 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem1Select, checklist.SaleInformationSelect1);
            if (checklist.SaleInformationSelect2 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem2Select, checklist.SaleInformationSelect2);
            if (checklist.SaleInformationSelect3 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem3Select, checklist.SaleInformationSelect3);
            if (checklist.SaleInformationSelect4 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem4Select, checklist.SaleInformationSelect4);
            if (checklist.SaleInformationSelect5 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem5Select, checklist.SaleInformationSelect5);
            if (checklist.SaleInformationSelect6 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem6Select, checklist.SaleInformationSelect6);
            if (checklist.SaleInformationSelect7 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem7Select, checklist.SaleInformationSelect7);
            if (checklist.SaleInformationSelect8 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem8Select, checklist.SaleInformationSelect8);
            if (checklist.SaleInformationSelect9 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem9Select, checklist.SaleInformationSelect9);
            if (checklist.SaleInformationSelect10 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem10Select, checklist.SaleInformationSelect10);
            if (checklist.SaleInformationSelect11 != "")
                ChooseSpecificSelectOption(checklistSaleInformationItem11Select, checklist.SaleInformationSelect11);
        }
    }
}
