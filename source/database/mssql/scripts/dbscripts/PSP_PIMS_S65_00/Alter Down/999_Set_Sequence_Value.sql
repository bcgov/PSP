/*******************************************************************************
Set the current value of the PIMS_ACQUISITION_FILE_PERSON_ID_SEQ sequence to the 
PIMS_ACQUISITION_FILE_TEAM_ID_SEQ sequence post-Alter_Down.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Oct-11  Original version.
*******************************************************************************/

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON

-- Copy the existing data for PIMS_ACQUISITION_FILE_PERSON_ID_SEQ
DECLARE @CurrVal BIGINT;
DECLARE @ExecStr NVARCHAR(1000);

SET @CurrVal = (SELECT SEQ_VAL FROM TMP_ACQUISITION_FILE_SEQ_VAL);
SET @ExecStr = N'ALTER SEQUENCE PIMS_ACQUISITION_FILE_PERSON_ID_SEQ RESTART WITH ' + CAST(@CurrVal AS NVARCHAR(10));

EXEC(@ExecStr);
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for TMP_ACQUISITION_FILE_SEQ_VAL
DROP TABLE IF EXISTS [dbo].[TMP_ACQUISITION_FILE_SEQ_VAL] 
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
