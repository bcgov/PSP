/* -----------------------------------------------------------------------------
Alter the data in the PIMS_SUBFILE_INTEREST_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Dec-19  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update/insert the "TENANT" type
PRINT N'Update the "TENANT" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'TENANT'

SELECT SUBFILE_INTEREST_TYPE_CODE
FROM   PIMS_SUBFILE_INTEREST_TYPE
WHERE  SUBFILE_INTEREST_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_SUBFILE_INTEREST_TYPE
  SET    DESCRIPTION                = N'Tenant (Monthly)'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  SUBFILE_INTEREST_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_SUBFILE_INTEREST_TYPE (SUBFILE_INTEREST_TYPE_CODE, DESCRIPTION)
    VALUES (N'TENANT',  N'Tenant (Monthly)');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update/insert the "LSHOLDR" type
PRINT N'Update the "LSHOLDR" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LSHOLDR'

SELECT SUBFILE_INTEREST_TYPE_CODE
FROM   PIMS_SUBFILE_INTEREST_TYPE
WHERE  SUBFILE_INTEREST_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_SUBFILE_INTEREST_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  SUBFILE_INTEREST_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_SUBFILE_INTEREST_TYPE (SUBFILE_INTEREST_TYPE_CODE, DESCRIPTION)
    VALUES (N'LSHOLDR',  N'Leaseholder');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update/insert the "MOBILE" type
PRINT N'Update/insert the "MOBILE" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOBILE'

SELECT SUBFILE_INTEREST_TYPE_CODE
FROM   PIMS_SUBFILE_INTEREST_TYPE
WHERE  SUBFILE_INTEREST_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_SUBFILE_INTEREST_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  SUBFILE_INTEREST_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_SUBFILE_INTEREST_TYPE (SUBFILE_INTEREST_TYPE_CODE, DESCRIPTION)
    VALUES (N'MOBILE',  N'Mobile Home Owner');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prt
SET    prt.DISPLAY_ORDER              = seq.ROW_NUM
     , prt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_SUBFILE_INTEREST_TYPE prt JOIN
       (SELECT SUBFILE_INTEREST_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_SUBFILE_INTEREST_TYPE
        WHERE  SUBFILE_INTEREST_TYPE_CODE <> N'OTHER') seq  ON seq.SUBFILE_INTEREST_TYPE_CODE = prt.SUBFILE_INTEREST_TYPE_CODE
WHERE  prt.SUBFILE_INTEREST_TYPE_CODE <> N'OTHER'
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_SUBFILE_INTEREST_TYPE
SET    DISPLAY_ORDER              = 999
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  SUBFILE_INTEREST_TYPE_CODE = N'OTHER'
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
