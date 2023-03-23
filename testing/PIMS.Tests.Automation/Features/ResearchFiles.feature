Feature: ResearchFiles

A short summary of the feature

@Reseach-Files
Scenario: 01. Create a new Research File with Properties, Documents, Notes
	Given I create a new Research File
	When I add additional information to an existing Research File
	And I add several Properties to the research File
	And I navigate to the Documents Tab, validate Documents Types and attach a Document
	Then A new research file is created successfully

Scenario: 02. Create a new research file from Property pop-up
	Given I create a Research File from a pin on map
	Then A new research file is created successfully

Scenario: 03. Update a research file with unexistent PID
	Given I create a new Research File
	When I look for an incorrect PID
	Then No results are found

Scenario: 04. Update a research file with too many properties results found
	Given I create a new Research File
	When I look for a Property by Legal Description
	Then More than 15 results are found

Scenario: 05. Update a Property Information from Research File
	Given I update Property Details from a Research File
	Then Property Information is displayed correctly

Scenario: 06. Changes on Properties from Research File
	Given I update an existing research file properties
	Then Property Research Tab has been updated successfully

Scenario: 07. Cancel creation of new research file
	Given I start creating and cancel a new Research File
	Then The create research file form is no longer displayed

Scenario: 08. Cancel changes on research File Properties
	Given I update and cancel changes on existing research file properties
	Then Research File Properties remain unchanged

Scenario: 09. Cancel the update of a research file
	Given I update and cancel existing research file form
	Then Research File View Form renders successfully

Scenario: 10. Cancel changes on a Property Information from a research file
	Given I cancel changes on a Property Details from a Research File
	Then  Research File Properties remain unchanged

Scenario: 11. Verify List View and Results Content
	Given I search for an existing Research File
	Then Expected Research Files Content is displayed on Research File Table
