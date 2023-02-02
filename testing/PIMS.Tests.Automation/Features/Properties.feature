Feature: Properties

Property Inventory and Information Details test cases

@Properties
Scenario: Property Map Filters and Details validation
	Given I search for a Property in the Inventory by different filters
	Then LTSA Pop-up Information validation is successful

Scenario: Invalid Property Not Found
	Given I search for an Invalid Property
	Then No Properties were found

Scenario: Property Information Tab Details
	Given I review a Property's Information
	When I make some changes on the selected property information
	Then A Property Information is saved successfully
	
Scenario: Non-Inventory Property Information
	Given I search for a non MOTI property
	Then Non-Inventory property renders correctly
