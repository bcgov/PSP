@workLists
Feature: Worklist feature

Scenario: Property Worklist
    Given I search for several properties from row number 0 and add to the worklist
    And I verify the count of the worklist items from row number 0
    And I navigate to the Worklists Page
    And I verify the Worklist view form for Property Worklist
    And I delete properties on the worklist
    Then The Worklist section is rendered successfully

Scenario: Create file from worklist
    Given I search for several properties from row number 0 and add to the worklist
    And I navigate to the Worklists Page
    And I create a file from the worklist
    Then The file is created successfully from the worklist

Scenario: Property Strata Worklist
    Given I search for a Strata plan and several properties from row number 1 and add to the worklist
    And I navigate to the Worklists Page
    And I verify the Worklist view form for Property Strata Worklist
    Then The Worklist section is rendered successfully