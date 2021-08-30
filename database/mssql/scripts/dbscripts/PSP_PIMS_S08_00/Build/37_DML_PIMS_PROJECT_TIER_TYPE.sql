/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROJECT_TIER_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_TIER_TYPE
GO

INSERT INTO PIMS_PROJECT_TIER_TYPE (PROJECT_TIER_TYPE_CODE, DESCRIPTION)
VALUES
  (N'TIER1', N'Properties with a net value of less than $1M.'),
  (N'TIER2', N'Properties with a net value of $1M or more and less than $10M'),
  (N'TIER3', N'Properties from a single parcels with a net value of $10M or more'),
  (N'TIER4', N'Properties from multiple parcels with a cumulative net value of $10M or more');