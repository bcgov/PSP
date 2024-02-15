/*******************************************************************************
Copy the current rows from TMP_PIMS_DSP_PURCH_AGENT and 
TMP_PIMS_DSP_PURCH_SOLICITOR to PIMS_DISPOSITION_SALE post-Alter_Up.
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

-- Copy the existing data from TMP_PIMS_DSP_PURCH_AGENT
UPDATE sale
SET    sale.DSP_PURCH_AGENT_ID         = temp.DSP_PURCH_AGENT_ID
     , sale.CONCURRENCY_CONTROL_NUMBER = sale.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DISPOSITION_SALE    sale JOIN
       TMP_PIMS_DSP_PURCH_AGENT temp ON temp.DISPOSITION_SALE_ID = sale.DISPOSITION_SALE_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Copy the existing data from TMP_PIMS_DSP_PURCH_SOLICITOR
UPDATE sale
SET    sale.DSP_PURCH_SOLICITOR_ID     = temp.DSP_PURCH_SOLICITOR_ID
     , sale.CONCURRENCY_CONTROL_NUMBER = sale.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_DISPOSITION_SALE        sale JOIN
       TMP_PIMS_DSP_PURCH_SOLICITOR temp ON temp.DISPOSITION_SALE_ID = sale.DISPOSITION_SALE_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_DSP_PURCH_AGENT
DROP TABLE IF EXISTS TMP_PIMS_DSP_PURCH_AGENT
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table TMP_PIMS_DSP_PURCH_SOLICITOR
DROP TABLE IF EXISTS TMP_PIMS_DSP_PURCH_SOLICITOR
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
