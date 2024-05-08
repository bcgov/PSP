DELETE
FROM   PIMS_FILE_NUMBER
GO

INSERT INTO PIMS_FILE_NUMBER(PROPERTY_ID, FILE_NUMBER_TYPE_CODE, FILE_NUMBER, OTHER_FILE_NUMBER_TYPE)
  VALUES
    (1, N'LISNO',    N'0309-000',      NULL),
    (1, N'PROPNEG',  N'PN1235',        NULL),
    (1, N'PSNO',     N'01-9389-11',    NULL),
    (1, N'PUBWORKS', N'PW1234',        NULL),
    (1, N'REGLSLIC', N'LL12345',       NULL),
    (1, N'RESERVE',  N'RSRV01',        NULL),
    (1, N'OTHER',    N'Other value',   N'Something else entirely'),
    (2, N'LISNO',    N'0309-001',      NULL),
    (2, N'PROPNEG',  N'PN12356',       NULL),
    (2, N'PSNO',     N'01-9389-12',    NULL),
    (2, N'PUBWORKS', N'PW15667',       NULL),
    (2, N'REGLSLIC', N'LL12awd345',    NULL),
    (2, N'RESERVE',  N'RSRV0awe1',     NULL),
    (2, N'OTHER',    N'Another value', N'Something else entirely again');
GO
