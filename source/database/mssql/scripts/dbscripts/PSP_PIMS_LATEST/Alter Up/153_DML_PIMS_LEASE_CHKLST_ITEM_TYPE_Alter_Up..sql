/* -----------------------------------------------------------------------------
Alter the data in the PIMS_LEASE_CHKLST_ITEM_TYPE table.
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

-- Disable "REFERAPPR" section items
PRINT N'Disable "REFERAPPR" section items'
GO
UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
SET    IS_DISABLED                = 1
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  LEASE_CHKLST_SECTION_TYPE_CODE = N'REFERAPPR';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/Enable the "RELCONSOBTND" type
PRINT N'Insert/Enable the "RELCONSOBTND" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'RELCONSOBTND'

SELECT LEASE_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_ITEM_TYPE
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_CHKLST_ITEM_TYPE (LEASE_CHKLST_SECTION_TYPE_CODE, LEASE_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, HINT, DISPLAY_ORDER, IS_DISABLED)
    VALUES (N'APPRCOND', N'RELCONSOBTND', N'Relevant consultations conducted and approvals obtained', N'Applicable consultations / approvals are done (i.e. district, engineering, first nations etc.)', 7, 0);
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert/Enable the "DETLSRECRDED" type
PRINT N'Insert/Enable the "DETLSRECRDED" type'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'DETLSRECRDED'

SELECT LEASE_CHKLST_ITEM_TYPE_CODE
FROM   PIMS_LEASE_CHKLST_ITEM_TYPE
WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_LEASE_CHKLST_ITEM_TYPE
  SET    IS_DISABLED                = 0
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  LEASE_CHKLST_ITEM_TYPE_CODE = @CurrCd;
ELSE
  INSERT INTO PIMS_LEASE_CHKLST_ITEM_TYPE (LEASE_CHKLST_SECTION_TYPE_CODE, LEASE_CHKLST_ITEM_TYPE_CODE, DESCRIPTION, HINT, DISPLAY_ORDER, IS_DISABLED)
    VALUES (N'APPRCOND', N'DETLSRECRDED', N'Approval / consultation details and documents have been recorded for each instance', N'Appropriate details are recorded in the file and any relevant documentation has been uploaded', 8, 0);
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
