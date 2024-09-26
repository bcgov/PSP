@Reseach-Files
Feature: ResearchFiles

Research files regression tests

Scenario: 01. Research File Details
	Given I create a basic Research File from row number 1
	When I add additional details to Research File
	And I update a Research File Details from row number 4
	Then A new Research File is created successfully

Scenario: 02. Research File Properties
	Given I create a basic Research File from row number 2
	When I add Properties to a Research File
	And I update a Research File Properties from row number 5
	Then A new Research File is created successfully

Scenario: 03. Research File Documents
	Given I create a basic Research File from row number 3
	When I create Digital Documents for a "Research File" row number 3
	And I edit a Digital Document for a "Research File" from row number 6
	Then A new Research File is created successfully

Scenario: 04. Research File Notes
	Given I create a basic Research File from row number 1
	When I create a new Note on the Notes Tab from row number 1
	And I edit a Note on the Notes Tab from row number 2
	Then A new Research File is created successfully

Scenario: 05. Research File from Pin
	Given I create a Research File from a pin on map and from row number 6
	Then A new Research File is created successfully

Scenario: 06. Research File List View
	Given I search for Research Files from row number 4
	Then Research File Properties remain unchanged
