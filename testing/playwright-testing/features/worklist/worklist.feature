@workLists
Feature: Worklist feature

Scenario: Property Worklist
	Given I navigate to the Worklists Page
    When I verify the Worklist view form for Property Worklist
    And I search for several properties from row number 1
    And I verify the count of the worklist items
    And I delete properties on the worklist
    Then The Worklist section is rendered successfully