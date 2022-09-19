Feature: Contacts

A short summary of the feature

@Regression-Contacts
Scenario: Create new Organization Complete Contact
	Given I create a new Organization Contact with maximum fields
	Then A new Organization contact is successfully created

Scenario: Create new Individual Complete Contact
	Given I create a new Individual Contact with maximum fields
	Then A new Individual contact is successfully created

Scenario: Cancel creating a new contact
	Given I cancel creating a new contact
	Then Search Contacts screen is correctly rendered

Scenario: Update an Existing Organization Contact
	Given I update an existing Organization Contact
	Then A new Organization contact is successfully created

Scenario: Search for an Existing Contact
	Given I search for an existing contact
	Then Search Contacts screen is correctly rendered

Scenario: Search for a non-existing Contact
	Given I search for an non-existing contact
	Then No contacts results are found



