DELETE 
FROM   PIMS_ACQUISITION_OWNER;
GO

INSERT INTO [dbo].[PIMS_ACQUISITION_OWNER] ([ACQUISITION_FILE_ID], [LAST_NAME_AND_CORP_NAME], [CONCURRENCY_CONTROL_NUMBER]) 
	VALUES((SELECT MAX(ACQUISITION_FILE_ID) FROM PIMS_ACQUISITION_FILE), N'Beer Bob', 1)
GO


INSERT INTO [dbo].[PIMS_ACQUISITION_OWNER] ([ACQUISITION_FILE_ID], [LAST_NAME_AND_CORP_NAME], [CONCURRENCY_CONTROL_NUMBER]) 
	VALUES((SELECT MIN(ACQUISITION_FILE_ID) FROM PIMS_ACQUISITION_FILE), N'Water Bob', 1)
GO
