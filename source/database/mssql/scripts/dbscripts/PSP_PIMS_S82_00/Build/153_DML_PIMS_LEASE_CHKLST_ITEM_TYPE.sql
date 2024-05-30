/* -----------------------------------------------------------------------------
Populate the PIMS_LEASE_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-17  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_CHKLST_ITEM_TYPE
GO

INSERT INTO PIMS_LEASE_CHKLST_ITEM_TYPE (LEASE_CHKLST_SECTION_TYPE_CODE, LEASE_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  -- File Completion
  (N'FILEINIT', N'FIINITIDOC',   N'Initiating Document',                             1),
  (N'FILEINIT', N'FICURRTTL',    N'Current title',                                   2),
  (N'FILEINIT', N'FICOMPSRCH',   N'Company search',                                  3),
  (N'FILEINIT', N'FILNDTTLDOC',  N'Land title documents or plans',                   4),
  (N'FILEINIT', N'FICURRHISFIL', N'Current/historical files reviewed for conflicts', 5),
  (N'FILEINIT', N'FIUPDTPRDATA', N'Update property data in the system',              6),

  -- Referrals and Approvals
  (N'REFERAPPR', N'RAFIRSTNTN',  N'First Nations',                                   7),
  (N'REFERAPPR', N'RASTRATRE',   N'Strategic Real Estate (SRE)',                     8),
  (N'REFERAPPR', N'RARGNLPLAN',  N'Regional planning',                               9),
  (N'REFERAPPR', N'RARGNLPRSVC', N'Regional Property Services',                     10),
  (N'REFERAPPR', N'RADISTRICT',  N'District',                                       11),
  (N'REFERAPPR', N'RAHQ',        N'Headquarters (HQ)',                              12),
  (N'REFERAPPR', N'RALGLRVW',    N'Legal review',                                   13),
  (N'REFERAPPR', N'RAADDLREVW',  N'Additional reviews or approvals, if applicable', 14),

  -- Agreement Preparation
  (N'AGREEPREP', N'APFEEDETER',  N'Fee determination',                              15),
  (N'AGREEPREP', N'APDRAFTAGG',  N'Draft appropriate agreement type',               16),
  (N'AGREEPREP', N'APDEPOSIT',   N'Deposit',                                        17),
  (N'AGREEPREP', N'APAPPREVW',   N'Appraisal and Reviews',                          18),
  (N'AGREEPREP', N'APAGREEAPP',  N'Agreement approval',                             19),
  (N'AGREEPREP', N'APINSURDTL',  N'Insurance details',                              20),
  (N'AGREEPREP', N'APOTHRRPTS',  N'Other reports',                                  21),
  (N'AGREEPREP', N'APPREPHOTO',  N'Pre-tenancy photos',                             22),
  (N'AGREEPREP', N'APFINTMNOT',  N'Finance team notified',                          23),
  (N'AGREEPREP', N'APAGREESGN',  N'Agreement signed by both parties',               24),
  (N'AGREEPREP', N'APEXECUTED',  N'Executed agreement sent to other party (BCTFA)', 25),
  (N'AGREEPREP', N'APLLORDRSP',  N'Landlord responsibilities noted',                26),
  (N'AGREEPREP', N'APBCASNOT',   N'BC Assessment notified',                         27),

  -- Lease/Licence Completion 
  (N'LLCOMPLTN', N'LCDATAENTD',  N'File data entered into PIMS',                      28),
  (N'LLCOMPLTN', N'LCUPDTSTAT',  N'Update the lease/license status',                  29);
GO
