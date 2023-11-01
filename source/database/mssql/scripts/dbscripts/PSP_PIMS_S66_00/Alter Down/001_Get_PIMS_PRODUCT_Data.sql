/*******************************************************************************
Get the current rows from PIMS_PROJECT_PRODUCT to copy to TMP_PIMS_PRODUCT 
pre-Alter_Down.
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

-- Drop the existing temporary table TMP_PIMS_PRODUCT
DROP TABLE IF EXISTS TMP_PIMS_PRODUCT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table TMP_PIMS_PRODUCT
CREATE TABLE TMP_PIMS_PRODUCT (
    ID                BIGINT,
    PARENT_PROJECT_ID BIGINT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data from PIMS_PRODUCT
INSERT INTO TMP_PIMS_PRODUCT (ID, PARENT_PROJECT_ID)
  SELECT PRODUCT_ID
       , PROJECT_ID
  FROM   PIMS_PROJECT_PRODUCT
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
