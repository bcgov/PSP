/*******************************************************************************
Drop/create the views
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Jul-22  Initial version.
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DROP VIEW IF EXISTS [dbo].[PIMS_PERSON_CONTACT_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

CREATE VIEW [dbo].[PIMS_PERSON_CONTACT_VW] AS
SELECT DISTINCT PERSON_ID
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'FAX'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS FAX
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'PERSMOBIL'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS PERSONAL_MOBILE
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'PERSPHONE'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS PERSONAL_PHONE
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'WORKMOBIL'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS WORK_MOBILE
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'WORKPHONE'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS WORK_PHONE
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'WORKEMAIL'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS WORK_EMAIL
     , (SELECT   TOP 1 CONTACT_METHOD_VALUE
        FROM     PIMS_CONTACT_METHOD
        WHERE    CONTACT_METHOD_TYPE_CODE = 'PERSEMAIL'
             AND PERSON_ID = TMP.PERSON_ID
        ORDER BY IS_PREFERRED_METHOD      DESC
               , DB_LAST_UPDATE_TIMESTAMP DESC) AS PERSONAL_EMAIL
FROM   PIMS_CONTACT_METHOD TMP
WHERE  ORGANIZATION_ID IS NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DROP VIEW IF EXISTS [dbo].[PIMS_CONTACT_MGR_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

CREATE VIEW [dbo].[PIMS_CONTACT_MGR_VW] AS
WITH
  -- Return the most recently updated mailing address for the person
  topPMailingAddr_CTE (PERSON_ID, ADDRESS_ID, ADDRESS_USAGE_TYPE_CODE, APP_LAST_UPDATE_TIMESTAMP, RN)
    AS
      (SELECT PERSON_ID
            , ADDRESS_ID
            , ADDRESS_USAGE_TYPE_CODE
            , APP_LAST_UPDATE_TIMESTAMP
            , ROW_NUMBER() OVER (PARTITION BY PERSON_ID
                                 ORDER     BY APP_LAST_UPDATE_TIMESTAMP DESC) AS RN
       FROM   PIMS_PERSON_ADDRESS
       WHERE  ADDRESS_USAGE_TYPE_CODE = 'MAILING'
          AND PERSON_ID              IS NOT NULL),
       
  -- Return the most recently updated mailing address for the organization
  topOMailingAddr_CTE (ORGANIZATION_ID, ADDRESS_ID, APP_LAST_UPDATE_TIMESTAMP, RN)
    AS
      (SELECT ORGANIZATION_ID
            , ADDRESS_ID
            , APP_LAST_UPDATE_TIMESTAMP
            , ROW_NUMBER() OVER (PARTITION BY ORGANIZATION_ID
                                 ORDER     BY APP_LAST_UPDATE_TIMESTAMP DESC) AS RN
       FROM   PIMS_ORGANIZATION_ADDRESS
       WHERE  ADDRESS_USAGE_TYPE_CODE = 'MAILING'
          AND ORGANIZATION_ID        IS NOT NULL),
       
  -- Return the most recently updated work email address for the organization
  topOWorkEmail_CTE (ORGANIZATION_ID, CONTACT_METHOD_VALUE, APP_LAST_UPDATE_TIMESTAMP, RN)
    AS
       (SELECT ORGANIZATION_ID
             , CONTACT_METHOD_VALUE
             , APP_LAST_UPDATE_TIMESTAMP
             , ROW_NUMBER() OVER (PARTITION BY ORGANIZATION_ID
                                  ORDER     BY APP_LAST_UPDATE_TIMESTAMP DESC) AS RN
        FROM   PIMS_CONTACT_METHOD
        WHERE  CONTACT_METHOD_TYPE_CODE = 'WORKEMAIL'
           AND ORGANIZATION_ID         IS NOT NULL)

  SELECT CONCAT('P', PER.PERSON_ID)                                                                              AS ID
       , PER.PERSON_ID
       , ORG.ORGANIZATION_ID                                                                                     AS ORGANIZATION_ID                
       , PER.IS_DISABLED
       , TRIM(CONCAT_WS(' ', NULLIF(PER.FIRST_NAME, ''), NULLIF(PER.MIDDLE_NAMES, ''), NULLIF(PER.SURNAME, ''))) AS SUMMARY
       , PER.SURNAME
       , PER.FIRST_NAME
       , PER.MIDDLE_NAMES
       , ORG.ORGANIZATION_NAME                                                                                   AS ORGANIZATION_NAME
       , PAD.ADDRESS_ID
       , ADR.STREET_ADDRESS_1                                                                                    AS MAILING_ADDRESS
       , ADR.MUNICIPALITY_NAME
       , PRV.PROVINCE_STATE_CODE                                                                                 AS PROVINCE_STATE
       , COALESCE(PVW.WORK_EMAIL, PVW.PERSONAL_EMAIL)                                                            AS EMAIL_ADDRESS
  FROM   PIMS_PERSON              PER                                                  LEFT JOIN
         topPMailingAddr_CTE      PAD ON PAD.PERSON_ID         = PER.PERSON_ID
                                     AND PAD.RN                = 1                     LEFT JOIN
         PIMS_ADDRESS             ADR ON ADR.ADDRESS_ID        = PAD.ADDRESS_ID        LEFT JOIN 
         PIMS_PROVINCE_STATE      PRV ON PRV.PROVINCE_STATE_ID = ADR.PROVINCE_STATE_ID LEFT JOIN
         PIMS_PERSON_CONTACT_VW   PVW ON PVW.PERSON_ID         = PER.PERSON_ID         LEFT JOIN
         (SELECT   PERSON_ID
                 , MAX(ORGANIZATION_ID) AS ORGANIZATION_ID 
          FROM     PIMS_PERSON_ORGANIZATION
          GROUP BY PERSON_ID)     POR ON POR.PERSON_ID         = PER.PERSON_ID         LEFT JOIN
         PIMS_ORGANIZATION        ORG ON ORG.ORGANIZATION_ID   = POR.ORGANIZATION_ID
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
  FROM   PIMS_ORGANIZATION         ORG                                                  LEFT JOIN 
         topOMailingAddr_CTE       OAD ON OAD.ORGANIZATION_ID   = ORG.ORGANIZATION_ID
                                      AND OAD.RN                = 1                     LEFT JOIN
         PIMS_ADDRESS              ADR ON ADR.ADDRESS_ID        = OAD.ADDRESS_ID        LEFT JOIN 
         PIMS_PROVINCE_STATE       PRV ON PRV.PROVINCE_STATE_ID = ADR.PROVINCE_STATE_ID LEFT JOIN
         topOWorkEmail_CTE         CON ON CON.ORGANIZATION_ID   = ORG.ORGANIZATION_ID
                                      AND CON.RN                = 1
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DROP VIEW IF EXISTS [dbo].[PIMS_HISTORICAL_FILE_NUMBER_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

CREATE VIEW [dbo].[PIMS_HISTORICAL_FILE_NUMBER_VW] AS
SELECT PROPERTY_ID  
     , STRING_AGG(HISTORICAL_FILE_NUMBER_STR, N', ') AS HISTORICAL_FILE_NUMBER_STR
FROM   (SELECT fnum.PROPERTY_ID
             , fnum.HISTORICAL_FILE_NUMBER
             , fdsc.DISPLAY_ORDER
             , CASE
                 WHEN fnum.HISTORICAL_FILE_NUMBER_TYPE_CODE <> N'OTHER' THEN 
                   fdsc.DESCRIPTION + N': ' + fnum.HISTORICAL_FILE_NUMBER
                 ELSE 
                   fnum.OTHER_HIST_FILE_NUMBER_TYPE_CODE + N': ' + fnum.HISTORICAL_FILE_NUMBER
               END AS HISTORICAL_FILE_NUMBER_STR
        FROM   PIMS_HISTORICAL_FILE_NUMBER      fnum JOIN
               PIMS_HISTORICAL_FILE_NUMBER_TYPE fdsc ON fdsc.HISTORICAL_FILE_NUMBER_TYPE_CODE = fnum.HISTORICAL_FILE_NUMBER_TYPE_CODE) AS HISTORICAL_FILE_NUMBER
GROUP BY PROPERTY_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_BOUNDARY_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

CREATE VIEW [dbo].[PIMS_PROPERTY_BOUNDARY_VW] AS
SELECT PROP.PROPERTY_ID
     , PROP.PID   
     , RIGHT('000000000' + CAST(PROP.PID AS VARCHAR(9)), 9) AS PID_PADDED
     , PROP.PIN
     , PROP.PROPERTY_TYPE_CODE     
     , PROP.PROPERTY_STATUS_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE
     , PROP.PROPERTY_CLASSIFICATION_TYPE_CODE
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE  TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYPE TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
                                                      AND TNTY.PROPERTY_ID               = PROP.PROPERTY_ID) temp) AS PROPERTY_TENURE_TYPE_CODE
     , ADDR.STREET_ADDRESS_1
     , ADDR.STREET_ADDRESS_2
     , ADDR.STREET_ADDRESS_3
     , ADDR.MUNICIPALITY_NAME
     , ADDR.POSTAL_CODE
     , PROV.PROVINCE_STATE_CODE
     , PROV.DESCRIPTION AS PROVINCE_NAME
     , CNTY.COUNTRY_CODE                                                                                                                             
     , CNTY.DESCRIPTION AS COUNTRY_NAME
     , PROP.NAME
     , PROP.DESCRIPTION
     , PROP.ADDRESS_ID
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.BOUNDARY AS GEOMETRY  
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA                                            
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , PROP.ENCUMBRANCE_REASON        
     , PROP.IS_SENSITIVE
     , PROP.IS_OWNED        
     , PROP.IS_RETIRED              
     , PROP.IS_VISIBLE_TO_OTHER_AGENCIES                                                                                                                                               
     , PROP.ZONING
     , PROP.ZONING_POTENTIAL        
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_DISPOSITION_FILE_PROPERTY DFPR JOIN
                          PIMS_DISPOSITION_FILE          DISP   ON DISP.DISPOSITION_FILE_ID = DFPR.DISPOSITION_FILE_ID
                                                               AND DFPR.PROPERTY_ID         = PROP.PROPERTY_ID
                   WHERE  DISP.DISPOSITION_FILE_STATUS_TYPE_CODE = N'COMPLETE'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_DISPOSED 
     , CASE
         WHEN EXISTS (SELECT 1    
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID            
                      WHERE  TAKE.IS_NEW_LAND_ACT       = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE = 'COMPLETE'                                                                                                                              
                         AND TAKE.LAND_ACT_TYPE_CODE   IN (N'Section 15', N'Section 16', N'Section 17', N'Section 66', N'NOI')) THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  TAKE.IS_NEW_INTEREST_IN_SRW = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE  = N'COMPLETE') THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID    
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  TAKE.IS_NEW_LICENSE_TO_CONSTRUCT = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE       = N'COMPLETE') THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1                                                                                                                                    
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID    
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  IS_ACTIVE_LEASE       = 1
                         AND TAKE_STATUS_TYPE_CODE = N'COMPLETE') THEN CONVERT([bit],(1))
         ELSE CONVERT([bit],(0))
         END AS IS_OTHER_INTEREST                                                                 
     , IIF(EXISTS (SELECT 1                        
                   FROM   PIMS_PROPERTY_ACQUISITION_FILE PRAF JOIN
                          PIMS_ACQUISITION_FILE          ACQF   ON ACQF.ACQUISITION_FILE_ID = PRAF.ACQUISITION_FILE_ID
                                                               AND PRAF.PROPERTY_ID         = PROP.PROPERTY_ID
                   WHERE  ACQF.ACQUISITION_FILE_STATUS_TYPE_CODE IN (N'DRAFT', N'ACTIVE')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS HAS_ACTIVE_ACQUISITION_FILE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_RESEARCH_FILE PRSF JOIN
                          PIMS_RESEARCH_FILE          RSHF   ON RSHF.RESEARCH_FILE_ID = PRSF.RESEARCH_FILE_ID
                                                            AND PRSF.PROPERTY_ID      = PROP.PROPERTY_ID
                   WHERE  RSHF.RESEARCH_FILE_STATUS_TYPE_CODE = N'ACTIVE'), CONVERT([bit],(1)), CONVERT([bit],(0)))  AS HAS_ACTIVE_RESEARCH_FILE                                                              
     , IIF(EXISTS (SELECT 1                                            
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN (N'PYBLMOTI', N'PYBLBCTFA')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_PAYABLE_LEASE   
     , IIF(EXISTS (SELECT 1                                                                                                            
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_PERIOD   TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN (N'PYBLMOTI', N'PYBLBCTFA')
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE          ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE IS NULL  ) OR
                           (getutcdate() BETWEEN TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_RECEIVABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_PERIOD   TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE          ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE IS NULL  ) OR
                           (getutcdate() BETWEEN TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_RECEIVABLE_LEASE          
     , FNVW.HISTORICAL_FILE_NUMBER_STR
FROM   PIMS_PROPERTY                  PROP                                                    LEFT OUTER JOIN   
       PIMS_HISTORICAL_FILE_NUMBER_VW FNVW ON FNVW.PROPERTY_ID       = PROP.PROPERTY_ID       LEFT OUTER JOIN         
       PIMS_ADDRESS                   ADDR ON ADDR.ADDRESS_ID        = PROP.ADDRESS_ID        LEFT OUTER JOIN
       PIMS_PROVINCE_STATE            PROV ON PROV.PROVINCE_STATE_ID = ADDR.PROVINCE_STATE_ID LEFT OUTER JOIN
       PIMS_COUNTRY                   CNTY ON CNTY.COUNTRY_ID        = ADDR.COUNTRY_ID   
WHERE  PROP.BOUNDARY IS NOT NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_LOCATION_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

CREATE VIEW [dbo].[PIMS_PROPERTY_LOCATION_VW] AS
SELECT PROP.PROPERTY_ID
     , PROP.PID
     , RIGHT('000000000' + CAST(PROP.PID AS VARCHAR(9)), 9) AS PID_PADDED
     , PROP.PIN                   
     , PROP.PROPERTY_TYPE_CODE
     , PROP.PROPERTY_STATUS_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE
     , PROP.PROPERTY_CLASSIFICATION_TYPE_CODE  
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE  TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYPE TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
                                                      AND TNTY.PROPERTY_ID               = PROP.PROPERTY_ID) temp) AS PROPERTY_TENURE_TYPE_CODE
     , ADDR.STREET_ADDRESS_1
     , ADDR.STREET_ADDRESS_2
     , ADDR.STREET_ADDRESS_3
     , ADDR.MUNICIPALITY_NAME
     , ADDR.POSTAL_CODE
     , PROV.PROVINCE_STATE_CODE
     , PROV.DESCRIPTION AS PROVINCE_NAME
     , CNTY.COUNTRY_CODE
     , CNTY.DESCRIPTION AS COUNTRY_NAME
     , PROP.NAME                                                                                                                                                                                 
     , PROP.DESCRIPTION
     , PROP.ADDRESS_ID
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.LOCATION AS GEOMETRY
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , PROP.ENCUMBRANCE_REASON          
     , PROP.IS_SENSITIVE
     , PROP.IS_OWNED              
     , PROP.IS_RETIRED                               
     , PROP.IS_VISIBLE_TO_OTHER_AGENCIES 
     , PROP.ZONING
     , PROP.ZONING_POTENTIAL                                                                                                                                                        
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_DISPOSITION_FILE_PROPERTY DFPR JOIN
                          PIMS_DISPOSITION_FILE          DISP   ON DISP.DISPOSITION_FILE_ID = DFPR.DISPOSITION_FILE_ID
                                                               AND DFPR.PROPERTY_ID         = PROP.PROPERTY_ID
                   WHERE  DISP.DISPOSITION_FILE_STATUS_TYPE_CODE = N'COMPLETE'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_DISPOSED 
     , CASE
         WHEN EXISTS (SELECT 1    
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID            
                      WHERE  TAKE.IS_NEW_LAND_ACT       = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE = 'COMPLETE'                                                                                                                              
                         AND TAKE.LAND_ACT_TYPE_CODE   IN (N'Section 15', N'Section 16', N'Section 17', N'Section 66', N'NOI')) THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  TAKE.IS_NEW_INTEREST_IN_SRW = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE  = N'COMPLETE') THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID    
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID                                                     
                      WHERE  TAKE.IS_NEW_LICENSE_TO_CONSTRUCT = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE       = N'COMPLETE') THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1                                                                                                                                    
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID    
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  IS_ACTIVE_LEASE       = 1
                         AND TAKE_STATUS_TYPE_CODE = N'COMPLETE') THEN CONVERT([bit],(1))
         ELSE CONVERT([bit],(0))
         END AS IS_OTHER_INTEREST                                                                 
     , IIF(EXISTS (SELECT 1                        
                   FROM   PIMS_PROPERTY_ACQUISITION_FILE PRAF JOIN
                          PIMS_ACQUISITION_FILE          ACQF   ON ACQF.ACQUISITION_FILE_ID = PRAF.ACQUISITION_FILE_ID
                                                               AND PRAF.PROPERTY_ID         = PROP.PROPERTY_ID
                   WHERE  ACQF.ACQUISITION_FILE_STATUS_TYPE_CODE IN (N'DRAFT', N'ACTIVE')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS HAS_ACTIVE_ACQUISITION_FILE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_RESEARCH_FILE PRSF JOIN
                          PIMS_RESEARCH_FILE          RSHF   ON RSHF.RESEARCH_FILE_ID = PRSF.RESEARCH_FILE_ID
                                                            AND PRSF.PROPERTY_ID      = PROP.PROPERTY_ID
                   WHERE  RSHF.RESEARCH_FILE_STATUS_TYPE_CODE = N'ACTIVE'), CONVERT([bit],(1)), CONVERT([bit],(0)))  AS HAS_ACTIVE_RESEARCH_FILE                                                              
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN ('PYBLMOTI', 'PYBLBCTFA')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_PERIOD   TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN ('PYBLMOTI', 'PYBLBCTFA')
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE        ) OR   
                           (getutcdate() >=      LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE IS NULL) OR
                           (getutcdate() BETWEEN TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_RECEIVABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN      
                          PIMS_LEASE_PERIOD   TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE        ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE IS NULL) OR
                           (getutcdate() BETWEEN TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_RECEIVABLE_LEASE                     
     , FNVW.HISTORICAL_FILE_NUMBER_STR
FROM   PIMS_PROPERTY                  PROP                                                    LEFT OUTER JOIN                                                                                        
       PIMS_HISTORICAL_FILE_NUMBER_VW FNVW ON FNVW.PROPERTY_ID       = PROP.PROPERTY_ID       LEFT OUTER JOIN
       PIMS_ADDRESS                   ADDR ON ADDR.ADDRESS_ID        = PROP.ADDRESS_ID        LEFT OUTER JOIN
       PIMS_PROVINCE_STATE            PROV ON PROV.PROVINCE_STATE_ID = ADDR.PROVINCE_STATE_ID LEFT OUTER JOIN
       PIMS_COUNTRY                   CNTY ON CNTY.COUNTRY_ID        = ADDR.COUNTRY_ID
WHERE  PROP.LOCATION IS NOT NULL
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

CREATE VIEW [dbo].[PIMS_PROPERTY_VW] AS
SELECT PROP.PROPERTY_ID
     , PROP.PID   
     , RIGHT('000000000' + CAST(PROP.PID AS VARCHAR(9)), 9) AS PID_PADDED
     , PROP.PIN
     , PROP.PROPERTY_TYPE_CODE
     , PROP.PROPERTY_STATUS_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE
     , PROP.PROPERTY_CLASSIFICATION_TYPE_CODE
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE  TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYPE TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
                                                      AND TNTY.PROPERTY_ID               = PROP.PROPERTY_ID) temp) AS PROPERTY_TENURE_TYPE_CODE
     , ADDR.STREET_ADDRESS_1
     , ADDR.STREET_ADDRESS_2
     , ADDR.STREET_ADDRESS_3
     , ADDR.MUNICIPALITY_NAME
     , ADDR.POSTAL_CODE
     , PROV.PROVINCE_STATE_CODE
     , PROV.DESCRIPTION AS PROVINCE_NAME
     , CNTY.COUNTRY_CODE
     , CNTY.DESCRIPTION AS COUNTRY_NAME
     , PROP.NAME
     , PROP.DESCRIPTION
     , PROP.ADDRESS_ID
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.BOUNDARY AS GEOMETRY  
     , PROP.BOUNDARY AS LOCATION
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA                                            
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , PROP.ENCUMBRANCE_REASON        
     , PROP.IS_SENSITIVE
     , PROP.IS_OWNED        
     , PROP.IS_RETIRED              
     , PROP.IS_VISIBLE_TO_OTHER_AGENCIES
     , PROP.ZONING
     , PROP.ZONING_POTENTIAL        
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_DISPOSITION_FILE_PROPERTY DFPR JOIN
                          PIMS_DISPOSITION_FILE          DISP   ON DISP.DISPOSITION_FILE_ID = DFPR.DISPOSITION_FILE_ID
                                                               AND DFPR.PROPERTY_ID         = PROP.PROPERTY_ID
                   WHERE  DISP.DISPOSITION_FILE_STATUS_TYPE_CODE = N'COMPLETE'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_DISPOSED 
     , CASE
         WHEN EXISTS (SELECT 1    
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID            
                      WHERE  TAKE.IS_NEW_LAND_ACT       = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE = 'COMPLETE'                                                                                                                              
                         AND TAKE.LAND_ACT_TYPE_CODE   IN (N'Section 15', N'Section 16', N'Section 17', N'Section 66', N'NOI')) THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  TAKE.IS_NEW_INTEREST_IN_SRW = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE  = N'COMPLETE') THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID    
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  TAKE.IS_NEW_LICENSE_TO_CONSTRUCT = 1
                         AND TAKE.TAKE_STATUS_TYPE_CODE       = N'COMPLETE') THEN CONVERT([bit],(1))
         WHEN EXISTS (SELECT 1                                                                                                                                    
                      FROM   PIMS_TAKE                      TAKE JOIN
                             PIMS_PROPERTY_ACQUISITION_FILE PRAF   ON PRAF.PROPERTY_ACQUISITION_FILE_ID = TAKE.PROPERTY_ACQUISITION_FILE_ID    
                                                                  AND PRAF.PROPERTY_ID                  = PROP.PROPERTY_ID
                      WHERE  IS_ACTIVE_LEASE       = 1                        
                         AND TAKE_STATUS_TYPE_CODE = N'COMPLETE') THEN CONVERT([bit],(1))
         ELSE CONVERT([bit],(0))
         END AS IS_OTHER_INTEREST                                                                 
     , IIF(EXISTS (SELECT 1                        
                   FROM   PIMS_PROPERTY_ACQUISITION_FILE PRAF JOIN
                          PIMS_ACQUISITION_FILE          ACQF   ON ACQF.ACQUISITION_FILE_ID = PRAF.ACQUISITION_FILE_ID
                                                               AND PRAF.PROPERTY_ID         = PROP.PROPERTY_ID
                   WHERE  ACQF.ACQUISITION_FILE_STATUS_TYPE_CODE IN (N'DRAFT', N'ACTIVE')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS HAS_ACTIVE_ACQUISITION_FILE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_RESEARCH_FILE PRSF JOIN
                          PIMS_RESEARCH_FILE          RSHF   ON RSHF.RESEARCH_FILE_ID = PRSF.RESEARCH_FILE_ID
                                                            AND PRSF.PROPERTY_ID      = PROP.PROPERTY_ID
                   WHERE  RSHF.RESEARCH_FILE_STATUS_TYPE_CODE = N'ACTIVE'), CONVERT([bit],(1)), CONVERT([bit],(0)))  AS HAS_ACTIVE_RESEARCH_FILE                                                              
     , IIF(EXISTS (SELECT 1                                            
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN (N'PYBLMOTI', N'PYBLBCTFA')), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_PAYABLE_LEASE   
     , IIF(EXISTS (SELECT 1                                                                                                            
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_PERIOD   TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE IN (N'PYBLMOTI', N'PYBLBCTFA')
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE          ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE IS NULL  ) OR
                           (getutcdate() BETWEEN TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_PAYABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_RECEIVABLE_LEASE
     , IIF(EXISTS (SELECT 1
                   FROM   PIMS_PROPERTY_LEASE PRLS                                          JOIN
                          PIMS_LEASE          LEAS ON PRLS.PROPERTY_ID = PROP.PROPERTY_ID
                                                  AND PRLS.LEASE_ID    = LEAS.LEASE_ID LEFT JOIN
                          PIMS_LEASE_PERIOD   TERM ON TERM.LEASE_ID    = LEAS.LEASE_ID
                   WHERE  LEAS.LEASE_PAY_RVBL_TYPE_CODE = 'RCVBL'
                      AND ((getutcdate() BETWEEN LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE          ) OR
                           (getutcdate() >=      LEAS.ORIG_START_DATE   AND LEAS.ORIG_EXPIRY_DATE IS NULL  ) OR
                           (getutcdate() BETWEEN TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE        ) OR
                           (getutcdate() >=      TERM.PERIOD_START_DATE AND TERM.PERIOD_EXPIRY_DATE IS NULL))), CONVERT([bit],(1)), CONVERT([bit],(0))) AS IS_ACTIVE_RECEIVABLE_LEASE                 
     , FNVW.HISTORICAL_FILE_NUMBER_STR
FROM   PIMS_PROPERTY                  PROP                                                    LEFT OUTER JOIN                                                                                        
       PIMS_HISTORICAL_FILE_NUMBER_VW FNVW ON FNVW.PROPERTY_ID       = PROP.PROPERTY_ID       LEFT OUTER JOIN
       PIMS_ADDRESS                   ADDR ON ADDR.ADDRESS_ID        = PROP.ADDRESS_ID        LEFT OUTER JOIN
       PIMS_PROVINCE_STATE            PROV ON PROV.PROVINCE_STATE_ID = ADDR.PROVINCE_STATE_ID LEFT OUTER JOIN
       PIMS_COUNTRY                   CNTY ON CNTY.COUNTRY_ID        = ADDR.COUNTRY_ID
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

COMMIT TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO
DECLARE @Success AS BIT
SET @Success = 1
SET NOEXEC OFF
IF (@Success = 1) PRINT 'The database update succeeded'
ELSE BEGIN
   IF @@TRANCOUNT > 0 ROLLBACK TRANSACTION
   PRINT 'The database update failed'
END
GO
