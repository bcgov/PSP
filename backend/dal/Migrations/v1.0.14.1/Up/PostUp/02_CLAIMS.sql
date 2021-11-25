declare @sysadmin bigint,@rem bigint,@rea bigint;
SELECT @sysadmin=ROLE_ID FROM PIMS_ROLE WHERE NAME = 'System Administrator';
SELECT @rem=ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Real Estate Manager';
SELECT @rea=ROLE_ID FROM PIMS_ROLE WHERE NAME = 'Real Estate Analyst';

INSERT INTO
    dbo.[PIMS_CLAIM] ([CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [Description], [Is_Disabled])
VALUES
(
    '57748ff6-6fe9-4f54-89ad-003dcb8e6551',
    'contact-view',
    '57748ff6-6fe9-4f54-89ad-003dcb8e6551',
    'Ability view existing contacts.',
    0
), (
    '8ff7ca03-0b66-4078-b347-5d11807e2a49',
    'contact-add',
    '8ff7ca03-0b66-4078-b347-5d11807e2a49',
    'Ability to add new contacts.',
    0
), (
    '8ff39453-9628-4d25-b1dd-12485f688e9d',
    'contact-edit',
    '8ff39453-9628-4d25-b1dd-12485f688e9d',
    'Ability to edit existing contacts.',
    0
), (
    '4c82ebbf-d04e-45bb-a1b2-6ea13d855b47',
    'contact-delete',
    '4c82ebbf-d04e-45bb-a1b2-6ea13d855b47',
    'Ability to delete contacts.',
    0
);

declare @view bigint,@add bigint,@edit bigint,@delete bigint;
SELECT @view=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-view';
SELECT @add=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-add';
SELECT @edit=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-edit';
SELECT @delete=CLAIM_ID FROM PIMS_CLAIM WHERE NAME = 'contact-delete';


INSERT INTO dbo.[PIMS_ROLE_CLAIM]
    (
    [Role_Id]
    , [Claim_Id]
    )
VALUES
(
    @sysadmin
    , @view   -- system admin, contact-view
),(
    @sysadmin
    , @add  -- system admin, contact-add
),(
    @sysadmin
    , @edit   -- system admin, contact-edit
),(
    @sysadmin
    , @delete   -- system admin, contact-delete
),(
    @rem
    , @view   -- real estate manager, contact-view
),(
    @rem
    , @add   -- real estate manager, contact-add
),(
    @rem
    , @edit   -- real estate manager, contact-edit
),(
    @rea
    , @view   -- real estate analyst, contact-view
)