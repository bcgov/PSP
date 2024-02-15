/*******************************************************************************
Delete rows of test data from PIMS_DSP_PURCH_AGENT and PIMS_DSP_PURCH_SOLICITOR
that are duplicate Agents and Solicitors to a Disposition Sale.  This state will 
not occur in Production as Disposition Sales are not present in Production.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-31  Original version.
*******************************************************************************/

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete the duplicate test rows in PIMS_DSP_PURCH_AGENT
WITH CTE AS 
  (SELECT *
          , ROW_NUMBER() OVER (PARTITION BY DISPOSITION_SALE_ID ORDER BY DISPOSITION_SALE_ID, DSP_PURCH_AGENT_ID) AS DupRow
   FROM     PIMS_DSP_PURCH_AGENT)
DELETE FROM CTE WHERE DupRow <> 1
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete the duplicate test rows in PIMS_DSP_PURCH_SOLICITOR
WITH CTE AS 
  (SELECT *
          , ROW_NUMBER() OVER (PARTITION BY DISPOSITION_SALE_ID ORDER BY DISPOSITION_SALE_ID, DSP_PURCH_SOLICITOR_ID) AS DupRow
   FROM     PIMS_DSP_PURCH_SOLICITOR)
DELETE FROM CTE WHERE DupRow <> 1
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
