/* -----------------------------------------------------------------------------
Populate the PIMS_DSP_INITIATING_BRANCH_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DSP_INITIATING_BRANCH_TYPE
GO

INSERT INTO PIMS_DSP_INITIATING_BRANCH_TYPE (DSP_INITIATING_BRANCH_TYPE_CODE, DESCRIPTION)
VALUES
  (N'STHCOAST', N'South Coast Region'),
  (N'SOUTHERN', N'Southern Interior Region'),
  (N'NORTHERN', N'Northern Region'),
  (N'PLMB',     N'PLMB'),
  (N'MAJORPRJ', N'Major Projects');
GO
