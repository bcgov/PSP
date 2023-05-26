@Regression-Leases
Feature: Leases and Licenses

This feature tests all test cases related to Leases and Licenses.

Scenario: 01. Create a Complete Lease and License
	Given I create a new Lease from row number 1
	#And  I create Digital Documents for a "Lease" row number 3
	And I create a new Note on the Notes Tab from row number 5
	Then A new lease is created successfully

Scenario: 02. Edit Existing Lease and License
	Given I update an existing lease from row number 2
	And I edit a Note on the Notes Tab from row number 2
	Then An existing lease is updated successfully

Scenario: 03. Create a Lease from an Inventory Property
	Given I create a new Lease through a Property Pin from row number 4
	Then A new lease is created successfully

Scenario: 04. Create a Lease from a Property of Interest
	Given I create a new Lease through a Property of Interest from row number 5
	Then A new lease is created successfully

Scenario: 05. Create a Lease from a Payable Marker
	Given I create a new Lease through a Payable Marker from row number 6
	Then A new lease is created successfully

Scenario: 06. Verify Leases List View, Content and Results Content
	Given I search for an existing Lease or License from row number 1
	Then Expected Lease File Content is displayed on Leases Table
