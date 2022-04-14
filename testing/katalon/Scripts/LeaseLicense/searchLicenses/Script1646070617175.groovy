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

// Search by L-File
WebUI.setText(findTestObject('LeaseLicenses/searchLease/inputSearchByFile'), '036')
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent("L-000-036", true)

// Search by Civic Address
WebUI.click(findTestObject('LeaseLicenses/searchLease/selectSearchBy'))
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/searchLease/selectSearchBy'), 'address',
	true)
WebUI.setText(findTestObject('LeaseLicenses/searchLease/inputSearchByAddress'), 'Souris')
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('456 Souris Street, PO Box 250', true)

// Search by PIN/PID
WebUI.click(findTestObject('LeaseLicenses/searchLease/selectSearchBy'))
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/searchLease/selectSearchBy'), 'pinOrPid',
	true)
WebUI.setText(findTestObject('LeaseLicenses/searchLease/inputSearchByPIDPIN'), '758')
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('758018854', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

// Search by Status
WebUI.click(findTestObject('LeaseLicenses/searchLease/selectStatus'))
WebUI.selectOptionByValue(findTestObject('LeaseLicenses/searchLease/selectStatus'), 'DRAFT',
	true)
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-334', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

// Search by Expiry Date
WebUI.setText(findTestObject('LeaseLicenses/searchLease/inputExpiryStartDate'), '02/02/2020')
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-009', true)
WebUI.setText(findTestObject('LeaseLicenses/searchLease/inputExpiryEndDate'), '02/02/2022')
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-386', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

// Search by Program Name
WebUI.click(findTestObject('LeaseLicenses/searchLease/divProgramName'))
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/liProgramName01'))
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-023', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/divProgramName'))
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/liProgramName02'))
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-029', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

// Search by Tenant Name
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/searchLease/inputTenantName'), 'Peter')
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-009', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

// Search by Regions
WebUI.selectOptionByValue(findTestObject('Object Repository/LeaseLicenses/searchLease/selectRegions'), '1',
	true)
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-020', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

// Search by Keyword
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/searchLease/inputKeyword'), 'Lorem')
WebUI.click(findTestObject('Object Repository/LeaseLicenses/searchLease/buttonSearchLease'))
WebUI.verifyTextPresent('L-000-010', true)
WebUI.click(findTestObject('LeaseLicenses/searchLease/buttonReset'))

WebUI.closeBrowser()


