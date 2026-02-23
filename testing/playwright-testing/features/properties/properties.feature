@properties
Feature: Properties

Scenario: Multi-property Pop-up
    Given I search for a property by Plan number from row number 1
    When I verify the Strata popup on map
    Then The Property View on Map is rendered successfully

# Scenario: Property Plan View on Map
#     Given I search for a property by Plan number from row number 1
#     When I verify the Property's Plan number details
#     Then The Property View on Map is rendered successfully

# Scenario: Property Coordinates View on Map
#     Given I search for a property by coordinates from row number 1
#     When I verify the Property's coordinates details
#     Then The Property View on Map is rendered successfully

