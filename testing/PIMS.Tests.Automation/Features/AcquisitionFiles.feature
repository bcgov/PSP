﻿@Acquisition-Files
Feature: AcquisitionFiles

Test Acquisition file features.

Scenario: 01. Acquisition File Details
	Given I create a new Acquisition File from row number 1
	When I add additional information to the Acquisition File Details
	And I update the File details from an existing Acquisition File from row number 2
	Then A new Acquisition file is created successfully

Scenario: 02. Acquisition File Properties
	Given I create a new Acquisition File from row number 1
	When I add Properties to the Acquisition File
	And I update an Acquisition File's Properties from row number 2
	And I update a Property details from row number 4
	Then A new Acquisition file is created successfully

#Scenario: 03. Acquisition File Property Takes

Scenario: 04. Acquisition Checklist Tab
	Given I create a new Acquisition File from row number 1
	When I insert Checklist information to an Acquisition File
	Then Acquisition File's Checklist has been saved successfully

Scenario: 05. Acquisition Files Digital Documents
	Given I create a new Acquisition File from row number 1
	When I create Digital Documents for a "Acquisition File" row number 4
	And  I edit a Digital Document for a "Acquisition File" from row number 7
	Then A new Acquisition file is created successfully

Scenario: 06. Acquisition File Notes
	Given I create a new Acquisition File from row number 1
	When  I create a new Note on the Notes Tab from row number 3
	And  I edit a Note on the Notes Tab from row number 7
	Then A new Acquisition file is created successfully

Scenario: 07. Acquisition File Agreements Tab
	Given I create a new Acquisition File from row number 1
	When I create Agreements within an Acquisition File
	And I update an Agreement within an Acquisition File from row number 2
	Then A new Acquisition file is created successfully

Scenario: 08. Acquisition File Stakeholders Tab
	Given I create a new Acquisition File from row number 1
	When I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I update Stakeholders within an Acquisition File
	Then A new Acquisition file is created successfully

Scenario: 09. Acquisition File Compensation Tab
	Given I create a new Acquisition File from row number 1
	When I add additional information to the Acquisition File Details
	And I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I create Compensation Requisition within an Acquisition File
	And I update Compensation Requisition within an Acquisition File from row number 2
	Then A new Acquisition file is created successfully

#Scenario: 10. Acquisition File Expropriation Tab

Scenario: 11. Acquisition File from Pin
	Given I create an Acquisition File from a pin on map from row number 5
	Then A new Acquisition file is created successfully

Scenario: 12. Acquisition Files List View
	Given I search for an existing Acquisition File from row number 1
	Then Expected Acquisition File Content is displayed on Acquisition File Table


