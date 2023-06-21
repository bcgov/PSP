/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_ACQ_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQ_CHKLST_ITEM_TYPE
GO

INSERT INTO PIMS_ACQ_CHKLST_ITEM_TYPE (ACQ_CHKLST_SECTION_TYPE_CODE, ACQ_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, HINT)
VALUES
  (N'FILEINIT',  N'PREACQTTL',   N'Pre-acquisition title',                     1,  NULL),
  (N'FILEINIT',  N'BCASSESS',    N'BC Assessment',                             2,  NULL),
  (N'FILEINIT',  N'COMPSRCH',    N'Company search',                            3,  NULL),
  (N'FILEINIT',  N'LANDTTL',     N'Land Title documents or plans',             4,  NULL),
  (N'FILEINIT',  N'SGNPAPLAN',   N'Property Acquisition (PA) Plan',            5,  N'A signed copy of the PA Plan is required'),
  (N'FILEMGMT',  N'PHOTOS',      N'Photos',                                    6,  NULL),
  (N'FILEMGMT',  N'APPRAISE',    N'Appraisals and reviews',                    7,  NULL),
  (N'FILEMGMT',  N'LGLSURVEY',   N'Legal survey(s)',                           8,  NULL),
  (N'FILEMGMT',  N'OTHERRPTS',   N'Other reports',                             9,  N'Final copies of professional reports (e.g. Environmental reports)'),
  (N'FILEMGMT',  N'RECNEGOT',    N'Record of negotiation',                    10,  NULL),
  (N'FILEMGMT',  N'CORRESCPY',   N'Copies of correspondence',                 11,  NULL),
  (N'FILEMGMT',  N'SAASIGNED',   N'Spending Authority Approval(s) - SAA',     12,  N'The signed SAA is required'),
  (N'FILEMGMT',  N'FINALAGRM',   N'Final agreements with owner(s)',           13,  N'H179 P/T/A, TLCA, LTC, etc.'),
  (N'FILEMGMT',  N'H120COMPREQ', N'H120(s) processed for compensation',       14,  NULL),
  (N'FILEMGMT',  N'LEGALDOCS',   N'Instructions or documents sent to lawyer', 15,  NULL),
  (N'FILEMGMT',  N'OWNERCOMP',   N'Copy of owners'' compensation cheque(s)',  16,  NULL),
  (N'FILEMGMT',  N'ENTRYCOND',   N'Conditions of Entry (H0443)',              17,  NULL),
  (N'FILEMGMT',  N'CONVEYCLOS',  N'Conveyance closing documents',             18,  NULL),
  (N'FILEMGMT',  N'REGSTRDPLAN', N'Registered Plan',                          19,  N'Letter size (8.5" x 11")'),
  (N'FILEMGMT',  N'ALCORDER',    N'ALC order',                                20,  N'If applicable'),
  (N'FILEMGMT',  N'NEWTTLACOMP', N'Copy of new title',                        21,  N'Copy of new title after completion'),
  (N'FILEMGMT',  N'BCTFANOTIF',  N'BCTFA notified',                           22,  N'Applicable if this is a total take with a surplus'),
  (N'CROWNLND',  N'CRWNLNDRSCH', N'Crown Land research',                      23,  N'Existing / overlapping / conflicting tenures'),
  (N'CROWNLND',  N'SUBCLTENAPP', N'Submit Crown Land tenure application',     24,  N'Section 16 / Section 17 applications'),
  (N'CROWNLND',  N'CRWNGRNTPKG', N'Crown grant package',                      25,  NULL),
  (N'SCTN3AGR',  N'AVNCDPMTNTY', N'Notice of Advanced Payment (Form 8)',      26,  N'The signed Form 8 is required'),
  (N'SCTN3AGR',  N'CLAIMRELSS3', N'Release of Claims',                        27,  NULL),
  (N'SCTN3AGR',  N'CONVEYLTR',   N'Conveyance letter',                        28,  NULL),
  (N'SCTN3AGR',  N'EXPROPACT',   N'Copy of Expropriation Act',                29,  NULL),
  (N'SCTN3AGR',  N'SGNDAGRMNT',  N'Signed Section 3 Agreement',               30,  NULL),
  (N'SCTN3AGR',  N'TTLCHRGS',    N'Title and charges',                        31,  NULL),
  (N'SCTN3AGR',  N'CURRYRASSMN', N'Current year property assessment',         32,  NULL),
  (N'SCTN3AGR',  N'S3CHEQCOPY',  N'Copy of cheque to owner(s)',               33,  NULL),
  (N'SCTN3AGR',  N'S3APPRCOPY',  N'Copy of appraisial to owner(s)',           34,  NULL),
  (N'SCTN6XPRP', N'EXPROPCOPY',  N'Copy of offer(s) extended to owner(s)',    35,  NULL),
  (N'SCTN6XPRP', N'EXPROPAPPRV', N'Copy of expropriation approval packages',  36,  N'Executive and case summaries'),
  (N'SCTN6XPRP', N'EXPROPFORM1', N'Notice of Expropriation (Form 1)',         37,  N'The signed Form 1 is required'),
  (N'SCTN6XPRP', N'EXPROPFORM5', N'Approval of Expropriation (Form 5)',       38,  N'The signed Form 5 is required'),
  (N'SCTN6XPRP', N'ADVPAYMENT8', N'Notice of Advanced Payment (Form 8)',      39,  N'The signed Form 8 is required'),
  (N'SCTN6XPRP', N'VESTNOTICE',  N'Vesting Notice (Form 9)',                  40,  N'The signed Form 9 is required'),
  (N'SCTN6XPRP', N'SVCAFFADAV',  N'Copies of Affidavits of Service',          41,  NULL),
  (N'SCTN6XPRP', N'S6CHEQCOPY',  N'Copy of cheque to owner(s)',               42,  NULL),
  (N'SCTN6XPRP', N'S6APPRCOPY',  N'Copy of appraisial to owner(s)',           43,  NULL),
  (N'SCTN6XPRP', N'CLAIMRELSS6', N'Release of Claims',                        44,  NULL),
  (N'SCTN6XPRP', N'EASEMENTCPY', N'Copy of Easement',                         45,  NULL),
  (N'ACQCOMPAC', N'PIMSUPDTFIL', N'File data entered into PIMS',              46,  N'Data is complete in this system');
GO
