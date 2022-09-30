/* -----------------------------------------------------------------------------
Delete all data from the PIMS_INSURANCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_INSURANCE_TYPE
GO

INSERT INTO PIMS_INSURANCE_TYPE (INSURANCE_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'GENERAL',  N'Commercial General Liability (CGL)', 2),
  (N'VEHICLE',  N'Vehicle Liability Coverage',         4),
  (N'AIRCRAFT', N'Aircraft Liability Coverage',        1),
  (N'MARINE',   N'Marine Liability Coverage',          3),
  (N'OTHER',    N'Other Insurance Coverage',          99);
  