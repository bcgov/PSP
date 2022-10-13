/* -----------------------------------------------------------------------------
Delete all data from the PIMS_INSURANCE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_INSURANCE_TYPE
GO

INSERT INTO PIMS_INSURANCE_TYPE (INSURANCE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'GENERAL',  N'Commercial General Liability (CGL)'),
  (N'VEHICLE',  N'Vehicle Liability Coverage'),
  (N'AIRCRAFT', N'Aircraft Liability Coverage'),
  (N'MARINE',   N'Marine Liability Coverage'),
  (N'OTHER',    N'Other Insurance Coverage');