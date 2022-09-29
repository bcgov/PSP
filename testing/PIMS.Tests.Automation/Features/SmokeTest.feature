Feature: SmokeTest

Test cases allocated for Smoke Testing

@Smoke-Test
Scenario: Create a new Individual Contact with minimum fields
	Given I create a new Individual Contact with minimum fields
	Then A new Individual contact is successfully created

Scenario: Create a new Organization Contact with minimum fields
	Given I create a new Organization Contact with minimum fields
	Then A new Organization contact is successfully created

Scenario: Create Minimum Lease and License with One Tenant
	Given I create a new Lease with minimum fields
	Then A new lease is created successfully

