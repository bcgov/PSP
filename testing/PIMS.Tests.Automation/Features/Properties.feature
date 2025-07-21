@Regression-Properties
Feature: Properties

Property Inventory and Information Details test cases

Scenario: 01._Property_Information_Tab
	Given I review a Property's Information from row number 3
	When I update a Property details
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

Scenario: 04._Property_Management_Activity_Digital_Documents
	Given I search for a property in the inventory by PID from row number 23
	When I insert activities to the Property Management Tab from row number 4
	And I create Digital Documents for a Property Management row number 11
	And I delete all activities from the Property Management Tab
	Then Property Management Tab has been updated successfully

Scenario: 05._Properties_Map_and_List_Filters
	Given I search for a Property in the Map by different filters from row number 9
	When I search for a Property in the Properties List by different filters from row number 29
	Then Properties filters works successfully

Scenario: 06._Non-Inventory_Property_Information
	Given I search for a non MOTI property from row number 6
	Then Non-Inventory property renders correctly

Scenario: 07._Invalid_Property_Not_Found
	Given I search for an Invalid Property from row number 10
	Then No Properties were found

Scenario: 08._Map_Features
	Given I verify the Maps Layers
	When I verify the Maps Filters
	Then Map Features rendered successfully

Scenario Outline: 09._Property_Management_Lease_Active_Indicator
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

Scenario: 10._Properties_Digital_Documents
	Given I search for a property in the inventory by PID from row number 37
	When I create Digital Documents for a "Property" row number 16
	Then A Property Information is saved successfully




