@Regression-Projects
Feature: Projects

Scenario Outline: Type_of_Projects
	Given I create a new Project from row number <RowNumber>
	Then A new Project is created successfully
	Examples:
	| RowNumber |
	| 1         |
	| 2         |
	| 3         |
	| 8         |

Scenario: 01._Project_Details
	Given I create a new Project from row number 4
	When I update an existing project from row number 5
	Then A new Project is created successfully

Scenario: 02._Project_Documents
	Given I create a new Project from row number 6
	When I create Digital Documents for a "Project" row number 8
	And  I edit a Digital Document for a "Project" from row number 9
	Then A new Project is created successfully

Scenario: 03._Project_Notes
	Given I create a new Project from row number 7
	When  I create a new Note on the Notes Tab from row number 6
	And  I edit a Note on the Notes Tab from row number 7
	Then A new Project is created successfully

Scenario: 04._Project_List_View
	Given I search for existing Projects from row number 2
	Then Expected Project Content is displayed on Projects Table

Scenario: 05._Duplicate_Project
	Given I create a duplicate Project from row number 2
	Then Duplicate Project Alert is displayed