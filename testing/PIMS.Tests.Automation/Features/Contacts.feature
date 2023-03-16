Feature: Contacts

A short summary of the feature

@Regression-Contacts
Scenario: Verify Contacts List View
	Given I verify the Contacts List View
	Then Expected Content is displayed on Contacts Table Individual Lee

Scenario: Create new Organization Complete Contact
	Given I create a new Organization Contact with maximum fields Automation Test Corp II
	Then A new Organization contact is successfully created

Scenario: Create new Individual Complete Contact
	Given I create a new Individual Contact with maximum fields Lee
	Then A new Individual contact is successfully created

Scenario: Cancel creating a new contact
	Given I cancel creating a new contact
	Then Search Contacts screen is correctly rendered

Scenario: Update an Existing Organization Contact
	Given I update an existing Organization Contact
	Then A new Organization contact is successfully created

Scenario: Update an Existing Individual Contact
	Given I update an existing Individual Contact
	Then A new Individual contact is successfully created

Scenario: Search for an Existing Contact
	Given I search for an existing contact Lee
	Then Search Contacts screen is correctly rendered

Scenario: Search for a non-existing Contact
	Given I search for an non-existing contact
	Then No contacts results are found


