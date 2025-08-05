/* -----------------------------------------------------------------------------
Set the sequence values for the sequences based on the current maximum value of 
the primary key.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-28  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROPERTY_ACTIVITY_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PIMS_PROPERTY_ACTIVITY_ID) + 1
FROM   dbo.PIMS_PROPERTY_ACTIVITY;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROPERTY_ACTIVITY_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROPERTY_ACTIVITY_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROPERTY_ACTIVITY_HIST_ID) + 1
FROM   dbo.PIMS_PROPERTY_ACTIVITY_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROPERTY_ACTIVITY_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROP_PROP_ACTIVITY_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROP_PROP_ACTIVITY_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PROP_PROP_ACTIVITY_ID) + 1
FROM   dbo.PIMS_PROP_PROP_ACTIVITY;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_PROP_ACTIVITY_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROP_PROP_ACTIVITY_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROP_PROP_ACTIVITY_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROP_PROP_ACTIVITY_HIST_ID) + 1
FROM   dbo.PIMS_PROP_PROP_ACTIVITY_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_PROP_ACTIVITY_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the PIMS_MANAGEMENT_ACTIVITY_ID_SEQ sequence.
PRINT N'Drop the PIMS_MANAGEMENT_ACTIVITY_ID_SEQ sequence.'
DROP SEQUENCE IF EXISTS PIMS_MANAGEMENT_ACTIVITY_ID_SEQ
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the PIMS_MANAGEMENT_ACTIVITY_PROPERTY_ID_SEQ sequence.
PRINT N'Drop the PIMS_MANAGEMENT_ACTIVITY_PROPERTY_ID_SEQ sequence.'
DROP SEQUENCE IF EXISTS PIMS_MANAGEMENT_ACTIVITY_PROPERTY_ID_SEQ
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
