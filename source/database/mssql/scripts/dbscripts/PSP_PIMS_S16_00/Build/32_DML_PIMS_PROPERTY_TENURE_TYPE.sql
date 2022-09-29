/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_TENURE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_TENURE_TYPE
GO

INSERT INTO PIMS_PROPERTY_TENURE_TYPE (PROPERTY_TENURE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PL', N'Payable Contract'),
  (N'TM', N'Titled Land - MoTI'),
  (N'CL', N'Crown Land Reserve'),
  (N'TT', N'Titled Land - TFA'),
  (N'RW', N'Right of Way'),
  (N'CR', N'Closed Road');