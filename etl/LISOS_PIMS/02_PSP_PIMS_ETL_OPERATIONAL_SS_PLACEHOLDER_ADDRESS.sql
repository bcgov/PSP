IF NOT EXISTS (SELECT * FROM [PIMS_ADDRESS] WHERE STREET_ADDRESS_1 = 'LISOS Placeholder Address')
BEGIN
INSERT INTO [dbo].[PIMS_ADDRESS]
           ([REGION_CODE]
           ,[DISTRICT_CODE]
           ,[PROVINCE_STATE_ID]
           ,[COUNTRY_ID]
           ,[STREET_ADDRESS_1]
           ,[MUNICIPALITY_NAME]
           ,[APP_CREATE_USERID]
           ,[APP_LAST_UPDATE_USERID]
           ,[DB_CREATE_USERID]
           ,[DB_LAST_UPDATE_USERID]
		   ,[APP_CREATE_USER_DIRECTORY]
		   ,[APP_LAST_UPDATE_USER_DIRECTORY])
     VALUES
           (1
           ,1
           ,1
           ,1
           ,'LISOS Placeholder Address'
           ,'Placeholder'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL'
		   ,'LISOS_ETL')
END


