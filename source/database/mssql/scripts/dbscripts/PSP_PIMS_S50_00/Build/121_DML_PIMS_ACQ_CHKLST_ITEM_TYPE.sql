/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_ACQ_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_CHKLST_ITEM_TYPE
GO

INSERT INTO PIMS_ACQ_CHKLST_ITEM_TYPE (ACQ_CHKLST_SECTION_TYPE_CODE, ACQ_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, EFFECTIVE_DATE, HINT)
VALUES
  (N'FILEINIT',  N'PREACQTTL',   N'Pre-acquisition title',                                      1, getutcdate(), NULL),
  (N'FILEINIT',  N'BCASSESS',    N'BC Assessment',                                              2, getutcdate(), NULL),
  (N'FILEINIT',  N'COMPSRCH',    N'Company search',                                             3, getutcdate(), NULL),
  (N'FILEINIT',  N'LANDTTL',     N'Land Title documents or plans',                              4, getutcdate(), NULL),
  (N'FILEINIT',  N'SGNPAPLAN',   N'Property Acquisition (PA) Plan',                             5, getutcdate(), 'A signed copy of the PA Plan is required'),
  (N'FILEMGMT',  N'PHOTOS',      N'Photos',                                                     6, getutcdate(), NULL),
  (N'FILEMGMT',  N'APPRAISE',    N'Appraisals and reviews',                                     7, getutcdate(), NULL),
  (N'FILEMGMT',  N'OTHERRPTS',   N'Other reports',                                              8, getutcdate(), 'Final copies of professional reports (e.g. Environmental reports)'),
  (N'FILEMGMT',  N'RECNEGOT',    N'Record of negotiation',                                      9, getutcdate(), NULL),
  (N'FILEMGMT',  N'CORRESCPY',   N'Copies of correspondence',                                  10, getutcdate(), NULL),
  (N'FILEMGMT',  N'SAASIGNED',   N'Spending Authority Approval(s) - SAA',                      11, getutcdate(), 'The signed SAA is required'),
  (N'FILEMGMT',  N'FINALAGRM',   N'Final agreements with owner(s)',                            12, getutcdate(), 'H179 P/T/A, TLCA, LTC, etc.'),
  (N'FILEMGMT',  N'H120COMPREQ', N'H120(s) processed for compensation',                        13, getutcdate(), NULL),
  (N'FILEMGMT',  N'LEGALDOCS',   N'Instructions or documents sent to lawyer',                  14, getutcdate(), NULL),
  (N'FILEMGMT',  N'OWNERCOMP',   N'Copy of owners'' compensation cheque(s)',                   15, getutcdate(), NULL),
  (N'FILEMGMT',  N'ENTRYCOND',   N'Conditions of Entry (H0443)',                               16, getutcdate(), NULL),
  (N'FILEMGMT',  N'CONVEYCLOS',  N'Conveyance closing documents',                              17, getutcdate(), NULL),
  (N'FILEMGMT',  N'REGSTRDPLAN', N'Registered Plan',                                           18, getutcdate(), 'Letter size (8.5" x 11")'),
  (N'FILEMGMT',  N'NEWTTLACOMP', N'Copy of new title',                                         19, getutcdate(), 'Copy of new title after completion'),
  (N'FILEMGMT',  N'BCTFANOTIF',  N'BCTFA notified',                                            20, getutcdate(), 'Applicable if this is a total take with a surplus'),
  (N'SCTN3AGR',  N'AVNCDPMTNTY', N'Notice of Advanced Payment (Form 8)',                       21, getutcdate(), 'The signed Form 8 is required'),
  (N'SCTN3AGR',  N'CLAIMRELSS3', N'Release of Claims',                                         22, getutcdate(), NULL),
  (N'SCTN3AGR',  N'CONVEYLTR',   N'Conveyance letter',                                         23, getutcdate(), NULL),
  (N'SCTN3AGR',  N'SGNDAGRMNT',  N'Signed Section 3 Agreement',                                24, getutcdate(), NULL),
  (N'SCTN3AGR',  N'TTLCHRGS',    N'Title and charges',                                         25, getutcdate(), NULL),
  (N'SCTN3AGR',  N'ALCORDER',    N'ALC order',                                                 26, getutcdate(), 'If applicable'),
  (N'SCTN3AGR',  N'CURRYRASSMN', N'Current year property assessment',                          27, getutcdate(), NULL),
  (N'SCTN3AGR',  N'S3CHEQCOPY',  N'Copy of cheque to owner(s)',                                28, getutcdate(), NULL),
  (N'SCTN3AGR',  N'S3APPRCOPY',  N'Copy of appraisial to owner(s)',                            29, getutcdate(), NULL),
  (N'SCTN6XPRP', N'EXPROPAPPRV', N'Copy of expropriation approval packages',                   30, getutcdate(), 'Executive and case summaries'),
  (N'SCTN6XPRP', N'EXPROPFORM1', N'Notice of Expropriation (Form 1)',                          31, getutcdate(), 'The signed Form 1 is required'),
  (N'SCTN6XPRP', N'EXPROPFORM5', N'Approval of Expropriation (Form 5)',                        32, getutcdate(), 'The signed Form 5 is required'),
  (N'SCTN6XPRP', N'ADVPAYMENT8', N'Notice of Advanced Payment (Form 8)',                       33, getutcdate(), 'The signed Form 8 is required'),
  (N'SCTN6XPRP', N'VESTNOTICE',  N'Vesting Notice (Form 9)',                                   34, getutcdate(), 'The signed Form 9 is required'),
  (N'SCTN6XPRP', N'SVCAFFADAV',  N'Copies of Affidavits of Service',                           35, getutcdate(), NULL),
  (N'SCTN6XPRP', N'S6CHEQCOPY',  N'Copy of cheque to owner(s)',                                36, getutcdate(), NULL),
  (N'SCTN6XPRP', N'S6APPRCOPY',  N'Copy of appraisial to owner(s)',                            37, getutcdate(), NULL),
  (N'SCTN6XPRP', N'CLAIMRELSS6', N'Release of Claims',                                         38, getutcdate(), NULL),
  (N'SCTN6XPRP', N'EASEMENTCPY', N'Copy of Easement',                                          39, getutcdate(), NULL),
  (N'ACQCOMPAC', N'PIMSUPDTFIL', N'File data entered into PIMS',                               40, getutcdate(), 'Data is complete in this system');
GO
