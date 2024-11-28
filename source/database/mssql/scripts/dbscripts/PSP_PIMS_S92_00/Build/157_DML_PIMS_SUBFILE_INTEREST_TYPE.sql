/* -----------------------------------------------------------------------------
Populate the PIMS_SUBFILE_INTEREST_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Oct-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_SUBFILE_INTEREST_TYPE
GO

INSERT INTO PIMS_SUBFILE_INTEREST_TYPE (SUBFILE_INTEREST_TYPE_CODE, DESCRIPTION)
VALUES
  (N'TENANT',     'Tenant (Monthly) Leaseholder'),
  (N'SUBTENANT',  'Sub-Tenant / Lease'),
  (N'LICOCCUPY',  'License of Occupation'),
  (N'STRATALOT',  'Strata Lot Owner'),
  (N'EASEMENT',   'Easement Holder'),
  (N'SRWUTILITY', 'SRW / Utility Holder'),
  (N'MORTGAGE',   'Mortgage Interest'),
  (N'XINGPERMIT', 'Crossing Permit'),
  (N'LIEN',       'Lis Pendens / Lien'),
  (N'OTHER',      'Other ');
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prnt
SET    prnt.DISPLAY_ORDER              = chld.ROW_NUM
     , prnt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_SUBFILE_INTEREST_TYPE prnt JOIN
       (SELECT SUBFILE_INTEREST_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_SUBFILE_INTEREST_TYPE
        WHERE  SUBFILE_INTEREST_TYPE_CODE <> N'OTHER') chld ON chld.SUBFILE_INTEREST_TYPE_CODE = prnt.SUBFILE_INTEREST_TYPE_CODE
WHERE  prnt.SUBFILE_INTEREST_TYPE_CODE <> N'OTHER'
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_SUBFILE_INTEREST_TYPE
SET    DISPLAY_ORDER              = 999
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  SUBFILE_INTEREST_TYPE_CODE = N'OTHER'
GO
