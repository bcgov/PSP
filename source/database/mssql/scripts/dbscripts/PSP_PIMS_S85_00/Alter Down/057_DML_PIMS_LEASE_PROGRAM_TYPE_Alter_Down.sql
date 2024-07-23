/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_PROGRAM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-11  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable new types
PRINT N'Disable new types'
GO
UPDATE PIMS_LEASE_PROGRAM_TYPE
SET    IS_DISABLED = 1
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PROGRAM_TYPE_CODE IN (N'PUBTRANS', N'ENGINEER', N'MOTIUSE', N'AGRIC', N'RAIL', N'PARKING');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable various types
PRINT N'Disable various types'
GO
UPDATE PIMS_LEASE_PROGRAM_TYPE
SET    IS_DISABLED                = 0
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_PROGRAM_TYPE_CODE IN (N'BCFERRIES', N'BCTRANSIT', N'BELLETERM', N'TRANSLINK');
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the description for the "LCLGOVT" type
PRINT N'Update the description for the "LCLGOVT" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LCLGOVT'

SELECT LEASE_PROGRAM_TYPE_CODE
FROM   PIMS_LEASE_PROGRAM_TYPE
WHERE  LEASE_PROGRAM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_PROGRAM_TYPE
  SET    DESCRIPTION                = N'Local Government/RCMP'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PROGRAM_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the description for the "OTHER" type
PRINT N'Update the description for the "OTHER" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'OTHER'

SELECT LEASE_PROGRAM_TYPE_CODE
FROM   PIMS_LEASE_PROGRAM_TYPE
WHERE  LEASE_PROGRAM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_PROGRAM_TYPE
  SET    DESCRIPTION                = N'Other Licencing'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PROGRAM_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the description for the "TMEP" type
PRINT N'Update the description for the "TMEP" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'TMEP'

SELECT LEASE_PROGRAM_TYPE_CODE
FROM   PIMS_LEASE_PROGRAM_TYPE
WHERE  LEASE_PROGRAM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_PROGRAM_TYPE
  SET    DESCRIPTION                = N'TMEP'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_PROGRAM_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE tbl
SET    tbl.DISPLAY_ORDER              = seq.ROW_NUM
     , tbl.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_PROGRAM_TYPE tbl JOIN
       (SELECT LEASE_PROGRAM_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_PROGRAM_TYPE) seq  ON seq.LEASE_PROGRAM_TYPE_CODE = tbl.LEASE_PROGRAM_TYPE_CODE
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
