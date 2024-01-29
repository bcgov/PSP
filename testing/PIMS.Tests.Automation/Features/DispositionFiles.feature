@Disposition-Files
Feature: DispositionFiles

Test cases for Disposition Files feature

Scenario: 01. Disposition File Details
	Given I create a new Disposition File from row number 1
	#When I add additional information to the Disposition File Details
	#Then A new Disposition file is created successfully

	Scenario: 02. Disposition Checklist Tab
	Given I create a new Disposition File from row number 1
	When I insert Checklist information to an Disposition File
	Then Disposition File's Checklist has been saved successfully

	Scenario: 03. Disposition OffersAndSale Tab
	Given I create a new Disposition File from row number 5
	When I create Appraisal and Assessment section within Disposition File
	And I update Appraisal and Assessment and Offers section within Disposition File from row number 6
	#Then A new Disposition file is created successfully
