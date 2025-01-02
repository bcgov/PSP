/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_STATUS_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_STATUS_TYPE
GO

INSERT INTO PIMS_DISPOSITION_STATUS_TYPE (DISPOSITION_STATUS_TYPE_CODE, DESCRIPTION)
VALUES
  (N'UNKNOWN',   N'Unknown'),
  (N'PREMARKET', N'Pre-Marketing'),
  (N'LISTED',    N'Listed'),
  (N'PENDING',   N'Pending Sale'),
  (N'SOLD',      N'Sold'),
  (N'ONHOLD',    N'On Hold');
GO
