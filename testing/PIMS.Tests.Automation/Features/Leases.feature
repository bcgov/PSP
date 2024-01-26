@Regression-Leases
Feature: Leases and Licenses

This feature tests all test cases related to Leases and Licenses.

Scenario: 01. Lease and License File Details and Surplus Declaration
	Given I create a new minimum Lease from row number 1
	When  I add additional Information to the Lease Details
	And I update a Lease's Details from row number 2
	And I verify the Surplus section
	Then A new lease is created successfully

Scenario: 02. Lease and License Tenants
	Given I create a new minimum Lease from row number 1
	When  I add Tenants to the Lease
	And I update a Lease's Tenants from row number 2
	Then A new lease is created successfully

Scenario: 03. Lease and License Improvements
	Given I create a new minimum Lease from row number 1
	When  I add Improvements to the Lease
	And I update a Lease's Improvements from row number 2
	Then A new lease is created successfully

Scenario: 04. Lease and License Insurance
	Given I create a new minimum Lease from row number 1
	When  I add Insurance to the Lease
	And I update a Lease's Insurance from row number 2
	Then A new lease is created successfully

Scenario: 05. Lease and License Deposits
	Given I create a new minimum Lease from row number 1
	When  I add Deposits to the Lease
	And I update a Lease's Deposits from row number 2
	Then A new lease is created successfully

Scenario: 06. Lease and License Payments
	Given I create a new minimum Lease from row number 1
	When  I add Payments to the Lease
	And I update a Lease's Payments from row number 2
	Then A new lease is created successfully

Scenario: 07. Lease and License Documents
	Given I create a new minimum Lease from row number 1
	When  I create Digital Documents for a "Lease" row number 1
	And I edit a Digital Document for a "Lease" from row number 5
	Then A new lease is created successfully

Scenario: 08. Lease and License Notes
	Given I create a new minimum Lease from row number 1
	When I create a new Note on the Notes Tab from row number 5
	And I edit a Note on the Notes Tab from row number 2
	Then A new lease is created successfully

Scenario: 09. Lease and License from Inventory Property Pin
	Given I create a new Lease through a Property Pin from row number 4
	Then A new lease is created successfully

Scenario: 10. Lease and License List View
	Given I search for an existing Lease or License from row number 2
	Then Expected Lease File Content is displayed on Leases Table
