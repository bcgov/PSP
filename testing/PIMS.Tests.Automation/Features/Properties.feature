Feature: Properties

Property Inventory and Information Details test cases

@Properties
Scenario: Property Map Filters and Details validation
	Given I search for a Property in the Inventory by different filters from row number 9

Scenario: Invalid Property Not Found
	Given I search for an Invalid Property from row number 10
	Then No Properties were found

Scenario: Property Information Tab Details
	Given I review a Property's Information
	When I make changes on the selected property information from row number 3
	Then A Property Information is saved successfully
	
Scenario: Non-Inventory Property Information
	Given I search for a non MOTI property from row number 9
	Then Non-Inventory property renders correctly
