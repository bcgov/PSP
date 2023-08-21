/* -----------------------------------------------------------------------------
Migrate the data from PIMS_PROP_PROP_ADJACENT_LAND_TYPE to the temporary table.
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

-- Drop the existing temporary table for PIMS_PROP_PROP_ADJACENT_LAND_TYPE
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for PIMS_PROPERTY
CREATE TABLE [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE] (
    [PROPERTY_ID]                      BIGINT,
    [PROPERTY_ADJACENT_LAND_TYPE_CODE] NVARCHAR(50))
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_FORM_8
INSERT INTO [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE] (PROPERTY_ID, PROPERTY_ADJACENT_LAND_TYPE_CODE)
SELECT PROPERTY_ID
     , PROPERTY_ADJACENT_LAND_TYPE_CODE
FROM   PIMS_PROP_PROP_ADJACENT_LAND_TYPE
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
