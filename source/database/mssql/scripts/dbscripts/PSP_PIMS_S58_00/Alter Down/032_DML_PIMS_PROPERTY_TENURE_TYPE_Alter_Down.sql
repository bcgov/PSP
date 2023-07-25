/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROPERTY_TENURE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-20  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Enable "ADJLAND" type
UPDATE PIMS_PROPERTY_TENURE_TYPE
SET    IS_DISABLED                = CONVERT([bit],(0))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  PROPERTY_TENURE_TYPE_CODE = N'ADJLAND';
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Delete multiple types
DELETE
FROM   PIMS_PROPERTY_TENURE_TYPE
WHERE  PROPERTY_TENURE_TYPE_CODE IN (N'FSMOTI', N'FSBCTFA', N'FSPRIVAT', N'FSCROWN', N'NSRWMOTI', N'NSRWBCTFA', N'SRWMOTI', N'SRWBCTFA', N'SRWOTHER', N'LNDACTR', N'SPECUPMT', N'IRESERVE', N'UNCRWNLMD', N'LEASELIC');
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
