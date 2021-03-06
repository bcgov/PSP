PRINT N'Replace Insert Triggers'
GO

-- PIMS_ACCESS_REQUEST
CREATE TRIGGER PIMS_ACCRQT_I_S_I_TR ON PIMS_ACCESS_REQUEST INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ACCESS_REQUEST (
    [ACCESS_REQUEST_ID]
    , [USER_ID]
    , [NOTE]
    , [STATUS]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ACCESS_REQUEST_ID]
    , [USER_ID]
    , [NOTE]
    , [STATUS]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_ACCESS_REQUEST_AGENCY
CREATE TRIGGER PIMS_ACRQAG_I_S_I_TR ON PIMS_ACCESS_REQUEST_AGENCY INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ACCESS_REQUEST_AGENCY (
    [ACCESS_REQUEST_AGENCY_ID]
    , [ACCESS_REQUEST_ID]
    , [AGENCY_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ACCESS_REQUEST_AGENCY_ID]
    , [ACCESS_REQUEST_ID]
    , [AGENCY_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_ACCESS_REQUEST_ROLE
CREATE TRIGGER PIMS_ACCRQR_I_S_I_TR ON PIMS_ACCESS_REQUEST_ROLE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ACCESS_REQUEST_ROLE (
    [ACCESS_REQUEST_ROLE_ID]
    , [ROLE_ID]
    , [ACCESS_REQUEST_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ACCESS_REQUEST_ROLE_ID]
    , [ROLE_ID]
    , [ACCESS_REQUEST_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_ADDRESS
CREATE TRIGGER PIMS_ADDR_I_S_I_TR ON PIMS_ADDRESS INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ADDRESS (
    [ADDRESS_ID]
    , [PROVINCE_ID]
    , [ADDRESS1]
    , [ADDRESS2]
    , [POSTAL]
    , [ADMINISTRATIVE_AREA]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ADDRESS_ID]
    , [PROVINCE_ID]
    , [ADDRESS1]
    , [ADDRESS2]
    , [POSTAL]
    , [ADMINISTRATIVE_AREA]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_ADMINISTRATIVE_AREA
CREATE TRIGGER PIMS_ADMINA_I_S_I_TR ON PIMS_ADMINISTRATIVE_AREA INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ADMINISTRATIVE_AREA (
    [ADMINISTRATIVE_AREA_ID]
    , [NAME]
    , [GROUP_NAME]
    , [ABBREVIATION]
    , [BOUNDARY_TYPE]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ADMINISTRATIVE_AREA_ID]
    , [NAME]
    , [GROUP_NAME]
    , [ABBREVIATION]
    , [BOUNDARY_TYPE]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_AGENCY
CREATE TRIGGER PIMS_AGNCY_I_S_I_TR ON PIMS_AGENCY INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_AGENCY (
    [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [NAME]
    , [DESCRIPTION]
    , [CODE]
    , [EMAIL]
    , [SEND_EMAIL]
    , [ADDRESS_TO]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [AGENCY_ID]
    , [PARENT_AGENCY_ID]
    , [NAME]
    , [DESCRIPTION]
    , [CODE]
    , [EMAIL]
    , [SEND_EMAIL]
    , [ADDRESS_TO]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_BUILDING
CREATE TRIGGER PIMS_BUILDG_I_S_I_TR ON PIMS_BUILDING INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_BUILDING (
    [BUILDING_ID]
    , [AGENCY_ID]
    , [PROPERTY_TYPE_ID]
    , [PROPERTY_CLASSIFICATION_ID]
    , [BUILDING_CONSTRUCTION_TYPE_ID]
    , [BUILDING_PREDOMINATE_USE_ID]
    , [BUILDING_OCCUPANT_TYPE_ID]
    , [ADDRESS_ID]
    , [NAME]
    , [DESCRIPTION]
    , [OCCUPANT_NAME]
    , [BUILDING_FLOOR_COUNT]
    , [BUILDING_TENANCY]
    , [BUILDING_TENANCY_UPDATED_ON]
    , [TOTAL_AREA]
    , [RENTABLE_AREA]
    , [PROJECT_NUMBERS]
    , [LEASE_EXPIRY]
    , [TRANSFER_LEASE_ON_SALE]
    , [BOUNDARY]
    , [LOCATION]
    , [ENCUMBRANCE_REASON]
    , [LEASED_LAND_METADATA]
    , [IS_VISIBLE_TO_OTHER_AGENCIES]
    , [IS_SENSITIVE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [BUILDING_ID]
    , [AGENCY_ID]
    , [PROPERTY_TYPE_ID]
    , [PROPERTY_CLASSIFICATION_ID]
    , [BUILDING_CONSTRUCTION_TYPE_ID]
    , [BUILDING_PREDOMINATE_USE_ID]
    , [BUILDING_OCCUPANT_TYPE_ID]
    , [ADDRESS_ID]
    , [NAME]
    , [DESCRIPTION]
    , [OCCUPANT_NAME]
    , [BUILDING_FLOOR_COUNT]
    , [BUILDING_TENANCY]
    , [BUILDING_TENANCY_UPDATED_ON]
    , [TOTAL_AREA]
    , [RENTABLE_AREA]
    , [PROJECT_NUMBERS]
    , [LEASE_EXPIRY]
    , [TRANSFER_LEASE_ON_SALE]
    , [BOUNDARY]
    , [LOCATION]
    , [ENCUMBRANCE_REASON]
    , [LEASED_LAND_METADATA]
    , [IS_VISIBLE_TO_OTHER_AGENCIES]
    , [IS_SENSITIVE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_BUILDING_CONSTRUCTION_TYPE
CREATE TRIGGER PIMS_BLCNTY_I_S_I_TR ON PIMS_BUILDING_CONSTRUCTION_TYPE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_BUILDING_CONSTRUCTION_TYPE (
    [BUILDING_CONSTRUCTION_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [BUILDING_CONSTRUCTION_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_BUILDING_EVALUATION
CREATE TRIGGER PIMS_BLDEVL_I_S_I_TR ON PIMS_BUILDING_EVALUATION INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_BUILDING_EVALUATION (
    [BUILDING_EVALUATION_ID]
    , [BUILDING_ID]
    , [DATE]
    , [KEY]
    , [VALUE]
    , [NOTE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [BUILDING_EVALUATION_ID]
    , [BUILDING_ID]
    , [DATE]
    , [KEY]
    , [VALUE]
    , [NOTE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_BUILDING_FISCAL
CREATE TRIGGER PIMS_BLDFSC_I_S_I_TR ON PIMS_BUILDING_FISCAL INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_BUILDING_FISCAL (
    [BUILDING_FISCAL_ID]
    , [BUILDING_ID]
    , [FISCAL_YEAR]
    , [KEY]
    , [VALUE]
    , [NOTE]
    , [EFFECTIVE_DATE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [BUILDING_FISCAL_ID]
    , [BUILDING_ID]
    , [FISCAL_YEAR]
    , [KEY]
    , [VALUE]
    , [NOTE]
    , [EFFECTIVE_DATE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_BUILDING_OCCUPANT_TYPE
CREATE TRIGGER PIMS_BLOCCT_I_S_I_TR ON PIMS_BUILDING_OCCUPANT_TYPE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_BUILDING_OCCUPANT_TYPE (
    [BUILDING_OCCUPANT_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [BUILDING_OCCUPANT_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_BUILDING_PREDOMINATE_USE
CREATE TRIGGER PIMS_BLPRDU_I_S_I_TR ON PIMS_BUILDING_PREDOMINATE_USE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_BUILDING_PREDOMINATE_USE (
    [BUILDING_PREDOMINATE_USE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [BUILDING_PREDOMINATE_USE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_CLAIM
CREATE TRIGGER PIMS_CLAIM_I_S_I_TR ON PIMS_CLAIM INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_CLAIM (
    [CLAIM_ID]
    , [CLAIM_UID]
    , [NAME]
    , [KEYCLOAK_ROLE_ID]
    , [DESCRIPTION]
    , [IS_DISABLED]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [CLAIM_ID]
    , [CLAIM_UID]
    , [NAME]
    , [KEYCLOAK_ROLE_ID]
    , [DESCRIPTION]
    , [IS_DISABLED]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_NOTIFICATION_QUEUE
CREATE TRIGGER PIMS_NOTIFQ_I_S_I_TR ON PIMS_NOTIFICATION_QUEUE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_NOTIFICATION_QUEUE (
    [NOTIFICATION_QUEUE_ID]
    , [TO_AGENCY_ID]
    , [NOTIFICATION_TEMPLATE_ID]
    , [KEY]
    , [STATUS]
    , [PRIORITY]
    , [ENCODING]
    , [SEND_ON]
    , [TO]
    , [SUBJECT]
    , [BODY_TYPE]
    , [BODY]
    , [BCC]
    , [CC]
    , [TAG]
    , [CHES_MESSAGE_ID]
    , [CHES_TRANSACTION_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [NOTIFICATION_QUEUE_ID]
    , [TO_AGENCY_ID]
    , [NOTIFICATION_TEMPLATE_ID]
    , [KEY]
    , [STATUS]
    , [PRIORITY]
    , [ENCODING]
    , [SEND_ON]
    , [TO]
    , [SUBJECT]
    , [BODY_TYPE]
    , [BODY]
    , [BCC]
    , [CC]
    , [TAG]
    , [CHES_MESSAGE_ID]
    , [CHES_TRANSACTION_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_NOTIFICATION_TEMPLATE
CREATE TRIGGER PIMS_NTTMPL_I_S_I_TR ON PIMS_NOTIFICATION_TEMPLATE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_NOTIFICATION_TEMPLATE (
    [NOTIFICATION_TEMPLATE_ID]
    , [NAME]
    , [DESCRIPTION]
    , [TO]
    , [CC]
    , [BCC]
    , [AUDIENCE]
    , [ENCODING]
    , [BODY_TYPE]
    , [PRIORITY]
    , [SUBJECT]
    , [BODY]
    , [TAG]
    , [IS_DISABLED]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [NOTIFICATION_TEMPLATE_ID]
    , [NAME]
    , [DESCRIPTION]
    , [TO]
    , [CC]
    , [BCC]
    , [AUDIENCE]
    , [ENCODING]
    , [BODY_TYPE]
    , [PRIORITY]
    , [SUBJECT]
    , [BODY]
    , [TAG]
    , [IS_DISABLED]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PARCEL
CREATE TRIGGER PIMS_PARCEL_I_S_I_TR ON PIMS_PARCEL INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PARCEL (
    [PARCEL_ID]
    , [AGENCY_ID]
    , [PROPERTY_CLASSIFICATION_ID]
    , [PROPERTY_TYPE_ID]
    , [ADDRESS_ID]
    , [NAME]
    , [DESCRIPTION]
    , [PID]
    , [PIN]
    , [LAND_AREA]
    , [LAND_LEGAL_DESCRIPTION]
    , [ZONING]
    , [ZONING_POTENTIAL]
    , [NOT_OWNED]
    , [BOUNDARY]
    , [LOCATION]
    , [ENCUMBRANCE_REASON]
    , [PROJECT_NUMBERS]
    , [IS_VISIBLE_TO_OTHER_AGENCIES]
    , [IS_SENSITIVE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PARCEL_ID]
    , [AGENCY_ID]
    , [PROPERTY_CLASSIFICATION_ID]
    , [PROPERTY_TYPE_ID]
    , [ADDRESS_ID]
    , [NAME]
    , [DESCRIPTION]
    , [PID]
    , [PIN]
    , [LAND_AREA]
    , [LAND_LEGAL_DESCRIPTION]
    , [ZONING]
    , [ZONING_POTENTIAL]
    , [NOT_OWNED]
    , [BOUNDARY]
    , [LOCATION]
    , [ENCUMBRANCE_REASON]
    , [PROJECT_NUMBERS]
    , [IS_VISIBLE_TO_OTHER_AGENCIES]
    , [IS_SENSITIVE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PARCEL_BUILDING
CREATE TRIGGER PIMS_PRCLBL_I_S_I_TR ON PIMS_PARCEL_BUILDING INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PARCEL_BUILDING (
    [PARCEL_BUILDING_ID]
    , [PARCEL_ID]
    , [BUILDING_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PARCEL_BUILDING_ID]
    , [PARCEL_ID]
    , [BUILDING_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PARCEL_EVALUATION
CREATE TRIGGER PIMS_PREVAL_I_S_I_TR ON PIMS_PARCEL_EVALUATION INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;


  INSERT INTO PIMS_PARCEL_EVALUATION (
    [PARCEL_EVALUATION_ID]
    , [PARCEL_ID]
    , [DATE]
    , [KEY]
    , [FIRM]
    , [VALUE]
    , [NOTE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PARCEL_EVALUATION_ID]
    , [PARCEL_ID]
    , [DATE]
    , [KEY]
    , [FIRM]
    , [VALUE]
    , [NOTE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PARCEL_FISCAL
CREATE TRIGGER PIMS_PRFSCL_I_S_I_TR ON PIMS_PARCEL_FISCAL INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PARCEL_FISCAL (
    [PARCEL_FISCAL_ID]
    , [PARCEL_ID]
    , [FISCAL_YEAR]
    , [KEY]
    , [VALUE]
    , [NOTE]
    , [EFFECTIVE_DATE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PARCEL_FISCAL_ID]
    , [PARCEL_ID]
    , [FISCAL_YEAR]
    , [KEY]
    , [VALUE]
    , [NOTE]
    , [EFFECTIVE_DATE]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PARCEL_PARCEL
CREATE TRIGGER PIMS_PRCPRC_I_S_I_TR ON PIMS_PARCEL_PARCEL INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PARCEL_PARCEL (
    [PARCEL_PARCEL_ID]
    , [PARCEL_ID]
    , [SUBDIVISION_PARCEL_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PARCEL_PARCEL_ID]
    , [PARCEL_ID]
    , [SUBDIVISION_PARCEL_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PROPERTY_CLASSIFICATION
CREATE TRIGGER PIMS_PRPCLS_I_S_I_TR ON PIMS_PROPERTY_CLASSIFICATION INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PROPERTY_CLASSIFICATION (
    [PROPERTY_CLASSIFICATION_ID]
    , [NAME]
    , [IS_VISIBLE]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PROPERTY_CLASSIFICATION_ID]
    , [NAME]
    , [IS_VISIBLE]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PROPERTY_TYPE
CREATE TRIGGER PIMS_PRPTYP_I_S_I_TR ON PIMS_PROPERTY_TYPE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PROPERTY_TYPE (
    [PROPERTY_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PROPERTY_TYPE_ID]
    , [NAME]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_PROVINCE
CREATE TRIGGER PIMS_PROV_I_S_I_TR ON PIMS_PROVINCE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_PROVINCE (
    [PROVINCE_ID]
    , [PROVINCE_CODE]
    , [NAME]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [PROVINCE_ID]
    , [PROVINCE_CODE]
    , [NAME]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_ROLE
CREATE TRIGGER PIMS_ROLE_I_S_I_TR ON PIMS_ROLE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ROLE (
    [ROLE_ID]
    , [ROLE_UID]
    , [NAME]
    , [KEYCLOAK_GROUP_ID]
    , [DESCRIPTION]
    , [IS_PUBLIC]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ROLE_ID]
    , [ROLE_UID]
    , [NAME]
    , [KEYCLOAK_GROUP_ID]
    , [DESCRIPTION]
    , [IS_PUBLIC]
    , [IS_DISABLED]
    , [DISPLAY_ORDER]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_ROLE_CLAIM
CREATE TRIGGER PIMS_ROLCLM_I_S_I_TR ON PIMS_ROLE_CLAIM INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_ROLE_CLAIM (
    [ROLE_CLAIM_ID]
    , [ROLE_ID]
    , [CLAIM_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [ROLE_CLAIM_ID]
    , [ROLE_ID]
    , [CLAIM_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_TENANT
CREATE TRIGGER PIMS_TENANT_I_S_I_TR ON PIMS_TENANT INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_TENANT (
    [TENANT_ID]
    , [CODE]
    , [NAME]
    , [DESCRIPTION]
    , [SETTINGS]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [TENANT_ID]
    , [CODE]
    , [NAME]
    , [DESCRIPTION]
    , [SETTINGS]
    , [CONCURRENCY_CONTROL_NUMBER]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_USER
CREATE TRIGGER PIMS_USER_I_S_I_TR ON PIMS_USER INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_USER (
    [USER_ID]
    , [USER_UID]
    , [USERNAME]
    , [DISPLAY_NAME]
    , [FIRST_NAME]
    , [MIDDLE_NAME]
    , [LAST_NAME]
    , [EMAIL]
    , [POSITION]
    , [EMAIL_VERIFIED]
    , [NOTE]
    , [IS_SYSTEM]
    , [LAST_LOGIN]
    , [APPROVED_ON]
    , [IS_DISABLED]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [USER_ID]
    , [USER_UID]
    , [USERNAME]
    , [DISPLAY_NAME]
    , [FIRST_NAME]
    , [MIDDLE_NAME]
    , [LAST_NAME]
    , [EMAIL]
    , [POSITION]
    , [EMAIL_VERIFIED]
    , [NOTE]
    , [IS_SYSTEM]
    , [LAST_LOGIN]
    , [APPROVED_ON]
    , [IS_DISABLED]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_USER_AGENCY
CREATE TRIGGER PIMS_USRAGC_I_S_I_TR ON PIMS_USER_AGENCY INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_USER_AGENCY (
    [USER_AGENCY_ID]
    , [USER_ID]
    , [AGENCY_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [USER_AGENCY_ID]
    , [USER_ID]
    , [AGENCY_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO

-- PIMS_USER_ROLE
CREATE TRIGGER PIMS_USRROL_I_S_I_TR ON PIMS_USER_ROLE INSTEAD OF INSERT AS
SET NOCOUNT ON
BEGIN TRY
  IF NOT EXISTS(SELECT * FROM inserted)
    RETURN;

  INSERT INTO PIMS_USER_ROLE (
    [USER_ROLE_ID]
    , [USER_ID]
    , [ROLE_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , [DB_CREATE_TIMESTAMP]
    , [DB_CREATE_USERID]
  )
  SELECT [USER_ROLE_ID]
    , [USER_ID]
    , [ROLE_ID]
    , [CONCURRENCY_CONTROL_NUMBER]
    , [APP_CREATE_TIMESTAMP]
    , [APP_CREATE_USERID]
    , [APP_CREATE_USER_GUID]
    , [APP_CREATE_USER_DIRECTORY]
    , [APP_LAST_UPDATE_TIMESTAMP]
    , [APP_LAST_UPDATE_USERID]
    , [APP_LAST_UPDATE_USER_GUID]
    , [APP_LAST_UPDATE_USER_DIRECTORY]
    , getutcdate()
    , user_name()
  FROM inserted;

END TRY
BEGIN CATCH
   IF @@trancount > 0 ROLLBACK TRANSACTION
   EXEC pims_error_handling
END CATCH;
GO
