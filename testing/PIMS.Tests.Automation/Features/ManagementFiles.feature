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
	Given I create a new Management File from row number 6
	When I create Digital Documents for a "Management File" row number 14
	And  I edit a Digital Document for a "Management File" from row number 15
	Then A new Management file is created successfully

Scenario: 04._Management_File_Notes
	Given I create a new Management File from row number 7
	When  I create a new Note on the Notes Tab from row number 11
	And  I edit a Note on the Notes Tab from row number 12
	Then A new Management file is created successfully

#Scenario: 05._Acquisition_File_from_Pin
#	Given I create an Acquisition File from a pin on map from row number 5
#	Then A new Acquisition file is created successfully

Scenario: 06._Management_Files_List_View
	Given I search for an existing Management File from row number 2
	Then Expected Management File Content is displayed on Management File Table
