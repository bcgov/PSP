/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_DATA_SOURCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DATA_SOURCE_TYPE
GO

INSERT INTO PIMS_DATA_SOURCE_TYPE (DATA_SOURCE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OPSS', N'Operational Spreadsheet'),
  (N'LIS', N'Lease Information System (LIS)'),
  (N'PAIMS', N'Property Acquisition and Inventory Management System (PAIMS)'),
  (N'GAZ', N'BC Gazette');

-- Create foreign key constraint PIM_PIDSRT_PIM_PRPRTY_FK
PRINT N'Create foreign key constraint PIM_PIDSRT_PIM_PRPRTY_FK'
GO
ALTER TABLE [dbo].[PIMS_PROPERTY]
	ADD FOREIGN KEY([PROPERTY_DATA_SOURCE_TYPE_CODE])
	REFERENCES [dbo].[PIMS_DATA_SOURCE_TYPE]([DATA_SOURCE_TYPE_CODE])
	ON DELETE NO ACTION 
	ON UPDATE NO ACTION 
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO