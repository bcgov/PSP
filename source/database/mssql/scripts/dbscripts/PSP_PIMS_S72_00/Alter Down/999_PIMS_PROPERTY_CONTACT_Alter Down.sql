-- ----------------------------------------------------------------------------------
-- Migrate the contact data from the PIMS_PROPERTY_CONTACT table to the PIMS_PROPERTY
-- table.
-- ----------------------------------------------------------------------------------

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the PROPERTY_MANAGER_ID data
PRINT N'Migrate the PROPERTY_MANAGER_ID data'
GO
UPDATE prop
SET    prop.PROPERTY_MANAGER_ID        = cnct.PERSON_ID
     , prop.CONCURRENCY_CONTROL_NUMBER = prop.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY         prop join
       PIMS_PROPERTY_CONTACT cnct ON cnct.PROPERTY_ID = prop.PROPERTY_ID
WHERE  cnct.PERSON_ID           IS NOT NULL
   AND prop.PROPERTY_MANAGER_ID IS NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the ORGANIZATION_ID data
PRINT N'Migrate the ORGANIZATION_ID data'
GO
UPDATE prop
SET    prop.PROP_MGMT_ORG_ID           = cnct.ORGANIZATION_ID
     , prop.CONCURRENCY_CONTROL_NUMBER = prop.CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY         prop join
       PIMS_PROPERTY_CONTACT cnct ON cnct.PROPERTY_ID = prop.PROPERTY_ID
WHERE  cnct.ORGANIZATION_ID  IS NOT NULL
   AND prop.PROP_MGMT_ORG_ID IS NULL
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
