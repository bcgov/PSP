DELETE
FROM   PIMS_DOCUMENT;
GO

INSERT INTO [dbo].[PIMS_DOCUMENT] ([DOCUMENT_TYPE_ID], [DOCUMENT_STATUS_TYPE_CODE], [FILE_NAME], [MAYAN_ID], [CONCURRENCY_CONTROL_NUMBER]) 
	VALUES(1, N'SENT', N'Man-go Away!', 123, 1)
GO
