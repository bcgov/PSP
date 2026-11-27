-- Grant agreement viewing to the Disposition functional role.
-- After applying, synchronize PIMS roles to Keycloak and verify refreshed user permissions.
SET XACT_ABORT ON;

SET TRANSACTION ISOLATION LEVEL SERIALIZABLE;

BEGIN TRANSACTION;

DECLARE @agreementView AS BIGINT;

DECLARE @seedTime AS DATETIME = GETUTCDATE();

DECLARE @appUserGuid AS UNIQUEIDENTIFIER = NEWID();

SELECT @agreementView = CLAIM_ID
FROM   dbo.PIMS_CLAIM
WHERE  NAME = N'agreement-view';

IF @agreementView IS NULL
    THROW 50000, 'Required claim agreement-view was not found.', 1;

IF NOT EXISTS (SELECT 1
               FROM   dbo.PIMS_ROLE
               WHERE  NAME = N'Disposition functional')
    THROW 50000, 'Disposition functional role must exist before granting agreement-view.', 1;

-- Enable an existing disabled assignment without changing an enabled assignment.
-- UPDATE rc
-- SET IS_DISABLED = 0,
--     CONCURRENCY_CONTROL_NUMBER = rc.CONCURRENCY_CONTROL_NUMBER + 1,
--     APP_LAST_UPDATE_TIMESTAMP = @seedTime,
--     APP_LAST_UPDATE_USERID = N'SEED',
--     APP_LAST_UPDATE_USER_GUID = @appUserGuid,
--     APP_LAST_UPDATE_USER_DIRECTORY = N''
-- FROM dbo.PIMS_ROLE_CLAIM AS rc
-- INNER JOIN dbo.PIMS_ROLE AS r ON r.ROLE_ID = rc.ROLE_ID
-- WHERE r.NAME = N'Disposition functional'
--   AND rc.CLAIM_ID = @agreementView
--   AND rc.IS_DISABLED = 1;
-- Insert the assignment if it does not already exist.
INSERT INTO dbo.PIMS_ROLE_CLAIM (
    ROLE_ID,
    CLAIM_ID,
    IS_DISABLED,
    CONCURRENCY_CONTROL_NUMBER,
    APP_CREATE_TIMESTAMP,
    APP_CREATE_USERID,
    APP_CREATE_USER_GUID,
    APP_CREATE_USER_DIRECTORY,
    APP_LAST_UPDATE_TIMESTAMP,
    APP_LAST_UPDATE_USERID,
    APP_LAST_UPDATE_USER_GUID,
    APP_LAST_UPDATE_USER_DIRECTORY
)
SELECT r.ROLE_ID,
       @agreementView,
       0,
       1,
       @seedTime,
       N'SEED',
       @appUserGuid,
       N'',
       @seedTime,
       N'SEED',
       @appUserGuid,
       N''
FROM   dbo.PIMS_ROLE AS r
WHERE  r.NAME = N'Disposition functional'
       AND NOT EXISTS (SELECT 1
                       FROM   dbo.PIMS_ROLE_CLAIM AS rc
                       WHERE  rc.ROLE_ID = r.ROLE_ID
                              AND rc.CLAIM_ID = @agreementView);

COMMIT TRANSACTION;