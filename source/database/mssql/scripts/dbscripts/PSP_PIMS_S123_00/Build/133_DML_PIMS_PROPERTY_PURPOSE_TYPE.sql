/* -----------------------------------------------------------------------------
Populate the PIMS_PROPERTY_PURPOSE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-11  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_PURPOSE_TYPE
GO

INSERT INTO PIMS_PROPERTY_PURPOSE_TYPE (PROPERTY_PURPOSE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACTVTRANCORR',  N'Active Transportation Corridor'),
  (N'BCFERRIES',     N'BC Ferries'),
  (N'BRIDGETRESTLE', N'Bridges/Trestles'),
  (N'ENVIROFFSET',   N'Environmental Offsetting'),
  (N'ENVIRPROTECT',  N'Environmental Protection'),
  (N'FNACCOMRECON',  N'FN Accommodation/Reconciliation'),
  (N'FUTUREHWYPROJ', N'Future Highway Project'),
  (N'GRAVELPIT',     N'Gravel Pit'),
  (N'GUIDEWAY',      N'Guideway (Perm SRW)'),
  (N'HMYMTCYARD',    N'Highway Maintenance Yard'),
  (N'HOUSINGDEV',    N'Housing Development'),
  (N'LANDXCHNG',     N'Land Exchange'),
  (N'LEASELIC',      N'Lease/License'),
  (N'MAPRESERVE',    N'Map Reserve'),
  (N'OTHER',         N'Other'),
  (N'PARKNRIDE',     N'Park n Ride'),
  (N'PROJCONSTRUCT', N'Project Construction (Temp SRW)'),
  (N'PROJLAYDOWN',   N'Project Laydown (Temporary)'),
  (N'ROADHWY',       N'Road/Highway'),
  (N'SKYTRAIN',      N'Skytrain'),
  (N'SLOPESTABILTY', N'Slope Stability'),
  (N'STATIONSITE',   N'Station Site (Fee Simple)'),
  (N'SURPLUSDISPO',  N'Surplus Disposition'),
  (N'SURPLUSENCUMB', N'Surplus Encumbered'),
  (N'SURPLUSCONSOL', N'Surplus, Consolidation Only'),
  (N'TRANSITORDEV',  N'Transit Oriented Development');
GO
