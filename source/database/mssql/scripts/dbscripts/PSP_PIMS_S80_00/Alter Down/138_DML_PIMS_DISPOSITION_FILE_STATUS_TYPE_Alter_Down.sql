/* -----------------------------------------------------------------------------
 Alter the data in the PIMS_DISPOSITION_FILE_STATUS_TYPE table.
 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
 Author        Date         Comment
 ------------  -----------  -----------------------------------------------------
 Doug Filteau  2024-May-01  Initial version.  Disable DRAFT.
 ----------------------------------------------------------------------------- */
SET
  XACT_ABORT ON
GO
SET
  TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
  BEGIN TRANSACTION
GO
  IF @@ERROR <> 0
SET
  NOEXEC ON
GO
  -- Disable the "DRAFT" type
  DECLARE @CurrCd NVARCHAR(20)
SET
  @CurrCd = N'DRAFT'
SELECT
  DISPOSITION_FILE_STATUS_TYPE_CODE
FROM
  PIMS_DISPOSITION_FILE_STATUS_TYPE
WHERE
  DISPOSITION_FILE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1 BEGIN
UPDATE
  PIMS_DISPOSITION_FILE_STATUS_TYPE
SET
  IS_DISABLED = 0,
  CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE
  DISPOSITION_FILE_STATUS_TYPE_CODE = @CurrCd;

END
GO
  IF @@ERROR <> 0
SET
  NOEXEC ON
GO
  COMMIT TRANSACTION
GO
  IF @@ERROR <> 0
SET
  NOEXEC ON
GO
  DECLARE @Success AS BIT
SET
  @Success = 1
SET
  NOEXEC OFF IF (@Success = 1) PRINT 'The database update succeeded'
  ELSE BEGIN IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION PRINT 'The database update failed'
END
GO