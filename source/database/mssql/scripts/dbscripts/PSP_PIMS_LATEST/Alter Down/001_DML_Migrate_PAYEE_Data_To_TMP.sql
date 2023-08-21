/* -----------------------------------------------------------------------------
Migrate the data from PIMS_ACQUISITION_PAYEE to the temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Aug-03  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for PIMS_ACQUISITION_PAYEE
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_ACQUISITION_PAYEE] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for the PIMS_ACQUISITION_PAYEE data
CREATE TABLE [dbo].[TMP_PIMS_ACQUISITION_PAYEE] (
    [COMPENSATION_REQUISITION_ID] BIGINT,
    [ACQUISITION_OWNER_ID]        BIGINT,
    [INTEREST_HOLDER_ID]          BIGINT,
    [ACQUISITION_FILE_PERSON_ID]  BIGINT,
    [GST_NUMBER]                  NVARCHAR(50),
    [IS_PAYMENT_IN_TRUST]         BIT)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PIMS_ACQUISITION_PAYEE
INSERT INTO [dbo].[TMP_PIMS_ACQUISITION_PAYEE] (COMPENSATION_REQUISITION_ID, ACQUISITION_OWNER_ID, INTEREST_HOLDER_ID, ACQUISITION_FILE_PERSON_ID, GST_NUMBER, IS_PAYMENT_IN_TRUST)
SELECT COMPENSATION_REQUISITION_ID
     , ACQUISITION_OWNER_ID
     , INTEREST_HOLDER_ID
     , ACQUISITION_FILE_PERSON_ID
     , GST_NUMBER
     , IS_PAYMENT_IN_TRUST
FROM   PIMS_COMPENSATION_REQUISITION
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
