Feature: AcquisitionFiles

A short summary of the feature

@Acquisition-Files
Scenario: Create a new Acquisition File with pins
	Given I create a new Acquisition File
	When I add additional information to the Acquisition File
	And I add several Properties to the Acquisition File
	Then A new Acquisition file is created successfully

Scenario: Create an Acquisition File from Pin and Edition
	Given I create an Acquisition File from a pin on map
	When I edit an existing Acquisition File
	Then A new Acquisition file is created successfully

Scenario: Create an Activity within an Acquisition File
	Given I create a new Acquisition File
	When I create and delete an activity
	Then An activity is deleted successfully
