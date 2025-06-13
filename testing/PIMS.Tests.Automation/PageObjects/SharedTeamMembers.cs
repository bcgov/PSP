using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.PageObjects
{
    public class SharedTeamMembers : PageObjectBase
    {
        private readonly By membersTeamSubtitle = By.XPath("//h2/div/div[contains(text(),'Team')]");
        private readonly By membersTeamAddAnotherMemberLink = By.CssSelector("button[data-testid='add-team-member']");
        private readonly By fileTeamMembersGroup = By.XPath("//div[contains(text(),'Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row']");
        private readonly By membersTeamFirstMemberDeleteBttn = By.XPath("//div[contains(text(),'Team')]/parent::div/parent::h2/following-sibling::div/div[@class='py-3 row'][1]/div[3]/button");
        private readonly By membersTeamInvalidTeamMemberMessage = By.XPath("//div[contains(text(),'Select a team member')]");
        private readonly By membersTeamInvalidProfileMessage = By.XPath("//div[contains(text(),'Select a profile')]");

        //Acquisition File Confirmation Modal Elements
        private readonly By fileConfirmationModal = By.CssSelector("div[class='modal-content']");

        private SharedModals sharedModals;
        private SharedSelectContact sharedSelectContact;

        public SharedTeamMembers(IWebDriver webDriver) : base(webDriver)
        {
            sharedModals = new SharedModals(webDriver);
            sharedSelectContact = new SharedSelectContact(webDriver);
        }

        public void AddTeamMembers(TeamMember teamMember)
        {
            Wait();
            FocusAndClick(membersTeamAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(fileTeamMembersGroup).Count() -1;

            WaitUntilVisible(By.CssSelector("select[id='input-team."+ teamMemberIndex +".contactTypeCode']"));
            ChooseSpecificSelectOption(By.CssSelector("select[id='input-team."+ teamMemberIndex +".contactTypeCode']"), teamMember.TeamMemberRole);
            FocusAndClick(By.CssSelector("div[data-testid='teamMemberRow["+ teamMemberIndex +"]'] div[data-testid='contact-input'] button[title='Select Contact']"));

            Wait();
            sharedSelectContact.SelectContact(teamMember.TeamMemberContactName, teamMember.TeamMemberContactType);

            Wait();
            if (webDriver.FindElements(By.Id("input-team."+ teamMemberIndex +".primaryContactId")).Count > 0)
                ChooseSpecificSelectOption(By.Id("input-team."+ teamMemberIndex +".primaryContactId"), teamMember.TeamMemberPrimaryContact);
        }

        public void AddMgmtTeamMembers(TeamMember teamMember)
        {
            Wait();
            FocusAndClick(membersTeamAddAnotherMemberLink);

            Wait();
            var teamMemberIndex = webDriver.FindElements(fileTeamMembersGroup).Count() -1;

            WaitUntilVisible(By.CssSelector("select[id='input-team."+ teamMemberIndex +".teamProfileTypeCode']"));
            ChooseSpecificSelectOption(By.CssSelector("select[id='input-team."+ teamMemberIndex +".teamProfileTypeCode']"), teamMember.TeamMemberRole);
            FocusAndClick(By.CssSelector("div[data-testid='teamMemberRow["+ teamMemberIndex +"]'] div[data-testid='contact-input'] button[title='Select Contact']"));

            Wait();
            sharedSelectContact.SelectContact(teamMember.TeamMemberContactName, teamMember.TeamMemberContactType);

            Wait();
            if (webDriver.FindElements(By.Id("input-team."+ teamMemberIndex +".primaryContactId")).Count > 0)
                ChooseSpecificSelectOption(By.Id("input-team."+ teamMemberIndex +".primaryContactId"), teamMember.TeamMemberPrimaryContact);
        }

        public void VerifyTeamMembersViewForm(List<TeamMember> teamMembers)
        {
            var index = 1;

            for (var i = 0; i < teamMembers.Count; i++)
            {
                AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/label"), teamMembers[i].TeamMemberRole + ":");
                AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a"), teamMembers[i].TeamMemberContactName);

                if (teamMembers[i].TeamMemberPrimaryContact != "")
                {
                    index++;
                    AssertTrueContentEquals(By.XPath("//h2/div/div[contains(text(),'Team')]/parent::div/parent::h2/following-sibling::div/div[" + index + "]/div/a"), teamMembers[i].TeamMemberPrimaryContact);
                }

                index++;
            }
        }

        public void VerifyRequiredTeamMemberMessages()
        {
            //Add a new Team member form
            Wait();
            WaitUntilClickable(membersTeamAddAnotherMemberLink);
            webDriver.FindElement(membersTeamAddAnotherMemberLink).Click();

            //Verify that invalid team member message is displayed
            ChooseSpecificSelectOption(By.Id("input-team.0.contactTypeCode"), "Expropriation agent");
            webDriver.FindElement(membersTeamSubtitle).Click();
            AssertTrueIsDisplayed(membersTeamInvalidTeamMemberMessage);

            //verify that invalid profile message is displayed
            webDriver.FindElement(By.CssSelector("div[data-testid='contact-input'] button[title='Select Contact']")).Click();
            sharedSelectContact.SelectContact("Test", "");
            ChooseSpecificSelectOption(By.Id("input-team.0.contactTypeCode"), "Select profile...");
            webDriver.FindElement(membersTeamSubtitle).Click();
            AssertTrueIsDisplayed(membersTeamInvalidProfileMessage);
        }

        public void DeleteFirstStaffMember()
        {
            WaitUntilClickable(membersTeamFirstMemberDeleteBttn);
            webDriver.FindElement(membersTeamFirstMemberDeleteBttn).Click();

            WaitUntilVisible(fileConfirmationModal);
            Assert.Equal("Remove Team Member", sharedModals.ModalHeader());
            //Assert.Equal("Do you wish to remove this team member?", sharedModals.ModalContent());

            sharedModals.ModalClickOKBttn();
        }
    }
}
