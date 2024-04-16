using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;
using System.Data;

namespace PIMS.Tests.Automation.StepDefinitions
{
    [Binding]
    public class SubdivisionsConsolidationsSteps
    {
        private readonly LoginSteps loginSteps;
        private readonly SubdivisionConsolidationProperties subdivisionConsolidationProps;

        private readonly string userName = "TRANPSP1";

        private PropertySubdivision propertySubdivision;
        private PropertyConsolidation propertyConsolidation;

        public SubdivisionsConsolidationsSteps(BrowserDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            subdivisionConsolidationProps = new SubdivisionConsolidationProperties(driver.Current);

            propertySubdivision = new PropertySubdivision();
            propertyConsolidation = new PropertyConsolidation();
        }

        [StepDefinition(@"I create a Subdivision from row number (.*)")]
        public void CreateSubdivision(int rowNumber)
        {
            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Subdivision Menu
            PopulateSubdivisionData(rowNumber);
            subdivisionConsolidationProps.NavigateToCreateNewSubdivision();

            //Validate Subdivision Form 
            subdivisionConsolidationProps.VerifyInitCreateSubdivisionForm();

            //Create a Subdivision
            subdivisionConsolidationProps.CreateSubdivision(propertySubdivision);

            //Save Acquisition File
            subdivisionConsolidationProps.CancelSubdivisionConsolidation();

            //Navigate to Subdivision Menu
            subdivisionConsolidationProps.NavigateToCreateNewSubdivision();

            //Validate Subdivision Form 
            subdivisionConsolidationProps.VerifyInitCreateSubdivisionForm();

            //Create a Subdivision
            subdivisionConsolidationProps.CreateSubdivision(propertySubdivision);

            //Save Acquisition File
            subdivisionConsolidationProps.SaveSubdivision();
        }

        [StepDefinition(@"Subdivision is created successfully")]
        public void SubdivisionCreatedSuccessfully()
        {
            subdivisionConsolidationProps.VerifySubdivisionConsolidationHistory("Subdivision", propertySubdivision);
        }

        [StepDefinition(@"Subdivision has a Parent that is not in the MOTI Inventory error")]
        public void SourceNotInventoryError()
        {
            subdivisionConsolidationProps.VerifyInvalidChildrenMessage();
        }

        private void PopulateSubdivisionData(int rowNumber)
        {
            DataTable subdivisionConsolidationSheet = ExcelDataContext.GetInstance().Sheets["SubdivisionConsolidation"]!;
            ExcelDataContext.PopulateInCollection(subdivisionConsolidationSheet);

            propertySubdivision = new PropertySubdivision();

            propertySubdivision.SubdivisionSource.PropertyHistoryIdentifier = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryIdentifier");
            propertySubdivision.SubdivisionSource.PropertyHistoryPlan = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryPlan");
            propertySubdivision.SubdivisionSource.PropertyHistoryStatus = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryStatus");
            propertySubdivision.SubdivisionSource.PropertyHistoryArea = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryArea");

            var destinationIndex = rowNumber + 1;

            while (ExcelDataContext.ReadData(destinationIndex, "PropertyHistoryType") == "Destination")
            {
                PropertyHistory destinationProperty = new PropertyHistory();

                destinationProperty.PropertyHistoryIdentifier = ExcelDataContext.ReadData(destinationIndex, "PropertyHistoryIdentifier");
                destinationProperty.PropertyHistoryPlan = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryPlan");
                destinationProperty.PropertyHistoryStatus = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryStatus");
                destinationProperty.PropertyHistoryArea = ExcelDataContext.ReadData(rowNumber, "PropertyHistoryArea");

                propertySubdivision.SubdivisionDestination.Add(destinationProperty);

                destinationIndex++;
            }
        }

    }
}
