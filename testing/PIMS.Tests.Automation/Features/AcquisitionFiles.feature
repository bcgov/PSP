@Acquisition-Files
Feature: AcquisitionFiles

A short summary of the feature

Scenario: Create a new complete Acquisition File with pins
	Given I create a new Acquisition File
	When I add additional information to the Acquisition File
	And I add several Properties to the Acquisition File
	And  I create Digital Documents for a "Acquisition File" row number 5
	And I create a new Note on the Notes Tab from row number 3
	Then A new Acquisition file is created successfully

Scenario: Edit existing Acquisition File and Properties
	Given I search for an existing acquisition file
	When I edit an existing Acquisition File
	Then An existing Acquisition file has been edited successfully

Scenario: Create an Acquisition File from Pin
	Given I create an Acquisition File from a pin on map
	Then A new Acquisition file is created successfully

Scenario: Verify Acquisition Files List View, Content and Results Content
	Given I search for an existing Acquisition File
	Then Expected Acquisition File Content is displayed on Acquisition File Table
