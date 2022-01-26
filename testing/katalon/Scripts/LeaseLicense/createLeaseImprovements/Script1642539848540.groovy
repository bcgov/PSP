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

// Click Improvements Link
WebUI.click(findTestObject('LeaseLicenses/leaseImprovements/improvementsLink'))

// Click on Improvements edit button
WebUI.click(findTestObject('LeaseLicenses/leaseImprovements/editImprovementsButton'))

// Inserting commercial Address input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/commecialAddressInput'), 
    '123 Main St.')

// Inserting Commercial structure size input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/commecialSizeInput'), 
    '123 sqft')

// Inserting Commercial description input 
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/commercialDescriptionTextarea'), 
    'Commercial property automation test')

// Inserting Other improvements Address input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/otherImprovementsAddressInput'), 
    '456 Richmond Ave.')

// Inserting Other improvements structure size input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/otherImprovementsSizeInput'), 
    '2344 sqft')

// Inserting Other improvements description input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/otherImprovementDescriptionTextarea'), 
    'Other improvements automation test')

// Inserting residential Address input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/residentialAddressInput'), 
    '788-1899 Hornby St.')

// Inserting Residential structure size input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/residentialSizeInput'), 
    '2344 sqft')

// Inserting Residential description input
WebUI.setText(findTestObject('LeaseLicenses/leaseImprovements/residentialDescriptionTextarea'), 
    'Residential Improvement Automation Test')

// Saving Improvements input
WebUI.click(findTestObject('LeaseLicenses/leaseImprovements/saveButton'))

// VERIFY ELEMENTS ON LEASE IMPROVEMENTS
WebUI.waitForElementVisible(findTestObject('LeaseLicenses/leaseImprovements/improvementsTitleLabel'), 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/commecialAddressInput'), 'value', '123 Main St.', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/commecialSizeInput'), 'value', '123 sqft', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/commercialDescriptionTextarea'), 'value', 'Commercial property automation test', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/otherImprovementsAddressInput'), 'value', '456 Richmond Ave.', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/otherImprovementsSizeInput'), 'value', '2344 sqft', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/otherImprovementDescriptionTextarea'), 'value', 'Other improvements automation test', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/residentialAddressInput'), 'value', '788-1899 Hornby St.', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/residentialSizeInput'), 'value', '2344 sqft', 0)

WebUI.verifyElementAttributeValue(findTestObject('LeaseLicenses/leaseImprovements/residentialDescriptionTextarea'), 'value', 'Residential Improvement Automation Test', 0)

WebUI.closeBrowser()

