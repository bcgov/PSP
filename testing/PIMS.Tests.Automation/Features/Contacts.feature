@Regression-Contacts
Feature: Contacts

A short summary of the feature

Scenario Outline: 01. Organization Contacts
	Given I create a new Organization Contact from row number <RowNumber>
	When I update an existing Organization Contact from row number <EditNumber>
	And I search for an existing contact from type "<ContactType>" row number <RowNumber>
	Then  Expected Content is displayed on Contacts Table from contact type "<ContactType>"
	Examples: 
	| ContactType  | RowNumber | EditNumber |
	| Organization | 1         | 2          |
	| Organization | 3         | 4          |

Scenario Outline: 02. Individual Contacts
	Given I create a new Individual Contact from row number <RowNumber>
	When I update an existing Individual Contact from row number <EditNumber>
	And I search for an existing contact from type "<ContactType>" row number <RowNumber>
	Then Expected Content is displayed on Contacts Table from contact type "<ContactType>"
	Examples:
	| ContactType | RowNumber | EditNumber |
	| Individual  | 1         | 2          |
	| Individual  | 3         | 4          |
	| Individual  | 5         | 6          |

Scenario Outline: 03. Search for a non-existing Contact
	Given I search for an non-existing contact from type "<ContactType>" row number <RowNumber>
	Then No contacts results are found
	Examples:
	| ContactType	| RowNumber |
	| Individual	| 8         |
	| Organization	| 6         |

Scenario: 04. Contacts List View
	Given I verify the Contacts List View from row number 1
	Then Expected Content is displayed on Contacts Table from contact type "Organization"



