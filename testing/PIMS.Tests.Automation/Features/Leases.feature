Feature: Leases and Licenses

This feature tests all test cases related to Leases and Licenses.

#Scenario: Login
#	Given I log in with IDIR credentials sutairak
#	Then I am on path /login

Scenario: Create Maximum Lease and License
	Given I create a new Lease with all fields
	Then A new lease is created successfully

