/*******************************************************************************
Drop the existing PIMS_CONTACT_MGR_VW view and recreate to reference the 
deprecated 'MAILADDR' code value for ADDRESS_USAGE_TYPE_CODE rather than the 
'MAILING' code value.
*******************************************************************************/

DROP VIEW IF EXISTS PIMS_CONTACT_MGR_VW;
GO

CREATE VIEW PIMS_CONTACT_MGR_VW AS
  SELECT CONCAT('P', PER.PERSON_ID) AS ID
       , PER.PERSON_ID
       , NULL                   AS ORGANIZATION_ID
       , PER.IS_DISABLED
       , TRIM(CONCAT_WS(' ', PER.FIRST_NAME, PER.MIDDLE_NAMES, PER.SURNAME)) AS SUMMARY
       , PER.SURNAME
       , PER.FIRST_NAME
       , PER.MIDDLE_NAMES
       , NULL                    AS ORGANIZATION_NAME
       , PAD.ADDRESS_ID	   
       , ADR.STREET_ADDRESS_1    AS MAILING_ADDRESS
       , ADR.MUNICIPALITY_NAME
       , PRV.PROVINCE_STATE_CODE AS PROVINCE_STATE 
  FROM   PIMS_PERSON         PER                                                 LEFT JOIN
         PIMS_PERSON_ADDRESS PAD ON PAD.PERSON_ID               = PER.PERSON_ID  
                                AND PAD.ADDRESS_USAGE_TYPE_CODE = 'MAILADDR'     LEFT JOIN
         PIMS_ADDRESS        ADR ON ADR.ADDRESS_ID              = PAD.ADDRESS_ID LEFT JOIN 
         PIMS_PROVINCE_STATE PRV ON PRV.PROVINCE_STATE_ID       = ADR.PROVINCE_STATE_ID
  UNION
  SELECT CONCAT('O', ORG.ORGANIZATION_ID) AS ID
       , NULL                        AS PERSON_ID
       , ORG.ORGANIZATION_ID
       , ORG.IS_DISABLED
       , ORG.ORGANIZATION_NAME       AS SUMMARY
       , NULL                        AS SURNAME
       , NULL                        AS FIRST_NAME
       , NULL                        AS MIDDLE_NAMES                                                        
       , ORG.ORGANIZATION_NAME
       , OAD.ADDRESS_ID
       , ADR.STREET_ADDRESS_1        AS MAILING_ADDRESS
       , ADR.MUNICIPALITY_NAME
       , PRV.PROVINCE_STATE_CODE     AS PROVINCE_STATE 
  FROM   PIMS_ORGANIZATION         ORG                                                 LEFT JOIN 
         PIMS_ORGANIZATION_ADDRESS OAD ON OAD.ORGANIZATION_ID         = ORG.ORGANIZATION_ID 
                                      AND OAD.ADDRESS_USAGE_TYPE_CODE = 'MAILADDR'     LEFT JOIN
         PIMS_ADDRESS              ADR ON ADR.ADDRESS_ID              = OAD.ADDRESS_ID LEFT JOIN 
         PIMS_PROVINCE_STATE       PRV ON PRV.PROVINCE_STATE_ID       = ADR.PROVINCE_STATE_ID;
GO
