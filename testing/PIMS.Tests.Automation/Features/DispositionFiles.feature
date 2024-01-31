@Disposition-Files
Feature: DispositionFiles

Test cases for Disposition Files feature

Scenario: 01. Disposition File Details
	Given I create a new Disposition File from row number 2
	When I add additional information to the Disposition File Details
	And I update the File details from an existing Disposition File from row number 3
	Then A new Disposition file is created successfully

#Scenario: 02. Disposition File Properties
#	Given I create a new Disposition File from row number 5
#	When I add Properties to the Disposition File
#	And I update a Disposition File's Properties from row number 6
#	And I update a Property details from row number 5
#	Then A new Disposition file is created successfully

Scenario: 03. Disposition Checklist Tab
	Given I create a new Disposition File from row number 7
	When I insert Checklist information to an Disposition File
	Then Disposition File's Checklist has been saved successfully

Scenario: 04. Disposition Offers and Sale Tab
	Given I create a new Disposition File from row number 8
	When I create Appraisal, Assessment and Offers within a Disposition File
	And I update Appraisal, Assessment and Offers section within Disposition File from row number 9
	Then A new Disposition file is created successfully

Scenario: 05. Disposition Files Digital Documents
	Given I create a new Disposition File from row number 10
	When I create Digital Documents for a "Disposition File" row number 12
	And  I edit a Digital Document for a "Disposition File" from row number 13
	Then A new Disposition file is created successfully

Scenario: 06. Disposition File Notes
	Given I create a new Disposition File from row number 11
	When  I create a new Note on the Notes Tab from row number 9
	And  I edit a Note on the Notes Tab from row number 10
	Then A new Disposition file is created successfully
