/*******************************************************************************
Obtain the current value for the PIMS_ACQUISITION_FILE_PERSON_ID_SEQ sequence to 
apply to the PIMS_ACQUISITION_FILE_TEAM_ID_SEQ sequence pre-Alter-Up.
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
GO

-- Drop the existing temporary table for TMP_ACQUISITION_FILE_SEQ_VAL
DROP TABLE IF EXISTS [dbo].[TMP_ACQUISITION_FILE_SEQ_VAL] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for TMP_ACQUISITION_FILE_SEQ_VAL
CREATE TABLE [dbo].[TMP_ACQUISITION_FILE_SEQ_VAL] (
    [SEQ_VAL] BIGINT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data for PIMS_ACQUISITION_FILE_PERSON_ID_SEQ
INSERT INTO [dbo].[TMP_ACQUISITION_FILE_SEQ_VAL] (SEQ_VAL)
  SELECT CONVERT(BIGINT, CURRENT_VALUE)
  FROM   SYS.SEQUENCES
  WHERE  NAME = 'PIMS_ACQUISITION_FILE_PERSON_ID_SEQ'
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
