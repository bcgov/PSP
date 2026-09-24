-- -------------------------------------------------------------------------------------------
-- Alter the PIMS_MANAGEMENT_FILE_PROFILE_TYPE table.
-- . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
-- Author        Date         Ticket     Comment
-- ------------  -----------  ---------  -----------------------------------------------------
-- Arturo Reyes  2026-Sep-01  PSP-11805  Re-enable the PIMS key contact.
-- -------------------------------------------------------------------------------------------
SET XACT_ABORT ON;


GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;


GO
BEGIN TRANSACTION;


GO
IF @@ERROR <> 0
    SET NOEXEC ON;


GO
-- Add/enable the KEYCNTCT code.
PRINT N'Add/enable the KEYCNTCT code.';


GO
DECLARE @CurrCd AS NVARCHAR (20);

SET @CurrCd = N'KEYCNTCT';

SELECT MANAGEMENT_FILE_PROFILE_TYPE_CODE
FROM   PIMS_MANAGEMENT_FILE_PROFILE_TYPE
WHERE  MANAGEMENT_FILE_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
    UPDATE PIMS_MANAGEMENT_FILE_PROFILE_TYPE
    SET    IS_DISABLED                = 0,
           CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
    WHERE  MANAGEMENT_FILE_PROFILE_TYPE_CODE = @CurrCd;
ELSE
    INSERT  INTO PIMS_MANAGEMENT_FILE_PROFILE_TYPE (
        MANAGEMENT_FILE_PROFILE_TYPE_CODE,
        DESCRIPTION,
        DISPLAY_ORDER,
        IS_DISABLED
    )
    VALUES                                        (N'KEYCNTCT', N'PIMS key contact', 0, 0);


GO
IF @@ERROR <> 0
    SET NOEXEC ON;


GO
COMMIT TRANSACTION;


GO
IF @@ERROR <> 0
    SET NOEXEC ON;


GO
DECLARE @Success AS BIT;

SET @Success = 1;

SET NOEXEC OFF;

IF (@Success = 1)
    PRINT 'The database update succeeded';
ELSE
    BEGIN
        IF @@TRANCOUNT > 0
            ROLLBACK;
        PRINT 'The database update failed';
    END
GO