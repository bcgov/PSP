-- Set the new sequence value
PRINT N'Set the new sequence value'
GO
DECLARE @StartVlu bigint;
DECLARE @Qry nvarchar(max);

SET @StartVlu = (SELECT MAX(PROPERTY_ID) + 1 FROM dbo.PIMS_PROPERTY)
SET @Qry      = 'ALTER SEQUENCE PIMS_PROPERTY_ID_SEQ RESTART WITH ' + CAST(@StartVlu AS NVARCHAR(20)) + ';'
EXEC SP_EXECUTESQL @Qry;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
