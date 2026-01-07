/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_DSP_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Dec-04  Initial version.
Doug Filteau  2024-Jan-10  Added 'SGNDXFRPPH' checklist item.
Doug Filteau  2025-Dec-08  Added 'UTILCOAPP', 'ADJLOREF', and 'ADCOMPLT' 
                           checklist items.
Doug Filteau  2025-Dec-08  Disabled 'STRNTHCLM' checklist item.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_CHKLST_ITEM_TYPE
GO

INSERT INTO PIMS_DSP_CHKLST_ITEM_TYPE (DSP_CHKLST_SECTION_TYPE_CODE, DSP_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER, HINT)
VALUES
  -- File Initiation
  (N'FILEINIT',  N'CURRTITL',   N'Current title',                                      1, NULL),
  (N'FILEINIT',  N'BCASSESS',   N'BC Assessment',                                      2, NULL),
  (N'FILEINIT',  N'INITDOC',    N'Initiating Document',                                3, N'Document that started the disposition process - Surplus declaration, H0222 etc.'),
  (N'FILEINIT',  N'PRAPPFEE',   N'Proof of Application Fee',                           4, N'Proof of receiving application fee (e.g.: for road closure).'),
  
  -- Disposition Preparation
  (N'DISPPREP',  N'APPRAISAL',  N'Appraisal',                                          5, NULL),
  (N'DISPPREP',  N'ENVIRON',    N'Environmental',                                      6, NULL),
  (N'DISPPREP',  N'LANDUSEPL',  N'Land use planning',                                  7, NULL),
  (N'DISPPREP',  N'LGLSURVEY',  N'Legal survey',                                       8, NULL),
  
  -- Referrals and Consultations
  (N'REFRCONS',  N'ENHNCDREF',  N'Enhanced referral (Complete/Exempt)',                9, N'Copy of documentation to show completion of, or exemption from, this referral.'),
  (N'REFRCONS',  N'STRNTHCLM',  N'Strength of Claim assessment',                      10, NULL),
  (N'REFRCONS',  N'1STNATION',  N'First Nations consultation',                        11, NULL),
  (N'REFRCONS',  N'LCLGOVTCON', N'Local Government consultation',                     12, NULL),
  (N'REFRCONS',  N'UTILCOREF',  N'Utility company referral',                          13, NULL),
  (N'REFRCONS',  N'ADJLNDOWN',  N'Adjacent landowner referral',                       14, NULL),
  (N'REFRCONS',  N'ADVERTCMP',  N'Advertising completed',                             15, N'The intent to dispose property has been advertised, if applicable (e.g. for road closures).'),
  
  -- Direct Sale or Road Closure
  (N'DIRCTSAL',  N'UTILCOAPP',  N'Utility company referral',                          16, NULL),
  (N'DIRCTSAL',  N'ADJLOREF',   N'Adjacent landowner referral',                       17, NULL),
  (N'DIRCTSAL',  N'ADCOMPLT',   N'Advertising completed',                             18, NULL),
  (N'DIRCTSAL',  N'SRVINSSENT', N'Survey instructions issued to Applicant',           19, N'Letter/notification to applicant issuing survey instructions, if applicable (e.g. Road closure).'),
  (N'DIRCTSAL',  N'LGLSURVAPP', N'Legal survey received and approved',                20, N'Survey is needed to proceed with appraisal. For Road closure this is commissioned by the applicant.'),
  (N'DIRCTSAL',  N'RDCLOSCMP',  N'Road closure plan completed',                       21, NULL),
  (N'DIRCTSAL',  N'CONSPLNCMP', N'Consolidation plan completed',                      22, NULL),
  (N'DIRCTSAL',  N'APPCPYSENT', N'Copy of appraisal sent to buyer(s)',                23, NULL),
  (N'DIRCTSAL',  N'GAZNOTSGND', N'Gazette notice signed by District Highway Manager', 24, NULL),
  (N'DIRCTSAL',  N'GAZNOTPUBD', N'Gazette notice published in BC Gazette',            25, NULL),
  (N'DIRCTSAL',  N'SGNDFRMAXF', N'Signed Form A transfer',                            26, NULL),
  (N'DIRCTSAL',  N'SGNDXFRPPH', N'Signed Transfer Closed PPH Between Gov. Agencies',  27, N'Signed Transfer of Discontinued and Closed Provincial Public Highway Lands To The Minister Responsible'),
  
  -- Sale Information
  (N'SALEINFO', N'SGNDLSTAGRE', N'Signed Listing agreement',                          28, N'Copy of documentation to show completion of, or exemption from, this referral.'),
  (N'SALEINFO', N'LWYRASSGND',  N'Lawyer assigned by Attorney General',               29, NULL),
  (N'SALEINFO', N'ACCOFFRRECD', N'Accepted offer recorded',                           30, N'Accepted offer details have been recorded and corresponding documentation has been uploaded.'),
  (N'SALEINFO', N'LTRINTNTPRP', N'Letter of intent prepared by the lawyer',           31, N'Letter has been prepared and final version has been uploaded.'),
  (N'SALEINFO', N'PSAPREPPED',  N'PSA prepared by the lawyer',                        32, N'Purchase and Sale Agreement (PSA) has been prepared'),
  (N'SALEINFO', N'PSAEXECUTED', N'PSA fully executed',                                33, N'Purchase and Sale Agreement (PSA) has been executed.'),
  (N'SALEINFO', N'FNLCNDRMVD',  N'Final conditions have been removed',                34, NULL),
  (N'SALEINFO', N'XFRPLNREGD',  N'Transfer/plan registered with LTSA',                35, N'Property transfer, or update plan ( for road closures) has been registered with the Land Title office.'),
  (N'SALEINFO', N'SALESPRCRCV', N'Sales proceeds received',                           36, N'Sale proceeds, if any, have been documented and corresponding documentation has been added to the file'),
  (N'SALEINFO', N'FINRPTGCMPL', N'Financial reporting completed',                     37, N'Financial reporting, if required, has been completed.'),
  (N'SALEINFO', N'CHQSNTBCTFA', N'Cheque sent to BCTFA',                              38, NULL);
GO

-- Disable the STRNTHCLM code value
UPDATE PIMS_DSP_CHKLST_ITEM_TYPE
SET    EXPIRY_DATE                = CAST('2025-01-01' AS DATE)
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  DSP_CHKLST_SECTION_TYPE_CODE = 'REFRCONS'
   AND DSP_CHKLST_ITEM_TYPE_CODE    = 'STRNTHCLM'
GO
