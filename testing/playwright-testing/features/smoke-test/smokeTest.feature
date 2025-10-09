@smokeTest
Feature: Smoke Test

Scenario: Help Desk Form
	Given I navigate to the Help Desk Form
    When I verify the Help Desk Form fields
    Then The Help Desk Form is rendered successfully

Scenario: Search Control Section
    Given I verify the Search Controls
    When I search for a property
    Then The Search Control section is rendered successfully

Scenario: Map Layers Section
    Given I navigate to the Map Layers Page
    When I verify the Map Layers List View
    Then The Map Layers section is rendered successfully

Scenario: Worklist Section
    Given I navigate to the Worklists Page
    When I verify the Worklist view form
    And I insert a property in the Worklist
    Then The Worklist section is rendered successfully

Scenario: Projects Section
    Given I navigate to the Projects Page
    When I verify the Projects List View
    And I verify the Projects Create Form fields
    Then The Projects section is rendered successfully

Scenario: Research Files Section
    Given I navigate to the Research Files Page
    When I verify the Research Files List View
    And I verify the Research Files Create Form fields
    Then The Research Files section is rendered successfully

Scenario: Acquisition Files Section
    Given I navigate to the Acquisition Files Page
    When I verify the Acquisition Files List View
    And I verify the Acquisition Files Create Form fields
    Then The Acquisition Files section is rendered successfully

Scenario: Management Files Section
    Given I navigate to the Management Files Page
    When I verify the Management Files List View
    And I verify the Management Files Create Form fields
    Then The Management Files section is rendered successfully

Scenario: Lease and Licences Section
    Given I navigate to the Lease and Licences Page
    When I verify the Lease and Licences List View
    And I verify the Lease and Licences Create Form fields
    Then The Lease and Licences section is rendered successfully

Scenario: Disposition Files Section
    Given I navigate to the Disposition Files Page
    When I verify the Disposition Files List View
    And I verify the Disposition Files Create Form fields
    Then The Disposition Files section is rendered successfully

Scenario: Contact Manager Section
    Given I navigate to the Contact Manager Page
    When I verify the Contact Manager List View
    And I verify the Contact Manager Create Form fields
    Then The Contact Manager section is rendered successfully

Scenario: Admin Users Section
    Given I navigate to the Admin Users Page
    When I verify the Admin Users List View
    And I verify the Admin Users Create Form fields
    Then The Admin Users section is rendered successfully

Scenario: Admin CDOGS Templates Section
    Given I navigate to the Admin CDOGS Templates Page
    When I verify the Admin CDOGS Templates List View
    Then The Admin CDOGS Templates section is rendered successfully

Scenario: Admin Financial Codes Section
    Given I navigate to the Admin Financial Codes Page
    When I verify the Admin Financial Codes List View
    And I verify the Admin Financial Codes Create Form fields
    Then The Admin Financial Codes section is rendered successfully

Scenario: Admin Manage BCFTA Property Ownership Section
    Given I navigate to the Admin Agencies Page
    Then The Admin Agencies section is rendered successfully



