/*******************************************************************************
Get the current rows from TMP_PIMS_AGREEMENT to copy to PIMS_AGREEMENT 
post-Alter_Down.
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

-- Insert the existing data to PIMS_AGREEMENT
UPDATE agr
SET    agr.AGREEMENT_STATUS_TYPE_CODE = IIF(tmp.IS_DRAFT = CONVERT([bit],(1)), N'DRAFT', N'FINAL')
     , agr.CONCURRENCY_CONTROL_NUMBER = agr.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_AGREEMENT     agr JOIN
       TMP_PIMS_AGREEMENT tmp ON tmp.AGREEMENT_ID = agr.AGREEMENT_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_PRODUCT
DROP TABLE IF EXISTS TMP_PIMS_AGREEMENT
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
