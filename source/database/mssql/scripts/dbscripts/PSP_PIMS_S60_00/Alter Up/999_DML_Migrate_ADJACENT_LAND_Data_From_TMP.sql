/* -----------------------------------------------------------------------------
Migrate the data to PIMS_PROPERTY.
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

-- First pass: Migrate the Adjacent Land data to PIMS_PROPERTY
INSERT INTO PIMS_PROP_PROP_TENURE_TYPE (PROPERTY_ID, PROPERTY_TENURE_TYPE_CODE)
SELECT PROPERTY_ID
     , CASE
         WHEN PROPERTY_ADJACENT_LAND_TYPE_CODE = N'MOLBCTFA'  THEN N'FSBCTFA'
         WHEN PROPERTY_ADJACENT_LAND_TYPE_CODE = N'MONLBCTFA' THEN N'FSBCTFA'
         WHEN PROPERTY_ADJACENT_LAND_TYPE_CODE = N'CROWN'     THEN N'FSCROWN'
       END
FROM   TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Second pass: Migrate the Adjacent Land data to PIMS_PROPERTY
INSERT INTO PIMS_PROP_PROP_TENURE_TYPE (PROPERTY_ID, PROPERTY_TENURE_TYPE_CODE)
SELECT PROPERTY_ID
     , N'LEASELIC'
FROM   TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE
WHERE  PROPERTY_ADJACENT_LAND_TYPE_CODE = 'MOLBCTFA'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing PIMS_PROP_PROP_TENURE_TYPE temporary table
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_PROP_PROP_ADJACENT_LAND_TYPE] 
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
