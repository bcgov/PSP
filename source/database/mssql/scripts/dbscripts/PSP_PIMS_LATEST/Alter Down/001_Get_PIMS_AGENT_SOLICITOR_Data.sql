/*******************************************************************************
Get the current rows from PIMS_DISPOSITION_SALE to copy to PIMS_DSP_PURCH_AGENT 
and PIMS_DSP_PURCH_SOLICITOR pre-Alter_Up.
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

-- Drop the existing temporary table TMP_PIMS_DISPOSITION_SALE
DROP TABLE IF EXISTS TMP_PIMS_DISPOSITION_SALE
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table TMP_PIMS_DISPOSITION_SALE
CREATE TABLE TMP_PIMS_DISPOSITION_SALE (
  DISPOSITION_SALE_ID    BIGINT,
  DSP_PURCH_AGENT_ID     BIGINT,
  DSP_PURCH_SOLICITOR_ID BIGINT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data into TMP_PIMS_DISPOSITION_SALE
INSERT INTO TMP_PIMS_DISPOSITION_SALE (DISPOSITION_SALE_ID, DSP_PURCH_AGENT_ID, DSP_PURCH_SOLICITOR_ID)
  SELECT DISPOSITION_SALE_ID
       , DSP_PURCH_AGENT_ID
       , DSP_PURCH_SOLICITOR_ID
  FROM   PIMS_DISPOSITION_SALE
  WHERE  DSP_PURCH_AGENT_ID     IS NOT NULL
     OR  DSP_PURCH_SOLICITOR_ID IS NOT NULL
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
