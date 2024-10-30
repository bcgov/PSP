/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_CHKLST_SECTION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Aug-09  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Disable the "REFERAPPR" type
PRINT N'Disable the "REFERAPPR" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'REFERAPPR'

SELECT LEASE_CHKLST_SECTION_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_SECTION_TYPE
WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_SECTION_TYPE
  SET    IS_DISABLED                = 1
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/Enable the "APPRCOND" type
PRINT N'Insert/Enable the "APPRCOND" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'APPRCOND'

SELECT LEASE_CHKLST_SECTION_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_SECTION_TYPE
WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_SECTION_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_CHKLST_SECTION_TYPE (LEASE_CHKLST_SECTION_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
    VALUES (N'APPRCOND',  N'Approvals / Consultations', 2);
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
