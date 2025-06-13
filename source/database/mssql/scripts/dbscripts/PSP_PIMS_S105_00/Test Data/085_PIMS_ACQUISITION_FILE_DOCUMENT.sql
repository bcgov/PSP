DELETE FROM PIMS_ACQUISITION_FILE_DOCUMENT
GO

INSERT INTO PIMS_ACQUISITION_FILE_DOCUMENT (ACQUISITION_FILE_ID, DOCUMENT_ID, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY) 
  VALUES
    (20,  1, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (46,  2, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (37,  3, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (34,  6, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (31,  7, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (34,  8, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (47,  8, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (22,  9, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (58,  9, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 2, 10, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (41, 10, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 9, 12, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (48, 12, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 6, 14, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (32, 14, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (52, 14, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (19, 15, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (44, 15, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (54, 15, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (20, 18, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (29, 18, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (37, 18, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (55, 18, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (50, 19, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (14, 20, 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER');
GO
