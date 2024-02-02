/* -----------------------------------------------------------------------------
Restart the sequence numbers for the Acquisition File and Compensation 
Requisition numbers.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jan-31  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter sequence PIMS_ACQUISITION_FILE_NO_SEQ
PRINT N'Alter sequence PIMS_ACQUISITION_FILE_NO_SEQ'
GO
ALTER SEQUENCE [dbo].[PIMS_ACQUISITION_FILE_NO_SEQ]
	RESTART WITH 850000
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter sequence PIMS_COMPENSATION_REQUISITION_ID_SEQ
PRINT N'Alter sequence PIMS_COMPENSATION_REQUISITION_ID_SEQ'
GO
ALTER SEQUENCE [dbo].[PIMS_COMPENSATION_REQUISITION_ID_SEQ]
	RESTART WITH 100000
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

