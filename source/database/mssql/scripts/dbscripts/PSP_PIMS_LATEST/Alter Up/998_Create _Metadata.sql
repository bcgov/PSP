/* -----------------------------------------------------------------------------
Generate the metadata for PIMS_PROPERTY_ACTIVITY_INVOICE
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-03  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Description of the invoice.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'DESCRIPTION'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'GST on the invoice.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'GST_AMT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Date of the invoice' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'INVOICE_DT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Number assigned to the invoice.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'INVOICE_NUM'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicates if the invoice is disabled.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'IS_DISABLED'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Indicates if the invoice requires PST.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'IS_PST_REQUIRED'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Subtotal of the invoice,' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'PRETAX_AMT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'PST on the invoice.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'PST_AMT'
GO
EXEC sp_addextendedproperty 
	@name = N'MS_Description', @value = N'Total cost of the invoice.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE', 
	@level2type = N'Column', @level2name = N'TOTAL_AMT'
GO
EXEC sp_updateextendedproperty 
	@name = N'MS_Description', @value = N'Defines the activities that are associated with this property.' , 
	@level0type = N'Schema', @level0name = N'dbo', 
	@level1type = N'Table', @level1name = N'PIMS_PROPERTY_ACTIVITY_INVOICE'
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
