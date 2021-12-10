/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_STATUS_TYPE
GO

INSERT INTO PIMS_LEASE_STATUS_TYPE (LEASE_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTIVE',     N'Active'),
  (N'EXPIRED',    N'Expired'),
  (N'TERMINATED', N'Terminated'),
  (N'DRAFT',      N'Draft');

-- Create foreign key constraint dbo.PIM_LSSTYP_PIM_LEASE_FK
PRINT N'Create foreign key constraint dbo.PIM_LSSTYP_PIM_LEASE_FK'
GO
ALTER TABLE [dbo].[PIMS_LEASE]
	ADD FOREIGN KEY([LEASE_STATUS_TYPE_CODE])
	REFERENCES [dbo].[PIMS_LEASE_STATUS_TYPE]([LEASE_STATUS_TYPE_CODE])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO