Feature: ResearchFiles

A short summary of the feature

@Reseach-Files
Scenario: Create a new complete Research File
	Given I create a new complete Research File from row number <RowNumber>
	#When I manage Documents Tab within a Research File
	#And I create a new Note on the Notes Tab
	Then A new Research File is created successfully
	Examples:
	| RowNumber |
	| 1         |

Scenario: Update a Research File
	Given I update an Existing Research File from row number 2
	When I update a Property details from row number 2
	Then Property Information is displayed correctly

Scenario: Create a new research file from Property pop-up
	Given I create a Research File from a pin on map and from row number 3
	Then A new Research File is created successfully

Scenario: Cancel changes on Research File and Search Filters
	Given I cancel changes done on Research Files from row number 5
	Then Research File Properties remain unchanged
