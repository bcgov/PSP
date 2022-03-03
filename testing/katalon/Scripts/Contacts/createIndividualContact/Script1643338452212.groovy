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
WebUI.click(findTestObject('Contacts/createIndividualContact/individualContactOption'))

// Insert First Name
WebUI.setText(findTestObject('Contacts/createIndividualContact/firstNameInput'), 'IndTest')

// Insert Last Name
WebUI.setText(findTestObject('Contacts/createIndividualContact/lastNameInput'), 'IndTestLN')

// Insert Preferred Name
WebUI.setText(findTestObject('Contacts/createIndividualContact/preferredNameInput'), 'Individual Test')

// Insert Organization Link
WebUI.setText(findTestObject('Contacts/createIndividualContact/organizationLinkInput'), 
    'Tes')

// Choose Organization from dropdown
WebUI.click(findTestObject('Contacts/createIndividualContact/orgTestLink'))


// Inserting email 01
WebUI.setText(findTestObject('Contacts/createIndividualContact/email01Input'), 
    'test@test.com')

// Inserting email 01 type
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/email01TypeSelect'), 
    'PERSEMAIL', true)

// Click on add another email link
WebUI.click(findTestObject('Contacts/createIndividualContact/addAnotherEmailAddressLink'))

// Inserting email 02
WebUI.setText(findTestObject('Contacts/createIndividualContact/email02Input'), 
    'test@test.ca')

// Inserting email 02 type
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/email02TypeSelect'), 
    'WORKEMAIL', true)

// Inserting phone 01
WebUI.setText(findTestObject('Contacts/createIndividualContact/phone01Input'), 
    '2333338900')

// Inserting phone 01 type
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/phone01TypeSelect'), 
    'PERSPHONE', true)

// Click on add another phone link
WebUI.click(findTestObject('Contacts/createIndividualContact/addAnotherPhoneLink'))

// Click on add another email link
WebUI.setText(findTestObject('Contacts/createIndividualContact/phone02Input'), 
    '45688889078')

// Click on add another phone link
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/phone02TypeSelect'), 
    'WORKMOBIL', true)

// Inserting mailing Line 1
WebUI.setText(findTestObject('Contacts/createIndividualContact/mailingAddressLine1Input'), 
    '123 Main St')

// Selecting country for mailing address
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/mailingCountrySelect'), 
    '1', true)

// Inserting city for mailing address 
WebUI.setText(findTestObject('Contacts/createIndividualContact/mailingCityInput'), 
    'Victoria')

// Selecting province for mailing address
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/mailingProvinceSelect'), 
    '1', true)

// Inserting mailing postal code
WebUI.setText(findTestObject('Contacts/createIndividualContact/mailingPostalCodeInput'), 
    'V0V0V0')

// Inserting property Line 1
WebUI.setText(findTestObject('Contacts/createIndividualContact/propertyAddressLine1Input'), 
    '567 Richmond')

// Selecting add a new line to address
WebUI.click(findTestObject('Contacts/createIndividualContact/addPropertyAddressLine'))

// Inserting property Line 2
WebUI.setText(findTestObject('Contacts/createIndividualContact/propertyAddressLine2Input'), 
    'Apt 566')

// Selecting country for property address
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/propertyCountrySelect'), 
    '3', true)

// Inserting city for property address
WebUI.setText(findTestObject('Contacts/createIndividualContact/propertyCityInput'), 
    'Mexico')

// Selecting province for property address
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/propertyProvinceSelect'), 
    '65', true)

// Inserting property postal code
WebUI.setText(findTestObject('Object Repository/Contacts/createIndividualContact/input_Postal Code_propertyAddress.postal'), 
    'Postal Code')

// Inserting billing Line 1
WebUI.setText(findTestObject('Contacts/createIndividualContact/billingAddressLine1'), 
    '890-900 Hawaii St.')

// Selecting country for billing address
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/billingCountrySelect'), 
    '2', true)

// Inserting city for billing address 
WebUI.setText(findTestObject('Contacts/createIndividualContact/billingCityInput'), 
    'Adelaine')

// Selecting state for billing address
WebUI.selectOptionByValue(findTestObject('Contacts/createIndividualContact/billingStateSelect'), 
    '30', true)

// Inserting billing postal code
WebUI.setText(findTestObject('Contacts/createIndividualContact/billingZipCodeInput'), 
    '123456')

// Adding comments
WebUI.setText(findTestObject('Contacts/createIndividualContact/commentsTextarea'), 'Testing comments')

// Saving new individual contact
WebUI.click(findTestObject('Contacts/createIndividualContact/saveButton'))

// Looking for the created individual
WebUI.setText(findTestObject('Contacts/createIndividualContact/searchInput'), 'IndTest')

// Click on search button
WebUI.click(findTestObject('Contacts/createIndividualContact/searchContactButton'))

// Click on new created contact
WebUI.click(findTestObject('Contacts/createIndividualContact/indContactLink'))

WebUI.closeBrowser()


