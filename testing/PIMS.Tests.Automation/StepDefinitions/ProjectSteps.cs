using System.Data;
using PIMS.Tests.Automation.Data;
using PIMS.Tests.Automation.Classes;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class ProjectSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly Projects projects;
        private readonly SearchProjects searchProjects;

        private readonly string userName = "TRANPSP1";

        private Project project;

        public ProjectSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            projects = new Projects(driver.Current);
            searchProjects = new SearchProjects(driver.Current);

            project = new Project();
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
            for (int i = 0; i < project.ProductsCount; i++)
            {
                projects.CreateProduct(project.Products[i], i);
            }

            //Save Project
            projects.SaveProject();
        }

        [StepDefinition(@"I verify The Project View Form")]
        public void VerifyCreatedProject()
        {
            //Verify Project View
            projects.VerifyProjectViewForm(project);

            //Verify Products within a Project
            for (int i = 0; i < project.ProductsCount; i++)
            {
                projects.VerifyProductViewForm(project.Products[i], i, "Create");
            }
        }

        [StepDefinition(@"I search for an existing project")]
        public void SearchExistingProject()
        {
            /* TEST COVERAGE:  */

            //Navigate to Search a Contact
            searchProjects.NavigateToSearchProject();

            //Look for existing Project by name
            searchProjects.SearchProjectByName(project.Name);
        }

        [StepDefinition(@"I update an existing project from row number (.*)")]
        public void UpdateProject(int rowNumber)
        {
            /* TEST COVERAGE: PSP-5321, PSP-5536, PSP-5537 */

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Manage Projects
            searchProjects.NavigateToSearchProject();

            //Look for Projects by number
            searchProjects.SearchProjectByNumber("AU-0003");
            Assert.True(searchProjects.TotalSearchedProjects().Equals(1));

            //Look for Projects by Region
            searchProjects.SearchProjectByRegion("Northern Region");
            Assert.True(searchProjects.TotalSearchedProjects().Equals(10));

            //Look for Projects by Status
            searchProjects.SearchProjectByStatus("Planning (PL)");
            Assert.True(searchProjects.TotalSearchedProjects().Equals(6));

            //Look for existing Project by name
            PopulateProjectData(rowNumber);
            searchProjects.SearchProjectByName(project.Name);

            //Select the 1st found Project
            searchProjects.SelectFirstResult();

            //Edit selected project
            projects.UpdateProject(project);

            //Edit Products
            for (int i = 0; i < project.ProductsCount; i++)
            {
                projects.UpdateProduct(project.Products[i], i);
            }

            //Save Project
            projects.SaveProject();
        }

        [StepDefinition(@"I navigate back to Project Details")]
        public void NavigateProjectDetails()
        {
            projects.NavigateProjectDetails();
        }

        [StepDefinition(@"Expected Content is displayed on Projects Table")]
        public void VerifyProjectsTableContent()
        {
            /* TEST COVERAGE: PSP-5319 */
            searchProjects.VerifySearchView();
            searchProjects.VerifyViewSearchResult(project);
        }

        [StepDefinition(@"The Project is updated successfully")]
        public void UpdateProjectSuccessfully()
        {
            //Verify Project View
            projects.VerifyProjectViewForm(project);

            //Verify Products within a Project
            for (int i = 0; i < project.ProductsCount; i++)
            {
                projects.VerifyProductViewForm(project.Products[i], i, "Update");
            }
        }

        [StepDefinition(@"Duplicate Project Alert is displayed")]
        public void DuplicateProject()
        {
            /* TEST COVERAGE:  PSP-5670 */

            Assert.True(projects.duplicateProject());
        }

        private void PopulateProjectData(int rowNumber)
        {
            DataTable projectsSheet = ExcelDataContext.GetInstance().Sheets["Projects"];
            ExcelDataContext.PopulateInCollection(projectsSheet);

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
            {
                PopulateProductCollection(project.ProductsRowStart, project.ProductsCount);
            }
        }
 
        private void PopulateProductCollection(int startRow, int rowsCount)
        {
            DataTable projectProductsSheet = ExcelDataContext.GetInstance().Sheets["ProjectsProducts"];
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
