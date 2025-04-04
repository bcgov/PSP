-- File generated on 12/17/2024 10:35:16 AM.
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

   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/002_PSP_PIMS_Alter_Up.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/002_PSP_PIMS_Alter_Up.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/010_DML_PIMS_STATIC_VARIABLE_VERSION.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/010_DML_PIMS_STATIC_VARIABLE_VERSION.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/159_DML_PIMS_ACQ_FILE_PROGESS_TYPE.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/159_DML_PIMS_ACQ_FILE_PROGESS_TYPE.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/160_DML_PIMS_ACQ_FILE_APPRAISAL_TYPE.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/160_DML_PIMS_ACQ_FILE_APPRAISAL_TYPE.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/161_DML_PIMS_ACQ_FILE_LGL_SRVY_TYPE.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/161_DML_PIMS_ACQ_FILE_LGL_SRVY_TYPE.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/162_DML_PIMS_ACQ_FILE_TAKE_TYPE.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/162_DML_PIMS_ACQ_FILE_TAKE_TYPE.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/163_DML_PIMS_ACQ_FILE_EXPROP_RISK_TYPE.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/163_DML_PIMS_ACQ_FILE_EXPROP_RISK_TYPE.sql"
   :r $(filepath)
   PRINT '- Executing PSP_PIMS_S96_00/Alter Up/164_DML_PIMS_ROLE_NAME.sql '
   :setvar filepath "PSP_PIMS_S96_00/Alter Up/164_DML_PIMS_ROLE_NAME.sql"
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
