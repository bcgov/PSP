@managementFiles
Feature: Management File

 Scenario: User creates a Management File
     Given I create a new Management File with row number 0
     And I add additional information to the Management File Details
     And I update the File details from an existing Management File from row number 1
     Then A new Management file is created or updated successfully