DELETE
FROM   PIMS_RESEARCH_FILE;
GO

INSERT INTO [dbo].[PIMS_RESEARCH_FILE]([RESEARCH_FILE_STATUS_TYPE_CODE], [REQUEST_SOURCE_TYPE_CODE], [NAME], [RFILE_NUMBER], [REQUEST_DATE], [ROAD_NAME], [ROAD_ALIAS], [CONCURRENCY_CONTROL_NUMBER])
VALUES
  (N'ACTIVE', N'HQ', N'Bubba BBQ Whirled',  N'R-Bubba1', '2022-08-02', N'Bubba Loo Lane',      N'Bubba Loo Expressway', 1),
  (N'ACTIVE', N'HQ', N'Bubba BBQ World',    N'R-Bubba2', '2022-07-02', N'Bubba Loo Street',    N'Bubba Loo Freeway',    1),
  (N'ACTIVE', N'HQ', N'Bubba BBQ Shack',    N'R-Bubba3', '2022-06-02', N'Bubba Loo Avenue',    N'Bubba Loo Parkway',    1),
  (N'ACTIVE', N'HQ', N'Bubba BBQ Joint',    N'R-Bubba4', '2022-05-02', N'Bubba Loo Boulevard', N'Bubba Loo No Way',     1),
  (N'ACTIVE', N'HQ', N'Bubba BBQ Domicile', N'R-Bubba5', '2022-04-02', N'Bubba Loo Circle',    N'Bubba Loo Yo Way',     1);
GO
