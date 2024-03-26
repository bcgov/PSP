Feature: SubdivisionConsolidation

A short summary of the feature

@SubdivisionConsolidation
Scenario Outline: 01. Create Subdivisions
	When I create a Subdivision from row number <RowNumber>
	Then Subdivision is created successfully

Examples: 
	 | RowNumber |
	 | 1         |
	 | 4         |
