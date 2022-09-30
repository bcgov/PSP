-- Drop view dbo.PIMS_CONTACT_MGR_VW
PRINT N'Drop view dbo.PIMS_CONTACT_MGR_VW'
GO
DROP VIEW [dbo].[PIMS_CONTACT_MGR_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create view dbo.PIMS_CONTACT_MGR_VW
PRINT N'Create view dbo.PIMS_CONTACT_MGR_VW'
GO
CREATE VIEW [dbo].[PIMS_CONTACT_MGR_VW] AS
  SELECT CONCAT('P', PER.PERSON_ID)                                          AS ID
       , PER.PERSON_ID
       , ORG.ORGANIZATION_ID                                                 AS ORGANIZATION_ID
       , PER.IS_DISABLED
       , TRIM(CONCAT_WS(' ', PER.FIRST_NAME, PER.MIDDLE_NAMES, PER.SURNAME)) AS SUMMARY
       , PER.SURNAME
       , PER.FIRST_NAME
       , PER.MIDDLE_NAMES
       , ORG.ORGANIZATION_NAME                                               AS ORGANIZATION_NAME
       , PAD.ADDRESS_ID
       , ADR.STREET_ADDRESS_1                                                AS MAILING_ADDRESS
       , ADR.MUNICIPALITY_NAME
       , PRV.PROVINCE_STATE_CODE                                             AS PROVINCE_STATE
       , COALESCE(PVW.WORK_EMAIL, PVW.PERSONAL_EMAIL)                        AS EMAIL_ADDRESS
  FROM   PIMS_PERSON              PER                                                         LEFT JOIN
         PIMS_PERSON_ADDRESS      PAD ON PAD.PERSON_ID                = PER.PERSON_ID
                                     AND PAD.ADDRESS_USAGE_TYPE_CODE  = 'MAILING'             LEFT JOIN
         PIMS_ADDRESS             ADR ON ADR.ADDRESS_ID               = PAD.ADDRESS_ID        LEFT JOIN
         PIMS_PROVINCE_STATE      PRV ON PRV.PROVINCE_STATE_ID        = ADR.PROVINCE_STATE_ID LEFT JOIN
         PIMS_PERSON_CONTACT_VW   PVW ON PVW.PERSON_ID                = PER.PERSON_ID         LEFT JOIN
         (SELECT PERSON_ID, MAX(ORGANIZATION_ID) as ORGANIZATION_ID FROM PIMS_PERSON_ORGANIZATION GROUP BY PERSON_ID) as POR 
                                      ON POR.PERSON_ID                = PER.PERSON_ID         LEFT JOIN
         PIMS_ORGANIZATION        ORG ON ORG.ORGANIZATION_ID          = POR.ORGANIZATION_ID
  UNION
  SELECT CONCAT('O', ORG.ORGANIZATION_ID)
       , NULL
       , ORG.ORGANIZATION_ID
       , ORG.IS_DISABLED
       , ORG.ORGANIZATION_NAME
       , NULL
       , NULL
       , NULL
       , ORG.ORGANIZATION_NAME
       , OAD.ADDRESS_ID
       , ADR.STREET_ADDRESS_1
       , ADR.MUNICIPALITY_NAME
       , PRV.PROVINCE_STATE_CODE
       , CON.CONTACT_METHOD_VALUE
  FROM   PIMS_ORGANIZATION         ORG                                                         LEFT JOIN
         PIMS_ORGANIZATION_ADDRESS OAD ON OAD.ORGANIZATION_ID          = ORG.ORGANIZATION_ID
                                      AND OAD.ADDRESS_USAGE_TYPE_CODE  = 'MAILING'             LEFT JOIN
         PIMS_ADDRESS              ADR ON ADR.ADDRESS_ID               = OAD.ADDRESS_ID        LEFT JOIN
         PIMS_PROVINCE_STATE       PRV ON PRV.PROVINCE_STATE_ID        = ADR.PROVINCE_STATE_ID LEFT JOIN
         PIMS_CONTACT_METHOD       CON ON CON.ORGANIZATION_ID          = ORG.ORGANIZATION_ID
                                      AND CON.CONTACT_METHOD_TYPE_CODE = 'WORKEMAIL'
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
