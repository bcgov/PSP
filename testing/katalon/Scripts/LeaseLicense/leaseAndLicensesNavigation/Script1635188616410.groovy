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

// Ensure Leases & Licenses are present in the list
WebUI.verifyTextPresent("Search for a Lease or License", false)

// Close browser
WebUI.closeBrowser()