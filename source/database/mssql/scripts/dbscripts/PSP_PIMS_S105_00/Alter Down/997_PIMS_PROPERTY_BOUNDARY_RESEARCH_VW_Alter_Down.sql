/* -----------------------------------------------------------------------------
Alter the PIMS_PROPERTY_BOUNDARY_RESEARCH_VW view.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-20  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop view PIMS_PROPERTY_BOUNDARY_RESEARCH_VW
PRINT N'Drop view PIMS_PROPERTY_BOUNDARY_RESEARCH_VW'
GO
DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_BOUNDARY_RESEARCH_VW];
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create view PIMS_PROPERTY_BOUNDARY_RESEARCH_VW
PRINT N'Create view PIMS_PROPERTY_BOUNDARY_RESEARCH_VW'
GO
CREATE VIEW [dbo].[PIMS_PROPERTY_BOUNDARY_RESEARCH_VW] AS
with cteDistinct (PROPERTY_ID) AS
  (SELECT DISTINCT prp.PROPERTY_ID
   FROM   PIMS_PROPERTY               prp JOIN
          PIMS_PROPERTY_RESEARCH_FILE prf ON prf.PROPERTY_ID = prp.PROPERTY_ID)  

SELECT ct.PROPERTY_ID
     , pr.LOCATION
FROM   cteDistinct   ct JOIN
       PIMS_PROPERTY pr ON pr.PROPERTY_ID = ct.PROPERTY_ID
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
