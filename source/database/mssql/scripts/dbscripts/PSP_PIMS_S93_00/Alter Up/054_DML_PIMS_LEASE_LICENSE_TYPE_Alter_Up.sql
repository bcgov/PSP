/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_LICENSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-11  Initial version.
Doug Filteau  2024-Oct-21  Corrected DESCRPTION for OTHER code.
Doug Filteau  2024-Oct-30  Corrected DESCRPTION for LIPPUBHWY code.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Update the "LIPPUBHWY" type
PRINT N'Update the "LIPPUBHWY" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'LIPPUBHWY'

SELECT LEASE_LICENSE_TYPE_CODE
FROM   PIMS_LEASE_LICENSE_TYPE
WHERE  LEASE_LICENSE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_LICENSE_TYPE
  SET    DESCRIPTION                = N'Licence of Occupation of Provincial Public Highway'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_LICENSE_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_LICENSE_TYPE (LEASE_LICENSE_TYPE_CODE, DESCRIPTION)
    VALUES (N'LIPPUBHWY',  N'Licence of Occupation of Provincial Public Highway');
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
