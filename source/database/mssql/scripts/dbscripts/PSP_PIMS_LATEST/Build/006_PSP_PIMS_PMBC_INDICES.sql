/* -----------------------------------------------------------------------------
Migrate the spatial indices on PIMS_PROPERTY.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Aug-03  Initial version
Doug Filteau  2023-Aug-29  Check for existence added.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Check for the existence of the index prior to creation.
IF NOT EXISTS (SELECT 1
               FROM   SYS.INDEXES si                                JOIN
                      SYS.OBJECTS so ON si.OBJECT_ID = so.OBJECT_ID JOIN
                      SYS.SCHEMAS sc ON so.SCHEMA_ID = sc.SCHEMA_ID
               WHERE  sc.NAME = 'dbo'
                  AND so.NAME = 'PMBC_PARCEL_POLYGON_FABRIC'
                  AND si.NAME = 'PLYFAB_PID_NUMBER_IDX')
  BEGIN
  CREATE NONCLUSTERED INDEX [PLYFAB_PID_NUMBER_IDX] 
    ON [pmbc].[PMBC_PARCEL_POLYGON_FABRIC] ([PID_NUMBER] ASC);
  END;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Check for the existence of the index prior to creation.
IF NOT EXISTS (SELECT 1
               FROM   SYS.INDEXES si                                JOIN
                      SYS.OBJECTS so ON si.OBJECT_ID = so.OBJECT_ID JOIN
                      SYS.SCHEMAS sc ON so.SCHEMA_ID = sc.SCHEMA_ID
               WHERE  sc.NAME = 'dbo'
                  AND so.NAME = 'PMBC_PARCEL_POLYGON_FABRIC'
                  AND si.NAME = 'PLYFAB_PLAN_NUMBER_IDX')
  BEGIN
  CREATE NONCLUSTERED INDEX [PLYFAB_PLAN_NUMBER_IDX] 
    ON [pmbc].[PMBC_PARCEL_POLYGON_FABRIC] ([PLAN_NUMBER] ASC);
  END;
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Check for the existence of the index prior to creation.
IF NOT EXISTS (SELECT 1
               FROM   SYS.INDEXES si                                JOIN
                      SYS.OBJECTS so ON si.OBJECT_ID = so.OBJECT_ID JOIN
                      SYS.SCHEMAS sc ON so.SCHEMA_ID = sc.SCHEMA_ID
               WHERE  sc.NAME = 'dbo'
                  AND so.NAME = 'PMBC_PARCEL_POLYGON_FABRIC'
                  AND si.NAME = 'PLYFAB_SHAPE_IDX')
  BEGIN
  CREATE SPATIAL INDEX [PLYFAB_SHAPE_IDX] ON [pmbc].[PMBC_PARCEL_POLYGON_FABRIC] (SHAPE) 
    USING GEOMETRY_AUTO_GRID
    WITH (BOUNDING_BOX =( XMIN =200000, YMIN =300000, XMAX =1900000, YMAX =1800000 ));
  END;
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
