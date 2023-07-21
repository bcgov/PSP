/* -----------------------------------------------------------------------------
Migrate the data from the PIMS_ACQUISITION_PAYEE and PIMS_INTEREST_HOLDER tables 
to the temporary tables.
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
    [INTEREST_HOLDER_ID]        BIGINT,
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

-- Migrate the existing data to the temporary entity
INSERT INTO [dbo].[TMP_INTEREST_HOLDER] (INTEREST_HOLDER_ID, INTEREST_HOLDER_TYPE_CODE, ACQUISITION_FILE_ID, PERSON_ID, ORGANIZATION_ID, COMMENT, IS_DISABLED)
SELECT INTEREST_HOLDER_ID
     , INTEREST_HOLDER_TYPE_CODE
     , ACQUISITION_FILE_ID
     , PERSON_ID
     , ORGANIZATION_ID
     , COMMENT
     , IS_DISABLED
FROM   PIMS_INTEREST_HOLDER
WHERE  INTEREST_HOLDER_TYPE_CODE IN (N'AOREP', N'AOSLCTR')
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
    [ACQUISITION_PAYEE_ID]      BIGINT,
    [INTEREST_HOLDER_ID]        BIGINT,
    [INTEREST_HOLDER_TYPE_CODE] NVARCHAR(20)
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_PAYEE
-- for Owner Representatives
INSERT INTO [dbo].[TMP_ACQUISITION_PAYEE] (ACQUISITION_PAYEE_ID, INTEREST_HOLDER_ID, INTEREST_HOLDER_TYPE_CODE)
SELECT pay.ACQUISITION_PAYEE_ID
     , pay.INTEREST_HOLDER_ID
     , hld.INTEREST_HOLDER_TYPE_CODE
FROM   PIMS_ACQUISITION_PAYEE pay JOIN
       PIMS_INTEREST_HOLDER   hld ON  hld.INTEREST_HOLDER_ID = pay.INTEREST_HOLDER_ID
                                  AND hld.INTEREST_HOLDER_TYPE_CODE IN ('AOREP', 'AOSLCTR')
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
