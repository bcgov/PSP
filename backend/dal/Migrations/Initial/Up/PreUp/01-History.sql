PRINT N'Create history tables'
GO

-- PIMS_ACCESS_REQUEST_HIST
CREATE SEQUENCE [dbo].[PIMS_ACCESS_REQUEST_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_ACCESS_REQUEST_HIST](
  _ACCESS_REQUEST_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_ACCESS_REQUEST_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [ACCESS_REQUEST_ID] bigint NOT NULL, [USER_ID] bigint NOT NULL, [STATUS] int NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_ACCESS_REQUEST_HIST ADD CONSTRAINT PIMS_ACCRQT_H_PK PRIMARY KEY CLUSTERED (_ACCESS_REQUEST_HIST_ID);
ALTER TABLE PIMS_ACCESS_REQUEST_HIST ADD CONSTRAINT PIMS_ACCRQT_H_UK UNIQUE (_ACCESS_REQUEST_HIST_ID, END_DATE_HIST)
GO

-- PIMS_ACCESS_REQUEST_AGENCY_HIST
CREATE SEQUENCE [dbo].[PIMS_ACCESS_REQUEST_AGENCY_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_ACCESS_REQUEST_AGENCY_HIST](
  _ACCESS_REQUEST_AGENCY_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_ACCESS_REQUEST_AGENCY_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [ACCESS_REQUEST_AGENCY_ID] bigint NOT NULL, [ACCESS_REQUEST_ID] bigint NOT NULL, [AGENCY_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_ACCESS_REQUEST_AGENCY_HIST ADD CONSTRAINT PIMS_ACRQAG_H_PK PRIMARY KEY CLUSTERED (_ACCESS_REQUEST_AGENCY_HIST_ID);
ALTER TABLE PIMS_ACCESS_REQUEST_AGENCY_HIST ADD CONSTRAINT PIMS_ACRQAG_H_UK UNIQUE (_ACCESS_REQUEST_AGENCY_HIST_ID, END_DATE_HIST)
GO

-- PIMS_ACCESS_REQUEST_ROLE_HIST
CREATE SEQUENCE [dbo].[PIMS_ACCESS_REQUEST_ROLE_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_ACCESS_REQUEST_ROLE_HIST](
  _ACCESS_REQUEST_ROLE_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_ACCESS_REQUEST_ROLE_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [ACCESS_REQUEST_ROLE_ID] bigint NOT NULL, [ROLE_ID] bigint NOT NULL, [ACCESS_REQUEST_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_ACCESS_REQUEST_ROLE_HIST ADD CONSTRAINT PIMS_ACCRQR_H_PK PRIMARY KEY CLUSTERED (_ACCESS_REQUEST_ROLE_HIST_ID);
ALTER TABLE PIMS_ACCESS_REQUEST_ROLE_HIST ADD CONSTRAINT PIMS_ACCRQR_H_UK UNIQUE (_ACCESS_REQUEST_ROLE_HIST_ID, END_DATE_HIST)
GO

-- PIMS_ADDRESS_HIST
CREATE SEQUENCE [dbo].[PIMS_ADDRESS_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_ADDRESS_HIST](
  _ADDRESS_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_ADDRESS_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [ADDRESS_ID] bigint NOT NULL, [PROVINCE_ID] bigint NOT NULL, [ADDRESS1] nvarchar(150) NULL, [ADDRESS2] nvarchar(150) NULL, [POSTAL] nvarchar(6) NULL, [ADMINISTRATIVE_AREA] nvarchar(450) NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_ADDRESS_HIST ADD CONSTRAINT PIMS_ADDR_H_PK PRIMARY KEY CLUSTERED (_ADDRESS_HIST_ID);
ALTER TABLE PIMS_ADDRESS_HIST ADD CONSTRAINT PIMS_ADDR_H_UK UNIQUE (_ADDRESS_HIST_ID, END_DATE_HIST)
GO

-- PIMS_AGENCY_HIST
CREATE SEQUENCE [dbo].[PIMS_AGENCY_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_AGENCY_HIST](
  _AGENCY_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_AGENCY_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [AGENCY_ID] bigint NOT NULL, [PARENT_AGENCY_ID] bigint NULL, [NAME] nvarchar(150) NOT NULL, [DESCRIPTION] nvarchar(500) NULL, [CODE] nvarchar(6) NOT NULL, [EMAIL] nvarchar(250) NULL, [SEND_EMAIL] bit NOT NULL, [ADDRESS_TO] nvarchar(100) NULL, [IS_DISABLED] bit NOT NULL, [DISPLAY_ORDER] int NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_AGENCY_HIST ADD CONSTRAINT PIMS_AGNCY_H_PK PRIMARY KEY CLUSTERED (_AGENCY_HIST_ID);
ALTER TABLE PIMS_AGENCY_HIST ADD CONSTRAINT PIMS_AGNCY_H_UK UNIQUE (_AGENCY_HIST_ID, END_DATE_HIST)
GO

-- PIMS_BUILDING_HIST
CREATE SEQUENCE [dbo].[PIMS_BUILDING_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_BUILDING_HIST](
  _BUILDING_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_BUILDING_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [BUILDING_ID] bigint NOT NULL, [AGENCY_ID] bigint NULL, [PROPERTY_TYPE_ID] bigint NOT NULL, [PROPERTY_CLASSIFICATION_ID] bigint NOT NULL, [BUILDING_CONSTRUCTION_TYPE_ID] bigint NOT NULL, [BUILDING_PREDOMINATE_USE_ID] bigint NOT NULL, [BUILDING_OCCUPANT_TYPE_ID] bigint NOT NULL, [ADDRESS_ID] bigint NOT NULL, [NAME] nvarchar(250) NULL, [DESCRIPTION] nvarchar(2000) NULL, [OCCUPANT_NAME] nvarchar(100) NULL, [BUILDING_FLOOR_COUNT] int NOT NULL, [BUILDING_TENANCY] nvarchar(450) NOT NULL, [BUILDING_TENANCY_UPDATED_ON] datetime NULL, [TOTAL_AREA] real NOT NULL, [RENTABLE_AREA] real NOT NULL, [PROJECT_NUMBERS] nvarchar(2000) NULL, [LEASE_EXPIRY] datetime NULL, [TRANSFER_LEASE_ON_SALE] bit NOT NULL, [ENCUMBRANCE_REASON] nvarchar(500) NULL, [IS_VISIBLE_TO_OTHER_AGENCIES] bit NOT NULL, [IS_SENSITIVE] bit NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_BUILDING_HIST ADD CONSTRAINT PIMS_BUILDG_H_PK PRIMARY KEY CLUSTERED (_BUILDING_HIST_ID);
ALTER TABLE PIMS_BUILDING_HIST ADD CONSTRAINT PIMS_BUILDG_H_UK UNIQUE (_BUILDING_HIST_ID, END_DATE_HIST)
GO

-- PIMS_BUILDING_EVALUATION_HIST
CREATE SEQUENCE [dbo].[PIMS_BUILDING_EVALUATION_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_BUILDING_EVALUATION_HIST](
  _BUILDING_EVALUATION_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_BUILDING_EVALUATION_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [BUILDING_EVALUATION_ID] bigint NOT NULL, [BUILDING_ID] bigint NOT NULL, [DATE] date NOT NULL, [KEY] int NOT NULL, [VALUE] money NOT NULL, [NOTE] nvarchar(500) NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_BUILDING_EVALUATION_HIST ADD CONSTRAINT PIMS_BLDEVL_H_PK PRIMARY KEY CLUSTERED (_BUILDING_EVALUATION_HIST_ID);
ALTER TABLE PIMS_BUILDING_EVALUATION_HIST ADD CONSTRAINT PIMS_BLDEVL_H_UK UNIQUE (_BUILDING_EVALUATION_HIST_ID, END_DATE_HIST)
GO

-- PIMS_BUILDING_FISCAL_HIST
CREATE SEQUENCE [dbo].[PIMS_BUILDING_FISCAL_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_BUILDING_FISCAL_HIST](
  _BUILDING_FISCAL_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_BUILDING_FISCAL_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [BUILDING_FISCAL_ID] bigint NOT NULL, [BUILDING_ID] bigint NOT NULL, [FISCAL_YEAR] int NOT NULL, [KEY] int NOT NULL, [VALUE] money NOT NULL, [NOTE] nvarchar(500) NULL, [EFFECTIVE_DATE] date NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_BUILDING_FISCAL_HIST ADD CONSTRAINT PIMS_BLDFSC_H_PK PRIMARY KEY CLUSTERED (_BUILDING_FISCAL_HIST_ID);
ALTER TABLE PIMS_BUILDING_FISCAL_HIST ADD CONSTRAINT PIMS_BLDFSC_H_UK UNIQUE (_BUILDING_FISCAL_HIST_ID, END_DATE_HIST)
GO

-- PIMS_NOTIFICATION_QUEUE_HIST
CREATE SEQUENCE [dbo].[PIMS_NOTIFICATION_QUEUE_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_NOTIFICATION_QUEUE_HIST](
  _NOTIFICATION_QUEUE_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_NOTIFICATION_QUEUE_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [NOTIFICATION_QUEUE_ID] bigint NOT NULL, [TO_AGENCY_ID] bigint NULL, [NOTIFICATION_TEMPLATE_ID] bigint NULL, [KEY] uniqueidentifier NOT NULL, [STATUS] int NOT NULL, [PRIORITY] nvarchar(50) NOT NULL, [ENCODING] nvarchar(50) NOT NULL, [SEND_ON] datetime NOT NULL, [TO] nvarchar(500) NULL, [SUBJECT] nvarchar(200) NOT NULL, [BODY_TYPE] nvarchar(50) NOT NULL, [BCC] nvarchar(500) NULL, [CC] nvarchar(500) NULL, [TAG] nvarchar(50) NULL, [CHES_MESSAGE_ID] uniqueidentifier NULL, [CHES_TRANSACTION_ID] uniqueidentifier NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_NOTIFICATION_QUEUE_HIST ADD CONSTRAINT PIMS_NOTIFQ_H_PK PRIMARY KEY CLUSTERED (_NOTIFICATION_QUEUE_HIST_ID);
ALTER TABLE PIMS_NOTIFICATION_QUEUE_HIST ADD CONSTRAINT PIMS_NOTIFQ_H_UK UNIQUE (_NOTIFICATION_QUEUE_HIST_ID, END_DATE_HIST)
GO

-- PIMS_PARCEL_HIST
CREATE SEQUENCE [dbo].[PIMS_PARCEL_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_PARCEL_HIST](
  _PARCEL_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_PARCEL_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [PARCEL_ID] bigint NOT NULL, [AGENCY_ID] bigint NULL, [PROPERTY_CLASSIFICATION_ID] bigint NOT NULL, [PROPERTY_TYPE_ID] bigint NOT NULL, [ADDRESS_ID] bigint NOT NULL, [NAME] nvarchar(250) NULL, [DESCRIPTION] nvarchar(2000) NULL, [PID] int NOT NULL, [PIN] int NULL, [LAND_AREA] real NOT NULL, [LAND_LEGAL_DESCRIPTION] nvarchar(500) NULL, [ZONING] nvarchar(50) NULL, [ZONING_POTENTIAL] nvarchar(50) NULL, [NOT_OWNED] bit NOT NULL, [ENCUMBRANCE_REASON] nvarchar(500) NULL, [PROJECT_NUMBERS] nvarchar(2000) NULL, [IS_VISIBLE_TO_OTHER_AGENCIES] bit NOT NULL, [IS_SENSITIVE] bit NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_PARCEL_HIST ADD CONSTRAINT PIMS_PARCEL_H_PK PRIMARY KEY CLUSTERED (_PARCEL_HIST_ID);
ALTER TABLE PIMS_PARCEL_HIST ADD CONSTRAINT PIMS_PARCEL_H_UK UNIQUE (_PARCEL_HIST_ID, END_DATE_HIST)
GO

-- PIMS_PARCEL_BUILDING_HIST
CREATE SEQUENCE [dbo].[PIMS_PARCEL_BUILDING_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_PARCEL_BUILDING_HIST](
  _PARCEL_BUILDING_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_PARCEL_BUILDING_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [PARCEL_BUILDING_ID] bigint NOT NULL, [PARCEL_ID] bigint NOT NULL, [BUILDING_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_PARCEL_BUILDING_HIST ADD CONSTRAINT PIMS_PRCLBL_H_PK PRIMARY KEY CLUSTERED (_PARCEL_BUILDING_HIST_ID);
ALTER TABLE PIMS_PARCEL_BUILDING_HIST ADD CONSTRAINT PIMS_PRCLBL_H_UK UNIQUE (_PARCEL_BUILDING_HIST_ID, END_DATE_HIST)
GO

-- PIMS_PARCEL_EVALUATION_HIST
CREATE SEQUENCE [dbo].[PIMS_PARCEL_EVALUATION_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_PARCEL_EVALUATION_HIST](
  _PARCEL_EVALUATION_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_PARCEL_EVALUATION_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [PARCEL_EVALUATION_ID] bigint NOT NULL, [PARCEL_ID] bigint NOT NULL, [DATE] date NOT NULL, [KEY] int NOT NULL, [FIRM] nvarchar(150) NULL, [VALUE] money NOT NULL, [NOTE] nvarchar(500) NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_PARCEL_EVALUATION_HIST ADD CONSTRAINT PIMS_PREVAL_H_PK PRIMARY KEY CLUSTERED (_PARCEL_EVALUATION_HIST_ID);
ALTER TABLE PIMS_PARCEL_EVALUATION_HIST ADD CONSTRAINT PIMS_PREVAL_H_UK UNIQUE (_PARCEL_EVALUATION_HIST_ID, END_DATE_HIST)
GO

-- PIMS_PARCEL_FISCAL_HIST
CREATE SEQUENCE [dbo].[PIMS_PARCEL_FISCAL_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_PARCEL_FISCAL_HIST](
  _PARCEL_FISCAL_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_PARCEL_FISCAL_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [PARCEL_FISCAL_ID] bigint NOT NULL, [PARCEL_ID] bigint NOT NULL, [FISCAL_YEAR] int NOT NULL, [KEY] int NOT NULL, [VALUE] money NOT NULL, [NOTE] nvarchar(500) NULL, [EFFECTIVE_DATE] date NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_PARCEL_FISCAL_HIST ADD CONSTRAINT PIMS_PRFSCL_H_PK PRIMARY KEY CLUSTERED (_PARCEL_FISCAL_HIST_ID);
ALTER TABLE PIMS_PARCEL_FISCAL_HIST ADD CONSTRAINT PIMS_PRFSCL_H_UK UNIQUE (_PARCEL_FISCAL_HIST_ID, END_DATE_HIST)
GO

-- PIMS_PARCEL_PARCEL_HIST
CREATE SEQUENCE [dbo].[PIMS_PARCEL_PARCEL_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_PARCEL_PARCEL_HIST](
  _PARCEL_PARCEL_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_PARCEL_PARCEL_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [PARCEL_PARCEL_ID] bigint NOT NULL, [PARCEL_ID] bigint NOT NULL, [SUBDIVISION_PARCEL_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_PARCEL_PARCEL_HIST ADD CONSTRAINT PIMS_PRCPRC_H_PK PRIMARY KEY CLUSTERED (_PARCEL_PARCEL_HIST_ID);
ALTER TABLE PIMS_PARCEL_PARCEL_HIST ADD CONSTRAINT PIMS_PRCPRC_H_UK UNIQUE (_PARCEL_PARCEL_HIST_ID, END_DATE_HIST)
GO

-- PIMS_ROLE_CLAIM_HIST
CREATE SEQUENCE [dbo].[PIMS_ROLE_CLAIM_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_ROLE_CLAIM_HIST](
  _ROLE_CLAIM_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_ROLE_CLAIM_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [ROLE_CLAIM_ID] bigint NOT NULL, [ROLE_ID] bigint NOT NULL, [CLAIM_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_ROLE_CLAIM_HIST ADD CONSTRAINT PIMS_ROLCLM_H_PK PRIMARY KEY CLUSTERED (_ROLE_CLAIM_HIST_ID);
ALTER TABLE PIMS_ROLE_CLAIM_HIST ADD CONSTRAINT PIMS_ROLCLM_H_UK UNIQUE (_ROLE_CLAIM_HIST_ID, END_DATE_HIST)
GO

-- PIMS_USER_HIST
CREATE SEQUENCE [dbo].[PIMS_USER_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_USER_HIST](
  _USER_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_USER_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [USER_ID] bigint NOT NULL, [USER_UID] uniqueidentifier NULL, [USERNAME] nvarchar(25) NOT NULL, [DISPLAY_NAME] nvarchar(100) NOT NULL, [FIRST_NAME] nvarchar(100) NOT NULL, [MIDDLE_NAME] nvarchar(100) NULL, [LAST_NAME] nvarchar(100) NOT NULL, [EMAIL] nvarchar(100) NOT NULL, [POSITION] nvarchar(100) NULL, [EMAIL_VERIFIED] bit NOT NULL, [NOTE] nvarchar(1000) NULL, [IS_SYSTEM] bit NOT NULL, [LAST_LOGIN] datetime NULL, [APPROVED_ON] datetime NULL, [IS_DISABLED] bit NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_USER_HIST ADD CONSTRAINT PIMS_USER_H_PK PRIMARY KEY CLUSTERED (_USER_HIST_ID);
ALTER TABLE PIMS_USER_HIST ADD CONSTRAINT PIMS_USER_H_UK UNIQUE (_USER_HIST_ID, END_DATE_HIST)
GO

-- PIMS_USER_AGENCY_HIST
CREATE SEQUENCE [dbo].[PIMS_USER_AGENCY_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_USER_AGENCY_HIST](
  _USER_AGENCY_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_USER_AGENCY_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [USER_AGENCY_ID] bigint NOT NULL, [USER_ID] bigint NOT NULL, [AGENCY_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_USER_AGENCY_HIST ADD CONSTRAINT PIMS_USRAGC_H_PK PRIMARY KEY CLUSTERED (_USER_AGENCY_HIST_ID);
ALTER TABLE PIMS_USER_AGENCY_HIST ADD CONSTRAINT PIMS_USRAGC_H_UK UNIQUE (_USER_AGENCY_HIST_ID, END_DATE_HIST)
GO

-- PIMS_USER_ROLE_HIST
CREATE SEQUENCE [dbo].[PIMS_USER_ROLE_H_ID_SEQ] AS [bigint] START WITH 1 INCREMENT BY 1 MINVALUE 1 MAXVALUE 2147483647 CACHE 50;

CREATE TABLE [dbo].[PIMS_USER_ROLE_HIST](
  _USER_ROLE_HIST_ID [bigint] DEFAULT (NEXT VALUE FOR [PIMS_USER_ROLE_H_ID_SEQ]) NOT NULL
  , EFFECTIVE_DATE_HIST [datetime] NOT NULL default getutcdate()
  , END_DATE_HIST [datetime]
  , [USER_ROLE_ID] bigint NOT NULL, [USER_ID] bigint NOT NULL, [ROLE_ID] bigint NOT NULL, [CONCURRENCY_CONTROL_NUMBER] bigint NOT NULL, [APP_CREATE_TIMESTAMP] datetime NOT NULL, [APP_CREATE_USERID] nvarchar(30) NOT NULL, [APP_CREATE_USER_GUID] uniqueidentifier NULL, [APP_CREATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [APP_LAST_UPDATE_USERID] nvarchar(30) NOT NULL, [APP_LAST_UPDATE_USER_GUID] uniqueidentifier NULL, [APP_LAST_UPDATE_USER_DIRECTORY] nvarchar(30) NOT NULL, [DB_CREATE_TIMESTAMP] datetime NOT NULL, [DB_CREATE_USERID] nvarchar(30) NOT NULL, [DB_LAST_UPDATE_TIMESTAMP] datetime NOT NULL, [DB_LAST_UPDATE_USERID] nvarchar(30) NOT NULL
  )
ALTER TABLE PIMS_USER_ROLE_HIST ADD CONSTRAINT PIMS_USRROL_H_PK PRIMARY KEY CLUSTERED (_USER_ROLE_HIST_ID);
ALTER TABLE PIMS_USER_ROLE_HIST ADD CONSTRAINT PIMS_USRROL_H_UK UNIQUE (_USER_ROLE_HIST_ID, END_DATE_HIST)
GO
