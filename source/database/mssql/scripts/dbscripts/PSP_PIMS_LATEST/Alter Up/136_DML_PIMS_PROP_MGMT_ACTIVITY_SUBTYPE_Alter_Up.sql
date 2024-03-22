/* -----------------------------------------------------------------------------
Alter the data in the PIMS_PROP_MGMT_ACTIVITY_SUBTYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Feb-26  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Insert the new data source types
INSERT INTO PIMS_PROP_MGMT_ACTIVITY_SUBTYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, PROP_MGMT_ACTIVITY_SUBTYPE_CODE, DESCRIPTION)
VALUES
  (N'UTILITYBILL', N'ELECTRICITYBILL', N'Electricity'),
  (N'UTILITYBILL', N'GASBILL',         N'Gas'),
  (N'UTILITYBILL', N'INTERNETBILL',    N'Internet'),
  (N'UTILITYBILL', N'SEWERWATERBILL',  N'Sewer and Water'),
  (N'UTILITYBILL', N'TELEPHONEBILL',   N'Telephone'),
  (N'TAXESLEVIES', N'MUNIPROPTAX',     N'Municipal property taxes'),
  (N'TAXESLEVIES', N'WATERTAX',        N'Water'),
  (N'TAXESLEVIES', N'OTHERTAX',        N'Other');
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

