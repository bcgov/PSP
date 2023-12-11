/*******************************************************************************
Get the current rows from TMP_PIMS_PRODUCT to copy to PIMS_PROJECT_PRODUCT 
post-Alter_Down.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Oct-18  Original version.
*******************************************************************************/

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the existing data to PIMS_PROJECT_PRODUCT
INSERT INTO PIMS_PROJECT_PRODUCT (PRODUCT_ID, PROJECT_ID)
  SELECT ID
       , PARENT_PROJECT_ID
  FROM   TMP_PIMS_PRODUCT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_PRODUCT
DROP TABLE IF EXISTS TMP_PIMS_PRODUCT
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
