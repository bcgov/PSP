/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_ACQ_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_CHKLST_ITEM_TYPE
GO

INSERT INTO PIMS_ACQ_CHKLST_ITEM_TYPE (ACQ_CHKLST_SECTION_TYPE_CODE, ACQ_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE)
VALUES
  (N'FILEINIT',  N'PREACQTTL',   N'Pre-acquisition title',                                      1, getutcdate()),
  (N'FILEINIT',  N'BCASSESS',    N'BC Assessment',                                              2, getutcdate()),
  (N'FILEINIT',  N'COMPSRCH',    N'Company search',                                             3, getutcdate()),
  (N'FILEINIT',  N'LANDTTL',     N'Land title documents or plans',                              4, getutcdate()),
  (N'FILEINIT',  N'SGNPAPLAN',   N'Property Acquisition (PA) plan',                             5, getutcdate()),
  (N'FILEMGMT',  N'PHOTOS',      N'Photos',                                                     6, getutcdate()),
  (N'FILEMGMT',  N'APPRAISE',    N'Appraisals and reviews',                                     7, getutcdate()),
  (N'FILEMGMT',  N'OTHERRPTS',   N'Other reports',                                              8, getutcdate()),
  (N'FILEMGMT',  N'RECNEGOT',    N'Record of negotiation',                                      9, getutcdate()),
  (N'FILEMGMT',  N'CORRESCPY',   N'Copies of correspondence',                                  10, getutcdate()),
  (N'FILEMGMT',  N'SAASIGNED',   N'Spending Authority Approval(s) - SAA',                      11, getutcdate()),
  (N'FILEMGMT',  N'FINALAGRM',   N'Copy of agreements with owner(s)',                          12, getutcdate()),
  (N'FILEMGMT',  N'H120COMPREQ', N'H120(s) processed for compensation',                        13, getutcdate()),
  (N'FILEMGMT',  N'LEGALDOCS',   N'Copies of any instructions or documents sent to lawyer(s)', 14, getutcdate()),
  (N'FILEMGMT',  N'OWNERCOMP',   N'Copy of owners'' compensation cheque(s)',                   15, getutcdate()),
  (N'FILEMGMT',  N'ENTRYCOND',   N'Condition of entry (H0443)',                                16, getutcdate()),
  (N'FILEMGMT',  N'CONVEYCLOS',  N'Conveyance closing documents',                              17, getutcdate()),
  (N'FILEMGMT',  N'REGSTRDPLAN', N'Registered plan',                                           18, getutcdate()),
  (N'FILEMGMT',  N'NEWTTLACOMP', N'Copy of new title',                                         19, getutcdate()),
  (N'FILEMGMT',  N'BCTFANOTIF',  N'BCTFA is notified (If there is a total take with surplus)', 20, getutcdate()),
  (N'SCTN3AGR',  N'AVNCDPMTNTY', N'Notice of advanced payment (Form 8)',                       21, getutcdate()),
  (N'SCTN3AGR',  N'CLAIMRELSS3', N'Release of claims',                                         22, getutcdate()),
  (N'SCTN6XPRP', N'EXPROPAPPRV', N'Copy of expropriation approval packages',                   23, getutcdate()),
  (N'SCTN6XPRP', N'EXPROPFORM1', N'Notice of Expropriation (Form 1)',                          24, getutcdate()),
  (N'SCTN6XPRP', N'EXPROPFORM5', N'Approval of Expropriation (Form 5)',                        25, getutcdate()),
  (N'SCTN6XPRP', N'VESTNOTICE',  N'Vesting notice (Form 9)',                                   26, getutcdate()),
  (N'SCTN6XPRP', N'SVCAFFADAV',  N'Copies of affidavits of service',                           27, getutcdate()),
  (N'SCTN6XPRP', N'CLAIMRELSS6', N'Release of claims',                                         28, getutcdate()),
  (N'ACQCOMPAC', N'PIMSUPDTFIL', N'File data entered into PIMS',                               29, getutcdate());
GO
