@RegressionTesting
Feature: RegressionTesting

A short summary of the feature

Scenario: 01._Help_Desk_Section
	Given I review the Help Desk Section
	Then Help Desk rendered successfully

Scenario: 02._User_Management_List_View
	Given I enter to the User Management List View
	Then User Management rendered successfully

Scenario: 03._CDOGS_Templates
	Given I create a CDOGS template
	Then CDOGS rendered successfully

Scenario: 04._Financial_Codes_List_View
	Given I search for an existing Financial Code from row number 1
	Then Financial Codes rendered successfully

Scenario: 05._Acquisition_File_Details
	Given I create a new Acquisition File from row number 1
	When I add additional information to the Acquisition File Details
	And I update the File details from an existing Acquisition File from row number 2
	Then A new Acquisition file is created successfully

Scenario: 06._Acquisition_File_Property_Takes
	Given I create a new Acquisition File from row number 5
	When I add Properties to the Acquisition File
	And I create Takes within Acquisition File's Properties
	And I update Takes within Acquisition File's Properties from row number 6
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

Scenario: 11._Acquisition_File_from_Land_Parcel
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

Scenario: 14._Lease_and_License_File_Details
	Given I create a new minimum Lease from row number 1
	When  I add additional Information to the Lease Details
	And I update a Lease's Details from row number 2
	Then A new lease is created successfully

Scenario: 15._Leases_Properties
	Given I create a new minimum Lease from row number 3
	When I add Properties to the Lease Details
	And I update a Lease's Properties from row number 4
	Then A new lease is created successfully

Scenario: 16._Leases_Approval_Consultations
	Given I create a new minimum Lease from row number 20
	When I insert new consultations to the Lease
	And I update a Lease's consultation from row number 21
	Then A new lease is created successfully

Scenario: 17._Lease_Checklist_Tab
	Given I create a new minimum Lease from row number 22
	When I insert Checklist information to a Lease
	Then A new lease is created successfully

Scenario: 18._Lease_and_License_Tenants_Tab
	Given I create a new minimum Lease from row number 6
	When  I add Tenants or Payees to the Lease
	And I update a Lease's Tenants from row number 7
	Then A new lease is created successfully

Scenario: 19._Lease_and_License_Insurance_Tab
	Given I create a new minimum Lease from row number 25
	When  I add Insurance to the Lease
	And I update a Lease's Insurance from row number 26
	Then A new lease is created successfully

Scenario: 20._Lease_and_License_Deposits_Tab
	Given I create a new minimum Lease from row number 27
	When  I add Deposits to the Lease
	And I update a Lease's Deposits from row number 28
	Then A new lease is created successfully

Scenario: 21._Lease_and_License_Periods_and_Payments_Tab
	Given I create a new minimum Lease from row number 29
	When  I add Periods and Payments to the Lease
	And I update a Lease's Payments from row number 30
	Then A new lease is created successfully

Scenario: 22._Lease_and_License_Documents_Tab
	Given I create a new minimum Lease from row number 32
	When  I create Digital Documents for a "Lease" from row number 1
	And I edit a Digital Document for a "Lease" from row number 5
	Then A new lease is created successfully

Scenario: 23._Lease_and_License_from_Inventory_Search_Control
	Given I create a new Lease through a Property Pin from row number 33
	Then A new lease is created successfully

Scenario: 24._Lease_and_License_List_View
	Given I search for an existing Lease or License from row number 2
	Then Expected Lease File Content is displayed on Leases Table

Scenario: 25._Research_File_Details
	Given I create a basic Research File from row number 1
	When I add additional details to Research File
	And I update a Research File Details from row number 4
	Then A new Research File is created successfully

Scenario: 26._Research_File_Notes
	Given I create a basic Research File from row number 1
	When I create a new Note on the Notes Tab from row number 1
	And I edit a Note on the Notes Tab from row number 2
	Then A new Research File is created successfully

Scenario: 27._Research_File_from_Search
	Given I create a Research File from a search on map and from row number 6
	Then A new Research File is created successfully

Scenario: 28._Research_File_List_View
	Given I search for Research Files from row number 4
	Then Research File Properties remain unchanged

Scenario: 29._Disposition_File_Details
	Given I create a new Disposition File from row number 2
	When I add additional information to the Disposition File Details
	And I update the File details from an existing Disposition File from row number 3
	Then A new Disposition file is created successfully

Scenario: 30._Disposition_Offers_and_Sale_Tab
	Given I create a new Disposition File from row number 8
	When I create Appraisal, Assessment, Offers and Sales Details within a Disposition File
	And I update Appraisal, Assessment and Offers section within Disposition File from row number 9
	Then A new Disposition file is created successfully

Scenario: 31._Disposition_File_from_Parcel
	Given I create a Disposition File from a pin on map from row number 12
	Then A new Disposition file is created successfully

Scenario: 32._Disposition_Files_List_View
	Given I search for an existing Disposition File from row number 3
	Then Expected Disposition File Content is displayed on Disposition File List View

Scenario: 33._Organization_Contacts
	Given I create a new Organization Contact from row number 1
	When I update an existing Organization Contact from row number 2
	And I search for an existing contact from type "Organization" row number 1
	Then  Expected Content is displayed on Contacts Table from contact type "Organization"

Scenario: 34._Individual_Contacts
	Given I create a new Individual Contact from row number 1
	When I update an existing Individual Contact from row number 2
	And I search for an existing contact from type "Individual" row number 1
	Then Expected Content is displayed on Contacts Table from contact type "Individual"	

Scenario: 35._Contacts_List_View
	Given I verify the Contacts List View from row number 1
	Then Expected Content is displayed on Contacts Table from contact type "Organization"
