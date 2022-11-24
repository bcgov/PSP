Feature: ResearchFiles

A short summary of the feature

@Reseach-Files
Scenario: Create a new research file with pins
	Given I create a new Research File
	When I add additional information to an existing Research File
	And I add several Properties to the research File
	Then A new research file is created successfully

Scenario: Update a research file with unexistent PID
	Given I create a new Research File
	When I look for an incorrect PID
	Then No results are found

Scenario: Update a research file with too many properties results found
	Given I create a new Research File
	When I look for a Property by Legal Description
	Then More than 15 results are found

Scenario: Cancel creation of new research file
	Given I start creating and cancel a new Research File
	Then The create research file form is no longer displayed

Scenario: Cancel changes on Research File Properties
	Given I update and cancel changes on existing research file properties
	Then Research File Properties remain unchanged

Scenario: Cancel the update of a research file
	Given I update and cancel existing research file form
	Then Research File View Form renders successfully

Scenario: Verify List View and Results Content
	Given I search for an existing Research File
	Then Expected Content is displayed on Research File Table

Scenario: Changes on Properties from Research File
	Given I update an existing research file properties
	
Scenario: Create an activity
	Given I create a new Research File
	When I create a new activity
	
