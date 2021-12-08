/* -----------------------------------------------------------------------------
Apply the display order to the countries.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Dec-08  Initial version
----------------------------------------------------------------------------- */

UPDATE PIMS_COUNTRY SET DISPLAY_ORDER = 1  WHERE COUNTRY_CODE = 'CA';
GO
UPDATE PIMS_COUNTRY SET DISPLAY_ORDER = 2  WHERE COUNTRY_CODE = 'US';
G0
UPDATE PIMS_COUNTRY SET DISPLAY_ORDER = 3  WHERE COUNTRY_CODE = 'MX';
GO
UPDATE PIMS_COUNTRY SET DISPLAY_ORDER = 99 WHERE COUNTRY_CODE = 'OTHER';
GO
