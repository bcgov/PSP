@managementFiles
Feature: Management File

 Scenario: Management File Details
     Given I create a new Management File with row number 0
     And I add additional information to the Management File Details
     And I update the File details from an existing Management File from row number 1
     Then A new Management file is created or updated successfully

Scenario: Management File Properties
	Given I create a new Management File with row number 2
	When I add Properties to the Management File
	And I update a Management File's Properties from row number 3
	Then A new Management file is created or updated successfully

Scenario: Management Files Digital Documents
	Given I create a new Management File with row number 5
	When I create Digital Documents for a "Management File" row number 14
	And  I edit a Digital Document for a "Management File" from row number 15
	Then A new Management file is created or updated successfully

Scenario: Management File Notes
	Given I create a new Management File with row number 6
	When  I create a new Note on the Notes Tab with row number 11
	And  I edit a Note on the Notes Tab with row number 11
	Then A new Management file is created or updated successfully

Scenario: Management Files List View
	Given I search for an existing Management File from row number 2
	Then Expected Management File Content is displayed on Management File Table

Scenario: Management Activities Tab
	Given I create a new Management File from row number 7
	When I add Properties to the Management File
	And I insert activities to the Management Activities Tab
	And I update information in the Property Management Tab from row number 8
	Then A new Management file is created or updated successfully

Scenario: Management Activity Digital Documents
	Given I create a new Management File with row number 9
	When I insert activities to the Management Activities Tab
	And I create Digital Documents for a Management Activity from row number 14
	Then A new Management file is created or updated successfully

Scenario: Management Property Documents
	Given I create a new Management File with row number 9
	When I add Properties to the Management File
	And I create Digital Documents for a Management Activity from row number 14
	Then A new Management file is created or updated successfully