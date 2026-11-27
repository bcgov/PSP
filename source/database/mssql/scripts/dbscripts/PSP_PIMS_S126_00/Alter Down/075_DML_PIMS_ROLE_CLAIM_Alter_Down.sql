-- Revoke agreement viewing from the Disposition functional role.
-- Rollback removes this assignment, including if it existed before Alter Up.
-- Synchronize PIMS roles to Keycloak after rollback.
SET XACT_ABORT ON;
BEGIN TRANSACTION;

DELETE rc
FROM dbo.PIMS_ROLE_CLAIM AS rc
INNER JOIN dbo.PIMS_ROLE AS r ON r.ROLE_ID = rc.ROLE_ID
INNER JOIN dbo.PIMS_CLAIM AS c ON c.CLAIM_ID = rc.CLAIM_ID
WHERE r.NAME = N'Disposition functional'
  AND c.NAME = N'agreement-view';

COMMIT TRANSACTION;
GO
