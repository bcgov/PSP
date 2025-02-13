@Smoke-Test
Feature: SmokeTest

Test cases allocated for Smoke Testing

Scenario: 01._Individual_Contact
	Given I create a new Individual Contact from row number 7
	When I search for an existing contact from type "Individual" row number 7
	Then  Expected Content is displayed on Contacts Table from contact type "Individual"

Scenario: 02._Organization_Contact
	Given I create a new Organization Contact from row number 5
	When I search for an existing contact from type "Organization" row number 5
	Then Expected Content is displayed on Contacts Table from contact type "Organization"

Scenario: 03._Lease_and_License_Tenants
	Given I create a new minimum Lease from row number 5
	When  I add Tenants to the Lease
	Then A new lease is created successfully

Scenario: 04._Lease_and_License_Improvements
	Given I create a new minimum Lease from row number 5
	When  I add Improvements to the Lease
	Then A new lease is created successfully

Scenario: 05._Lease_and_License_Insurance
	Given I create a new minimum Lease from row number 5
	When  I add Insurance to the Lease
	Then A new lease is created successfully

Scenario: 06._Lease_and_License_Deposits
	Given I create a new minimum Lease from row number 5
	When  I add Deposits to the Lease
	Then A new lease is created successfully

Scenario: 07._Lease_and_License_Payments
	Given I create a new minimum Lease from row number 5
	When  I add Periods and Payments to the Lease
	Then A new lease is created successfully

Scenario: 08._Research_File_Properties
	Given I create a basic Research File from row number 10
	When I add Properties to a Research File
	Then A new Research File is created successfully

Scenario: 09._Research_File_Notes
	Given I create a basic Research File from row number 9
	When I create a new Note on the Notes Tab from row number 8
	Then A new Research File is created successfully

Scenario: 10._Acquisition_Checklist_Tab
	Given I create a new Acquisition File from row number 18
	When I insert Checklist information to an Acquisition File
	Then Acquisition File's Checklist has been saved successfully

Scenario: 11._Acquisition_File_Agreements_Tab
	Given I create a new Acquisition File from row number 18
	When I create Agreements within an Acquisition File
	Then A new Acquisition file is created successfully

Scenario: 12._Acquisition_File_Compensation_Tab
	Given I create a new Acquisition File from row number 18
	When I add additional information to the Acquisition File Details
	And I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I create Compensation Requisition within an Acquisition File
	Then A new Acquisition file is created successfully

Scenario: 13._Disposition_Checklist_Tab
	Given I create a new Disposition File from row number 1
	When I insert Checklist information to an Disposition File
	Then Disposition File's Checklist has been saved successfully

Scenario: 14._Disposition_Offers_and_Sale_Tab
	Given I create a new Disposition File from row number 1
	When I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	Then A new Disposition file is created successfully

