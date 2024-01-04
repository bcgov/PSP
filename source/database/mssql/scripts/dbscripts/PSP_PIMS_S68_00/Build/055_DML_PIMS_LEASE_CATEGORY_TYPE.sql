/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_CATEGORY_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_CATEGORY_TYPE
GO

INSERT INTO PIMS_LEASE_CATEGORY_TYPE (LEASE_CATEGORY_TYPE_CODE, DESCRIPTION)
VALUES
  (N'RESID', N'Residential'),
  (N'COMM', N'Commercial'),
  (N'INDUS', N'Industrial'),
  (N'AGRIC', N'Agricultural'),
  (N'OTHER', N'Other'),
  (N'GOVGOV', N'Gov''t to Gov''t');