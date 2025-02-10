/* -----------------------------------------------------------------------------
Alter the display order of the PIMS_TAKE_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-04  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/Enable the "HOLD" type
PRINT N'Update the "HOLD" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'HOLD'

SELECT TAKE_STATUS_TYPE_CODE
FROM   PIMS_TAKE_STATUS_TYPE
WHERE  TAKE_STATUS_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_TAKE_STATUS_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  TAKE_STATUS_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_TAKE_STATUS_TYPE (TAKE_STATUS_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
    VALUES (N'HOLD',  N'Hold', 4);
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the display order
PRINT N'Alter the display order'
GO
UPDATE PIMS_TAKE_STATUS_TYPE
SET    DISPLAY_ORDER = CASE TAKE_STATUS_TYPE_CODE
                         WHEN N'CANCELLED'  THEN 1
                         WHEN N'INPROGRESS' THEN 2
                         WHEN N'COMPLETE'   THEN 3
                         WHEN N'HOLD'       THEN 4
                       END
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
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
