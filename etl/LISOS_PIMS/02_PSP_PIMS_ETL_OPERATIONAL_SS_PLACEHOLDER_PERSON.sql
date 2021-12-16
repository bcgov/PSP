IF NOT EXISTS (SELECT * FROM PIMS_PERSON WHERE SURNAME = 'LISOS' and FIRST_NAME = 'Placeholder')
BEGIN
INSERT INTO [dbo].[PIMS_PERSON]
           ([SURNAME]
           ,[FIRST_NAME]
           ,[IS_DISABLED]
           ,[APP_CREATE_USERID]
           ,[APP_LAST_UPDATE_USERID]
           ,[DB_CREATE_USERID]
           ,[DB_LAST_UPDATE_USERID]
		   ,[APP_CREATE_USER_DIRECTORY]
		   ,[APP_LAST_UPDATE_USER_DIRECTORY])
     VALUES
           ('LISOS'
           ,'Placeholder'
           ,1
           ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL')

INSERT INTO [dbo].[PIMS_PERSON_ADDRESS]
			([PERSON_ID]
			,[ADDRESS_ID]
			,[ADDRESS_USAGE_TYPE_CODE]
           ,[IS_DISABLED]
           ,[APP_CREATE_USERID]
           ,[APP_LAST_UPDATE_USERID]
           ,[DB_CREATE_USERID]
           ,[DB_LAST_UPDATE_USERID]
		   ,[APP_CREATE_USER_DIRECTORY]
		   ,[APP_LAST_UPDATE_USER_DIRECTORY])
     VALUES
           ((Select TOP(1) PERSON_ID FROM PIMS_PERSON WHERE [SURNAME] = 'LISOS')
           ,(Select TOP(1) ADDRESS_ID FROM PIMS_ADDRESS WHERE [STREET_ADDRESS_1] = 'LISOS Placeholder Address')
           ,'MAILING'
		   ,1
           ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL')
END

