@Regression-Properties
Feature: Properties

Property Inventory and Information Details test cases

Scenario: Property Map Filters and Details validation
	Given I search for a Property in the Inventory by different filters from row number 9
	Then Properties filters works successfully

Scenario: Invalid Property Not Found
	Given I search for an Invalid Property from row number 10
	Then No Properties were found

Scenario: Property Information Tab
	Given I review a Property's Information
	When I update a Property details from row number 3
	Then A Property Information is saved successfully

Scenario: Property PIMS Files Tab
	Given I search for a property in the inventory by PID from row number 26
	When I verify the PIMS Files Tab
	Then PIMS Files Tab has rendered successfully

Scenario: Property Management Tab
	Given I search for a property in the inventory by PID from row number 25
	When I insert information in the Property Management Tab from row number 1
	And I update information in the Property Management Tab from row number 2
	And I clean up the Property Management Tab from row number 3
	Then Property Management Tab has been updated successfully

Scenario: Property Management Activity Digital Documents
	Given I search for a property in the inventory by PID from row number 25
	When I insert activities to the Property Management Tab from row number 4
	And I create Digital Documents for a Property Management row number 11
	And I delete all activities from the Property Management Tab
	Then Property Management Tab has been updated successfully
	
Scenario: Non-Inventory Property Information
	Given I search for a non MOTI property from row number 9
	Then Non-Inventory property renders correctly


