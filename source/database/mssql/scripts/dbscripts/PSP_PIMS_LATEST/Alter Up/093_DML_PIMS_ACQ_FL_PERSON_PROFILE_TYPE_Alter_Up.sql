/* -----------------------------------------------------------------------------
Alter the data in the PIMS_ACQ_FL_PERSON_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Mar-22  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTILAWYER'

SELECT ACQ_FL_PERSON_PROFILE_TYPE_CODE
FROM   PIMS_ACQ_FL_PERSON_PROFILE_TYPE
WHERE  ACQ_FL_PERSON_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACQ_FL_PERSON_PROFILE_TYPE (ACQ_FL_PERSON_PROFILE_TYPE_CODE, DESCRIPTION)
  VALUES
    (N'MOTILAWYER', N'MoTI Solicitor');
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
