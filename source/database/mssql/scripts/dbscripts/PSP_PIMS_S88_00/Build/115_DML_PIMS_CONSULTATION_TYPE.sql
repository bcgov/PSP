/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_CONSULTATION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version.
Doug Filteau  2024-Aug-12  Added "ENGINEERG" and changed sort order.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_CONSULTATION_TYPE
GO

INSERT INTO PIMS_CONSULTATION_TYPE (CONSULTATION_TYPE_CODE, DESCRIPTION, OTHER_DESCRIPTION)
VALUES
  (N'ENGINEERG', N'Engineering',                 ''),
  (N'1STNATION', N'First Nation',                ''),
  (N'STRATRE',   N'Strategic Real Estate (SRE)', ''),
  (N'REGPLANG',  N'Regional planning',           ''),
  (N'REGPRPSVC', N'Regional property services',  ''),
  (N'DISTRICT',  N'District',                    ''),
  (N'HQ',        N'Headquarter (HQ)',            ''),
  (N'OTHER',     N'Other',                       'Describe other*');
GO

-- --------------------------------------------------------------
-- Update the display order with the exception of the OTHER type.
-- --------------------------------------------------------------
UPDATE prt
SET    prt.DISPLAY_ORDER              = seq.ROW_NUM
     , prt.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_CONSULTATION_TYPE prt JOIN
       (SELECT CONSULTATION_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_CONSULTATION_TYPE
        WHERE  CONSULTATION_TYPE_CODE <> N'OTHER') seq  ON seq.CONSULTATION_TYPE_CODE = prt.CONSULTATION_TYPE_CODE
WHERE  prt.CONSULTATION_TYPE_CODE <> N'OTHER'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- --------------------------------------------------------------
-- Set the OTHER type to always appear at the end of the list.
-- --------------------------------------------------------------
UPDATE PIMS_CONSULTATION_TYPE
SET    DISPLAY_ORDER              = 99
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
WHERE  CONSULTATION_TYPE_CODE = N'OTHER'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
