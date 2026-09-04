/* -----------------------------------------------------------------------------
Update the Acquisition File OWNER_REP_COMMENT from the Interest Holder table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Eduardo H.    2026-Aug-09  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO


-- Temporarily disable the triggers on the table
PRINT N'Temporarily disable the triggers on the table'
GO
ALTER TABLE PIMS_ACQUISITION_FILE DISABLE TRIGGER PIMS_ACQNFL_I_S_U_TR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE PIMS_ACQUISITION_FILE DISABLE TRIGGER PIMS_ACQNFL_A_S_IUD_TR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE PIMS_ACQUISITION_FILE DISABLE TRIGGER PIMS_ACQNFL_I_S_I_TR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO


-- Migrate the TMP_PROPERTY_ACTIVITY data to the PIMS_PROPERTY_ACTIVITY table
PRINT N'Migrate the PIMS_INTEREST_HOLDER comment data to the PIMS_ACQUISITION_FILE table'
GO

  UPDATE acq
  SET acq.OWNER_REP_COMMENT = holder.COMMENT
  FROM [PIMS_ACQUISITION_FILE] as acq
  INNER JOIN [PIMS_INTEREST_HOLDER] as holder
    ON acq.ACQUISITION_FILE_ID = holder.ACQUISITION_FILE_ID
  WHERE holder.COMMENT IS NOT NULL AND holder.INTEREST_HOLDER_TYPE_CODE = 'AOREP';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO


-- Re-enable triggers on the table
PRINT N'Re-enable the triggers on the table'
GO
ALTER TABLE PIMS_ACQUISITION_FILE ENABLE TRIGGER PIMS_ACQNFL_I_S_U_TR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE PIMS_ACQUISITION_FILE ENABLE TRIGGER PIMS_ACQNFL_A_S_IUD_TR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE PIMS_ACQUISITION_FILE ENABLE TRIGGER PIMS_ACQNFL_I_S_I_TR
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

