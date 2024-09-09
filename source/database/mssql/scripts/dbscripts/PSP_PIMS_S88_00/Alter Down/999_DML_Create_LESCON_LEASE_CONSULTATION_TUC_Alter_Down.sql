/* -----------------------------------------------------------------------------
Create the LESCON_LEASE_CONSULTATION_TUC constraint.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Aug-23  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the LESCON_LEASE_CONSULTATION_TUC constraint
PRINT N'Create the LESCON_LEASE_CONSULTATION_TUC constraint'
GO
ALTER TABLE PIMS_LEASE_CONSULTATION
	ADD CONSTRAINT LESCON_LEASE_CONSULTATION_TUC
	UNIQUE (CONSULTATION_TYPE_CODE, LEASE_ID)
	WITH (
		DATA_COMPRESSION = NONE
	) ON [PRIMARY]
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
