/* *****************************************************************************
Migrate the address information for PIMS_PERSON and PIMS_ORGANIZATION from the 
temporary tables to populate the new associative entites.
***************************************************************************** */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the new assocaitive entity for PERSONs
INSERT INTO [dbo].[PIMS_PERSON_ADDRESS] (PERSON_ID, ADDRESS_ID, ADDRESS_USAGE_TYPE_CODE)
SELECT PERSON_ID
     , ADDRESS_ID
     , ADDRESS_USAGE_TYPE_CODE
FROM   TMP_PERSON_ADDRESS
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the new assocaitive entity for ORGANIZATIONs
INSERT INTO [dbo].[PIMS_ORGANIZATION_ADDRESS] (ORGANIZATION_ID, ADDRESS_ID, ADDRESS_USAGE_TYPE_CODE)
SELECT ORGANIZATION_ID
     , ADDRESS_ID
     , ADDRESS_USAGE_TYPE_CODE
FROM   TMP_ORGANIZATION_ADDRESS
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop the existing temporary table for PERSONs
DROP TABLE IF EXISTS [dbo].[TMP_PERSON_ADDRESS] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO       

-- Drop the existing temporary table for ORGANIZATIONs
DROP TABLE IF EXISTS [dbo].[TMP_ORGANIZATION_ADDRESS] 
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
