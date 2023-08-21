using Microsoft.Extensions.Configuration;


namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class LoginSteps
    {
        private readonly IEnumerable<IdirUser> idirUsers;
        private readonly Login login;

        public LoginSteps(BrowserDriver driver)
        {
            login = new Login(driver.Current);
            idirUsers = driver.Configuration.GetSection("Users").Get<IEnumerable<IdirUser>>();
        }

        [StepDefinition(@"I log in with IDIR credentials (.*)")]
        public void Idir(string userName)
        {
            login.LoginToPIMS();

            var user = idirUsers.SingleOrDefault(u => u.User.Equals(userName, StringComparison.OrdinalIgnoreCase));
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
