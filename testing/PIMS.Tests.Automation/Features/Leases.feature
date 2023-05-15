@Regression-Leases
Feature: Leases and Licenses

This feature tests all test cases related to Leases and Licenses.

Scenario: 01. Create Maximum Lease and License
	Given I create a new Lease with all fields and Properties
	Then A new lease is created successfully

Scenario: 02. Edit Existing Lease and License
	Given I update an existing lease
	Then An existing lease is updated successfully

Scenario: 03. Create a Lease from an Inventory Property
	Given I create a new Lease through a Property Pin
	Then A new lease is created successfully

Scenario: 04. Create a Lease from a Property of Interest
	Given I create a new Lease through a Property of Interest
	Then A new lease is created successfully

Scenario: 05. Create a Lease from a Payable Marker
	Given I create a new Lease through a Payable Marker
	Then A new lease is created successfully

Scenario: 06. Verify Leases List View, Content and Results Content
	Given I search for an existing Lease or License 
	Then Expected Lease File Content is displayed on Leases Table
