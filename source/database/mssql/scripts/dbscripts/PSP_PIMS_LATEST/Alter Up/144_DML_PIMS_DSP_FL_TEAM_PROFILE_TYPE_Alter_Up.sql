/* -----------------------------------------------------------------------------
Alter the PIMS_DSP_FL_TEAM_PROFILE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Aug-12  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Alter the MOTILAWYER code
PRINT N'Alter the MOTILAWYER code'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTILAWYER'

SELECT DSP_FL_TEAM_PROFILE_TYPE_CODE
FROM   PIMS_DSP_FL_TEAM_PROFILE_TYPE
WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_FL_TEAM_PROFILE_TYPE
  SET    DESCRIPTION                = N'MoTT Solicitor'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO 

-- Alter the MOTILEAD code
PRINT N'Alter the MOTILEAD code'
GO
DECLARE @CurrCd NVARCHAR(20)
SET     @CurrCd = N'MOTILEAD'

SELECT DSP_FL_TEAM_PROFILE_TYPE_CODE
FROM   PIMS_DSP_FL_TEAM_PROFILE_TYPE
WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 1
  UPDATE PIMS_DSP_FL_TEAM_PROFILE_TYPE
  SET    DESCRIPTION                = N'MoTT Lead'
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  DSP_FL_TEAM_PROFILE_TYPE_CODE = @CurrCd;
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
