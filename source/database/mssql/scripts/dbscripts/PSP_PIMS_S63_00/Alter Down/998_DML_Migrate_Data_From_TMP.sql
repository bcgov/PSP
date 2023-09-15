/* -----------------------------------------------------------------------------
Migrate the data from the temporary table to PIMS_PROPERTY.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-14  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data from the temporary entity
UPDATE prp 
SET    prp.PROPERTY_CLASSIFICATION_TYPE_CODE = tmp.PROPERTY_CLASSIFICATION_TYPE_CODE
     , prp.CONCURRENCY_CONTROL_NUMBER        = prp.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY     prp JOIN
       TMP_PIMS_PROPERTY tmp ON tmp.PROPERTY_ID = prp.PROPERTY_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for the Property data
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_PROPERTY] 
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
