@Regression-Projects
Feature: Projects

Scenario Outline: Create new Projects
	Given I create a new Project from row number <RowNumber>
	When I verify The Project View Form
	And I create Digital Documents for a "Project" row number <DocRowNumber>
	And I search for an existing project
	Then Expected Content is displayed on Projects Table
	Examples:
	| RowNumber | DocRowNumber |
	| 1         | 8            |
	| 2         | 9            |
	| 3         | 10           |
	| 4         | 11           |

Scenario: Update Project
	Given I update an existing project from row number 5
	When I edit a Digital Document for a "Project" from row number 12
	And I navigate back to Project Details
	Then The Project is updated successfully

Scenario: Duplicate Project
	Given I create a new Project from row number 1
	Then Duplicate Project Alert is displayed
