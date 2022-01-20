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

// Click expand option
WebUI.click(findTestObject('Navigation/ExpandButton'))

// Click admin tools
WebUI.click(findTestObject('Object Repository/Navigation/AdminToolsExpandedButton'))

// Click manage users
WebUI.click(findTestObject('Object Repository/Navigation/AdminNavigation/ManageUsers'))

// Filter by first name
WebUI.setText(findTestObject('AdminTools/ManageUsers/UserFilter/UserFirstName'), 'Jonny')

// Filter fire action
WebUI.click(findTestObject('Object Repository/AdminTools/ManageUsers/UserFilter/SearchButton'))

// Click the filtered username
WebUI.click(findTestObject('AdminTools/ManageUsers/UserFilter/FilteredUserName'))

// Fill in notes for user
WebUI.setText(findTestObject('AdminTools/ManageUsers/UserNote'), 'These notes have been generated from a FT test script')

// Click the save button 
WebUI.click(findTestObject('Object Repository/AdminTools/ManageUsers/ManageUserSaveButton'))

// Check for toast confirming user has been updated
WebUI.waitForElementPresent(findTestObject('Object Repository/AdminTools/ManageUsers/UserUpdatedToast'), 10)

// Close browser
WebUI.closeBrowser()