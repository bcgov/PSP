/* -----------------------------------------------------------------------------
Populate the PIMS_INTEREST_HOLDER_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-18  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_INTEREST_HOLDER_TYPE
GO

INSERT INTO PIMS_INTEREST_HOLDER_TYPE (INTEREST_HOLDER_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'INTHLDR', N'Interest Holder',                  1),
  (N'AOREP',   N'Acquisition Owner Representative', 2),
  (N'AOSLCTR', N'Acquisition Owner Solicitor',      3);
GO