DELETE
FROM   PIMS_ACQUISITION_FILE;
GO

INSERT INTO [dbo].[PIMS_ACQUISITION_FILE] ([ACQUISITION_FILE_STATUS_TYPE_CODE], [ACQUISITION_TYPE_CODE], [ACQUISITION_FUNDING_TYPE_CODE], [ACQ_PHYS_FILE_STATUS_TYPE_CODE], [REGION_CODE], [FILE_NAME], [CONCURRENCY_CONTROL_NUMBER]) 
	VALUES(N'ACTIVE', N'CONSEN', N'DFAA', N'ACTIVE', 1, N'Bippity-Bop', 1)
GO
