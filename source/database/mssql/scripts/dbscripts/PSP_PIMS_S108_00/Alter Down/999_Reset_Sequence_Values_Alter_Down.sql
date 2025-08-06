/* -----------------------------------------------------------------------------
Set the sequence values for the sequences based on the current maximum value of 
the primary key.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-25  Initial version.
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
-- Reset the dbo.PIMS_PROP_ACT_INVOLVED_PARTY_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the dbo.PIMS_PROP_ACT_INVOLVED_PARTY_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROP_ACT_INVOLVED_PARTY_HIST_ID) + 1
FROM   dbo.PIMS_PROP_ACT_INVOLVED_PARTY_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_ACT_INVOLVED_PARTY_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the dbo.PIMS_PROP_ACT_MIN_CONTACT_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the dbo.PIMS_PROP_ACT_MIN_CONTACT_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROP_ACT_MIN_CONTACT_HIST_ID) + 1
FROM   dbo.PIMS_PROP_ACT_MIN_CONTACT_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_ACT_MIN_CONTACT_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROP_ACTIVITY_MGMT_ACTIVITY_HIST_ID) + 1
FROM   dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROPERTY_ACTIVITY_DOCUMENT_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_DOCUMENT_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROPERTY_ACTIVITY_DOCUMENT_HIST_ID) + 1
FROM   dbo.PIMS_PROPERTY_ACTIVITY_DOCUMENT_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROPERTY_ACTIVITY_DOCUMENT_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROPERTY_ACTIVITY_INVOICE_H_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_INVOICE_H_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(_PROPERTY_ACTIVITY_INVOICE_HIST_ID) + 1
FROM   dbo.PIMS_PROPERTY_ACTIVITY_INVOICE_HIST;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROPERTY_ACTIVITY_INVOICE_H_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROP_ACT_INVOLVED_PARTY_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROP_ACT_INVOLVED_PARTY_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PROP_ACT_INVOLVED_PARTY_ID) + 1
FROM   dbo.PIMS_PROP_ACT_INVOLVED_PARTY;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_ACT_INVOLVED_PARTY_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROP_ACT_MIN_CONTACT_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROP_ACT_MIN_CONTACT_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PROP_ACT_MIN_CONTACT_ID) + 1
FROM   dbo.PIMS_PROP_ACT_MIN_CONTACT;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_ACT_MIN_CONTACT_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROP_ACTVTY_MGMT_ACTVTY_TYP_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROP_ACTVTY_MGMT_ACTVTY_TYP_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PROP_ACTVTY_MGMT_ACTVTY_TYP_ID) + 1
FROM   dbo.PIMS_PROP_ACTIVITY_MGMT_ACTIVITY;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROP_ACTVTY_MGMT_ACTVTY_TYP_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROPERTY_ACTIVITY_DOCUMENT_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_DOCUMENT_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PROPERTY_ACTIVITY_DOCUMENT_ID) + 1
FROM   dbo.PIMS_PROPERTY_ACTIVITY_DOCUMENT;

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROPERTY_ACTIVITY_DOCUMENT_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- -----------------------------------------------------------------------------
-- Reset the PIMS_PROPERTY_ACTIVITY_INVOICE_ID_SEQ sequence.
-- -----------------------------------------------------------------------------
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_INVOICE_ID_SEQ sequence.'
GO
DECLARE @CurrVlu BIGINT
      , @qry     NVARCHAR(500);

SELECT @CurrVlu = MAX(PROPERTY_ACTIVITY_INVOICE_ID) + 1
FROM   dbo.PIMS_PROPERTY_ACTIVITY_INVOICE

SET @qry = 'ALTER SEQUENCE dbo.PIMS_PROPERTY_ACTIVITY_INVOICE_ID_SEQ RESTART WITH ' + CONVERT(NVARCHAR, @CurrVlu);
EXEC sp_executesql @qry;
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
