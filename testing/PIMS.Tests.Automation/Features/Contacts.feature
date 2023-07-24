@Regression-Contacts
Feature: Contacts

A short summary of the feature

Scenario Outline: Create new Organization Complete Contact
	Given I create a new Organization Contact from row number <RowNumber>
	And I search for an existing contact from type "<ContactType>" row number <RowNumber>
	Then  Expected Content is displayed on Contacts Table from contact type "<ContactType>"
	Examples: 
	| ContactType	| RowNumber |
	| Organization  | 1         |
	| Organization  | 3         |

Scenario Outline: Create new Individual Complete Contact
	Given I create a new Individual Contact from row number <RowNumber>
	And I search for an existing contact from type "<ContactType>" row number <RowNumber>
	Then Expected Content is displayed on Contacts Table from contact type "<ContactType>"
	Examples:
	| ContactType	| RowNumber |
	| Individual	| 1         |
	| Individual	| 2         |
	| Individual	| 4         |

Scenario: Cancel creating a new contact
	Given I cancel creating a new contact from row number 2
	Then Search Contacts screen is correctly rendered

Scenario: Update an Existing Organization Contact
	Given I update an existing Organization Contact from row number 4
	Then An Organization contact is successfully updated

Scenario: Update an Existing Individual Contact
	Given I update an existing Individual Contact from row number 5
	Then An Individual contact is successfully updated

Scenario Outline: Search for a non-existing Contact
	Given I search for an non-existing contact from type "<ContactType>" row number <RowNumber>
	Then No contacts results are found
	Examples:
	| ContactType	| RowNumber |
	| Individual	| 6         |
	| Organization	| 5         |



