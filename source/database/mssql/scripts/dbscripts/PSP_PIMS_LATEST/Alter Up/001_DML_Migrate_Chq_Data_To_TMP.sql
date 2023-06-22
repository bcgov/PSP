/* -----------------------------------------------------------------------------
Migrate the data from the PIMS_ACQ_PAYEE_CHEQUE table to the temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-08  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for PERSONs
DROP TABLE IF EXISTS [dbo].[TMP_ACQ_PAYEE_CHEQUE] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for PERSONs
CREATE TABLE [dbo].[TMP_ACQ_PAYEE_CHEQUE] (
    [COMPENSATION_REQUISITION_ID] BIGINT,
    [GST_NUMBER]                  NVARCHAR(50),
    [IS_PAYMENT_IN_TRUST]         BIT
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for TMP_ACQ_PAYEE_CHEQUE
INSERT INTO [dbo].[TMP_ACQ_PAYEE_CHEQUE] (COMPENSATION_REQUISITION_ID, GST_NUMBER, IS_PAYMENT_IN_TRUST)
SELECT pay.COMPENSATION_REQUISITION_ID
     , chq.GST_NUMBER
     , chq.IS_PAYMENT_IN_TRUST
FROM   PIMS_ACQUISITION_PAYEE pay JOIN
       PIMS_ACQ_PAYEE_CHEQUE  chq ON chq.ACQUISITION_PAYEE_ID = pay.ACQUISITION_PAYEE_ID
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
