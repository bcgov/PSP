/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_LICENSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_LICENSE_TYPE
GO

INSERT INTO PIMS_LEASE_LICENSE_TYPE (LEASE_LICENSE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'LSREG', N'Lease - Registered'),
  (N'LSUNREG', N'Lease - Unregistered'),
  (N'LSGRND', N'Ground Lease'),
  (N'LIOCCTTLD', N'License of Occupation (titled)'),
  (N'LIOCCUSE', N'License of Occupation (use)'),
  (N'LIOCCACCS', N'License of Occupation (access)'),
  (N'LIOCCUTIL', N'License of Occupation (utilities)'),
  (N'LICONSTRC', N'License to Construct'),
  (N'LIPPUBHWY', N'License of Prov Public Highway'),
  (N'RESLNDTEN', N'Residential Tenancy Act'),
  (N'LIMOTIPRJ', N'MOTI Project Use License'),
  (N'MANUFHOME', N'Manufactured Home Act'),
  (N'ROADXING', N'Road Crossing'),
  (N'OTHER', N'Other');