/* -----------------------------------------------------------------------------
Create TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Aug-03  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE
CREATE TABLE [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE] (
    [PROPERTY_ID]                      BIGINT,
    [PROPERTY_ADJACENT_LAND_TYPE_CODE] NVARCHAR(50))
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
