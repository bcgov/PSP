/* -----------------------------------------------------------------------------
Create the missing sequences.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-14  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create the PIMS_PROPERTY_ACTIVITY_ID_SEQ sequence.
PRINT N'Reset the PIMS_PROPERTY_ACTIVITY_ID_SEQ sequence.'
GO
IF NOT EXISTS (SELECT *
               FROM   sys.sequences 
               WHERE  NAME = 'PIMS_PROPERTY_ACTIVITY_ID_SEQ')
  CREATE SEQUENCE [dbo].[PIMS_PROPERTY_ACTIVITY_ID_SEQ]
    AS bigint
    START WITH 1
    INCREMENT BY 1
    MINVALUE 1
    MAXVALUE 2147483647
    NO CYCLE
    CACHE 50
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO


-- Create the PIMS_PROP_PROP_ACTIVITY_ID_SEQ sequence.
PRINT N'Reset the PIMS_PROP_PROP_ACTIVITY_ID_SEQ sequence.'
GO
IF NOT EXISTS (SELECT *
               FROM   sys.sequences 
               WHERE  NAME = 'PIMS_PROP_PROP_ACTIVITY_ID_SEQ')
  CREATE SEQUENCE [dbo].[PIMS_PROP_PROP_ACTIVITY_ID_SEQ]
    AS bigint
    START WITH 1
    INCREMENT BY 1
    MINVALUE 1
    MAXVALUE 2147483647
    NO CYCLE
    CACHE 50
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
IF (@Success = 1) 
  PRINT 'The database update succeeded'
ELSE 
  BEGIN
  IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
    PRINT 'The database update failed'
  END
GO
