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

// Open Browser
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

// Select new status
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/editStatus/statusSelect'), 
    'EXPIRED', true)

// Save new status
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/saveButton'))

// Verifying saved status
WebUI.verifyElementText(findTestObject('LeaseLicenses/editStatus/licenseStatusLabel'), 'Expired')

//Click on Edit button on Lease Details Screen
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/editButton'))

// Select new status
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/editStatus/statusSelect'), 
    'ACTIVE', true)

// Save new status
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/saveButton'))

// Verifying saved status
WebUI.verifyElementText(findTestObject('LeaseLicenses/editStatus/licenseStatusLabel'), 'Active')

// Click on Edit button on Lease Details Screen
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/editButton'))

// Select new status
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/editStatus/statusSelect'), 
    'DRAFT', true)

// Save new status
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/saveButton'))

// Verifying saved status
WebUI.verifyElementText(findTestObject('LeaseLicenses/editStatus/licenseStatusLabel'), 'Draft')

// Click on Edit button on Lease Details Screen
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/editButton'))

WebUI.selectOptionByValue(findTestObject('LeaseLicenses/editStatus/statusSelect'), 
    'TERMINATED', true)

// Save Edited License
WebUI.click(findTestObject('LeaseLicenses/updateLeaseDetails/saveButton'))

// Verifying saved status
WebUI.verifyElementText(findTestObject('LeaseLicenses/editStatus/licenseStatusLabel'), 'Terminated')

// Closing browser
WebUI.closeBrowser()

