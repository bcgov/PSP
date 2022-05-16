import static com.kms.katalon.core.checkpoint.CheckpointFactory.findCheckpoint
import static com.kms.katalon.core.testcase.TestCaseFactory.findTestCase
import static com.kms.katalon.core.testdata.TestDataFactory.findTestData
import static com.kms.katalon.core.testobject.ObjectRepository.findTestObject
import static com.kms.katalon.core.testobject.ObjectRepository.findWindowsObject
import com.kms.katalon.core.checkpoint.Checkpoint as Checkpoint
import com.kms.katalon.core.cucumber.keyword.CucumberBuiltinKeywords as CucumberKW
import com.kms.katalon.core.mobile.keyword.MobileBuiltInKeywords as Mobile
import com.kms.katalon.core.model.FailureHandling as FailureHandling
import com.kms.katalon.core.testcase.TestCase as TestCase
import com.kms.katalon.core.testdata.TestData as TestData
import com.kms.katalon.core.testng.keyword.TestNGBuiltinKeywords as TestNGKW
import com.kms.katalon.core.testobject.TestObject as TestObject
import com.kms.katalon.core.webservice.keyword.WSBuiltInKeywords as WS
import com.kms.katalon.core.webui.keyword.WebUiBuiltInKeywords as WebUI
import com.kms.katalon.core.windows.keyword.WindowsBuiltinKeywords as Windows
import internal.GlobalVariable as GlobalVariable
import org.openqa.selenium.Keys as Keys

// Open browser to our test environment
WebUI.openBrowser('https://tst-pims.th.gov.bc.ca')

// Maximize browser window
WebUI.maximizeWindow()

// Click the sign in button
WebUI.click(findTestObject('Object Repository/Login/SignInButton'))

// Click the IDIR authentication button
WebUI.click(findTestObject('Object Repository/SiteMinder/IdirButton'))

// Enter IDIR in appropriate field
WebUI.setText(findTestObject('Object Repository/SiteMinder/IdirField'), GlobalVariable.userName)

// Enter password 
WebUI.setEncryptedText(findTestObject('Object Repository/SiteMinder/PasswordField'), GlobalVariable.password)

// Click continue 
WebUI.click(findTestObject('Object Repository/SiteMinder/ContinueButton'))

// Click research button from Main menu
WebUI.click(findTestObject('Object Repository/ResearchFiles/CreateResearchFile/researchMenuButton'))

// Click on search a research file option
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchResearchFileLink'))

// Look for research file with the word "automation" within the name
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchFileNameInput'), 'Automation')

// Click on search button
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchResearchFileButton'))

// Click on the first result record
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/firstAutoResearchFileLink'))

// UPDATE RESEARCH FILE
// Insert Data on road name input
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/roadNameInput'), 'Automation Road Name Test')

// Insert data on road alias
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/roadAliasInput'), 'Automation Road Alias Test')

// Click on Research purpose input
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeInput'))

// Select Research Purpose reasons
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeAcquisitionOption'))

WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeTenureCleanUpOption'))

// Click on request date datepicker
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/requestDateInput'), '05/09/2022')

// Click on source of request select
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/sourceRequestSelect'))

//Choose an option from select
WebUI.selectOptionByValue(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/sourceRequestSelect'), 'District', 
    true)

//Click on contacts button
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/contactButton'))

//Lookup for "Alisson"
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/contactsSearchInput'), 'allison')

WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchContactButton'))

//Choose the first option from results
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchContactFirstOption'))

WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/selectContactButton'))

//Insert a description of request
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/requestDescriptionInput'), 'Automation Description')

//Insert a research complete on date
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchCompletionDateInput'), '12/12/2022')

//Insert Result of request description
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/resultRequestTextarea'), 'Automation Results')

//Insert expropiation boolean
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/expropiationYesRadioButton'))

//Insert Expropriation notes
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/expropriationNotesTextarea'), 'Automation notes')

//Save update form
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/updateResearchFileSaveButton'))

WebUI.openBrowser('https://tst-pims.th.gov.bc.ca')

// Maximize browser window
WebUI.maximizeWindow()

// Click the sign in button
WebUI.click(findTestObject('Object Repository/Login/SignInButton'))

// Click the IDIR authentication button
WebUI.click(findTestObject('Object Repository/SiteMinder/IdirButton'))

// Enter IDIR in appropriate field
WebUI.setText(findTestObject('Object Repository/SiteMinder/IdirField'), GlobalVariable.userName)

// Enter password 
WebUI.setEncryptedText(findTestObject('Object Repository/SiteMinder/PasswordField'), GlobalVariable.password)

// Click continue 
WebUI.click(findTestObject('Object Repository/SiteMinder/ContinueButton'))

// Click research button from Main menu
WebUI.click(findTestObject('Object Repository/ResearchFiles/CreateResearchFile/researchMenuButton'))

// Click on search a research file option
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchResearchFileLink'))

// Look for research file with the word "automation" within the name
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchFileNameInput'), 'Automation')

// Click on search button
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchResearchFileButton'))

// Click on the first result record
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/firstAutoResearchFileLink'))

//Todo: Missing button that leads to edit mode

// UPDATE RESEARCH FILE
// Insert Data on road name input
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/roadNameInput'), 'Automation Road Name Test')

// Insert data on road alias
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/roadAliasInput'), 'Automation Road Alias Test')

// Click on Research purpose input
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeInput'))

// Select Research Purpose reasons
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeAcquisitionOption'))

WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeTenureCleanUpOption'))

// Click on request date datepicker
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/requestDateInput'), '05/09/2022')

// Click on source of request select
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/sourceRequestSelect'))

//Choose an option from select
WebUI.selectOptionByValue(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/sourceRequestSelect'), 'District', 
    true)

//Click on contacts button
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/contactButton'))

//Lookup for "Alisson"
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/contactsSearchInput'), 'allison')

WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchContactButton'))

//Choose the first option from results
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/searchContactFirstOption'))

WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/selectContactButton'))

//Insert a description of request
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/requestDescriptionInput'), 'Automation Description')

//Insert a research complete on date
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchCompletionDateInput'), '12/12/2022')

//Insert Result of request description
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/resultRequestTextarea'), 'Automation Results')

//Insert expropiation boolean
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/expropiationYesRadioButton'))

//Insert Expropriation notes
WebUI.setText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/expropriationNotesTextarea'), 'Automation notes')

//Save update form
WebUI.click(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/updateResearchFileSaveButton'))


//VIEW RESEARCH FILE
WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/roadNameDataDiv'), 'Automation Road Name Test')
WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/roadAliasDataDiv'), 'Automation Road Alias Test')

WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchPurposeDataDiv'), 'Dec 12, 2022') //Todo - missing expected text
WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/requestDateDataDiv'), 'Dec 12, 2022')
WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/sourceOfRequestDataDiv'), 'District')
WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/requesterDataDiv'), 'Allison Sommers')

WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/researchCompletedDateDataDiv'), 'May 09, 2022')

WebUI.verifyElementText(findTestObject('Object Repository/ResearchFiles/UpdateResearchFile/expropriationBooleanDataDiv'), 'Yes')

// Close browser
WebUI.closeBrowser()

