-- File generated on 03/11/2025 04:35:44 PM.
-- Autogenerated file. Do not manually modify.

:On Error Exit
SET XACT_ABORT ON
GO

PRINT '     == DB SCRIPT START ========'
GO

BEGIN TRANSACTION
PRINT '     == DB TRANSACTION START ========'
   GO

   --Script section

   PRINT '- Executing PSP_PIMS_S101_00/Alter Up/002_PSP_PIMS_Alter_Up.sql '
   :setvar filepath "PSP_PIMS_S101_00/Alter Up/002_PSP_PIMS_Alter_Up.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S101_00/Alter Up/004_PSP_PIMS_PMBC_Tables_Alter_Up.sql '
   :setvar filepath "PSP_PIMS_S101_00/Alter Up/004_PSP_PIMS_PMBC_Tables_Alter_Up.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S101_00/Alter Up/010_DML_PIMS_STATIC_VARIABLE_VERSION.sql '
   :setvar filepath "PSP_PIMS_S101_00/Alter Up/010_DML_PIMS_STATIC_VARIABLE_VERSION.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S101_00/Alter Up/76_DML_PIMS_CLAIMS.sql '
   :setvar filepath "PSP_PIMS_S101_00/Alter Up/76_DML_PIMS_CLAIMS.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S101_00/Alter Up/77_DML_PIMS_ROLE_CLAIM.sql '
   :setvar filepath "PSP_PIMS_S101_00/Alter Up/77_DML_PIMS_ROLE_CLAIM.sql"
   :r $(filepath)

   --End script section

PRINT '     == DB TRANSACTION FINISH ========'
GO

If XACT_STATE()=1
  BEGIN
  PRINT '- Success: Committing Transaction...'
  COMMIT TRANSACTION;
  END
ELSE IF XACT_STATE()=-1
  BEGIN
  PRINT '- Error: Rolling Back Transaction...'
  ROLLBACK TRANSACTION;
  END;
ELSE IF XACT_STATE()=0
  BEGIN
  PRINT '- Error: No pending transactions...'
  ROLLBACK TRANSACTION;
  END;
GO
