SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

ALTER TABLE [dbo].[PIMS_NOTIFICATION]
  ADD [LEASE_PERIOD_ID] BIGINT NULL;


GO
ALTER TABLE [dbo].[PIMS_NOTIFICATION_HIST]
  ADD [LEASE_PERIOD_ID] BIGINT NULL;


GO
ALTER TABLE [dbo].[PIMS_NOTIFICATION]
  ADD CONSTRAINT [PIM_LSPERD_PIM_NOTIFY_FK] FOREIGN KEY ([LEASE_PERIOD_ID]) REFERENCES [dbo].[PIMS_LEASE_PERIOD] ([LEASE_PERIOD_ID]) ON DELETE NO ACTION ON UPDATE NO ACTION;


GO
CREATE NONCLUSTERED INDEX [NOTIFY_LEASE_PERIOD_ID_IDX]
  ON [dbo].[PIMS_NOTIFICATION]([LEASE_PERIOD_ID]);


GO
-- Preserve LEASE_PERIOD_ID when inserting notifications.
ALTER TRIGGER [dbo].[PIMS_NOTIFY_I_S_I_TR]
  ON [dbo].[PIMS_NOTIFICATION]
  INSTEAD OF INSERT
  AS SET NOCOUNT ON;
     BEGIN TRY
       IF NOT EXISTS (SELECT *
                      FROM   inserted)
         RETURN;
       INSERT INTO dbo.PIMS_NOTIFICATION (
         NOTIFICATION_ID,
         NOTIFICATION_TYPE_CODE,
         ACQUISITION_FILE_ID,
         DISPOSITION_FILE_ID,
         RESEARCH_FILE_ID,
         MANAGEMENT_FILE_ID,
         LEASE_ID,
         LEASE_PERIOD_ID,
         TAKE_ID,
         INSURANCE_ID,
         LEASE_CONSULTATION_ID,
         NOTICE_OF_CLAIM_ID,
         LEASE_RENEWAL_ID,
         EXPROP_OWNER_HISTORY_ID,
         AGREEMENT_ID,
         NOTIFICATION_TRIGGER_DATE,
         NOTIFICATION_MESSAGE,
         CONCURRENCY_CONTROL_NUMBER,
         APP_CREATE_TIMESTAMP,
         APP_CREATE_USERID,
         APP_CREATE_USER_GUID,
         APP_CREATE_USER_DIRECTORY,
         APP_LAST_UPDATE_TIMESTAMP,
         APP_LAST_UPDATE_USERID,
         APP_LAST_UPDATE_USER_GUID,
         APP_LAST_UPDATE_USER_DIRECTORY
       )
       SELECT NOTIFICATION_ID,
              NOTIFICATION_TYPE_CODE,
              ACQUISITION_FILE_ID,
              DISPOSITION_FILE_ID,
              RESEARCH_FILE_ID,
              MANAGEMENT_FILE_ID,
              LEASE_ID,
              LEASE_PERIOD_ID,
              TAKE_ID,
              INSURANCE_ID,
              LEASE_CONSULTATION_ID,
              NOTICE_OF_CLAIM_ID,
              LEASE_RENEWAL_ID,
              EXPROP_OWNER_HISTORY_ID,
              AGREEMENT_ID,
              NOTIFICATION_TRIGGER_DATE,
              NOTIFICATION_MESSAGE,
              CONCURRENCY_CONTROL_NUMBER,
              APP_CREATE_TIMESTAMP,
              APP_CREATE_USERID,
              APP_CREATE_USER_GUID,
              APP_CREATE_USER_DIRECTORY,
              APP_LAST_UPDATE_TIMESTAMP,
              APP_LAST_UPDATE_USERID,
              APP_LAST_UPDATE_USER_GUID,
              APP_LAST_UPDATE_USER_DIRECTORY
       FROM   inserted;
     END TRY
     BEGIN CATCH
       IF @@TRANCOUNT > 0
         ROLLBACK;
       EXECUTE pims_error_handling ;
     END CATCH


GO
IF @@ERROR <> 0
  SET NOEXEC ON;


GO
-- Preserve LEASE_PERIOD_ID when updating notifications.
ALTER TRIGGER [dbo].[PIMS_NOTIFY_I_S_U_TR]
  ON [dbo].[PIMS_NOTIFICATION]
  INSTEAD OF UPDATE
  AS SET NOCOUNT ON;
     BEGIN TRY
       IF NOT EXISTS (SELECT *
                      FROM   deleted)
         RETURN;
       -- Validate concurrency control.
       IF EXISTS (SELECT 1
                  FROM   inserted AS i
                         INNER JOIN
                         deleted AS d
                         ON i.NOTIFICATION_ID = d.NOTIFICATION_ID
                  WHERE  i.CONCURRENCY_CONTROL_NUMBER != d.CONCURRENCY_CONTROL_NUMBER + 1)
         RAISERROR ('CONCURRENCY FAILURE.', 16, 1);
       UPDATE n
       SET    NOTIFICATION_ID                = i.NOTIFICATION_ID,
              NOTIFICATION_TYPE_CODE         = i.NOTIFICATION_TYPE_CODE,
              ACQUISITION_FILE_ID            = i.ACQUISITION_FILE_ID,
              DISPOSITION_FILE_ID            = i.DISPOSITION_FILE_ID,
              RESEARCH_FILE_ID               = i.RESEARCH_FILE_ID,
              MANAGEMENT_FILE_ID             = i.MANAGEMENT_FILE_ID,
              LEASE_ID                       = i.LEASE_ID,
              LEASE_PERIOD_ID                = i.LEASE_PERIOD_ID,
              TAKE_ID                        = i.TAKE_ID,
              INSURANCE_ID                   = i.INSURANCE_ID,
              LEASE_CONSULTATION_ID          = i.LEASE_CONSULTATION_ID,
              NOTICE_OF_CLAIM_ID             = i.NOTICE_OF_CLAIM_ID,
              LEASE_RENEWAL_ID               = i.LEASE_RENEWAL_ID,
              EXPROP_OWNER_HISTORY_ID        = i.EXPROP_OWNER_HISTORY_ID,
              AGREEMENT_ID                   = i.AGREEMENT_ID,
              NOTIFICATION_TRIGGER_DATE      = i.NOTIFICATION_TRIGGER_DATE,
              NOTIFICATION_MESSAGE           = i.NOTIFICATION_MESSAGE,
              CONCURRENCY_CONTROL_NUMBER     = i.CONCURRENCY_CONTROL_NUMBER,
              APP_LAST_UPDATE_TIMESTAMP      = i.APP_LAST_UPDATE_TIMESTAMP,
              APP_LAST_UPDATE_USERID         = i.APP_LAST_UPDATE_USERID,
              APP_LAST_UPDATE_USER_GUID      = i.APP_LAST_UPDATE_USER_GUID,
              APP_LAST_UPDATE_USER_DIRECTORY = i.APP_LAST_UPDATE_USER_DIRECTORY,
              DB_LAST_UPDATE_TIMESTAMP       = GETUTCDATE(),
              DB_LAST_UPDATE_USERID          = USER_NAME()
       FROM   dbo.PIMS_NOTIFICATION AS n
              INNER JOIN
              inserted AS i
              ON n.NOTIFICATION_ID = i.NOTIFICATION_ID;
     END TRY
     BEGIN CATCH
       IF @@TRANCOUNT > 0
         ROLLBACK;
       EXECUTE pims_error_handling ;
     END CATCH


GO
IF @@ERROR <> 0
  SET NOEXEC ON;


GO
-- Preserve LEASE_PERIOD_ID in notification history.
ALTER TRIGGER [dbo].[PIMS_NOTIFY_A_S_IUD_TR]
  ON [dbo].[PIMS_NOTIFICATION]
  FOR INSERT, UPDATE, DELETE
  AS SET NOCOUNT ON;
     BEGIN TRY
       DECLARE @curr_date AS DATETIME;
       SET @curr_date = GETUTCDATE();
       IF NOT EXISTS (SELECT *
                      FROM   inserted)
          AND NOT EXISTS (SELECT *
                          FROM   deleted)
         RETURN;
       -- Close the previous history record for updated/deleted notifications.
       IF EXISTS (SELECT *
                  FROM   deleted)
         UPDATE dbo.PIMS_NOTIFICATION_HIST
         SET    END_DATE_HIST = @curr_date
         WHERE  NOTIFICATION_ID IN (SELECT NOTIFICATION_ID
                                    FROM   deleted)
                AND END_DATE_HIST IS NULL;
       -- Record the new state for inserted/updated notifications.
       IF EXISTS (SELECT *
                  FROM   inserted)
         INSERT INTO dbo.PIMS_NOTIFICATION_HIST (
           NOTIFICATION_ID,
           NOTIFICATION_TYPE_CODE,
           ACQUISITION_FILE_ID,
           DISPOSITION_FILE_ID,
           RESEARCH_FILE_ID,
           MANAGEMENT_FILE_ID,
           LEASE_ID,
           LEASE_PERIOD_ID,
           TAKE_ID,
           INSURANCE_ID,
           LEASE_CONSULTATION_ID,
           NOTICE_OF_CLAIM_ID,
           LEASE_RENEWAL_ID,
           EXPROP_OWNER_HISTORY_ID,
           AGREEMENT_ID,
           NOTIFICATION_TRIGGER_DATE,
           NOTIFICATION_MESSAGE,
           CONCURRENCY_CONTROL_NUMBER,
           APP_CREATE_TIMESTAMP,
           APP_CREATE_USERID,
           APP_CREATE_USER_GUID,
           APP_CREATE_USER_DIRECTORY,
           APP_LAST_UPDATE_TIMESTAMP,
           APP_LAST_UPDATE_USERID,
           APP_LAST_UPDATE_USER_GUID,
           APP_LAST_UPDATE_USER_DIRECTORY,
           DB_CREATE_TIMESTAMP,
           DB_CREATE_USERID,
           DB_LAST_UPDATE_TIMESTAMP,
           DB_LAST_UPDATE_USERID,
           _NOTIFICATION_HIST_ID,
           END_DATE_HIST,
           EFFECTIVE_DATE_HIST
         )
         SELECT NOTIFICATION_ID,
                NOTIFICATION_TYPE_CODE,
                ACQUISITION_FILE_ID,
                DISPOSITION_FILE_ID,
                RESEARCH_FILE_ID,
                MANAGEMENT_FILE_ID,
                LEASE_ID,
                LEASE_PERIOD_ID,
                TAKE_ID,
                INSURANCE_ID,
                LEASE_CONSULTATION_ID,
                NOTICE_OF_CLAIM_ID,
                LEASE_RENEWAL_ID,
                EXPROP_OWNER_HISTORY_ID,
                AGREEMENT_ID,
                NOTIFICATION_TRIGGER_DATE,
                NOTIFICATION_MESSAGE,
                CONCURRENCY_CONTROL_NUMBER,
                APP_CREATE_TIMESTAMP,
                APP_CREATE_USERID,
                APP_CREATE_USER_GUID,
                APP_CREATE_USER_DIRECTORY,
                APP_LAST_UPDATE_TIMESTAMP,
                APP_LAST_UPDATE_USERID,
                APP_LAST_UPDATE_USER_GUID,
                APP_LAST_UPDATE_USER_DIRECTORY,
                DB_CREATE_TIMESTAMP,
                DB_CREATE_USERID,
                DB_LAST_UPDATE_TIMESTAMP,
                DB_LAST_UPDATE_USERID,
                 NEXT VALUE FOR dbo.PIMS_NOTIFICATION_H_ID_SEQ,
                NULL,
                @curr_date
         FROM   inserted;
     END TRY
     BEGIN CATCH
       IF @@TRANCOUNT > 0
         ROLLBACK;
       EXECUTE pims_error_handling ;
     END CATCH


GO
IF @@ERROR <> 0
  SET NOEXEC ON;

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
