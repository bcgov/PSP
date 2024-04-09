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

Scenario: 02. Subdivision Error Messages - Children in Inventory
	When I create a Subdivision from row number 7
	Then Subdivision has a Parent that is not in the MOTI Inventory error

Scenario: 03. Subdivision Error Messages - Repeated Child
	When I create a Subdivision from row number 25
	Then Subdivision has the same Child twice error

Scenario: 04. Subdivision Error Messages - Missing Child
	When I create a Subdivision from row number 28
	Then Subdivision has only one Child error


Scenario Outline: 05. Create Consolidations
	When I create a Consolidation from row number <RowNumber>
	Then Consolidation is created successfully

Examples: 
	 | RowNumber |
	 | 10        |
	 | 13        |

Scenario: 06. Consolidation Error Message - Child in Inventory
	When I create a Consolidation from row number 16
	Then Consolidation has a Child that is in the MOTI Inventory error

Scenario: 07. Consolidation Error Message - Repeated Parent
	When I create a Consolidation from row number 19
	Then Consolidation has the same Parent twice error

Scenario: 08. Consolidation Error Message - Missing Parent
	When I create a Consolidation from row number 22
	Then Consolidation has only one Parent error
