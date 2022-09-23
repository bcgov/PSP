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

// UPDATE AN EXISTING LEASE/ LICENSE
WebUI.openBrowser('https://tst-pims.th.gov.bc.ca/login')

// Maximize browser window
WebUI.maximizeWindow()


// Click the Sign in button
WebUI.click(findTestObject('Object Repository/Login/SignInButton'))

// Click the IDIR authentication button
WebUI.click(findTestObject('Object Repository/SiteMinder/IdirButton'))

// Insert IDIR user
WebUI.setText(findTestObject('Object Repository/SiteMinder/IdirField'), GlobalVariable.userName)

// Insert IDIR password
WebUI.setEncryptedText(findTestObject('Object Repository/SiteMinder/PasswordField'), GlobalVariable.password)

// Continue button
WebUI.click(findTestObject('Object Repository/SiteMinder/ContinueButton'))

// Click Left Menu Bar > Management link
WebUI.click(findTestObject('LeaseLicenses/createLease/menuManagementLink'))

// Click Left Menu Bar > Management link > Search for a Lease or License
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/searchLeaseLink'))

// Look for an existing L-File
WebUI.setText(findTestObject('LeaseLicenses/updateLeaseDetails/searchByLFileNoInput'), '367')

// Click on Search L-File
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/searchLeaseButton'))

// Click on the L-File result link
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/LFileLink'))

// Click on Edit button on Lease Details Screen
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/editButton'))

// Edit MOTI Contact
WebUI.setText(findTestObject('LeaseLicenses/updateLeaseDetails/motiContactInput'),
	' Update Automated Test')

// Edit Program Type
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/updateLeaseDetails/programTypeSelect'), 
    'RESRENTAL', true)

// Edit Type
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/updateLeaseDetails/typeSelect'), 
    'LSUNREG', true)

// Edit Category
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/updateLeaseDetails/categorySelect'), 
    'COMM', true)

// Edit Purpose
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/updateLeaseDetails/purposeSelect'), 
    'MARINEFAC', true)

// Edit Resposibility
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/updateLeaseDetails/initiatorSelect'), 
    'PROJECT', true)


// Edit Location of Documents Textarea
WebUI.setText(findTestObject('LeaseLicenses/updateLeaseDetails/locationOfDocumentsTextarea'), ' automated edition')


// Edit Description
WebUI.setText(findTestObject('LeaseLicenses/updateLeaseDetails/descriptionTextarea'), 
    '  automated edition')

// Deleting previous value
WebUI.setText(findTestObject('LeaseLicenses/updateLeaseDetails/notesTextarea'),
	'')

//Edit Notes
WebUI.setText(findTestObject('LeaseLicenses/updateLeaseDetails/notesTextarea'),
	' automated edition')


// Save Edited License
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/saveButton'))

// VERIFY ELEMENTS ON LEASE DETAILS
WebUI.waitForElementVisible(findTestObject('LeaseLicenses/viewLeaseDetails/leaseDetailsTitle'), 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseProgramName'), 'value', 'Residential Rentals', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseType'), 'value', 'Lease - Unregistered', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelReceivableTo'), 'value', 'Payable (BCTFA as tenant)', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelCategoryType'), 'value', 'Commercial', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelPurpose'), 'value', 'Marine Facility', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseInitiator'), 'value', 'Project', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseResponsibility'), 'value', 'Headquarters', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelMotiContact'), 'value', 'MOTI Automated Test Update Automated Test', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseLIS'), 'value', '123456', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeasePS'), 'value', '66667777', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseDescription'), 'value', 'Description Automated test  automated edition', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseNotes'), 'value', 'Automated Notes Test automated edition', 0)

WebUI.closeBrowser()
