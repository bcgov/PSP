/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_LICENSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_LICENSE_TYPE
GO

INSERT INTO PIMS_LEASE_LICENSE_TYPE (LEASE_LICENSE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'LSREG',     N'Lease - Registered (payable)',                       0),
  (N'LSUNREG',   N'Lease - Unregistered (payable)',                     0),
  (N'LSGRND',    N'Land Lease (receivable)',                            0),
  (N'LIOCCTTLD', N'Licence of Occupation (BCTFA fee simple)',           1),
  (N'LIOCCUSE',  N'Licence of Occupation (BCTFA fee simple)',           1),
  (N'LIOCCACCS', N'Licence of Occupation (BCTFA fee simple)',           1),
  (N'LIOCCUTIL', N'Licence of Occupation (BCTFA fee simple)',           1),
  (N'LICONSTRC', N'Licence to Construct',                               1),
  (N'LIPPUBHWY', N'Licence of Occupation of Provincial Public Highway', 0),
  (N'RESLNDTEN', N'Residential Tenancy Agreement',                      0),
  (N'LIMOTIPRJ', N'Licence of Occupation (BCTFA fee simple)',           1),
  (N'MANUFHOME', N'Manufactured Home Tenancy',                          0),
  (N'ROADXING',  N'Crossing Agreement',                                 0),
  (N'OTHER',     N'Other',                                              0),
  (N'LTRINTENT', N'Letter of Intended Use',                             0),
  (N'LTRINDMNY', N'Indemnity letter',                                   0),
  (N'AMNDAGREE', N'Amending Agreement',                                 0),
  (N'BLDGLSRCV', N'Building Lease (receivable)',                        0),
  (N'LIOCCHMK',  N'Licence of Occupation (HMK fee simple)',             0),
  (N'LOOBCTFA',  N'Licence of Occupation (BCTFA fee simple)',           0);
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE tbl
SET    tbl.DISPLAY_ORDER              = seq.ROW_NUM
     , tbl.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_LEASE_LICENSE_TYPE tbl JOIN
       (SELECT LEASE_LICENSE_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_LEASE_LICENSE_TYPE) seq  ON seq.LEASE_LICENSE_TYPE_CODE = tbl.LEASE_LICENSE_TYPE_CODE
GO
