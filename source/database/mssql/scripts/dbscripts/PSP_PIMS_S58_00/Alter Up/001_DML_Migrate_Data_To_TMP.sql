/* -----------------------------------------------------------------------------
Migrate the data from the PIMS_ACQUISITION_OWNER_SOLICITOR and 
PIMS_ACQUISITION_OWNER_REP tables to the temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jul-06  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- =============================================================================
-- Interest Holders ============================================================
-- =============================================================================

-- Drop the existing temporary table for the Interest Holders
DROP TABLE IF EXISTS [dbo].[TMP_INTEREST_HOLDER] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the Interest Holders
CREATE TABLE [dbo].[TMP_INTEREST_HOLDER] (
    [LEGACY_AO_REP_SOL_ID]      BIGINT,
    [INTEREST_HOLDER_TYPE_CODE] NVARCHAR(20),
    [ACQUISITION_FILE_ID]       BIGINT,
    [PERSON_ID]                 BIGINT,
    [ORGANIZATION_ID]           BIGINT,
    [COMMENT]                   NVARCHAR(2000),
    [IS_DISABLED]               BIT
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_OWNER_SOLICITOR
INSERT INTO [dbo].[TMP_INTEREST_HOLDER] (LEGACY_AO_REP_SOL_ID, INTEREST_HOLDER_TYPE_CODE, ACQUISITION_FILE_ID, PERSON_ID, ORGANIZATION_ID, IS_DISABLED)
SELECT OWNER_SOLICITOR_ID
     , N'AOSLCTR'
     , ACQUISITION_FILE_ID
     , PERSON_ID
     , ORGANIZATION_ID
     , IS_DISABLED
FROM   PIMS_ACQUISITION_OWNER_SOLICITOR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_OWNER_REP
INSERT INTO [dbo].[TMP_INTEREST_HOLDER] (LEGACY_AO_REP_SOL_ID, INTEREST_HOLDER_TYPE_CODE, ACQUISITION_FILE_ID, PERSON_ID, COMMENT, IS_DISABLED)
SELECT OWNER_REPRESENTATIVE_ID
     , N'AOREP'
     , ACQUISITION_FILE_ID
     , PERSON_ID
     , COMMENT
     , IS_DISABLED
FROM   PIMS_ACQUISITION_OWNER_REP
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- =============================================================================
-- Acquisition Payees ==========================================================
-- =============================================================================

-- Drop the existing temporary table for Acquisition Payess
DROP TABLE IF EXISTS [dbo].[TMP_ACQUISITION_PAYEE] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the Acquisition Payess
CREATE TABLE [dbo].[TMP_ACQUISITION_PAYEE] (
    [ACQUISITION_PAYEE_ID] BIGINT,
    [LEGACY_AO_REP_SOL_ID] BIGINT
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_PAYEE
-- for Owner Representatives
INSERT INTO [dbo].[TMP_ACQUISITION_PAYEE] (ACQUISITION_PAYEE_ID, LEGACY_AO_REP_SOL_ID)
SELECT ACQUISITION_PAYEE_ID
     , OWNER_REPRESENTATIVE_ID
FROM   PIMS_ACQUISITION_PAYEE
WHERE  OWNER_REPRESENTATIVE_ID IS NOT NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_PAYEE
-- for Owner Solicitors
INSERT INTO [dbo].[TMP_ACQUISITION_PAYEE] (ACQUISITION_PAYEE_ID, LEGACY_AO_REP_SOL_ID)
SELECT ACQUISITION_PAYEE_ID
     , OWNER_SOLICITOR_ID
FROM   PIMS_ACQUISITION_PAYEE
WHERE  OWNER_SOLICITOR_ID IS NOT NULL
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
