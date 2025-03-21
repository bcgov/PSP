DECLARE @ltsaOwnershipView BIGINT;

DECLARE @ltsaOwnershipAdd BIGINT;

DECLARE @ltsaOwnershipEdit BIGINT;

DECLARE @ltsaOwnershipDelete BIGINT;

SELECT
    @ltsaOwnershipView = CLAIM_ID
FROM
    PIMS_CLAIM
WHERE
    NAME = 'bctfa-ownership-view';

SELECT
    @ltsaOwnershipAdd = CLAIM_ID
FROM
    PIMS_CLAIM
WHERE
    NAME = 'bctfa-ownership-add';

SELECT
    @ltsaOwnershipEdit = CLAIM_ID
FROM
    PIMS_CLAIM
WHERE
    NAME = 'bctfa-ownership-edit';

SELECT
    @ltsaOwnershipDelete = CLAIM_ID
FROM
    PIMS_CLAIM
WHERE
    NAME = 'bctfa-ownership-delete';

DELETE FROM
    [dbo].[PIMS_ROLE_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipView;

DELETE FROM
    [dbo].[PIMS_ROLE_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipEdit;

DELETE FROM
    [dbo].[PIMS_ROLE_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipAdd;

DELETE FROM
    [dbo].[PIMS_ROLE_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipDelete;

DELETE FROM
    [dbo].[PIMS_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipView;

DELETE FROM
    [dbo].[PIMS_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipEdit;

DELETE FROM
    [dbo].[PIMS_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipAdd;

DELETE FROM
    [dbo].[PIMS_CLAIM]
WHERE
    CLAIM_ID = @ltsaOwnershipDelete;