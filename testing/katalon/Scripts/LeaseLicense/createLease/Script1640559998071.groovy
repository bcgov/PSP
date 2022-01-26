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

// CREATING A NEW LEASE/ LICENSE
// PIMS Test
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

// Click Add new lease/ license option
WebUI.click(findTestObject('LeaseLicenses/createLease/addLicenseLink'))

// Click on license start date calendar
WebUI.click(findTestObject('LeaseLicenses/createLease/newLicenseStartDateInput'))

// Select day 26 from calendar
WebUI.click(findTestObject('LeaseLicenses/createLease/licenseCalendarStartDateDay'))

// Select Receivable or Payable option 
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectPayable'), 'PYBLBCTFA', true)

// Insert MOTI Contact
WebUI.setText(findTestObject('LeaseLicenses/createLease/inputMotiContact'), 'MOTI Automated Test')

// Insert MOTI Region
WebUI.setText(findTestObject('LeaseLicenses/createLease/inputMotiRegion'), 'MOTI Region')

// Select Program
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectProgram'), 'BCFERRIES', true)

// Select Type
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectType'), 'LSGRND', true)

// Select Category
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectCategory'), 'AGRIC', true)

// Select Purpose
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectPurpose'), 'GRAVEL', true)

// Select Initiator
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectInitiator'), 'HQ', true)

// Select Responsability
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/createLease/selectResponsible'), 'HQ', true)

// Open Effective date of responsibility calendar
WebUI.click(findTestObject('LeaseLicenses/createLease/inputEffectiveDateResponsibility'))

// Select date 30 from calendar
WebUI.click(findTestObject('LeaseLicenses/createLease/licenseSelectResponsibilityDay'))

// Insert Location of document test text
WebUI.setText(findTestObject('LeaseLicenses/createLease/inputLocationDocuments'), 'Location of documents automated test')

// Insert LIS# 
WebUI.setText(findTestObject('LeaseLicenses/createLease/inputLIS'), '123456')

// Insert PS#
WebUI.setText(findTestObject('LeaseLicenses/createLease/inputPS'), '66667777')

// Insert PID 
WebUI.setText(findTestObject('LeaseLicenses/createLease/inputPID'), '6227481')

// Insert description
WebUI.setText(findTestObject('LeaseLicenses/createLease/textareaDescription'), 'Description Automated test')

// Insert notes
WebUI.setText(findTestObject('LeaseLicenses/createLease/textareaNotes'), 'Automated Notes Test')

// Click Save button
WebUI.click(findTestObject('LeaseLicenses/createLease/buttonSave'))

// Click Save anyways button
WebUI.click(findTestObject('LeaseLicenses/createLease/buttonSaveAnyways'))

// VERIFY ELEMENTS ON LEASE DETAILS
WebUI.waitForElementVisible(findTestObject('LeaseLicenses/viewLeaseDetails/leaseDetailsTitle'), 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseProgramName'), 'value', 'BC Ferries', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseType'), 'value', 'Ground Lease', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelReceivableTo'), 'value', 'Payable (BCTFA as tenant)', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelCategoryType'), 'value', 'Agricultural', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelPurpose'), 'value', 'Gravel', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseInitiator'), 'value', 'Headquarters', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseResponsibility'), 'value', 'Headquarters', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelMotiContact'), 'value', 'MOTI Test Contact', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseLIS'), 'value', '123456', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeasePS'), 'value', '66667777', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseDescription'), 'value', 'Description test', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/viewLeaseDetails/labelLeaseNotes'), 'value', 'Notes Test', 0)

WebUI.closeBrowser()

