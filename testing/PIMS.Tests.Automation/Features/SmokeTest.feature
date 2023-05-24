Feature: SmokeTest

Test cases allocated for Smoke Testing

@Smoke-Test
Scenario: Create Minimum Individual Contact
	Given I create a new Individual Contact from row number 2
	And I search for an existing contact from type "Individual" row number 2
	Then  Expected Content is displayed on Contacts Table from contact type "Individual"

Scenario: Create Minimum Organization Contact
	Given I create a new Organization Contact from row number 2
	And I search for an existing contact from type "Organization" row number 2
	Then Expected Content is displayed on Contacts Table from contact type "Organization"

Scenario: Create Minimum Lease and License
	Given I create a new Lease with minimum fields
	Then A new lease is created successfully

Scenario: Create Minimum Research File
	Given I create a new complete Research File from row number 4
	#When I manage Documents Tab within a Research File
	#And I create a new Note on the Notes Tab
	Then A new Research File is created successfully

