/* -----------------------------------------------------------------------------
Migrate the data from PIMS_PROPERTY to the temporary table.
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

-- Drop the existing temporary table for the Property data
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_PROPERTY] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the Property data
CREATE TABLE [dbo].[TMP_PIMS_PROPERTY] (
    PROPERTY_ID                       BIGINT,
    PROPERTY_CLASSIFICATION_TYPE_CODE NVARCHAR(20))
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity
INSERT INTO [dbo].[TMP_PIMS_PROPERTY] (PROPERTY_ID, PROPERTY_CLASSIFICATION_TYPE_CODE)
SELECT PROPERTY_ID
     , PROPERTY_CLASSIFICATION_TYPE_CODE
FROM   PIMS_PROP_PROP_CLASSIFICATION
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
