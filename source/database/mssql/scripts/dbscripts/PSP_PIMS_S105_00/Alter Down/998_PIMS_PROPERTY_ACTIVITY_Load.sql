/* -----------------------------------------------------------------------------
Populate the PIMS_PROPERTY_ACTIVITY table from the 
TMP_PROPERTY_ACTIVITY temporary table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-May-16  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Populate the PIMS_PROPERTY_ACTIVITY table from the TMP_PROPERTY_ACTIVITY table.
PRINT N'Populate the PIMS_PROPERTY_ACTIVITY table from the TMP_PROPERTY_ACTIVITY table.'
GO
UPDATE ppa
SET    ppa.PROP_MGMT_ACTIVITY_SUBTYPE_CODE = tmp.PROP_MGMT_ACTIVITY_SUBTYPE_CODE
     , ppa.CONCURRENCY_CONTROL_NUMBER      = ppa.CONCURRENCY_CONTROL_NUMBER + 1
FROM   TMP_PROPERTY_ACTIVITY  tmp JOIN
       PIMS_PROPERTY_ACTIVITY ppa ON ppa.PIMS_PROPERTY_ACTIVITY_ID = tmp.PROPERTY_ACTIVITY_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create foreign key constraint dbo.PIM_PRACST_PIM_PRPACT_FK
PRINT N'Create foreign key constraint dbo.PIM_PRACST_PIM_PRPACT_FK'
GO
IF (OBJECT_ID('dbo.PIM_PRACST_PIM_PRPACT_FK', 'F') IS NULL)
  BEGIN
  ALTER TABLE [dbo].[PIMS_PROPERTY_ACTIVITY]
    ADD CONSTRAINT [PIM_PRACST_PIM_PRPACT_FK]
    FOREIGN KEY([PROP_MGMT_ACTIVITY_SUBTYPE_CODE])
    REFERENCES [dbo].[PIMS_PROP_MGMT_ACTIVITY_SUBTYPE]([PROP_MGMT_ACTIVITY_SUBTYPE_CODE])
    ON DELETE NO ACTION 
    ON UPDATE NO ACTION 
  END
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
