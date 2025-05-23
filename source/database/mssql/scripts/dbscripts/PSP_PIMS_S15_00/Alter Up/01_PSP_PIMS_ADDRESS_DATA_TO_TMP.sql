/* *****************************************************************************
Migrate the address information for PIMS_PERSON and PIMS_ORGANIZATION into 
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

-- Drop the existing temporary table for PERSONs
DROP TABLE IF EXISTS [dbo].[TMP_PERSON_ADDRESS] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for PERSONs
CREATE TABLE [dbo].[TMP_PERSON_ADDRESS] (
    [PERSON_ID] BIGINT,
    [ADDRESS_ID] BIGINT,
    [ADDRESS_USAGE_TYPE_CODE] NVARCHAR(20) NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the tempoary entity for PERSONs
INSERT INTO [dbo].[TMP_PERSON_ADDRESS] (PERSON_ID, ADDRESS_ID, ADDRESS_USAGE_TYPE_CODE)
SELECT PER.PERSON_ID
     , ADR.ADDRESS_ID
     , ADR.ADDRESS_USAGE_TYPE_CODE
FROM   PIMS_PERSON  PER JOIN
       PIMS_ADDRESS ADR ON ADR.ADDRESS_ID = PER.ADDRESS_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
       

-- Drop the existing temporary table for ORGANIZATIONs
DROP TABLE IF EXISTS [dbo].[TMP_ORGANIZATION_ADDRESS] 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the temporary table for ORGANIZATIONs
CREATE TABLE [dbo].[TMP_ORGANIZATION_ADDRESS] (
    [ORGANIZATION_ID] BIGINT,
    [ADDRESS_ID] BIGINT,
    [ADDRESS_USAGE_TYPE_CODE] NVARCHAR(20) NOT NULL
)
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Migrate the existing data to the temporary entity for PERSONs
INSERT INTO [dbo].[TMP_ORGANIZATION_ADDRESS] (ORGANIZATION_ID, ADDRESS_ID, ADDRESS_USAGE_TYPE_CODE)
SELECT ORG.ORGANIZATION_ID
     , ADR.ADDRESS_ID
     , ADR.ADDRESS_USAGE_TYPE_CODE
FROM   PIMS_ORGANIZATION ORG JOIN
       PIMS_ADDRESS      ADR ON ADR.ADDRESS_ID = ORG.ADDRESS_ID
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
