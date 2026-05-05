-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_LEASE_LICENSE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Eduardo H.    2026-Apr-22  PSP-11377  Added MAJORWORX.
-- -------------------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Add/enable the MAJORWORX code.
PRINT N'Add/enable the MAJORWORX code.'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MAJORWORX'

SELECT LEASE_LICENSE_TYPE_CODE
FROM   PIMS_LEASE_LICENSE_TYPE
WHERE  LEASE_LICENSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_LICENSE_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_LICENSE_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_LICENSE_TYPE (LEASE_LICENSE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES  (N'MAJORWORX', N'Major Works Contract/Notice to Contractor', 0);
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prt
SET    prt.DISPLAY_ORDER              = seq.ROW_NUM
     , prt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_LICENSE_TYPE prt JOIN
       (SELECT LEASE_LICENSE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_LICENSE_TYPE
        WHERE  LEASE_LICENSE_TYPE_CODE <> N'OTHER') seq  ON seq.LEASE_LICENSE_TYPE_CODE = prt.LEASE_LICENSE_TYPE_CODE
WHERE  prt.LEASE_LICENSE_TYPE_CODE <> N'OTHER'
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_LEASE_LICENSE_TYPE
SET    DISPLAY_ORDER              = 999
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_LICENSE_TYPE_CODE = N'OTHER'
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
