/* -----------------------------------------------------------------------------
 Alter the data in the PIMS_FORM_TYPE table.
 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
 Author        Date         Comment
 ------------  -----------  -----------------------------------------------------
 Doug Filteau  2024-Jun-06  Initial version
 ----------------------------------------------------------------------------- */
SET
    XACT_ABORT ON
GO
SET
    TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
    BEGIN TRANSACTION
GO
    IF @@ERROR <> 0
SET
    NOEXEC ON
GO
    -- Insert the "H1005" type
    DECLARE @CurrCd NVARCHAR(20)
SET
    @CurrCd = N'H1005'
SELECT
    FORM_TYPE_CODE
FROM
    PIMS_FORM_TYPE
WHERE
    FORM_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0 BEGIN
INSERT INTO
    PIMS_FORM_TYPE (FORM_TYPE_CODE, DESCRIPTION)
VALUES
    (
        N'H1005',
        N'Licence of Occupation for Provincial Public Highway (H-1005)'
    );

END
GO
    IF @@ERROR <> 0
SET
    NOEXEC ON
GO
    COMMIT TRANSACTION
GO
    IF @@ERROR <> 0
SET
    NOEXEC ON
GO
    DECLARE @Success AS BIT
SET
    @Success = 1
SET
    NOEXEC OFF IF (@Success = 1) PRINT 'The database update succeeded'
    ELSE BEGIN IF @ @TRANCOUNT > 0 ROLLBACK TRANSACTION PRINT 'The database update failed'
END
GO