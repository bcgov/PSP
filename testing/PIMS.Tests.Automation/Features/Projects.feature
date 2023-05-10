@Regression-Projects
Feature: Projects

Scenario Outline: Create new Projects
	Given I create a new Project from row number <RowNumber>
	When I verify The Project View Form
	And I search for an existing project
	Then Expected Content is displayed on Projects Table
	Examples:
	| RowNumber |
	| 1         |
	| 2			|
	| 3         |
	| 4         |

Scenario: Update Project
	Given I update an existing project from row number 5
	Then The Project is updated successfully

Scenario: Duplicate Project
	Given I create a new Project from row number 1
	Then Duplicate Project Alert is displayed
