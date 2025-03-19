@AdminTools
Feature: AdminTools

Feature that covers User Access, User lists, CDOGS and Help Desk sections

Scenario: 01._Help_Desk_Section
	Given I review the Help Desk Section
	Then Help Desk rendered successfully

Scenario: 02._User_Management_List_View
	Given I enter to the User Management List View
	Then User Management rendered successfully

Scenario: 03._CDOGS_Templates
	Given I create a CDOGS template
	Then CDOGS rendered successfully

Scenario: 04._Financial_Codes
	Given I create a Financial Code from row number 1
	And I update a Financial Code from row number 2
	Then Financial Codes rendered successfully

Scenario: 05._Duplicate_existing_Financial_Code
	Given I attempt to duplicate a Financial Code from row number 1
	Then Financial Code cannot be duplicated successfully

Scenario: 06._Financial_Codes_List_View
	Given I search for an existing Financial Code from row number 1
	Then Financial Codes rendered successfully
