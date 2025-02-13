using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using PIMS.Tests.Automation.Reports;


namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly IEnumerable<IdirUser> idirUsers;
        private readonly Login login;
        private readonly GenericSteps genericSteps;


        public LoginSteps(IWebDriver driver)
        {
            login = new Login(driver);
            genericSteps = new GenericSteps(driver);
            this.idirUsers = genericSteps.ReadConfiguration().GetSection("Users").Get<IEnumerable<IdirUser>>()!;
        }

        [StepDefinition(@"I log in with IDIR credentials (.*)")]
        public void Idir(string userName)
        {
            login.LoginToPIMS();

            var user = this.idirUsers.SingleOrDefault(u => u.User.Equals(userName, StringComparison.OrdinalIgnoreCase));
            if (user == null) throw new InvalidOperationException($"User {userName} not found in the test configuration");

            login.LoginUsingIDIR(user.User, user.Password);
        }
    }

    public class IdirUser
    {
       public string User { get; set; } = null!;
       public string Password { get; set; } = null!;
    }
}
