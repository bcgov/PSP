@Smoke-Test
Feature: SmokeTest

Test cases allocated for Smoke Testing

Scenario: Create Minimum Individual Contact
	Given I create a new Individual Contact from row number 2
	And I search for an existing contact from type "Individual" row number 2
	Then  Expected Content is displayed on Contacts Table from contact type "Individual"

Scenario: Create Minimum Organization Contact
	Given I create a new Organization Contact from row number 2
	And I search for an existing contact from type "Organization" row number 2
	Then Expected Content is displayed on Contacts Table from contact type "Organization"

Scenario: Create Minimum Lease and License
	Given I create a new Lease from row number 2
	And  I create Digital Documents for a "Lease" row number 3
	Then A new lease is created successfully

Scenario: Create Minimum Research File
	Given I create a new complete Research File from row number 4
	And I create a new Note on the Notes Tab from row number 5
	Then A new Research File is created successfully


