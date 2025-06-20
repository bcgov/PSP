@Disposition-Files
Feature: DispositionFiles

Test cases for Disposition Files feature

Scenario: 01._Disposition_File_Details
	Given I create a new Disposition File from row number 2
	When I add additional information to the Disposition File Details
	And I update the File details from an existing Disposition File from row number 3
	Then A new Disposition file is created successfully

Scenario: 02._Disposition_File_Properties
	Given I create a new Disposition File from row number 5
	When I add Properties to the Disposition File
	And I update a Disposition File's Properties from row number 6
	And I update a Property details from a file from row number 5
	Then A new Disposition file is created successfully

Scenario: 03._Disposition_Checklist_Tab
	Given I create a new Disposition File from row number 7
	When I insert Checklist information to an Disposition File
	Then Disposition File's Checklist has been saved successfully

Scenario: 04._Disposition_Offers_and_Sale_Tab
	Given I create a new Disposition File from row number 8
	When I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	And I update Appraisal, Assessment and Offers section within Disposition File from row number 9
	Then A new Disposition file is created successfully

Scenario: 05._Disposition_Files_Digital_Documents
	Given I create a new Disposition File from row number 10
	When I create Digital Documents for a "Disposition File" row number 12
	And  I edit a Digital Document for a "Disposition File" from row number 13
	Then A new Disposition file is created successfully

Scenario: 06._Disposition_File_Notes
	Given I create a new Disposition File from row number 11
	When  I create a new Note on the Notes Tab from row number 9
	And  I edit a Note on the Notes Tab from row number 10
	Then A new Disposition file is created successfully

Scenario: 07._Disposition_File_from_PIN
	Given I create a Disposition File from a pin on map from row number 12
	Then A new Disposition file is created successfully

Scenario: 08._Disposition_Files_List_View
	Given I search for an existing Disposition File from row number 3
	Then Expected Disposition File Content is displayed on Disposition File List View

Scenario: 09._Disposition_File_Main_Path
	Given I create a new Disposition File from row number 13
	When I add Properties to the Disposition File
	And I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	And I add additional information to the Disposition File Details
	Then Disposition File Main Path completed successfully

Scenario: 10._Disposition_File_Sales_Price_Error
	Given I create a new Disposition File from row number 14
	When I change status of the Disposition File
	Then Disposition File without Sales Price error appears

Scenario: 11._Disposition_File_Not_Sold_Status_Error
	Given I create a new Disposition File from row number 14
	When I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	And I change status of the Disposition File
	Then Disposition File without SOLD Status error appears

Scenario: 12._Disposition_File_Non-Core_Inventory_Error
	Given I create a new Disposition File from row number 15
	When I add Properties to the Disposition File
	And I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	And I change status of the Disposition File
	Then Disposition File with non-Core property error appears
