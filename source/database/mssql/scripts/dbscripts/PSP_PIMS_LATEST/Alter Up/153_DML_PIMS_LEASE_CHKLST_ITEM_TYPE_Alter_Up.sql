/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_CHKLST_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jun-14  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter PIMS_LEASE_CHKLST_ITEM_TYPE
PRINT N'Alter PIMS_LEASE_CHKLST_ITEM_TYPE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Document that initiated the lease/license request, such as Term sheet or an email.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'FIINITIDOC'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Title reviewed to ensure BCTFA ownership, no indications of established highway (i.e. E & F clause) and no conflicting registered charges (i.e. limiting use SRW)'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'FICURRTTL'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Verify that there are no conflicts (i.e. other agreements) or other matters of concern that may preclude leasing/licencing (i.e. environmental offset or contamination)'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'FICURRHISFIL'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Data under the ''Property details'' tab on the ''Property Information'' panel is correct / complete and any other licence files are in appropriate status'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'FIUPDTPRDATA'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Required if proposed use could impact aboriginal rights (i.e. long term, non-exclusive use agreement, ground disturbance or tree removal). Verify with MOTI IR if unsure.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RAFIRSTNTN'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Required if proposed use may conflict with current or future land sale'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RASTRATRE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Required if proposed use may conflict with future MOTI project or land use'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RARGNLPLAN'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Required if proposed use may conflict with existing regional agreement and to understand the land purpose (i.e. held for slope stabilization)'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RARGNLPRSVC'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Required to determine if district has any recommendations for use or, for a renewal, feedback on existing licensee.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RADISTRICT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Senior Manager of Land Operations approval required'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RAHQ'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO     

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Required if proposed use could conflict with other ministry branch''s/division''s needs and/or advice required from technical group (i.e. geotech or enviro)'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'RAADDLREVW'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Determine appropriate fee - Nominal / Licence administration fee / Fair market value'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APFEEDETER'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Determine and draft appropriate agreement type - titled (H1005a), over highway (H1005), Lease, Letter of Intended Use, Indemnity Letter. Legal to draft/review agreement if not a standard lease/licence'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APDRAFTAGG'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Security deposit required if higher risk use or tenant.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APDEPOSIT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Appraisal required for fair market value tenancies, if valuation can''t be reasonably performed in house.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APAPPREVW'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Manager approval of draft agreement.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APAGREEAPP'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Completed confirmation of Insurance required on ministry form (H0111 lease/licence for fee simple) or (H0111 for highway).'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APINSURDTL'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Final copies of professional reports (i.e. environmental report).'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APOTHRRPTS'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Pre-tenancy photos taken and saved to PIMS.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APPREPHOTO'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Finance department notified of expected payment and provided copy of agreement. Required for non-nominal agreements'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APFINTMNOT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Fully executed agreement sent to other party (BCTFA always signs last).'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APEXECUTED'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Any Landlord responsibilities agreed to need to be noted (i.e. payments for services or tenant improvements, new appliances, hydro, water etc.)'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APLLORDRSP'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'BCA notified if that property has taxable occupier, if applicable.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'APBCASNOT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Data entry and document uploads are complete in this system when closing a lease/licence.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'LCDATAENTD'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    HINT                       = N'Set the file under appropriate status - Cancelled / Terminated.'
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1 
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = N'LCUPDTSTAT'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
