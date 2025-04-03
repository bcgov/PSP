@Regression-Projects
Feature: Projects

Scenario Outline: Type of Projects
	Given I create a new Project from row number <RowNumber>
	Then A new Project is created successfully
	Examples:
	| RowNumber |
	| 1         | 
	| 2         |
	| 3         |

Scenario: 01. Project Details
	Given I create a new Project from row number 4
	When I update an existing project from row number 5
	Then A new Project is created successfully

Scenario: 02. Project Documents
	Given I create a new Project from row number 6
	When I create Digital Documents for a "Project" row number 8
	And  I edit a Digital Document for a "Project" from row number 9
	Then A new Project is created successfully

Scenario: 03. Project Notes
	Given I create a new Project from row number 7
	When  I create a new Note on the Notes Tab from row number 6
	And  I edit a Note on the Notes Tab from row number 7
	Then A new Project is created successfully
	
Scenario: 04. Project List View
	Given I search for existing Projects from row number 2
	Then Expected Project Content is displayed on Projects Table

Scenario: 05. Duplicate Project
	Given I create a duplicate Project from row number 2
	Then Duplicate Project Alert is displayed
