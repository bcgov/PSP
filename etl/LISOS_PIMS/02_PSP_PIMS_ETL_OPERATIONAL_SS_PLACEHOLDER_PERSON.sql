IF NOT EXISTS (SELECT * FROM PIMS_PERSON WHERE SURNAME = 'LISOS' and FIRST_NAME = 'Placeholder')
BEGIN
INSERT INTO [dbo].[PIMS_PERSON]
           ([SURNAME]
           ,[FIRST_NAME]
           ,[IS_DISABLED]
           ,[ADDRESS_ID]
           ,[APP_CREATE_USERID]
           ,[APP_LAST_UPDATE_USERID]
           ,[DB_CREATE_USERID]
           ,[DB_LAST_UPDATE_USERID])
     VALUES
           ('LISOS'
           ,'Placeholder'
           ,1
           ,(Select TOP(1) ADDRESS_ID FROM PIMS_ADDRESS WHERE STREET_ADDRESS_1 = 'LISOS Placeholder Address')
           ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL')
END


