/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_SUBTYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_SUBTYPE
GO

INSERT INTO PIMS_LEASE_SUBTYPE (LEASE_SUBTYPE_CODE, DESCRIPTION)
VALUES
  (N'E', N'Expense'),
  (N'R', N'Revenue');