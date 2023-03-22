Feature: AdminTools

Feature that covers User Access, User lists, CDOGS and Help Desk sections

@AdminTools

Scenario: 01. Help Desk Section
	Given I review the Help Desk Section
	Then Help Desk rendered successfully
	
Scenario: 02. User Management List View
	Given I enter to the User Management List View
	Then User Management rendered successfully

Scenario: 03. CDOGS Templates
	Given I create a CDOGS template
	Then CDOGS rendered successfully

Scenario: 04. Create and Edit Financial Codes
	Given I create a Financial Code
	And I update a Financial Code
	Then Financial Codes rendered successfully

Scenario: 05. Duplicate existing Financial Code
	Given I attempt to duplicate a Financial Code
	Then Financial Code cannot be duplicated successfully
