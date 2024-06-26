DELETE
FROM   PIMS_DISPOSITION_PURCHASER
GO

INSERT INTO dbo.PIMS_DISPOSITION_PURCHASER(DISPOSITION_SALE_ID, PERSON_ID, ORGANIZATION_ID, PRIMARY_CONTACT_ID, IS_DISABLED, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY)
  VALUES
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    4, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),   12, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),   14, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    4, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),   15, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    4, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    6, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    4, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    2, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()),    5, NULL, NULL, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    2,   11, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    1,   16, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    3,   11, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    5,    5, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    3,   13, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    4,   13, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    5,   10, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    2,   12, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    4,   17, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ((SELECT TOP 1 DISPOSITION_SALE_ID FROM PIMS_DISPOSITION_SALE ORDER BY NEWID()), NULL,    3,    1, 0, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER');
GO
