@Smoke-Test
Feature: SmokeTest

Test cases allocated for Smoke Testing

Scenario: 01. Individual Contact
	Given I create a new Individual Contact from row number 7
	When I search for an existing contact from type "Individual" row number 7
	Then  Expected Content is displayed on Contacts Table from contact type "Individual"

Scenario: 02. Organization Contact
	Given I create a new Organization Contact from row number 5
	When I search for an existing contact from type "Organization" row number 5
	Then Expected Content is displayed on Contacts Table from contact type "Organization"

Scenario: 03. Lease and License Tenants
	Given I create a new minimum Lease from row number 5
	When  I add Tenants to the Lease
	Then A new lease is created successfully

Scenario: 04. Lease and License Improvements
	Given I create a new minimum Lease from row number 5
	When  I add Improvements to the Lease
	Then A new lease is created successfully

Scenario: 05. Lease and License Insurance
	Given I create a new minimum Lease from row number 5
	When  I add Insurance to the Lease
	Then A new lease is created successfully

Scenario: 06. Lease and License Deposits
	Given I create a new minimum Lease from row number 5
	When  I add Deposits to the Lease
	Then A new lease is created successfully

#Scenario: 07. Lease and License Payments
#	Given I create a new minimum Lease from row number 5
#	When  I add Payments to the Lease
#	Then A new lease is created successfully

Scenario: 08. Research File Properties
	Given I create a basic Research File from row number 10
	When I add Properties to a Research File
	Then A new Research File is created successfully

Scenario: 09. Research File Notes
	Given I create a basic Research File from row number 9
	When I create a new Note on the Notes Tab from row number 8
	Then A new Research File is created successfully

Scenario: 10. Acquisition Checklist Tab
	Given I create a new Acquisition File from row number 18
	When I insert Checklist information to an Acquisition File
	Then Acquisition File's Checklist has been saved successfully

Scenario: 11. Acquisition File Agreements Tab
	Given I create a new Acquisition File from row number 18
	When I create Agreements within an Acquisition File
	Then A new Acquisition file is created successfully

#Scenario: 12. Acquisition File Compensation Tab
#	Given I create a new Acquisition File from row number 18
#	When I add additional information to the Acquisition File Details
#	And I add Properties to the Acquisition File
#	And I create Stakeholders within an Acquisition File
#	And I create Compensation Requisition within an Acquisition File
#	Then A new Acquisition file is created successfully

#Scenario: 13. Disposition Checklist Tab
#	Given I create a new Disposition File from row number 1
#	When I insert Checklist information to an Disposition File
#	Then Disposition File's Checklist has been saved successfully

Scenario: 12. Disposition Offers and Sale Tab
	Given I create a new Disposition File from row number 1
	When I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	Then A new Disposition file is created successfully

