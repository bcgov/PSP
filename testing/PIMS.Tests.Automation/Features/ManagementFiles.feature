@Management-Files
Feature: ManagementFiles

Test Management file features.

Scenario: 01._Management_File_Details
	Given I create a new Management File from row number 1
	When I add additional information to the Management File Details
	And I update the File details from an existing Management File from row number 2
	Then A new Management file is created successfully

Scenario: 02._Management_File_Properties
	Given I create a new Management File from row number 3
	When I add Properties to the Management File
	And I update a Management File's Properties from row number 4
	Then A new Management file is created successfully

Scenario: 03._Management_Files_Digital_Documents
	Given I create a new Management File from row number 8
	When I create Digital Documents for a "Management File" row number 4
	And  I edit a Digital Document for a "Management File" from row number 7
	Then A new Management file is created successfully

Scenario: 04._Management_File_Notes
	Given I create a new Management File from row number 9
	When  I create a new Note on the Notes Tab from row number 3
	And  I edit a Note on the Notes Tab from row number 4
	Then A new Management file is created successfully
