/* -----------------------------------------------------------------------------
Migrate the data to PIMS_PROP_PROP_ADJACENT_LAND_TYPE.
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

-- Migrate the Adjacent Land data to PIMS_PROPERTY
INSERT INTO PIMS_PROP_PROP_ADJACENT_LAND_TYPE (PROPERTY_ID, PROPERTY_ADJACENT_LAND_TYPE_CODE)
SELECT PROPERTY_ID
     , PROPERTY_ADJACENT_LAND_TYPE_CODE
FROM   TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE temporary table
--DROP TABLE IF EXISTS [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE] 
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

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
