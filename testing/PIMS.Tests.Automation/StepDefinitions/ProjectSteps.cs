using System.Data;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.Classes;
using OpenQA.Selenium;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ProjectSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly Projects projects;
        private readonly SearchProjects searchProjects;
        private readonly SharedPagination sharedPagination;
        private readonly GenericSteps genericSteps;

        private readonly string userName = "TRANPSP1";

        private Project project;
        protected string projectName = "";

        public ProjectSteps(IWebDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            projects = new Projects(driver);
            searchProjects = new SearchProjects(driver);
            sharedPagination = new SharedPagination(driver);

            project = new Project();
            genericSteps = new GenericSteps(driver);
        }

        [StepDefinition(@"I create a new Project from row number (.*)")]
        public void CreateProject(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-5428, PSP-5429, PSP-5447, PSP-5534, PSP-5535 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            projects.NavigateToCreateNewProject();

            //Verify Create Project Form
            projects.VerifyCreateProjectForm();

            //Create a new Project
            PopulateProjectData(rowNumber);
            projects.CreateProject(project);

            //Verify Create Product Form
            projects.VerifyCreateProductForm();

            //Add Products
            if (project.Products.Count > 0)
            {
                for (int i = 0; i < project.ProductsCount; i++)
                    projects.CreateProduct(project.Products[i], i);  
            }

            //Add Team Members
            if (project.ProjectTeamMembers.First() != "")
            {
                for (int i = 0; i < project.ProjectTeamMembers.Count; i++)
                    projects.AddUpdateTeamMember(project.ProjectTeamMembers[i]);
            }

            //Save Project
            projects.SaveProject();

            //Get the project's name
            projectName = projects.GetProjectName();

            //Verify Project View
            projects.VerifyProjectViewForm(project);

            //Verify Products within a Project
            if (project.Products.Count > 0)
            {
                for (int i = 0; i < project.ProductsCount; i++)
                    projects.VerifyProductViewForm(project.Products[i], i, "Create");
            }

            //Verify Team Members within a Project
            if (project.ProjectTeamMembers.First() != "")
                projects.VerifyTeamMemberViewForm(project.ProjectTeamMembers);
            
        }

        [StepDefinition(@"I create a duplicate Project from row number (.*)")]
        public void CreateDuplicateProject(int rowNumber)
        {
            /* TEST COVERAGE:  PSP-5428, PSP-5429, PSP-5447, PSP-5534, PSP-5535 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Create new contact form
            projects.NavigateToCreateNewProject();

            //Verify Create Project Form
            projects.VerifyCreateProjectForm();

            //Create a new Project
            PopulateProjectData(rowNumber);
            projects.CreateProject(project);

            //Verify Create Product Form
            projects.VerifyCreateProductForm();

            //Add Products
            if (project.Products.Count > 0)
            {
                for (int i = 0; i < project.ProductsCount; i++)
                    projects.CreateProduct(project.Products[i], i);
            }

            //Add Team Members
            if (project.ProjectTeamMembers.First() != "")
            {
                for (int i = 0; i < project.ProjectTeamMembers.Count; i++)
                    projects.AddUpdateTeamMember(project.ProjectTeamMembers[i]);
            }

            //Save Project
            projects.SaveProject();
        }

        [StepDefinition(@"I update an existing project from row number (.*)")]
        public void UpdateProject(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5321, PSP-5536, PSP-5537 */

            //Navigate to Manage Projects
            searchProjects.NavigateToSearchProject();

            //Look for existing Project by name
            PopulateProjectData(rowNumber);
            searchProjects.SearchProjectByName(projectName);

            //Select the 1st found Project
            searchProjects.SelectFirstResult();

            //Edit selected project
            projects.UpdateProject(project);

            //Edit Products
            if (project.Products.Count > 0)
                for (int i = 0; i < project.ProductsCount; i++)
                    projects.UpdateProduct(project.Products[i], i);

            //Edit Team members
            if (project.ProjectTeamMembers.First() != "")
            {
                projects.DeleteTeamMembers();

                for (int i = 0; i < project.ProjectTeamMembers.Count; i++)
                    projects.AddUpdateTeamMember(project.ProjectTeamMembers[i]);
            }

            //Save Project
            projects.SaveProject();

            //Verify Project View
            projects.VerifyProjectViewForm(project);

            //Verify Products within a Project
            if (project.Products.Count > 0)
            {
                for (int i = 0; i < project.ProductsCount; i++)
                {
                    projects.VerifyProductViewForm(project.Products[i], i, "Update");
                }
            }

            //Verify Team Members within a Project
            if (project.ProjectTeamMembers.First() != "")
                projects.VerifyTeamMemberViewForm(project.ProjectTeamMembers);
        }

        [StepDefinition(@"I search for existing Projects from row number (.*)")]
        public void SearchExistingProjects(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5320, PSP-5321, PSP-5322, PSP-5536, PSP-5537 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Manage Projects
            PopulateProjectData(rowNumber);
            searchProjects.NavigateToSearchProject();

            //Verify Pagination
            sharedPagination.ChoosePaginationOption(5);
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(5));

            sharedPagination.ChoosePaginationOption(10);
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(10));

            sharedPagination.ChoosePaginationOption(20);
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(20));

            sharedPagination.ChoosePaginationOption(50);
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(50));

            sharedPagination.ChoosePaginationOption(100);
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(100));

            //Verify Column Sorting by Project Number
            searchProjects.OrderByProjectNumber();
            var firstProjectNbrDescResult = searchProjects.FirstProjectCode();

            searchProjects.OrderByProjectNumber();
            var firstProjectNbrAscResult = searchProjects.FirstProjectCode();

            Assert.NotEqual(firstProjectNbrDescResult, firstProjectNbrAscResult);

            //Verify Column Sorting by Project Name
            searchProjects.OrderByProjectName();
            var firstProjectNameResult = searchProjects.FirstProjectName();

            searchProjects.OrderByProjectName();
            var firstProjectNameAscResult = searchProjects.FirstProjectName();

            Assert.NotEqual(firstProjectNameResult, firstProjectNameAscResult);

            //Verify Column Sorting Last Updated By
            searchProjects.OrderByProjectLastUpdatedBy();
            var firstProjectLastUpdatedByDescResult = searchProjects.FirstProjectLastUpdatedBy();

            searchProjects.OrderByProjectLastUpdatedBy();
            var firstProjectLastUpdatedByAscResult = searchProjects.FirstProjectLastUpdatedBy();

            Assert.NotEqual(firstProjectLastUpdatedByDescResult, firstProjectLastUpdatedByAscResult);

            //Verify Column Sorting Last Updated Date
            searchProjects.OrderByProjectLastUpdatedDate();
            var firstProjectLastUpdatedDateDescResult = searchProjects.FirstProjectLastUpdatedDate();

            searchProjects.OrderByProjectLastUpdatedDate();
            var firstProjectLastUpdatedDateAscResult = searchProjects.FirstProjectLastUpdatedDate();

            Assert.NotEqual(firstProjectLastUpdatedDateDescResult, firstProjectLastUpdatedDateAscResult);

            //Verify Pagination display different set of results
            sharedPagination.ResetSearch();

            var firstProjectPage1 = searchProjects.FirstProjectName();
            sharedPagination.GoNextPage();
            var firstProjectPage2 = searchProjects.FirstProjectName();
            Assert.NotEqual(firstProjectPage1, firstProjectPage2);

            sharedPagination.ResetSearch();

            //Filter Projects by Region
            searchProjects.SearchProjectByRegion("Northern Region");
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(100));

            //Filter Projects by Status
            searchProjects.SearchProjectByStatus("Planning (PL)");
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(7));

            //Filter Projects by number
            searchProjects.SearchProjectByNumber(project.Number);
            Assert.True(searchProjects.ProjectsTableResultNumber().Equals(1));

        }

        [StepDefinition(@"A new Project is created successfully")]
        public void VerifyProjectsCreationSuccess()
        {
            //Navigate to Manage Projects
            searchProjects.NavigateToSearchProject();
            searchProjects.SearchProjectByName(projectName);

            Assert.True(searchProjects.SearchFoundResults());
        }

        [StepDefinition(@"Expected Project Content is displayed on Projects Table")]
        public void VerifyProjectsTableContent()
        {
            /* TEST COVERAGE: PSP-5319 */

            //Verify List View
            searchProjects.VerifySearchView();
            searchProjects.VerifyViewSearchResult(project);
        }

        [StepDefinition(@"Duplicate Project Alert is displayed")]
        public void DuplicateProject()
        {
            /* TEST COVERAGE:  PSP-5670 */

            projects.DuplicateProject();
        }

        private void PopulateProjectData(int rowNumber)
        {
            System.Data.DataTable projectsSheet = ExcelDataContext.GetInstance().Sheets["Projects"]!;
            ExcelDataContext.PopulateInCollection(projectsSheet);

            project = new Project();

            project.Number = ExcelDataContext.ReadData(rowNumber, "Number");
            project.Name = ExcelDataContext.ReadData(rowNumber, "Name");
            project.CodeName = ExcelDataContext.ReadData(rowNumber, "CodeName");
            project.ProjectStatus = ExcelDataContext.ReadData(rowNumber, "ProjectStatus");
            project.ProjectMOTIRegion = ExcelDataContext.ReadData(rowNumber, "ProjectMOTIRegion");
            project.Summary = ExcelDataContext.ReadData(rowNumber, "Summary");
            project.CostType = ExcelDataContext.ReadData(rowNumber, "CostType");
            project.WorkActivity = ExcelDataContext.ReadData(rowNumber, "WorkActivity");
            project.BusinessFunction = ExcelDataContext.ReadData(rowNumber, "BusinessFunction");
            project.CreatedBy = ExcelDataContext.ReadData(rowNumber, "CreatedBy");
            project.UpdatedBy = ExcelDataContext.ReadData(rowNumber, "UpdatedBy");
            project.ProductsCount = int.Parse(ExcelDataContext.ReadData(rowNumber, "ProductsCount"));
            project.ProductsRowStart = int.Parse(ExcelDataContext.ReadData(rowNumber, "ProductsRowStart"));
            
            if (project.ProductsCount != 0 && project.ProductsRowStart != 0)
                PopulateProductCollection(project.ProductsRowStart, project.ProductsCount);

            project.ProjectTeamMembers = genericSteps.PopulateLists(ExcelDataContext.ReadData(rowNumber, "ProjectTeamMembers"));
        }
 
        private void PopulateProductCollection(int startRow, int rowsCount)
        {
            System.Data.DataTable projectProductsSheet = ExcelDataContext.GetInstance().Sheets["ProjectsProducts"]!;
            ExcelDataContext.PopulateInCollection(projectProductsSheet);

            for (int i = startRow; i <= startRow + rowsCount; i++)
            {
                Product product = new Product();
                product.ProductCode = ExcelDataContext.ReadData(i, "ProductCode");
                product.ProductName = ExcelDataContext.ReadData(i, "ProductName");
                product.ProductCodeName = ExcelDataContext.ReadData(i, "ProductCodeName");
                product.CostEstimate = ExcelDataContext.ReadData(i, "CostEstimate");
                product.EstimateDate = ExcelDataContext.ReadData(i, "EstimateDate");
                product.StartDate = ExcelDataContext.ReadData(i, "StartDate");
                product.Objectives = ExcelDataContext.ReadData(i, "Objectives");
                product.Scope = ExcelDataContext.ReadData(i, "Scope");

                project.Products.Add(product);
            }
        }
    }

}
