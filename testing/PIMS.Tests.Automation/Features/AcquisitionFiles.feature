@Acquisition-Files
Feature: AcquisitionFiles

A short summary of the feature

Scenario: Create a new complete Acquisition File with pins
	Given I create a new Acquisition File from row number 1
	When I add additional information to the Acquisition File
	And  I create Digital Documents for a "Acquisition File" row number 5
	And I create a new Note on the Notes Tab from row number 3
	Then A new Acquisition file is created successfully

Scenario: Edit existing Acquisition File and Properties
	Given I edit an existing Acquisition File from row number 2
	When I update a Property details from row number 4
	And I navigate back to the Acquisition File Summary
	Then An existing Acquisition file has been edited successfully

Scenario: Create an Acquisition File from Pin
	Given I create an Acquisition File from a pin on map from row number 3
	Then A new Acquisition file is created successfully

Scenario: Verify Acquisition Files List View, Content and Results Content
	Given I search for an existing Acquisition File from row number 1
	Then Expected Acquisition File Content is displayed on Acquisition File Table
