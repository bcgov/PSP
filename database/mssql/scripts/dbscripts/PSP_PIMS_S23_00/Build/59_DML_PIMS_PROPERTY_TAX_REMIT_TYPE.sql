/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_TAX_REMIT_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_TAX_REMIT_TYPE
GO

INSERT INTO PIMS_PROPERTY_TAX_REMIT_TYPE (PROPERTY_TAX_REMIT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'LESSEE',   N'Lessee'),
  (N'LICENSEE', N'Licensee'),
  (N'PERMITEE', N'Permitee'),
  (N'BCTFA',    N'BCTFA'),
  (N'OTHER',    N'Other');