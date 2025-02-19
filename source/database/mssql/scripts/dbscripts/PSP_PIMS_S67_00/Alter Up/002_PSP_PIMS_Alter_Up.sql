-- Script generated by Aqua Data Studio Schema Synchronization for MS SQL Server 2016 on Tue Nov 07 15:48:59 PST 2023
-- Execute this script on:
-- 		PSP_PIMS_S66.00/dbo - This database/schema will be modified
-- to synchronize it with MS SQL Server 2016:
-- 		PSP_PIMS_S67.00/dbo

-- We recommend backing up the database prior to executing the script.

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop trigger dbo.PIMS_AGRMNT_I_S_I_TR
PRINT N'Drop trigger dbo.PIMS_AGRMNT_I_S_I_TR'
GO
DROP TRIGGER IF EXISTS [dbo].[PIMS_AGRMNT_I_S_I_TR]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop trigger dbo.PIMS_AGRMNT_I_S_U_TR
PRINT N'Drop trigger dbo.PIMS_AGRMNT_I_S_U_TR'
GO
DROP TRIGGER IF EXISTS [dbo].[PIMS_AGRMNT_I_S_U_TR]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop trigger dbo.PIMS_AGRMNT_A_S_IUD_TR
PRINT N'Drop trigger dbo.PIMS_AGRMNT_A_S_IUD_TR'
GO
DROP TRIGGER IF EXISTS [dbo].[PIMS_AGRMNT_A_S_IUD_TR]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop view dbo.PIMS_PROPERTY_BOUNDARY_VW
PRINT N'Drop view dbo.PIMS_PROPERTY_BOUNDARY_VW'
GO
DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_BOUNDARY_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop view dbo.PIMS_PROPERTY_LOCATION_VW
PRINT N'Drop view dbo.PIMS_PROPERTY_LOCATION_VW'
GO
DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_LOCATION_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create sequence dbo.PIMS_FILE_ENTITY_PERMISSIONS_ID_SEQ
PRINT N'Create sequence dbo.PIMS_FILE_ENTITY_PERMISSIONS_ID_SEQ'
GO
CREATE SEQUENCE [dbo].[PIMS_FILE_ENTITY_PERMISSIONS_ID_SEQ]
	AS bigint
	START WITH 1
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	NO CYCLE
	CACHE 50
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create sequence dbo.PIMS_FILE_ENTITY_ID_SEQ
PRINT N'Create sequence dbo.PIMS_FILE_ENTITY_ID_SEQ'
GO
CREATE SEQUENCE [dbo].[PIMS_FILE_ENTITY_ID_SEQ]
	AS bigint
	START WITH 1
	INCREMENT BY 1
	MINVALUE 1
	MAXVALUE 2147483647
	NO CYCLE
	CACHE 50
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create table dbo.PIMS_AGREEMENT_STATUS_TYPE
PRINT N'Create table dbo.PIMS_AGREEMENT_STATUS_TYPE'
GO
CREATE TABLE [dbo].[PIMS_AGREEMENT_STATUS_TYPE]  ( 
	[AGREEMENT_STATUS_TYPE_CODE]	nvarchar(20) NOT NULL,
	[DESCRIPTION]               	nvarchar(200) NOT NULL,
	[DISPLAY_ORDER]             	int NULL,
	[IS_DISABLED]               	bit NULL CONSTRAINT [AGRSTY_IS_DISABLED_DEF]  DEFAULT (CONVERT([bit],(0))),
	[CONCURRENCY_CONTROL_NUMBER]	bigint NOT NULL CONSTRAINT [AGRSTY_CONCURRENCY_CONTROL_NUMBER_DEF]  DEFAULT ((1)),
	[DB_CREATE_TIMESTAMP]       	datetime NOT NULL CONSTRAINT [AGRSTY_DB_CREATE_TIMESTAMP_DEF]  DEFAULT (getutcdate()),
	[DB_CREATE_USERID]          	nvarchar(30) NOT NULL CONSTRAINT [AGRSTY_DB_CREATE_USERID_DEF]  DEFAULT (user_name()),
	[DB_LAST_UPDATE_TIMESTAMP]  	datetime NOT NULL CONSTRAINT [AGRSTY_DB_LAST_UPDATE_TIMESTAMP_DEF]  DEFAULT (getutcdate()),
	[DB_LAST_UPDATE_USERID]     	nvarchar(30) NOT NULL CONSTRAINT [AGRSTY_DB_LAST_UPDATE_USERID_DEF]  DEFAULT (user_name()),
	CONSTRAINT [AGRSTY_PK] PRIMARY KEY CLUSTERED([AGREEMENT_STATUS_TYPE_CODE])
 ON [PRIMARY])
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Codified version of the agreement status.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_AGREEMENT_STATUS_TYPE', 
	@level2type = N'Column', @level2name = N'AGREEMENT_STATUS_TYPE_CODE'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of the agreement status type.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_AGREEMENT_STATUS_TYPE', 
	@level2type = N'Column', @level2name = N'DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Display order of the codes.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_AGREEMENT_STATUS_TYPE', 
	@level2type = N'Column', @level2name = N'DISPLAY_ORDER'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicates if the code value is inactive.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_AGREEMENT_STATUS_TYPE', 
	@level2type = N'Column', @level2name = N'IS_DISABLED'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Table that contains the codes and associated descriptions of the agreement types.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_AGREEMENT_STATUS_TYPE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_AGREEMENT
PRINT N'Alter table dbo.PIMS_AGREEMENT'
GO
ALTER TABLE [dbo].[PIMS_AGREEMENT]
	DROP CONSTRAINT IF EXISTS [AGRMNT_AGREEMENT_STATUS_DEF]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[PIMS_AGREEMENT]
	ADD [AGREEMENT_STATUS_TYPE_CODE] nvarchar(20) NOT NULL CONSTRAINT [AGRMNT_AGREEMENT_STATUS_TYPE_CODE_DEF] DEFAULT ('DRAFT'), 
	[CANCELLATION_NOTE] nvarchar(2000) NULL
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Note pertaining to the cancellation of the agreement.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_AGREEMENT', 
	@level2type = N'Column', @level2name = N'CANCELLATION_NOTE'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[PIMS_AGREEMENT]
	DROP COLUMN IF EXISTS [IS_DRAFT]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter table dbo.PIMS_AGREEMENT_HIST
PRINT N'Alter table dbo.PIMS_AGREEMENT_HIST'
GO
ALTER TABLE [dbo].[PIMS_AGREEMENT_HIST]
	ADD [AGREEMENT_STATUS_TYPE_CODE] nvarchar(20) NOT NULL DEFAULT 'DRAFT', 
	[CANCELLATION_NOTE] nvarchar(2000) NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
ALTER TABLE [dbo].[PIMS_AGREEMENT_HIST]
	DROP COLUMN IF EXISTS [IS_DRAFT]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create view dbo.PIMS_PROPERTY_LOCATION_VW
PRINT N'Create view dbo.PIMS_PROPERTY_LOCATION_VW'
GO
CREATE VIEW [dbo].[PIMS_PROPERTY_LOCATION_VW] AS
SELECT PROP.PROPERTY_ID
     , PROP.PID
     , RIGHT('000000000' + CAST(PROP.PID AS VARCHAR(9)), 9) AS PID_PADDED
     , PROP.PIN                   
     , PROP.PROPERTY_TYPE_CODE
     , PROP.PROPERTY_STATUS_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE
     , PROP.PROPERTY_CLASSIFICATION_TYPE_CODE  
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE  TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYPE TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
                                                      AND TNTY.PROPERTY_ID               = PROP.PROPERTY_ID) temp) AS PROPERTY_TENURE_TYPE_CODE
     , ADDR.STREET_ADDRESS_1
     , ADDR.STREET_ADDRESS_2
     , ADDR.STREET_ADDRESS_3
     , ADDR.MUNICIPALITY_NAME
     , ADDR.POSTAL_CODE
     , PROV.PROVINCE_STATE_CODE
     , PROV.DESCRIPTION AS PROVINCE_NAME
     , CNTY.COUNTRY_CODE
     , CNTY.DESCRIPTION AS COUNTRY_NAME
     , PROP.NAME
     , PROP.DESCRIPTION
     , PROP.ADDRESS_ID
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.LOCATION AS GEOMETRY
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , PROP.ENCUMBRANCE_REASON          
     , PROP.IS_SENSITIVE
     , PROP.IS_OWNED
     , PROP.IS_PROPERTY_OF_INTEREST
     , PROP.IS_VISIBLE_TO_OTHER_AGENCIES
     , PROP.ZONING
     , PROP.ZONING_POTENTIAL 
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN ('PYBLMOTI', 'PYBLBCTFA')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_TERM     TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN ('PYBLMOTI', 'PYBLBCTFA')
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE        ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE IS NULL) OR
                           (getutcdate() BETWEEN TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_RECEIVABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_TERM     TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE        ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE IS NULL) OR
                           (getutcdate() BETWEEN TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_RECEIVABLE_LEASE
FROM   PIMS_PROPERTY       PROP                                                    LEFT OUTER JOIN                                                                                     
       PIMS_ADDRESS        ADDR ON ADDR.ADDRESS_ID        = PROP.ADDRESS_ID        LEFT OUTER JOIN
       PIMS_PROVINCE_STATE PROV ON PROV.PROVINCE_STATE_ID = ADDR.PROVINCE_STATE_ID LEFT OUTER JOIN
       PIMS_COUNTRY        CNTY ON CNTY.COUNTRY_ID        = ADDR.COUNTRY_ID
WHERE  PROP.LOCATION IS NOT NULL

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create view dbo.PIMS_PROPERTY_BOUNDARY_VW
PRINT N'Create view dbo.PIMS_PROPERTY_BOUNDARY_VW'
GO
CREATE VIEW [dbo].[PIMS_PROPERTY_BOUNDARY_VW] AS
SELECT PROP.PROPERTY_ID
     , PROP.PID
     , RIGHT('000000000' + CAST(PROP.PID AS VARCHAR(9)), 9) AS PID_PADDED
     , PROP.PIN
     , PROP.PROPERTY_TYPE_CODE
     , PROP.PROPERTY_STATUS_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE
     , PROP.PROPERTY_CLASSIFICATION_TYPE_CODE
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE  TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYPE TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
                                                      AND TNTY.PROPERTY_ID               = PROP.PROPERTY_ID) temp) AS PROPERTY_TENURE_TYPE_CODE
     , ADDR.STREET_ADDRESS_1
     , ADDR.STREET_ADDRESS_2
     , ADDR.STREET_ADDRESS_3
     , ADDR.MUNICIPALITY_NAME
     , ADDR.POSTAL_CODE
     , PROV.PROVINCE_STATE_CODE
     , PROV.DESCRIPTION AS PROVINCE_NAME
     , CNTY.COUNTRY_CODE
     , CNTY.DESCRIPTION AS COUNTRY_NAME
     , PROP.NAME
     , PROP.DESCRIPTION
     , PROP.ADDRESS_ID
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.BOUNDARY AS GEOMETRY  
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , PROP.ENCUMBRANCE_REASON
     , PROP.IS_SENSITIVE
     , PROP.IS_OWNED
     , PROP.IS_PROPERTY_OF_INTEREST
     , PROP.IS_VISIBLE_TO_OTHER_AGENCIES
     , PROP.ZONING
     , PROP.ZONING_POTENTIAL
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN ('PYBLMOTI', 'PYBLBCTFA')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_TERM     TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN ('PYBLMOTI', 'PYBLBCTFA')
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE        ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE IS NULL) OR
                           (getutcdate() BETWEEN TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_RECEIVABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_TERM     TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE        ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE AND LEAS.ORIG_EXPIRY_DATE IS NULL) OR
                           (getutcdate() BETWEEN TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.TERM_START_DATE AND TERM.TERM_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_RECEIVABLE_LEASE
FROM   PIMS_PROPERTY       PROP                                                    LEFT OUTER JOIN
       PIMS_ADDRESS        ADDR ON ADDR.ADDRESS_ID        = PROP.ADDRESS_ID        LEFT OUTER JOIN
       PIMS_PROVINCE_STATE PROV ON PROV.PROVINCE_STATE_ID = ADDR.PROVINCE_STATE_ID LEFT OUTER JOIN
       PIMS_COUNTRY        CNTY ON CNTY.COUNTRY_ID        = ADDR.COUNTRY_ID
WHERE  PROP.BOUNDARY IS NOT NULL

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create index dbo.AGRMNT_AGREEMENT_STATUS_TYPE_CODE_IDX
PRINT N'Create index dbo.AGRMNT_AGREEMENT_STATUS_TYPE_CODE_IDX'
GO
CREATE NONCLUSTERED INDEX [AGRMNT_AGREEMENT_STATUS_TYPE_CODE_IDX]
	ON [dbo].[PIMS_AGREEMENT]([AGREEMENT_STATUS_TYPE_CODE])
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

---- Create foreign key constraint dbo.PIM_AGRSTY_PIM_AGRMNT_FK
--PRINT N'Create foreign key constraint dbo.PIM_AGRSTY_PIM_AGRMNT_FK'
--GO
--ALTER TABLE [dbo].[PIMS_AGREEMENT]
--	ADD CONSTRAINT [PIM_AGRSTY_PIM_AGRMNT_FK]
--	FOREIGN KEY([AGREEMENT_STATUS_TYPE_CODE])
--	REFERENCES [dbo].[PIMS_AGREEMENT_STATUS_TYPE]([AGREEMENT_STATUS_TYPE_CODE])
--	ON DELETE NO ACTION 
--	ON UPDATE NO ACTION 
--GO
--IF @@ERROR <> 0 SET NOEXEC ON
--GO

-- Create trigger dbo.PIMS_AGRMNT_A_S_IUD_TR
PRINT N'Create trigger dbo.PIMS_AGRMNT_A_S_IUD_TR'
GO
CREATE TRIGGER [dbo].[PIMS_AGRMNT_A_S_IUD_TR] ON PIMS_AGREEMENT FOR INSERT, UPDATE, DELETE AS
SET NOCOUNT ON
BEGIN TRY
DECLARE @curr_date datetime;
SET @curr_date = getutcdate();
  IF NOT EXISTS(SELECT * FROM inserted) AND NOT EXISTS(SELECT * FROM deleted) 
    RETURN;

  -- historical
  IF EXISTS(SELECT * FROM deleted)
    update PIMS_AGREEMENT_HIST set END_DATE_HIST = @curr_date where AGREEMENT_ID in (select AGREEMENT_ID from deleted) and END_DATE_HIST is null;
  
  IF EXISTS(SELECT * FROM inserted)
    insert into PIMS_AGREEMENT_HIST ([AGREEMENT_ID], [ACQUISITION_FILE_ID], [AGREEMENT_TYPE_CODE], [AGREEMENT_STATUS_TYPE_CODE], [AGREEMENT_DATE], [COMPLETION_DATE], [TERMINATION_DATE], [COMMENCEMENT_DATE], [DEPOSIT_AMOUNT], [NO_LATER_THAN_DAYS], [PURCHASE_PRICE], [LEGAL_SURVEY_PLAN_NUM], [OFFER_DATE], [EXPIRY_TS], [SIGNED_DATE], [INSPECTION_DATE], [EXPROPRIATION_DATE], [POSSESSION_DATE], [CANCELLATION_NOTE], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID], _AGREEMENT_HIST_ID, END_DATE_HIST, EFFECTIVE_DATE_HIST)
      select [AGREEMENT_ID], [ACQUISITION_FILE_ID], [AGREEMENT_TYPE_CODE], [AGREEMENT_STATUS_TYPE_CODE], [AGREEMENT_DATE], [COMPLETION_DATE], [TERMINATION_DATE], [COMMENCEMENT_DATE], [DEPOSIT_AMOUNT], [NO_LATER_THAN_DAYS], [PURCHASE_PRICE], [LEGAL_SURVEY_PLAN_NUM], [OFFER_DATE], [EXPIRY_TS], [SIGNED_DATE], [INSPECTION_DATE], [EXPROPRIATION_DATE], [POSSESSION_DATE], [CANCELLATION_NOTE], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID], (next value for [dbo].[PIMS_AGREEMENT_H_ID_SEQ]) as [_AGREEMENT_HIST_ID], null as [END_DATE_HIST], @curr_date as [EFFECTIVE_DATE_HIST] from inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create trigger dbo.PIMS_AGRSTY_I_S_U_TR
PRINT N'Create trigger dbo.PIMS_AGRSTY_I_S_U_TR'
GO
CREATE TRIGGER [dbo].[PIMS_AGRSTY_I_S_U_TR] ON PIMS_AGREEMENT_STATUS_TYPE INSTEAD OF UPDATE AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM deleted) 
    RETURN;

  -- validate concurrency control
  if exists (select 1 from inserted, deleted where inserted.CONCURRENCY_CONTROL_NUMBER != deleted.CONCURRENCY_CONTROL_NUMBER+1 AND inserted.AGREEMENT_STATUS_TYPE_CODE = deleted.AGREEMENT_STATUS_TYPE_CODE)
    raiserror('CONCURRENCY FAILURE.',16,1)


  -- update statement
  update PIMS_AGREEMENT_STATUS_TYPE
    set "AGREEMENT_STATUS_TYPE_CODE" = inserted."AGREEMENT_STATUS_TYPE_CODE",
      "DESCRIPTION" = inserted."DESCRIPTION",
      "DISPLAY_ORDER" = inserted."DISPLAY_ORDER",
      "IS_DISABLED" = inserted."IS_DISABLED",
      "CONCURRENCY_CONTROL_NUMBER" = inserted."CONCURRENCY_CONTROL_NUMBER"
    , DB_LAST_UPDATE_TIMESTAMP = getutcdate()
    , DB_LAST_UPDATE_USERID = user_name()
    from PIMS_AGREEMENT_STATUS_TYPE
    inner join inserted
    on (PIMS_AGREEMENT_STATUS_TYPE.AGREEMENT_STATUS_TYPE_CODE = inserted.AGREEMENT_STATUS_TYPE_CODE);

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create trigger dbo.PIMS_AGRMNT_I_S_U_TR
PRINT N'Create trigger dbo.PIMS_AGRMNT_I_S_U_TR'
GO
CREATE TRIGGER [dbo].[PIMS_AGRMNT_I_S_U_TR] ON PIMS_AGREEMENT INSTEAD OF UPDATE AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM deleted) 
    RETURN;

  -- validate concurrency control
  if exists (select 1 from inserted, deleted where inserted.CONCURRENCY_CONTROL_NUMBER != deleted.CONCURRENCY_CONTROL_NUMBER+1 AND inserted.AGREEMENT_ID = deleted.AGREEMENT_ID)
    raiserror('CONCURRENCY FAILURE.',16,1)


  -- update statement
  update PIMS_AGREEMENT
    set "AGREEMENT_ID" = inserted."AGREEMENT_ID",
      "ACQUISITION_FILE_ID" = inserted."ACQUISITION_FILE_ID",
      "AGREEMENT_TYPE_CODE" = inserted."AGREEMENT_TYPE_CODE",
      "AGREEMENT_STATUS_TYPE_CODE" = inserted."AGREEMENT_STATUS_TYPE_CODE",
      "AGREEMENT_DATE" = inserted."AGREEMENT_DATE",
      "COMPLETION_DATE" = inserted."COMPLETION_DATE",
      "TERMINATION_DATE" = inserted."TERMINATION_DATE",
      "COMMENCEMENT_DATE" = inserted."COMMENCEMENT_DATE",
      "DEPOSIT_AMOUNT" = inserted."DEPOSIT_AMOUNT",
      "NO_LATER_THAN_DAYS" = inserted."NO_LATER_THAN_DAYS",
      "PURCHASE_PRICE" = inserted."PURCHASE_PRICE",
      "LEGAL_SURVEY_PLAN_NUM" = inserted."LEGAL_SURVEY_PLAN_NUM",
      "OFFER_DATE" = inserted."OFFER_DATE",
      "EXPIRY_TS" = inserted."EXPIRY_TS",
      "SIGNED_DATE" = inserted."SIGNED_DATE",
      "INSPECTION_DATE" = inserted."INSPECTION_DATE",
      "EXPROPRIATION_DATE" = inserted."EXPROPRIATION_DATE",
      "POSSESSION_DATE" = inserted."POSSESSION_DATE",
      "CANCELLATION_NOTE" = inserted."CANCELLATION_NOTE",
      "CONCURRENCY_CONTROL_NUMBER" = inserted."CONCURRENCY_CONTROL_NUMBER",
      "APP_LAST_UPDATE_TIMESTAMP" = inserted."APP_LAST_UPDATE_TIMESTAMP",
      "APP_LAST_UPDATE_USERID" = inserted."APP_LAST_UPDATE_USERID",
      "APP_LAST_UPDATE_USER_GUID" = inserted."APP_LAST_UPDATE_USER_GUID",
      "APP_LAST_UPDATE_USER_DIRECTORY" = inserted."APP_LAST_UPDATE_USER_DIRECTORY"
    , DB_LAST_UPDATE_TIMESTAMP = getutcdate()
    , DB_LAST_UPDATE_USERID = user_name()
    from PIMS_AGREEMENT
    inner join inserted
    on (PIMS_AGREEMENT.AGREEMENT_ID = inserted.AGREEMENT_ID);

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create trigger dbo.PIMS_AGRSTY_I_S_I_TR
PRINT N'Create trigger dbo.PIMS_AGRSTY_I_S_I_TR'
GO
CREATE TRIGGER [dbo].[PIMS_AGRSTY_I_S_I_TR] ON PIMS_AGREEMENT_STATUS_TYPE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted) 
    RETURN;

  
  insert into PIMS_AGREEMENT_STATUS_TYPE ("AGREEMENT_STATUS_TYPE_CODE",
      "DESCRIPTION",
      "DISPLAY_ORDER",
      "IS_DISABLED",
      "CONCURRENCY_CONTROL_NUMBER")
    select "AGREEMENT_STATUS_TYPE_CODE",
      "DESCRIPTION",
      "DISPLAY_ORDER",
      "IS_DISABLED",
      "CONCURRENCY_CONTROL_NUMBER"
    from inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create trigger dbo.PIMS_AGRMNT_I_S_I_TR
PRINT N'Create trigger dbo.PIMS_AGRMNT_I_S_I_TR'
GO
CREATE TRIGGER [dbo].[PIMS_AGRMNT_I_S_I_TR] ON PIMS_AGREEMENT INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted) 
    RETURN;

  
  insert into PIMS_AGREEMENT ("AGREEMENT_ID",
      "ACQUISITION_FILE_ID",
      "AGREEMENT_TYPE_CODE",
      "AGREEMENT_STATUS_TYPE_CODE",
      "AGREEMENT_DATE",
      "COMPLETION_DATE",
      "TERMINATION_DATE",
      "COMMENCEMENT_DATE",
      "DEPOSIT_AMOUNT",
      "NO_LATER_THAN_DAYS",
      "PURCHASE_PRICE",
      "LEGAL_SURVEY_PLAN_NUM",
      "OFFER_DATE",
      "EXPIRY_TS",
      "SIGNED_DATE",
      "INSPECTION_DATE",
      "EXPROPRIATION_DATE",
      "POSSESSION_DATE",
      "CANCELLATION_NOTE",
      "CONCURRENCY_CONTROL_NUMBER",
      "APP_CREATE_TIMESTAMP",
      "APP_CREATE_USERID",
      "APP_CREATE_USER_GUID",
      "APP_CREATE_USER_DIRECTORY",
      "APP_LAST_UPDATE_TIMESTAMP",
      "APP_LAST_UPDATE_USERID",
      "APP_LAST_UPDATE_USER_GUID",
      "APP_LAST_UPDATE_USER_DIRECTORY")
    select "AGREEMENT_ID",
      "ACQUISITION_FILE_ID",
      "AGREEMENT_TYPE_CODE",
      "AGREEMENT_STATUS_TYPE_CODE",
      "AGREEMENT_DATE",
      "COMPLETION_DATE",
      "TERMINATION_DATE",
      "COMMENCEMENT_DATE",
      "DEPOSIT_AMOUNT",
      "NO_LATER_THAN_DAYS",
      "PURCHASE_PRICE",
      "LEGAL_SURVEY_PLAN_NUM",
      "OFFER_DATE",
      "EXPIRY_TS",
      "SIGNED_DATE",
      "INSPECTION_DATE",
      "EXPROPRIATION_DATE",
      "POSSESSION_DATE",
      "CANCELLATION_NOTE",
      "CONCURRENCY_CONTROL_NUMBER",
      "APP_CREATE_TIMESTAMP",
      "APP_CREATE_USERID",
      "APP_CREATE_USER_GUID",
      "APP_CREATE_USER_DIRECTORY",
      "APP_LAST_UPDATE_TIMESTAMP",
      "APP_LAST_UPDATE_USERID",
      "APP_LAST_UPDATE_USER_GUID",
      "APP_LAST_UPDATE_USER_DIRECTORY"
    from inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;

GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
