/* -----------------------------------------------------------------------------
Create the PIMS_PROPERTY_VW and PIMS_PROPERTY_VW_BKP views.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Jul-29  Initial version
----------------------------------------------------------------------------- */

SET XACT_ABORT ON
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO
BEGIN TRANSACTION
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Drop view PIMS_PROPERTY_VW
PRINT N'Drop view PIMS_PROPERTY_VW'
GO
DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_VW]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create view PIMS_PROPERTY_VW
PRINT N'Create view PIMS_PROPERTY_VW'
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
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYP TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
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
     , PROP.ADDRESS_ID
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.BOUNDARY AS GEOMETRY
     , PROP.LOCATION AS LOCATION
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , CASE 
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_PROPERTY_ACQUISITION_FILE PPAF                                                       JOIN
                             PIMS_ACQUISITION_FILE          PAF  ON PAF.ACQUISITION_FILE_ID = PPAF.ACQUISITION_FILE_ID JOIN
                             PIMS_DISPOSITION_FILE_PROPERTY PDFP ON PDFP.PROPERTY_ID        = PROP.PROPERTY_ID         JOIN
                             PIMS_DISPOSITION_FILE           PDF ON PDF.DISPOSITION_FILE_ID = PDFP.DISPOSITION_FILE_ID
                      WHERE  PPAF.PROPERTY_ID                      = PROP.PROPERTY_ID                
                         AND PAF.ACQUISITION_FILE_STATUS_TYPE_CODE = 'COMPLT'                                   
                         AND PAF.APP_LAST_UPDATE_TIMESTAMP         < COALESCE(PDF.COMPLETED_DT, PDF.APP_LAST_UPDATE_TIMESTAMP)) 
              THEN CAST(0 AS BIT)
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_PROPERTY_ACQUISITION_FILE PPAF                                                       JOIN
                             PIMS_ACQUISITION_FILE          PAF  ON PAF.ACQUISITION_FILE_ID = PPAF.ACQUISITION_FILE_ID JOIN
                             PIMS_DISPOSITION_FILE_PROPERTY PDFP ON PDFP.PROPERTY_ID        = PROP.PROPERTY_ID         JOIN
                             PIMS_DISPOSITION_FILE          PDF  ON PDF.DISPOSITION_FILE_ID = PDFP.DISPOSITION_FILE_ID
                      WHERE  PPAF.PROPERTY_ID                      = PROP.PROPERTY_ID
                         AND PAF.ACQUISITION_FILE_STATUS_TYPE_CODE = 'COMPLT'
                         AND PAF.APP_LAST_UPDATE_TIMESTAMP         > COALESCE(PDF.COMPLETED_DT, PDF.APP_LAST_UPDATE_TIMESTAMP)
                         AND PDF.DISPOSITION_FILE_STATUS_TYPE_CODE = 'COMPLETE') 
              THEN CAST(1 AS BIT)
              ELSE PROP.IS_OWNED
         END AS IS_OWNED
     , PROP.IS_RETIRED    
     , CASE
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_PROPERTY_ACQUISITION_FILE PPAF                                                       JOIN
                             PIMS_ACQUISITION_FILE          PAF  ON PAF.ACQUISITION_FILE_ID = PPAF.ACQUISITION_FILE_ID JOIN
                             PIMS_DISPOSITION_FILE_PROPERTY PDFP ON PDFP.PROPERTY_ID        = PROP.PROPERTY_ID         JOIN
                             PIMS_DISPOSITION_FILE          PDF  ON PDF.DISPOSITION_FILE_ID = PDFP.DISPOSITION_FILE_ID
                      WHERE  PPAF.PROPERTY_ID                      = PROP.PROPERTY_ID
                         AND PAF.ACQUISITION_FILE_STATUS_TYPE_CODE = 'COMPLT'
                         AND PAF.APP_LAST_UPDATE_TIMESTAMP         < COALESCE(PDF.COMPLETED_DT, PDF.APP_LAST_UPDATE_TIMESTAMP))
              THEN CAST(1 AS BIT)
         WHEN EXISTS (SELECT 1
                      FROM   PIMS_PROPERTY_ACQUISITION_FILE PPAF                                                       JOIN
                             PIMS_ACQUISITION_FILE          PAF  ON PAF.ACQUISITION_FILE_ID = PPAF.ACQUISITION_FILE_ID JOIN
                             PIMS_DISPOSITION_FILE_PROPERTY PDFP ON PDFP.PROPERTY_ID        = PROP.PROPERTY_ID         JOIN 
                             PIMS_DISPOSITION_FILE          PDF  ON PDF.DISPOSITION_FILE_ID = PDFP.DISPOSITION_FILE_ID
                      WHERE  PPAF.PROPERTY_ID                      = PROP.PROPERTY_ID  
                         AND PAF.ACQUISITION_FILE_STATUS_TYPE_CODE = 'COMPLT'
                         AND PAF.APP_LAST_UPDATE_TIMESTAMP         > COALESCE(PDF.COMPLETED_DT, PDF.APP_LAST_UPDATE_TIMESTAMP)
                         AND PDF.DISPOSITION_FILE_STATUS_TYPE_CODE = 'COMPLETE')
              THEN CAST(1 AS BIT)
              ELSE CAST(0 AS BIT)
         END AS IS_DISPOSED
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

-- Drop view PIMS_PROPERTY_VW_BKP
PRINT N'Drop view PIMS_PROPERTY_VW_BKP'
GO
DROP VIEW IF EXISTS [dbo].[PIMS_PROPERTY_VW_BKP]
GO
IF @@ERROR <> 0 SET NOEXEC ON
GO

-- Create view PIMS_PROPERTY_VW_BKP
PRINT N'Create view PIMS_PROPERTY_VW_BKP'
GO
CREATE VIEW [dbo].[PIMS_PROPERTY_VW_BKP] AS
SELECT PROP.PROPERTY_ID
     , PROP.PID   
     , RIGHT('000000000' + CAST(PROP.PID AS VARCHAR(9)), 9) AS PID_PADDED
     , PROP.PIN
     , PROP.PROPERTY_TYPE_CODE
     , PROP.PROPERTY_STATUS_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_TYPE_CODE
     , PROP.PROPERTY_DATA_SOURCE_EFFECTIVE_DATE
     , (SELECT STRING_AGG(TENURE_DESC, ', ')
        FROM   (SELECT TNUR.DESCRIPTION AS TENURE_DESC
                FROM   PIMS_PROPERTY_TENURE_TYPE TNUR INNER JOIN
                       PIMS_PROP_PROP_TENURE_TYP TNTY ON TNTY.PROPERTY_TENURE_TYPE_CODE = TNUR.PROPERTY_TENURE_TYPE_CODE
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
     , PROP.ADDRESS_ID                                                 
     , PROP.REGION_CODE
     , PROP.DISTRICT_CODE
     , PROP.BOUNDARY AS GEOMETRY
     , PROP.LOCATION AS LOCATION
     , PROP.PROPERTY_AREA_UNIT_TYPE_CODE
     , PROP.LAND_AREA
     , PROP.LAND_LEGAL_DESCRIPTION
     , PROP.SURVEY_PLAN_NUMBER
     , PROP.IS_OWNED
     , PROP.IS_RETIRED    
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
