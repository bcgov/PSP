/* -----------------------------------------------------------------------------
Set columns to not null in PIMS_PROPERTY.
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

-- Drop the index on PIMS_PROPERTY.PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX
DROP INDEX IF EXISTS PIMS_PROPERTY.PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Check to ensure there are no null values in the column before altering it
IF NOT EXISTS (SELECT 1
               FROM   PIMS_PROPERTY
               WHERE  PROPERTY_CLASSIFICATION_TYPE_CODE IS NULL)
  BEGIN
  ALTER TABLE [dbo].[PIMS_PROPERTY]
    ALTER COLUMN PROPERTY_CLASSIFICATION_TYPE_CODE nvarchar(20) NOT NULL;
  END
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the index on PIMS_PROPERTY.PROPERTY_CLASSIFICATION_TYPE_CODE
CREATE NONCLUSTERED INDEX [PRPRTY_PROPERTY_CLASSIFICATION_TYPE_CODE_IDX]
	ON [dbo].[PIMS_PROPERTY]([PROPERTY_CLASSIFICATION_TYPE_CODE])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the non-current history PROPERTY_CLASSIFICATION_TYPE_CODE columns to 
-- allow the alteration of the column to NULL.
UPDATE PIMS_PROPERTY_HIST
SET    PROPERTY_CLASSIFICATION_TYPE_CODE = ' '
WHERE  END_DATE_HIST IS NOT NULL
   AND PROPERTY_CLASSIFICATION_TYPE_CODE IS NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Check to ensure there are no null values in the column before altering it
IF NOT EXISTS (SELECT 1
               FROM   PIMS_PROPERTY_HIST
               WHERE  PROPERTY_CLASSIFICATION_TYPE_CODE IS NULL
                  AND EFFECTIVE_DATE_HIST               IS NULL)
  BEGIN
  ALTER TABLE [dbo].[PIMS_PROPERTY_HIST]
    ALTER COLUMN PROPERTY_CLASSIFICATION_TYPE_CODE nvarchar(20) NOT NULL;
  END
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
