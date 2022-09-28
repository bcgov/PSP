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

	#Scenario: Update a research file by adding an activity
