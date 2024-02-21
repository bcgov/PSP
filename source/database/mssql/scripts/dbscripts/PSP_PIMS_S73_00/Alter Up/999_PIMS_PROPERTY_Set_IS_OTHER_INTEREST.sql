-- ----------------------------------------------------------------------------------
-- Initialize the PIMS_PROPERTY.IS_OTHER_INTEREST column.
--
-- Author        Date         Comment
-- ------------  -----------  -------------------------------------------------------
-- Doug Filteau  2025-Jan-26  Initial version.
-- ----------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Initialize PIMS_PROPERTY.IS_OTHER_INTEREST
PRINT N'Initialize PIMS_PROPERTY.IS_OTHER_INTEREST'
GO
UPDATE PIMS_PROPERTY
SET    IS_OTHER_INTEREST          = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  IS_PROPERTY_OF_INTEREST = CONVERT([bit],(0))
   AND IS_OWNED                = CONVERT([bit],(0))
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
