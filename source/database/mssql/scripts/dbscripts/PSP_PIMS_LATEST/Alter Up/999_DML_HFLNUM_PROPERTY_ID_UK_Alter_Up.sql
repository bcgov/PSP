/* -----------------------------------------------------------------------------
Create a unique index for the PIMS_HISTORICAL_FILE_NUMBER table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-May-21  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create unique index dbo.HFLNUM_PROPERTY_ID_UK
PRINT N'Create unique index dbo.HFLNUM_PROPERTY_ID_UK'
GO
CREATE UNIQUE NONCLUSTERED INDEX [HFLNUM_PROPERTY_ID_UK] 
    ON [dbo].[PIMS_HISTORICAL_FILE_NUMBER] ([PROPERTY_ID] ASC,[HISTORICAL_FILE_NUMBER_TYPE_CODE] ASC,[HISTORICAL_FILE_NUMBER] ASC,[OTHER_HIST_FILE_NUMBER_TYPE_CODE] ASC,[IS_DISABLED] ASC)
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
