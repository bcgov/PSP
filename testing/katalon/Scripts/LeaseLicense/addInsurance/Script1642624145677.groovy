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

// Click the expand button on left navigation
WebUI.click(findTestObject('Navigation/ExpandButton'))

// Locate and click expanded management option
WebUI.click(findTestObject('Navigation/ManagementExpandedButton'))

// Click lease and licenses hyperlink
WebUI.click(findTestObject('LeaseLicenses/searchLease/LeaseLicenseHyperlink'))

// Filter for lease with file number "111-111-111"
WebUI.setText(findTestObject('LeaseLicenses/searchLease/LLumberFilter'), "L-000-043")

// Search
WebUI.click(findTestObject('LeaseLicenses/searchLease/FilterSearchButton'))


// Click the filtered result
WebUI.click(findTestObject('LeaseLicenses/searchLease/FilteredLease'))
WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/InsuranceHyperlink'))
WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/EditButton'))

if(WebUI.verifyElementNotChecked(findTestObject('Object Repository/LeaseLicenses/insurance/AircraftCheckbox'), 5, FailureHandling.OPTIONAL)) {
	WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/AircraftCheckbox'))
}

if(WebUI.verifyElementNotChecked(findTestObject('Object Repository/LeaseLicenses/insurance/GeneralCheckbox'), 5, FailureHandling.OPTIONAL)) {
	WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/GeneralCheckbox'))
}

if(WebUI.verifyElementNotChecked(findTestObject('Object Repository/LeaseLicenses/insurance/MarineCheckbox'), 5, FailureHandling.OPTIONAL)) {
	WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/MarineCheckbox'))
}

if(WebUI.verifyElementNotChecked(findTestObject('Object Repository/LeaseLicenses/insurance/OtherCheckbox'), 5, FailureHandling.OPTIONAL)) {
	WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/OtherCheckbox'))
}

if(WebUI.verifyElementNotChecked(findTestObject('Object Repository/LeaseLicenses/insurance/VehicleCheckbox'), 5, FailureHandling.OPTIONAL)) {
	WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/VehicleCheckbox'))
}

// aircraft insurance
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/AircraftForm/AircraftDescriptionOfCoverage'), 'Generated description by automation test')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/AircraftForm/AircraftLimit'), '1234')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/AircraftForm/AircraftPolicyExpiryDate'),'09/20/2025')

// general insurance
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/GeneralForm/GeneralDescriptionOfCoverage'), 'Generated description by automation test')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/GeneralForm/GeneralLimit'), '1234')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/GeneralForm/GeneralPolicyExpiryDate'),'09/20/2025')

// marine insurance
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/MarineForm/MarineDescriptionOfCoverage'), 'Generated description by automation test')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/MarineForm/MarineLimit'), '1234')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/MarineForm/MarinePolicyExpiryDate'),'09/20/2025')

// other insurance
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/OtherForm/OtherDescriptionOfCoverage'), 'Generated description by automation test')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/OtherForm/OtherLimit'), '1234')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/OtherForm/OtherPolicyExpiryDate'),'09/20/2025')

// vehicle insurance
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/VehicleForm/VehicleDescriptionOfCoverage'), 'Generated description by automation test')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/VehicleForm/VehicleLimit'), '1234')
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/insurance/VehicleForm/VehiclePolicyExpiryDate'),'09/20/2025')

WebUI.click(findTestObject('Object Repository/LeaseLicenses/insurance/SaveButton'))

// verify insurance view
// verify aircraft liability coverage view
WebUI.verifyTextPresent('Aircraft Liability Coverage', true)
WebUI.verifyTextPresent('Insurance in place:', true)
WebUI.verifyTextPresent('No', true)
WebUI.verifyTextPresent('Limit:', true)
WebUI.verifyTextPresent('1,234', true)
WebUI.verifyTextPresent('Sep 20, 2025', true)
WebUI.verifyTextPresent('Description of Coverage', true)
WebUI.verifyTextPresent('Generated description by automation test', true)

// verify commercial general liability coverage view
WebUI.verifyTextPresent('Commercial General Liability', true)
WebUI.verifyTextPresent('Insurance in place:', true)
WebUI.verifyTextPresent('No', true)
WebUI.verifyTextPresent('Limit:', true)
WebUI.verifyTextPresent('1,234', true)
WebUI.verifyTextPresent('Sep 20, 2025', true)
WebUI.verifyTextPresent('Description of Coverage', true)
WebUI.verifyTextPresent('Generated description by automation test', true)

// verify marine liability coverage view
WebUI.verifyTextPresent('Marine Liability Coverage', true)
WebUI.verifyTextPresent('Insurance in place:', true)
WebUI.verifyTextPresent('No', true)
WebUI.verifyTextPresent('Limit:', true)
WebUI.verifyTextPresent('1,234', true)
WebUI.verifyTextPresent('Sep 20, 2025', true)
WebUI.verifyTextPresent('Description of Coverage', true)
WebUI.verifyTextPresent('Generated description by automation test', true)

// verify vehicle liability coverage view
WebUI.verifyTextPresent('Vehicle Liability Coverage', true)
WebUI.verifyTextPresent('Insurance in place:', true)
WebUI.verifyTextPresent('No', true)
WebUI.verifyTextPresent('Limit:', true)
WebUI.verifyTextPresent('1,234', true)
WebUI.verifyTextPresent('Sep 20, 2025', true)
WebUI.verifyTextPresent('Description of Coverage', true)
WebUI.verifyTextPresent('Generated description by automation test', true)

// verify other liability coverage view
WebUI.verifyTextPresent('Other Insurance Coverage', true)
WebUI.verifyTextPresent('Insurance in place:', true)
WebUI.verifyTextPresent('No', true)
WebUI.verifyTextPresent('Limit:', true)
WebUI.verifyTextPresent('1,234', true)
WebUI.verifyTextPresent('Sep 20, 2025', true)
WebUI.verifyTextPresent('Description of Coverage', true)
WebUI.verifyTextPresent('Generated description by automation test', true)


//WebUI.closeBrowser()

