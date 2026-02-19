@Regression-Properties
Feature: Properties

Property Inventory and Information Details test cases

Scenario: 01._Property_Information_Tab
	Given I review a Property's Information from row number 3
	When I update a Property details
	Then A Property Information is saved successfully

Scenario: 02._Property_Improvements_Tab
	Given I review a Property's Information from row number 3
	When I create Property Improvements
	Then I update Property Improvements from row number 6
	Then A Property Information is saved successfully

Scenario: 02._Property_PIMS_Files_Tab
	Given I search for a property in the inventory by PID from row number 24
	When I verify the PIMS Files Tab
	Then PIMS Files Tab has rendered successfully

Scenario: 03._Property_Management_Tab
	Given I search for a property in the inventory by PID from row number 23
	When I insert information in the Property Management Tab from row number 1
	And I update information in the Property Management Tab from row number 2
	And I clean up the Property Management Tab from row number 3
	Then Property Management Tab has been updated successfully

Scenario: 04._Property_Documents_Tab
	Given I review a Property's Information from row number 3
	When  I create Digital Documents for a "Property" from row number 1
	And I edit a Digital Document for a "Property" from row number 16
	Then A Property Information is saved successfully

Scenario: 05._Property_Notes_Tab
	Given I review a Property's Information from row number 3
	When I create a new Note on the Notes Tab from row number 13
	And I edit a Note on the Notes Tab for a "Property" from row number 14
	Then A Property Information is saved successfully

Scenario: 06._Property_Management_Activity_Digital_Documents
	Given I search for a property in the inventory by PID from row number 23
	When I insert activities to the Property Management Tab from row number 4
	And I create Digital Documents for a Property Management row number 11
	And I delete all activities from the Property Management Tab
	Then Property Management Tab has been updated successfully

Scenario: 07._Verify_Digital_Documents_on_Related_Documents
	Given I search for a property in the inventory by PID from row number 37
	When I create Digital Documents for a "Property" from row number 16
	And  I create a Management File from row number 5 to check common data
	And I add Properties to the Management File
	Then The related documents appeared as expected

Scenario: 08._Properties_List
	Given I search for a Property in the Properties List by different filters from row number 29
	Then Properties filters works successfully

Scenario: 09._Non-Inventory_Property_Information
	Given I search for a non MOTI property from row number 6
	Then Non-Inventory property renders correctly

Scenario: 10._Invalid_Property_Not_Found
	Given I search for an Invalid Property from row number 10
	Then No Properties were found

Scenario: 11._Map_Features
	Given I verify the Maps Layers
	Then Map Features rendered successfully

Scenario Outline: 12._Property_Management_Lease_Active_Indicator
	Given I create a new minimum Lease from row number <RowNumber>
	When  I add additional Information to the Lease Details
	And I add Properties to the Lease Details
	And I search for a Property in the Properties List by PID from row number 33
	Then Expected Active Lease status is displayed as "<ActiveLeaseStatus>" successfully
	Examples:
	| ActiveLeaseStatus | RowNumber |
	| No                | 10        |
	| No                | 11        |
	| No                | 12        |
	| No                | 13        |
	| No                | 14        |
	| No                | 15        |
	| No				| 16        |
	| Yes               | 17        |

Scenario: 13._Strata_Properties_Tabs
	Given I search for a property by Plan Number from row number 39
	When I verify the MultiProperty Tabs
	Then Multiproperty property rendered successfully

Scenario: 14._Property_Hwy_and_Other_Tabs
	Given I search for a property by Plan Number from row number 40
	When I verify the Highway Tab
	And I verify the Other Tab
	Then Property Tabs rendered successfully

Scenario: 15._Property_PMBC_and_Crown_Tabs
	Given I search for a property by PID from row number 41
	When I verify the PMBC Tab
	And I verify the Crown Tab
	Then Property Tabs rendered successfully




