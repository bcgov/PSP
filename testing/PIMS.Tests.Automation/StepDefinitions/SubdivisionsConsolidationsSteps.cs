﻿using OpenQA.Selenium;
using PIMS.Tests.Automation.Classes;
using PIMS.Tests.Automation.Data;

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

        public SubdivisionsConsolidationsSteps(IWebDriver driver)
        {
            loginSteps = new LoginSteps(driver);
            subdivisionConsolidationProps = new SubdivisionConsolidationProperties(driver);

            propertySubdivision = new PropertySubdivision();
            propertyConsolidation = new PropertyConsolidation();
        }

        [StepDefinition(@"I create a Subdivision from row number (.*)")]
        public void CreateSubdivision(int rowNumber)
        {
            //TEST COVERAGE: PSP-7952, PSP-7953, PSP-7958

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

            //Create a Subdivision
            subdivisionConsolidationProps.CreateSubdivision(propertySubdivision);

            //Save Acquisition File
            subdivisionConsolidationProps.SaveSubdivision();
        }

        [StepDefinition(@"I create a Subdivision without finishing from row number (.*)")]
        public void CreateWithoutSaveSubdivision(int rowNumber)
        {
            //TEST COVERAGE: PSP-7952, PSP-7953, PSP-7958

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Subdivision Menu
            PopulateSubdivisionData(rowNumber);
            subdivisionConsolidationProps.NavigateToCreateNewSubdivision();

            //Create a Subdivision
            subdivisionConsolidationProps.CreateSubdivision(propertySubdivision);
        }

        [StepDefinition(@"I create a Consolidation from row number (.*)")]
        public void CreateConsolidation(int rowNumber)
        {
            //TEST COVERAGE: PSP-8029, PSP-8031, PSP-8032, PSP-8039, PSP-8042

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Consolidation Menu
            PopulateConsolidationData(rowNumber);
            subdivisionConsolidationProps.NavigateToCreateNewConsolidation();

            //Validate Consolidation Form 
            subdivisionConsolidationProps.VerifyInitCreateConsolidationForm();

            //Create a Consolidation
            subdivisionConsolidationProps.CreateConsolidation(propertyConsolidation);

            //Cancel Consolidation
            subdivisionConsolidationProps.CancelSubdivisionConsolidation();

            //Navigate to Subdivision Menu
            subdivisionConsolidationProps.NavigateToCreateNewConsolidation();

            //Create a Consolidation
            subdivisionConsolidationProps.CreateConsolidation(propertyConsolidation);

            //Save Consolidation
            subdivisionConsolidationProps.SaveConsolidation();
        }

        [StepDefinition(@"I attept to create a Consolidation from row number (.*)")]
        public void AttemptCreateConsolidation(int rowNumber)
        {
            //TEST COVERAGE: PSP-8029, PSP-8031, PSP-8032, PSP-8039, PSP-8042

            //Login to PIMS
            loginSteps.Idir(userName);

            //Navigate to Consolidation Menu
            PopulateConsolidationData(rowNumber);
            subdivisionConsolidationProps.NavigateToCreateNewConsolidation();

            //Create a Consolidation
            subdivisionConsolidationProps.InsertParentConsolidation(propertyConsolidation.ConsolidationSource[0].PropertyHistoryIdentifier);
        }

        [StepDefinition(@"Subdivision is created successfully")]
        public void SubdivisionCreatedSuccessfully()
        {
            subdivisionConsolidationProps.VerifySubdivisionHistory(propertySubdivision);
        }

        [StepDefinition(@"Consolidation is created successfully")]
        public void ConsolidationCreatedSuccessfully()
        {
            subdivisionConsolidationProps.VerifyConsolidationHistory(propertyConsolidation);
        }

        [StepDefinition(@"Subdivision has a Parent that is not in the MOTI Inventory error")]
        public void SourceNotInventoryError()
        {
            subdivisionConsolidationProps.VerifyInvalidSubdivisionChildrenMessage();
        }

        [StepDefinition(@"Subdivision has the same Child twice error")]
        public void SubdivisionSameDestinationTwiceError()
        {
            subdivisionConsolidationProps.VerifyInvalidSubdivisionChildMessage();
        }

        [StepDefinition(@"Subdivision has only one Child error")]
        public void SubdivisionOnlyOneDestinationError()
        {
            subdivisionConsolidationProps.VerifyMissingChildMessage();
        }

        [StepDefinition(@"Consolidation has a Child that is in the MOTI Inventory error")]
        public void DestinationNotInventoryError()
        {
            //TEST COVERAGE: PSP-8038

            subdivisionConsolidationProps.VerifyInvalidConsolidationChildMessage();
        }

        [StepDefinition(@"Consolidation has the same Parent twice error")]
        public void ConsolidationSameSourceTwiceError()
        {
            //TEST COVERAGE: PSP-8034

            subdivisionConsolidationProps.VerifyInvalidConsolidationRepeatedParentMessage();
        }

        [StepDefinition(@"Consolidation has only one Parent error")]
        public void ConsolidationOnlyOneSourceError()
        {
            //TEST COVERAGE: PSP-8036

            subdivisionConsolidationProps.VerifyMissingParentErrorMessage();
        }

        [StepDefinition(@"Consolidation Parent cannot be Disposed error")]
        public void ConsolidationParentDisposedError()
        {
            subdivisionConsolidationProps.VerifyMissingParentMessageModal();
        }

        private void PopulateSubdivisionData(int rowNumber)
        {
            System.Data.DataTable subdivisionConsolidationSheet = ExcelDataContext.GetInstance().Sheets["SubdivisionConsolidation"]!;
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
                destinationProperty.PropertyHistoryPlan = ExcelDataContext.ReadData(destinationIndex, "PropertyHistoryPlan");
                destinationProperty.PropertyHistoryStatus = ExcelDataContext.ReadData(destinationIndex, "PropertyHistoryStatus");
                destinationProperty.PropertyHistoryArea = ExcelDataContext.ReadData(destinationIndex, "PropertyHistoryArea");

                propertySubdivision.SubdivisionDestination.Add(destinationProperty);

                destinationIndex++;
            }
        }

        private void PopulateConsolidationData(int rowNumber)
        {
            System.Data.DataTable subdivisionConsolidationSheet = ExcelDataContext.GetInstance().Sheets["SubdivisionConsolidation"]!;
            ExcelDataContext.PopulateInCollection(subdivisionConsolidationSheet);

            propertyConsolidation = new PropertyConsolidation();

            var sourceIndex = rowNumber;

            while (ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryType") == "Source")
            {
                PropertyHistory sourceProperty = new PropertyHistory();

                sourceProperty.PropertyHistoryIdentifier = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryIdentifier");
                sourceProperty.PropertyHistoryPlan = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryPlan");
                sourceProperty.PropertyHistoryStatus = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryStatus");
                sourceProperty.PropertyHistoryArea = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryArea");

                propertyConsolidation.ConsolidationSource.Add(sourceProperty);

                sourceIndex++;
            }

            propertyConsolidation.ConsolidationDestination.PropertyHistoryIdentifier = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryIdentifier");
            propertyConsolidation.ConsolidationDestination.PropertyHistoryPlan = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryPlan");
            propertyConsolidation.ConsolidationDestination.PropertyHistoryStatus = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryStatus");
            propertyConsolidation.ConsolidationDestination.PropertyHistoryArea = ExcelDataContext.ReadData(sourceIndex, "PropertyHistoryArea");

            

           
        }
    }
}
