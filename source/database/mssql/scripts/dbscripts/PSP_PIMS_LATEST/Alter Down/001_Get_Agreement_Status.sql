/*******************************************************************************
Get the current rows from PIMS_AGREEMENT to copy to TMP_PIMS_AGREEMENT 
pre-Alter_Down.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-07  Original version.
*******************************************************************************/

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_AGREEMENT
DROP TABLE IF EXISTS TMP_PIMS_AGREEMENT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table TMP_PIMS_AGREEMENT
CREATE TABLE TMP_PIMS_AGREEMENT (
    AGREEMENT_ID BIGINT,
    IS_DRAFT     BIT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data from PIMS_AGREEMENT
INSERT INTO TMP_PIMS_AGREEMENT (AGREEMENT_ID, IS_DRAFT)
  SELECT AGREEMENT_ID
       , IIF(AGREEMENT_STATUS_TYPE_CODE = N'DRAFT', CONVERT([bit],(1)), CONVERT([bit],(0)))
  FROM   PIMS_AGREEMENT
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
