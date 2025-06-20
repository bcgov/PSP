DELETE
FROM   PIMS_DISPOSITION_OFFER
GO

INSERT INTO dbo.PIMS_DISPOSITION_OFFER(DISPOSITION_FILE_ID, DISPOSITION_OFFER_STATUS_TYPE_CODE, OFFER_NAME, OFFER_DT, OFFER_EXPIRY_DT, OFFER_AMT, OFFER_NOTE, CONCURRENCY_CONTROL_NUMBER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_GUID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_GUID, APP_LAST_UPDATE_USER_DIRECTORY)
  VALUES
    ( 1, N'OPEN',      N'OFFER_NAME 74EA301', '2000-07-25', '2001-10-19', 10358.69, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 1, N'REJECTED',  N'OFFER_NAME 946653F', '2030-11-03', '2031-01-09', 14303.82, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 7, N'ACCCEPTED', N'OFFER_NAME 9C44730', '2045-08-10', '2046-12-09',  7259.68, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 3, N'OPEN',      N'OFFER_NAME 998ACB8', '2000-02-17', '2001-02-01',  6873.84, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 6, N'COLLAPSED', N'OFFER_NAME 1DA2325', '2067-02-01', '2068-02-13',  2354.72, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 2, N'ACCCEPTED', N'OFFER_NAME D6960F4', '2027-11-04', '2028-11-06',  6745.07, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 5, N'COUNTERED', N'OFFER_NAME 0E23B9A', '2025-02-11', '2026-08-21',  7961.90, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 2, N'REJECTED',  N'OFFER_NAME 011DDAB', '1905-12-22', '1906-07-04',  4079.41, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 8, N'OPEN',      N'OFFER_NAME 3713E9C', '1964-07-31', '1965-04-18',  7308.79, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 4, N'REJECTED',  N'OFFER_NAME BC742A2', '2014-08-29', '2015-03-28',  7505.50, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 3, N'ACCCEPTED', N'OFFER_NAME EA7903E', '1942-12-12', '1943-11-04', 13469.08, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 3, N'ACCCEPTED', N'OFFER_NAME EDC5280', '2035-10-09', '2036-10-30',  6665.41, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 4, N'COUNTERED', N'OFFER_NAME C300C6B', '1996-09-21', '1997-11-10', 11864.58, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 2, N'REJECTED',  N'OFFER_NAME F35724B', '1960-10-31', '1961-04-10',  8080.46, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 7, N'ACCCEPTED', N'OFFER_NAME 5689EDA', '1962-06-09', '1962-12-29', 12687.63, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 8, N'OPEN',      N'OFFER_NAME FC5A81E', '2052-11-06', '2053-01-17',    11.67, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 8, N'OPEN',      N'OFFER_NAME A1F4D85', '1959-04-05', '1959-06-10',  5730.82, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 1, N'OPEN',      N'OFFER_NAME 599A8EE', '1913-02-01', '1914-04-26',  2219.33, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 9, N'COUNTERED', N'OFFER_NAME EC6AF75', '1981-11-16', '1982-05-08', 11053.50, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    (10, N'COLLAPSED', N'OFFER_NAME 462EF9D', '1910-11-25', '1911-09-14', 11761.82, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 2, N'ACCCEPTED', N'OFFER_NAME D8546D8', '1930-05-14', '1931-08-27',  8054.55, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 8, N'ACCCEPTED', N'OFFER_NAME B054AA2', '2077-02-27', '2078-09-03',  4878.58, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 7, N'ACCCEPTED', N'OFFER_NAME FFF6B77', '1973-12-18', '1974-11-25',  7775.35, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 5, N'COLLAPSED', N'OFFER_NAME E023D25', '1969-07-27', '1970-01-09',  5271.39, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER'),
    ( 9, N'OPEN',      N'OFFER_NAME C4F0D6B', '1991-10-17', '1992-12-08',  3155.70, N'This is a note.', 1, getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER', getutcdate(), N'FOUGSTER', NEWID(), N'FOUGSTER');
GO
