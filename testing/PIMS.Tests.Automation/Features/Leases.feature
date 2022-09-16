Feature: Leases and Licenses

This feature tests all test cases related to Leases and Licenses.

#Scenario: Login
#	Given I log in with IDIR credentials sutairak
#	Then I am on path /login

Scenario: Create Minimum Lease and License with One Tenant
	Given I create a new Lease with minimum requirements
