/* -----------------------------------------------------------------------------
Create and populate the temporary TMP_PROPERTY_ACTIVITY table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-12  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the tempoarary table
PRINT N'Create the tempoarary table'
GO
CREATE TABLE dbo.TMP_PROPERTY_ACTIVITY (
    PROPERTY_ACTIVITY_ID            BIGINT,
    PROP_MGMT_ACTIVITY_TYPE_CODE    NVARCHAR(20),
    PROP_MGMT_ACTIVITY_SUBTYPE_CODE NVARCHAR(20))
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the PIMS_PROP_ACTIVITY_MGMT_ACTIVITY data to the TMP_COMP_REQ_PAYEE table
PRINT N'Migrate the PIMS_PROP_ACTIVITY_MGMT_ACTIVITY data to the TMP_COMP_REQ_PAYEE table'
GO
INSERT INTO dbo.TMP_PROPERTY_ACTIVITY (PROPERTY_ACTIVITY_ID, PROP_MGMT_ACTIVITY_TYPE_CODE, PROP_MGMT_ACTIVITY_SUBTYPE_CODE)
  SELECT PIMS_PROPERTY_ACTIVITY_ID
       , PROP_MGMT_ACTIVITY_TYPE_CODE
       , PROP_MGMT_ACTIVITY_SUBTYPE_CODE
  FROM   dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY
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
