/* -----------------------------------------------------------------------------
Insert data into PIMS_DATA_SOURCE_TYPE.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version.
Doug Filteau  2024-Mar-22  Added LIS_OPSS_PAIMS_PMBC.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Ensure the code value does not already exist in the table

-- Create PMBC schema
IF NOT EXISTS (SELECT * 
               FROM   sys.schemas 
               WHERE  name = N'pmbc')
  EXEC('CREATE SCHEMA [pmbc]');
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
