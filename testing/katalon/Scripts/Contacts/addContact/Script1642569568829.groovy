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

// Click the contacts navigation button
WebUI.click(findTestObject('Object Repository/Navigation/ContactsExpandedButton'))

// Add a new contact 
WebUI.click(findTestObject('Object Repository/Contacts/AddNewContactButton'))
WebUI.setText(findTestObject('Object Repository/Contacts/FirstName'), 'Chester')
WebUI.setText(findTestObject('Object Repository/Contacts/LastName'), 'Tester')
WebUI.setText(findTestObject('Object Repository/Contacts/ContactPreferredName'), 'Chuck')
WebUI.setText(findTestObject('Object Repository/Contacts/ContactMiddleName'), 'Davis')
WebUI.setText(findTestObject('Object Repository/Contacts/ContactEmail'), 'chuck@test.com')
WebUI.selectOptionByValue(findTestObject('Object Repository/Contacts/ContactEmailType'), "WORKEMAIL", false)
WebUI.setText(findTestObject('Object Repository/Contacts/ContactPhoneNumber'), '2501234567')
WebUI.selectOptionByValue(findTestObject('Object Repository/Contacts/ContactPhoneType'), "WORKMOBIL", false)
WebUI.setText(findTestObject('Object Repository/Contacts/Address'),"1234 Fake St.")
WebUI.selectOptionByValue(findTestObject('Object Repository/Contacts/CountrySelect'), "1", false)
WebUI.setText(findTestObject('Object Repository/Contacts/City'), "West Kelowna")
WebUI.selectOptionByValue(findTestObject('Object Repository/Contacts/ProvinceSelect'), "1", false)
WebUI.setText(findTestObject('Object Repository/Contacts/ContactPostalCode'), "V0S 1N0")
WebUI.click(findTestObject('Object Repository/Contacts/SaveButton'))
WebUI.closeBrowser()

