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

//Click the sign in button
WebUI.click(findTestObject('Object Repository/Login/SignInButton'))

// Click the IDIR authentication button
WebUI.click(findTestObject('Object Repository/SiteMinder/IdirButton'))

// Enter IDIR in appropriate field
WebUI.setText(findTestObject('Object Repository/SiteMinder/IdirField'), GlobalVariable.userName)

// Enter password
WebUI.setEncryptedText(findTestObject('Object Repository/SiteMinder/PasswordField'), GlobalVariable.password)

// Click continue
WebUI.click(findTestObject('Object Repository/SiteMinder/ContinueButton'))

// Click on Contacts button
WebUI.click(findTestObject('Contacts/createIndividualContact/contactButton'))

// Add new Contact button
WebUI.click(findTestObject('Contacts/createIndividualContact/addContactButton'))

// Choose Individual Contact Option
WebUI.click(findTestObject('Contacts/createOrganizationContact/OrganizationContactOption'))

// Insert Organization Name
WebUI.setText(findTestObject('Contacts/createOrganizationContact/organizationNameInput'), 'Capsule Corporation')

// Insert Alias
WebUI.setText(findTestObject('Contacts/createOrganizationContact/orgAliasInput'), 'Cap Corp')

// Insert Incorporation Number
WebUI.setText(findTestObject('Contacts/createOrganizationContact/IncorporationNbrInput'), '123456')

// Inserting email 01
WebUI.setText(findTestObject('Contacts/createIndividualContact/email01Input'),
	'test@capcorp.com')

// Inserting email 01 type
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/email01TypeSelect'),
	'PERSEMAIL', true)

// Adding a second email
WebUI.click(findTestObject('Contacts/createOrganizationContact/addAnotherEmailLink'))

// Inserting second email input
WebUI.setText(findTestObject('Contacts/createOrganizationContact/email02Input'), 
    'test@capcorp.com')

// Selecting second email type
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/emailType02Select'), 
    'WORKEMAIL', true)

// Adding phone number
WebUI.setText(findTestObject('Contacts/createOrganizationContact/phone01Input'), 
    '2356789000')

// Selecting phone number type
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/phoneType01Select'), 
    'WORKMOBIL', true)

// Adding second phone number link
WebUI.click(findTestObject('Contacts/createOrganizationContact/addAnotherPhoneLink'))

// Inserting second phone number Input
WebUI.setText(findTestObject('Contacts/createOrganizationContact/phone02Input'), 
    '9999999999')

// Selecting second phone number type
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/phoneType02Select'), 
    'WORKPHONE', true)

// Adding mailing Address
WebUI.setText(findTestObject('Contacts/createOrganizationContact/mailingAddressLine1Input'), 
    '123 Margaret St')
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/mailingAddressCountrySelect'), 
    '4', true)
WebUI.setText(findTestObject('Contacts/createOrganizationContact/mailingCityInput'), 
    'Brisbane')
WebUI.setText(findTestObject('Contacts/createOrganizationContact/mailingOtherCountryInput'), 
    'Australia')
WebUI.setText(findTestObject('Contacts/createOrganizationContact/mailingAddressPostalInput'), 
    '4000')

// Adding Property Address
WebUI.setText(findTestObject('Contacts/createOrganizationContact/propertyAddressLine1Input'), 
    '123-788 French St')
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/propertyCountrySelect'), 
    '1', true)
WebUI.setText(findTestObject('Contacts/createOrganizationContact/propertyCityInput'), 
    'Vancouver')
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/propertyProvinceSelect'), 
    '1', true)
WebUI.setText(findTestObject('Contacts/createOrganizationContact/propertyPostalInput'), 
    'V0V0V0')

// Adding Billing Address
WebUI.setText(findTestObject('Contacts/createOrganizationContact/billingAddressLine1Input'), 
    '700 Main Rd')
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/billingCountrySelect'), 
    '3', true)
WebUI.setText(findTestObject('Contacts/createOrganizationContact/billingCityInput'), 
    'Guadalajara')
WebUI.selectOptionByValue(findTestObject('Contacts/createOrganizationContact/billingProvinceSelect'), 
    '65', true)
WebUI.setText(findTestObject('Contacts/createOrganizationContact/billingPostalCodeInput'), 
    '123444')

// Adding contact comments
WebUI.setText(findTestObject('Contacts/createOrganizationContact/commentsTextarea'), 
    'Organization Testing comments')

// Saving contact
WebUI.click(findTestObject('Contacts/createOrganizationContact/saveButton'))

// Looking for created contact
WebUI.setText(findTestObject('Contacts/createOrganizationContact/searchInput'), 'capsule')
WebUI.click(findTestObject('Contacts/createOrganizationContact/searchContactButton'))

// Viewing new contact details
WebUI.click(findTestObject('Contacts/createOrganizationContact/organizationFirstContactLink'))

WebUI.closeBrowser()

