@Acquisition-Files
Feature: AcquisitionFiles

A short summary of the feature

Scenario: 01. Create a new complete Acquisition File with pins
	Given I create a new Acquisition File
	When I add additional information to the Acquisition File
	And I add several Properties to the Acquisition File
	Then A new Acquisition file is created successfully

Scenario: 02. Create an Acquisition File from Pin
	Given I create an Acquisition File from a pin on map
	Then A new Acquisition file is created successfully

Scenario: 03. Edit existing Acquisition File and Properties
	Given I search for an existing acquisition file
	When I edit an existing Acquisition File
	Then An existing Acquisition file has been edited successfully

#Scenario: 04. Create an Activity within an Acquisition File
#	Given I create a new Acquisition File
#	When I create and delete an activity
#	Then An activity is deleted successfully

Scenario: 04. Cancel Creation of an Acquisition File
	Given I navigate to create new Acquisition File
	When I create and cancel new Acquisition Files
	Then The creation of an Acquisition File is cancelled successfully

#Scenario: 05. Notes on Acquisition File's activity
#	Given I create a new Acquisition File
#	When I create an activity with notes and delete notes
#	Then A note has been deleted successfully

#Scenario: 06. Digital Documents on Acquisition File's activity
#	Given I create a new Acquisition File
#	When I create an Acquisition File with activity and a document attached
#	Then A digital document has been uploaded successfully

Scenario: 05. Digital Documents on Acquisition File
	Given I create a new Acquisition File
	When I navigate to the Documents Tab, validate Documents Types and attach a Document
	Then A digital document has been uploaded successfully

#Scenario: 07. Edit Digital Documents on Acquisition File's activity
#	Given I create a new Acquisition File
#	When I create an Acquisition File with activity and edit attached document
#	Then A digital document has been deleted successfully

Scenario: 06. Edit Digital Documents on Acquisition File
	Given I create a new Acquisition File
	When I edit an attached document
	Then A digital document has been deleted successfully

Scenario: 07. Notes Tab on Acquisition File
	Given I create a Acquisition File with a new Note on the Notes Tab
	Then The Notes Tab rendered successfully

Scenario: 08. Verify Acquisition Files List View, Content and Results Content
	Given I search for an existing Acquisition File
	Then Expected Acquisition File Content is displayed on Acquisition File Table
