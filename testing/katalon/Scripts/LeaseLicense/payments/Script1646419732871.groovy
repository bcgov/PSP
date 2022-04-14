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
WebUI.openBrowser('https://tst-pims.th.gov.bc.ca');

// Maximize browser window
WebUI.maximizeWindow();

// Click the sign in button
WebUI.click(findTestObject('Object Repository/Login/SignInButton'));

// Click the IDIR authentication button
WebUI.click(findTestObject('Object Repository/SiteMinder/IdirButton'));

// Enter IDIR in appropriate field
WebUI.setText(findTestObject('Object Repository/SiteMinder/IdirField'), GlobalVariable.userName);

// Enter password
WebUI.setEncryptedText(findTestObject('Object Repository/SiteMinder/PasswordField'), GlobalVariable.password);

// Click continue
WebUI.click(findTestObject('Object Repository/SiteMinder/ContinueButton'));

// Click the expand button on left navigation
WebUI.click(findTestObject('Navigation/ExpandButton'));

// Locate and click expanded management option
WebUI.click(findTestObject('Navigation/ManagementExpandedButton'));

// Click lease and licenses hyperlink
WebUI.click(findTestObject('LeaseLicenses/searchLease/LeaseLicenseHyperlink'));

// Filter for lease with file number 
WebUI.setText(findTestObject('LeaseLicenses/searchLease/LLumberFilter'), "L-000-013");

// Search
WebUI.click(findTestObject('LeaseLicenses/searchLease/FilterSearchButton'));

// Click the filtered result
WebUI.click(findTestObject('LeaseLicenses/payments/PaymentLease'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/PaymentsHyperlink'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/AddTermButton'));

// Verify cancel button
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/TermModalCancelButton'));

// Reopen modal
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/AddTermButton'));

// Verify add term modal has appropriate fields
WebUI.verifyTextPresent("Start date", false);
WebUI.verifyTextPresent("End date", false);
WebUI.verifyTextPresent("Payment frequency", false);
WebUI.verifyTextPresent("Agreed payment", false);
WebUI.verifyTextPresent("Payments due", false);
WebUI.verifyTextPresent("Subject to GST?", false);
WebUI.verifyTextPresent("Term Status", false);

// Exercised term

//Select dates
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/StartDate'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/CalendarValue'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/EndDate'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/CalendarValue'));

// Select payment frequency type
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/PaymentFrequency'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/PaymentFrequencyValue'));

// Set payment amount
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/payments/PaymentAmount'), "1234");

// Set payments due value
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/payments/PaymentsDue'), "1st of each month.");

// Set exercised option
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/TermStatus'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/ExercisedOption'));

// Save the term
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/SaveTermButton'));

// Expand to record a payment
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/Expander'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/RecordAPaymentButton'));

// Fill in payment details
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/ReceivedDate'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/CalendarValue'));
WebUI.setText(findTestObject('Object Repository/LeaseLicenses/payments/TotalReceived'), "12345");
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/PaymentSave'));

// Delete payment
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/DeleteActual'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/ConfirmDeletePayment'));

// Change term to Not Exercised to delete
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/EditTerm'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/TermStatus'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/NotExercisedOption'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/SaveTermButton'));

// Should now be able to delete the term since it is no longer exercised
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/DeleteTerm'));
WebUI.click(findTestObject('Object Repository/LeaseLicenses/payments/ConfirmDeletePayment'));
WebUI.waitForElementPresent(findTestObject('Object Repository/LeaseLicenses/payments/TermDeletedModal'), 10)

WebUI.closeBrowser()

