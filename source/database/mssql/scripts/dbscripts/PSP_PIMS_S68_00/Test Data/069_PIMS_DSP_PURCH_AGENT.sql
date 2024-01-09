DELETE
FROM   PIMS_DSP_PURCH_AGENT
GO

INSERT INTO dbo.PIMS_DSP_PURCH_AGENT(DISPOSITION_SALE_ID, PERSON_ID, ORGANIZATION_ID, PRIMARY_CONTACT_ID, IS_DISABLED, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY)
  VALUES
    (10,   10, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (17,    9, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (18,   16, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (20,    3, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (15,    3, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (20,   15, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (23,    8, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (13,   13, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (21,   12, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (14,    2, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 6, NULL,    2,    7, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (17, NULL,    4,    2, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 1, NULL,    2,    1, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (13, NULL,    3,   15, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (23, NULL,    5,    1, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (23, NULL,    1,    7, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 9, NULL,    2,    4, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (10, NULL,    2,   16, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 6, NULL,    4,    7, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 9, NULL,    3,    6, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER');
GO
