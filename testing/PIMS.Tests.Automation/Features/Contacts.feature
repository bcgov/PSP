Feature: Contacts

A short summary of the feature

@Regression-Contacts
Scenario: 01. Create new Individual Complete Contact
	Given I create a new Individual Contact with maximum fields Lee
	Then A new Individual contact is successfully created

Scenario: 02. Create new Organization Complete Contact
	Given I create a new Organization Contact with maximum fields Automation Test Corp II
	Then A new Organization contact is successfully created

Scenario: 03. Cancel creating a new contact
	Given I cancel creating a new contact
	Then Search Contacts screen is correctly rendered

Scenario: 04. Update an Existing Organization Contact
	Given I update an existing Organization Contact
	Then A new Organization contact is successfully created

Scenario: 05. Update an Existing Individual Contact
	Given I update an existing Individual Contact
	Then A new Individual contact is successfully created

Scenario: 06. Search for an Existing Contact
	Given I search for an existing contact Lee
	Then Search Contacts screen is correctly rendered

Scenario: 07. Search for a non-existing Contact
	Given I search for an non-existing contact
	Then No contacts results are found

Scenario: 08. Verify Contacts List View
	Given I verify the Contacts List View
	Then Expected Content is displayed on Contacts Table Individual Lee


