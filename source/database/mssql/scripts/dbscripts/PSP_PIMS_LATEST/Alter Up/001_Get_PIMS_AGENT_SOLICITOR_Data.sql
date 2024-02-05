/*******************************************************************************
Get the current rows from PIMS_DSP_PURCH_AGENT and PIMS_DSP_PURCH_SOLICITOR to 
copy to PIMS_DISPOSITION_SALE pre-Alter_Up.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-29  Original version.
*******************************************************************************/

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_DSP_PURCH_AGENT
DROP TABLE IF EXISTS TMP_PIMS_DSP_PURCH_AGENT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table TMP_PIMS_DSP_PURCH_AGENT
CREATE TABLE TMP_PIMS_DSP_PURCH_AGENT (
  DSP_PURCH_AGENT_ID  BIGINT,
  DISPOSITION_SALE_ID BIGINT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_DSP_PURCH_SOLICITOR
DROP TABLE IF EXISTS TMP_PIMS_DSP_PURCH_SOLICITOR
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table TMP_PIMS_DSP_PURCH_SOLICITOR
CREATE TABLE TMP_PIMS_DSP_PURCH_SOLICITOR (
  DSP_PURCH_SOLICITOR_ID BIGINT,
  DISPOSITION_SALE_ID    BIGINT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data from PIMS_DSP_PURCH_AGENT
INSERT INTO TMP_PIMS_DSP_PURCH_AGENT (DSP_PURCH_AGENT_ID, DISPOSITION_SALE_ID)
  SELECT DSP_PURCH_AGENT_ID
       , DISPOSITION_SALE_ID
  FROM   PIMS_DSP_PURCH_AGENT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data from PIMS_DSP_PURCH_SOLICITOR
INSERT INTO TMP_PIMS_DSP_PURCH_SOLICITOR (DSP_PURCH_SOLICITOR_ID, DISPOSITION_SALE_ID)
  SELECT DSP_PURCH_SOLICITOR_ID
       , DISPOSITION_SALE_ID
  FROM   PIMS_DSP_PURCH_SOLICITOR
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
