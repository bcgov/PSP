/* -----------------------------------------------------------------------------
Conditional creation of the spatial indices on PIMS_PROPERTY.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-29  Initial version
----------------------------------------------------------------------------- */

IF NOT EXISTS(SELECT *
              FROM   SYS.INDEXES
              WHERE  name      = 'PRPRTY_BOUNDARY_IDX'
                 AND object_id = OBJECT_ID('dbo.PIMS_PROPERTY'))
  CREATE SPATIAL INDEX [PRPRTY_BOUNDARY_IDX] ON [dbo].[PIMS_PROPERTY] (BOUNDARY) 
    USING GEOMETRY_AUTO_GRID
      WITH (BOUNDING_BOX =( XMIN =200000, YMIN =300000, XMAX =1900000, YMAX =1800000 ))
GO
    
IF NOT EXISTS(SELECT *
              FROM   SYS.INDEXES
              WHERE  name      = 'PRPRTY_LOCATION_IDX'
                 AND object_id = OBJECT_ID('dbo.PIMS_PROPERTY'))
  CREATE SPATIAL INDEX [PRPRTY_LOCATION_IDX] ON [dbo].[PIMS_PROPERTY] (LOCATION) 
    USING GEOMETRY_AUTO_GRID
      WITH (BOUNDING_BOX =( XMIN =200000, YMIN =300000, XMAX =1900000, YMAX =1800000 ))
GO
