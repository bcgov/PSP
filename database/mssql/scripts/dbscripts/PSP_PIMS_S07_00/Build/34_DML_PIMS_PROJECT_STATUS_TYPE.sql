/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROJECT_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_STATUS_TYPE
GO

INSERT INTO PIMS_PROJECT_STATUS_TYPE (PROJECT_STATUS_TYPE_CODE, CODE_GROUP, DESCRIPTION, TEXT, IS_MILESTONE, IS_TERMINAL, DISPLAY_ORDER)
VALUES
  (N'DR', N'Draft', N'Draft', N'A new draft project that is not ready to submit to apply to be added to the Surplus Property Program.', 0, 0, 0),
  (N'DR-P', N'Draft', N'Select Properties', N'Add properties to the project.', 0, 0, 1),
  (N'DR-I', N'Draft', N'Update Information', N'Assign tier level, classification and update current financial information.', 0, 0, 2),
  (N'DR-D', N'Draft', N'Required Documentation', N'Required documentation has been completed and sent (Surplus Declaration & Readiness Checklist, Triple Bottom Line).', 0, 0, 3),
  (N'DR-A', N'Draft', N'Approval', N'The project is ready to be approved by owning agency.', 0, 0, 4),
  (N'DR-RE', N'Draft', N'Review', N'The project has been submitted for review to be added to the Surplus Property Program.', 0, 0, 5),
  (N'AS-I', N'Submitted', N'Submitted', N'Submitted project property information review.', 1, 0, 6),
  (N'AS-EXE', N'Submitted', N'Submitted Exemption', N'Project has been been submitted with a request for exemption.', 1, 0, 6),
  (N'AS-D', N'Submitted', N'Document Review', N'Documentation reviewed (Surplus Declaration & Readiness Checklist, Triple Bottom Line).', 0, 0, 7),
  (N'AS-AP', N'Submitted', N'Appraisal Review', N'Appraisal review process.', 0, 0, 8),
  (N'AS-FNC', N'Submitted', N'First Nation Consultation', N'First Nation Consultation process.', 0, 0, 9),
  (N'AS-EXP', N'Submitted', N'Exemption Review', N'Process to approve ERP exemption.', 0, 0, 10),
  (N'AP-ERP', N'Approved', N'Approved for ERP', N'The project has been approved to be added to the Surplus Property Program - Enhanced Referral Program.  This begins the 90 day internal marketing process.', 1, 0, 11),
  (N'AP-EXE', N'Approved', N'Approved for Exemption', N'Project has been approved for ERP exemption.', 1, 0, 11),
  (N'DE', N'Closed', N'Denied', N'The project has been denied to be added to the Surplus Property Program.', 1, 1, 11),
  (N'T-GRE', N'Closed', N'Transferred within the GRE', N'The project has been transferred within the Greater Reporting Entity', 1, 1, 21),
  (N'AP-SPL', N'Approved', N'Approved for SPL', N'The project has been approved to be added to the Surplus Property Program - Surplus Property List.  This begins the external marketing process.', 1, 0, 21),
  (N'AP-!SPL', N'Approved', N'Not in SPL', N'The project has been approved to not be included in the Surplus Property Program - Surplus Property List. ', 1, 0, 21),
  (N'CA', N'Closed', N'Cancelled', N'The project has been cancelled from the Surplus Property Program.', 1, 1, 21),
  (N'ERP-ON', N'ERP', N'In ERP', N'The project has is in the Enhanced Referral Program.', 0, 0, 1),
  (N'ERP-OH', N'ERP', N'On Hold', N'The project has been put on hold due to potential sale to an interested party.', 0, 0, 2),
  (N'DIS', N'Complete', N'Disposed', N'The project has been disposed externally.', 1, 1, 21),
  (N'SPL-PM', N'Pre-Marketing', N'Pre-Marketing', N'The project is in the pre-marketing stage of the Surplus Property List.', 0, 0, 18),
  (N'SPL-M', N'Marketing', N'On Market', N'The project is in the marketing stage of the Surplus Property List.', 0, 0, 19),
  (N'SPL-CIP-C', N'Contract in Place', N'Contract in Place - Conditional', N'The project has received a conditional offer.', 0, 0, 20),
  (N'SPL-CIP-U', N'Contract in Place', N'Contract in Place - Unconditional', N'The project has received an unconditional offer.', 0, 0, 20);