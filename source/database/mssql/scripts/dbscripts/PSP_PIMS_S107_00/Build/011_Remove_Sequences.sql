/* -----------------------------------------------------------------------------
Drop the replaced sequences.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-14  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the PIMS_PROPERTY_ACTIVITY_ID_SEQ sequence.
PRINT N'Drop the PIMS_PROPERTY_ACTIVITY_ID_SEQ sequence.'
DROP SEQUENCE IF EXISTS PIMS_PROPERTY_ACTIVITY_ID_SEQ
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the PIMS_PROP_PROP_ACTIVITY_ID_SEQ sequence.
PRINT N'Drop the PIMS_PROP_PROP_ACTIVITY_ID_SEQ sequence.'
DROP SEQUENCE IF EXISTS PIMS_PROP_PROP_ACTIVITY_ID_SEQ
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
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
