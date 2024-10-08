@Regression-Leases
Feature: Leases and Licenses

This feature tests all test cases related to Leases and Licenses.

Scenario: 01. Lease and License File Details
	Given I create a new minimum Lease from row number 1
	When  I add additional Information to the Lease Details
	And I update a Lease's Details from row number 2
	Then A new lease is created successfully

Scenario: 02. Leases Properties and Surplus Declaration
	Given I create a new minimum Lease from row number 3
	When I add Properties to the Lease Details
	And I update a Lease's Properties from row number 4
	And I verify the Surplus section
	Then A new lease is created successfully

Scenario: 03. Leases Approval/Consultations
	Given I create a new minimum Lease from row number 1
	When I insert new consultations to the Lease
	And I update a Lease's consultation from row number 2
	Then A new lease is created successfully

Scenario: 04. Lease Checklist Tab
	Given I create a new minimum Lease from row number 1
	When I insert Checklist information to a Lease
	Then A new lease is created successfully

Scenario: 05-1. Lease and License Payees Tab
	Given I create a new minimum Lease from row number 1
	When  I add Tenants to the Lease
	And I update a Lease's Tenants from row number 2
	Then A new lease is created successfully

Scenario: 05-2. Lease and License Tenants Tab
	Given I create a new minimum Lease from row number 6
	When  I add Tenants to the Lease
	And I update a Lease's Tenants from row number 7
	Then A new lease is created successfully

Scenario: 06. Lease and License Improvements Tab
	Given I create a new minimum Lease from row number 1
	When  I add Improvements to the Lease
	And I update a Lease's Improvements from row number 2
	Then A new lease is created successfully

Scenario: 07. Lease and License Insurance Tab
	Given I create a new minimum Lease from row number 1
	When  I add Insurance to the Lease
	And I update a Lease's Insurance from row number 2
	Then A new lease is created successfully

Scenario: 08. Lease and License Deposits Tab
	Given I create a new minimum Lease from row number 1
	When  I add Deposits to the Lease
	And I update a Lease's Deposits from row number 2
	Then A new lease is created successfully

Scenario: 09. Lease and License Periods and Payments Tab
	Given I create a new minimum Lease from row number 1
	When  I add Periods and Payments to the Lease
	And I update a Lease's Payments from row number 2
	Then A new lease is created successfully

Scenario: 10. Lease and License Documents Tab
	Given I create a new minimum Lease from row number 1
	When  I create Digital Documents for a "Lease" row number 1
	And I edit a Digital Document for a "Lease" from row number 5
	Then A new lease is created successfully

Scenario: 11. Lease and License Notes Tab
	Given I create a new minimum Lease from row number 1
	When I create a new Note on the Notes Tab from row number 5
	And I edit a Note on the Notes Tab from row number 2
	Then A new lease is created successfully

Scenario: 12. Lease Compensation Tab
	Given I create a new minimum Lease from row number 8
	When  I add additional Information to the Lease Details
	And I add Properties to the Lease Details
	And I add Tenants to the Lease
	And I create Compensation Requisition within an Lease/Licence
	And I update Compensation Requisition within an Lease from row number 9
	Then A new lease is created successfully

Scenario: 13. Lease and License from Inventory Property Pin
	Given I create a new Lease through a Property Pin from row number 5
	Then A new lease is created successfully

Scenario: 14. Lease and License List View
	Given I search for an existing Lease or License from row number 2
	Then Expected Lease File Content is displayed on Leases Table
