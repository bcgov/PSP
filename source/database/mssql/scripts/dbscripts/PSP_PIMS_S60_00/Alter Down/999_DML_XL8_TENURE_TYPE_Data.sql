/* -----------------------------------------------------------------------------
Translate the data in PIMS_PROP_PROP_TENURE_TYPE to the new values
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

-- Migrate the Payee data to PIMS_COMPENSATION_REQUISITION
UPDATE PIMS_PROP_PROP_TENURE_TYPE
SET    PROPERTY_TENURE_TYPE_CODE  = CASE PROPERTY_TENURE_TYPE_CODE
                                      WHEN N'FSBCTFA'  THEN N'TT'
                                      WHEN N'FSMOTI'   THEN N'TM'
                                      WHEN N'SRWOTHER' THEN N'RW'
                                      ELSE PROPERTY_TENURE_TYPE_CODE
                                    END
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing TMP_PIMS_ACQUISITION_PAYEE temporary table
DROP TABLE IF EXISTS [dbo].[TMP_PIMS_ACQUISITION_PAYEE] 
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
