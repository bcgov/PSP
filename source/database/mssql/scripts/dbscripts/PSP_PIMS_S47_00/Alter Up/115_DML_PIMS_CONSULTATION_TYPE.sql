/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_CONSULTATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_CONSULTATION_TYPE
GO

INSERT INTO PIMS_CONSULTATION_TYPE (CONSULTATION_TYPE_CODE, CONSULTATION_STATUS_TYPE_CODE, DESCRIPTION, OTHER_DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'1STNATION', N'UNKNOWN', N'First nation',                '',                1),
  (N'STRATRE',   N'UNKNOWN', N'Strategic Real Estate (SRE)', '',                2),
  (N'REGPLANG',  N'UNKNOWN', N'Regional planning',           '',                3),
  (N'REGPRPSVC', N'UNKNOWN', N'Regional property services',  '',                4),
  (N'DISTRICT',  N'UNKNOWN', N'District',                    '',                5),
  (N'HQ',        N'UNKNOWN', N'Headquarter (HQ)',            '',                6),
  (N'OTHER',     N'UNKNOWN', N'Other',                       'Describe other*', 7);
GO
