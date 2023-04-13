/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_CONSULTATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_CONSULTATION_TYPE
GO

INSERT INTO PIMS_CONSULTATION_TYPE (CONSULTATION_TYPE_CODE, DESCRIPTION, OTHER_DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'1STNATION', N'First nation',                '',                1),
  (N'STRATRE',   N'Strategic Real Estate (SRE)', '',                2),
  (N'REGPLANG',  N'Regional planning',           '',                3),
  (N'REGPRPSVC', N'Regional property services',  '',                4),
  (N'DISTRICT',  N'District',                    '',                5),
  (N'HQ',        N'Headquarter (HQ)',            '',                6),
  (N'OTHER',     N'Other',                       'Describe other*', 7);
GO
