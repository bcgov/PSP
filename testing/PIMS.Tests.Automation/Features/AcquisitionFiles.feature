@Acquisition-Files
Feature: AcquisitionFiles

Test Acquisition file features.

Scenario: 01._Acquisition_File_Details
	Given I create a new Acquisition File from row number 1
	When I add additional information to the Acquisition File Details
	And I update the File details from an existing Acquisition File from row number 2
	Then A new Acquisition file is created successfully

Scenario: 02._Acquisition_File_Details
	Given I create a new Acquisition File from row number 3
	When I add Properties to the Acquisition File
	And I update an Acquisition File's Properties from row number 4
	And I update a Property details from a file from row number 4
	Then A new Acquisition file is created successfully

Scenario: 03._Acquisition_File_Property_Takes
	Given I create a new Acquisition File from row number 5
	When I add Properties to the Acquisition File
	And I create Takes within Acquisition File's Properties
	And I update Takes within Acquisition File's Properties from row number 6
	Then A new Acquisition file is created successfully

Scenario: 04._Acquisition_Checklist_Tab
	Given I create a new Acquisition File from row number 7
	When I insert Checklist information to an Acquisition File
	Then Acquisition File's Checklist has been saved successfully

Scenario: 05._Acquisition_Files_Digital_Documents
	Given I create a new Acquisition File from row number 8
	When I create Digital Documents for a "Acquisition File" row number 4
	And  I edit a Digital Document for a "Acquisition File" from row number 7
	Then A new Acquisition file is created successfully

Scenario: 06._Acquisition_File_Notes
	Given I create a new Acquisition File from row number 9
	When  I create a new Note on the Notes Tab from row number 3
	And  I edit a Note on the Notes Tab from row number 4
	Then A new Acquisition file is created successfully

Scenario: 07._Acquisition_File_Agreements_Tab
	Given I create a new Acquisition File from row number 10
	When I create Agreements within an Acquisition File
	And I update an Agreement within an Acquisition File from row number 11
	Then A new Acquisition file is created successfully

Scenario: 08._Acquisition_File_Stakeholders_Tab
	Given I create a new Acquisition File from row number 12
	When I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	Then A new Acquisition file is created successfully

Scenario: 09._Acquisition_File_Compensation_Tab
	Given I create a new Acquisition File from row number 13
	When I add additional information to the Acquisition File Details
	And I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I create Compensation Requisition within an Acquisition File
	And I update Compensation Requisition within an Acquisition File from row number 14
	Then A new Acquisition file is created successfully

Scenario: 10._Acquisition_File_Expropriation_Tab
	Given I create a new Acquisition File from row number 15
	When I add additional information to the Acquisition File Details
	And I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I create Expropriations within an Acquisition File
	And I update Expropriation within an Acquisition File from row number 16
	Then A new Acquisition file is created successfully

Scenario: 11._Acquisition_File_from_Pin
	Given I create an Acquisition File from a pin on map from row number 17
	Then A new Acquisition file is created successfully

Scenario: 12._Acquisition_Files_List_View
	Given I search for an existing Acquisition File from row number 2
	Then Expected Acquisition File Content is displayed on Acquisition File Table

Scenario: 13._Acquisition_File_Subfiles-Subfile_Details
	Given I create a new Acquisition File from row number 22
	When I add additional information to the Acquisition File Details
	And I create a new Sub-file from row number 23
	Then A new Acquisition file is created successfully

Scenario: 14._Acquisition_File_Compensation_Requsition-Sum_of_Subfiles_Compensation_Requisitions
	Given I create a new Acquisition File from row number 22
	When I add additional information to the Acquisition File Details
	And I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I create Compensation Requisition within an Acquisition File
	And I create a new Sub-file from row number 23
	And I add Properties to the Acquisition File
	And I create Stakeholders within an Acquisition File
	And I create Compensation Requisition within an Acquisition Subfile
	Then Main Acquisition File totals are verified successfully from row number 22

Scenario: 15._Acquisition_File_Property_Takes_Logic
	Given I create a new Acquisition File from row number 19
	When I add Properties to the Acquisition File
	And I create Takes within Acquisition File's Properties
	Then A new Acquisition file is created successfully

Scenario: 16._Acquisition_File_Error_Message-Draft_Items
	Given I create a new Acquisition File from row number 20
	When I create Agreements within an Acquisition File
	And I add additional information to complete the Acquisition File
	Then Acquisition File cannot be completed due to Draft items

Scenario: 17._Acquisition_File_Error_Message-No_Takes
	Given I create a new Acquisition File from row number 20
	When I add additional information to complete the Acquisition File
	Then Acquisition File cannot be completed without Takes

Scenario: 18._Acquisition_File_Error_Message-Takes_In-Progress
	Given I create a new Acquisition File from row number 21
	When I add Properties to the Acquisition File
	And I create Takes within Acquisition File's Properties
	And I add additional information to complete the Acquisition File
	Then Acquisition File cannot be completed due to In-Progress Takes

Scenario: 19._Acquisition_File_Error_Message_H120_In-Progress
	Given I create a new Acquisition File from row number 20
	When I generate Compensation Requisition within an Acquisition File
	And I add additional information to complete the Acquisition File
	Then Acquisition File cannot be completed due to Draft items
