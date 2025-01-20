/* -----------------------------------------------------------------------------
Populate the PIMS_PROP_MGMT_ACTIVITY_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-11  Initial version.
Doug Filteau  2024-Feb-26  Added UTILITYBILL and TAXESLEVIES.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROP_MGMT_ACTIVITY_TYPE
GO

INSERT INTO PIMS_PROP_MGMT_ACTIVITY_TYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PROPERTYMGMT',  N'Property Management'),
  (N'PROPERTYMTC',   N'Property Maintenance'),
  (N'LANDLORDIMPRV', N'Landlord''s Improvements'),
  (N'TENANTIMPROV',  N'Tenant''s Improvements'),
  (N'INCDNTISSUE',   N'Incident and Issues'),
  (N'INQUIRY',       N'Inquiry'),
  (N'INVESTRPT',     N'Investigation/Report'),
  (N'1STNTNCONSULT', N'First Nations Consultation'),
  (N'APPLICPERMIT',  N'Applications/Permits'),
  (N'UTILITYBILL',   N'Utility bills'),
  (N'TAXESLEVIES',   N'Taxes and Levies');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE pmat
SET    pmat.DISPLAY_ORDER              = seq.ROW_NUM
     , pmat.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE pmat JOIN
       (SELECT PROP_MGMT_ACTIVITY_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_PROP_MGMT_ACTIVITY_TYPE) seq  ON seq.PROP_MGMT_ACTIVITY_TYPE_CODE = pmat.PROP_MGMT_ACTIVITY_TYPE_CODE
GO
