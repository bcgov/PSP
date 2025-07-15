@Management-Files
Feature: ManagementFiles

Test Management file features.

Scenario: 01._Management_File_Details
	Given I create a new Management File from row number 1
	When I add additional information to the Management File Details
	And I update the File details from an existing Management File from row number 2
	Then A new Management file is created or updated successfully

Scenario: 02._Management_File_Properties
	Given I create a new Management File from row number 3
	When I add Properties to the Management File
	And I update a Management File's Properties from row number 4
	Then A new Management file is created or updated successfully

Scenario: 03._Management_Files_Digital_Documents
	Given I create a new Management File from row number 6
	When I create Digital Documents for a "Management File" row number 14
	And  I edit a Digital Document for a "Management File" from row number 15
	Then A new Management file is created or updated successfully

Scenario: 04._Management_File_Notes
	Given I create a new Management File from row number 7
	When  I create a new Note on the Notes Tab from row number 11
	And  I edit a Note on the Notes Tab from row number 12
	Then A new Management file is created or updated successfully

Scenario: 05._Management_Files_List_View
	Given I search for an existing Management File from row number 2
	Then Expected Management File Content is displayed on Management File Table

Scenario: 06._Management_Activities_Tab
	Given I create a new Management File from row number 8
	When I add Properties to the Management File
	And I insert activities to the Management Activities Tab
	And I update information in the Property Management Tab from row number 9
	Then A new Management file is created or updated successfully

Scenario: 07._Management_Activity_Digital_Documents
	Given I create a new Management File from row number 10
	When I insert activities to the Management Activities Tab
	And I create Digital Documents for a Management Activity from row number 14
	Then A new Management file is created or updated successfully

Scenario: 08._Management_Property_Documents
	Given I create a new Management File from row number 10
	When I add Properties to the Management File
	And I create Digital Documents for a Management Activity from row number 14
	Then A new Management file is created or updated successfully
