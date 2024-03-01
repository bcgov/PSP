/* -----------------------------------------------------------------------------
Populate the PIMS_PROP_MGMT_ACTIVITY_SUBTYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Sep-11  Initial version.
Doug Filteau  2024-Feb-26  Added UTILITYBILL and TAXESLEVIES.
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROP_MGMT_ACTIVITY_SUBTYPE
GO

INSERT INTO PIMS_PROP_MGMT_ACTIVITY_SUBTYPE (PROP_MGMT_ACTIVITY_TYPE_CODE, PROP_MGMT_ACTIVITY_SUBTYPE_CODE, DESCRIPTION)
VALUES
  (N'PROPERTYMTC',   N'DEMOLITION',       N'Demolition'),
  (N'PROPERTYMTC',   N'SITEINSPECT',      N'Site Inspection'),
  (N'PROPERTYMTC',   N'LANDSCAPING',      N'Landscaping'),
  (N'PROPERTYMTC',   N'JANITORIAL',       N'Janitorial'),
  (N'PROPERTYMTC',   N'WINDOWCLEAN',      N'Window Cleaning'),
  (N'PROPERTYMTC',   N'SNOWREMOVAL',      N'Snow Removal'),
  (N'PROPERTYMTC',   N'HVAC',             N'HVAC'),
  (N'PROPERTYMTC',   N'ELEVATOR',         N'Elevator'),
  (N'PROPERTYMTC',   N'LIGHTING',         N'Lighting'),
  (N'PROPERTYMTC',   N'GARBAGERECYCLING', N'Garbage/Recycling'),
  (N'PROPERTYMTC',   N'WATERANDSEWER',    N' Water and Sewer'),
  (N'PROPERTYMTC',   N'HEATINGFUEL',      N'Heating Fuel'),
  (N'PROPERTYMTC',   N'ELECTRICAL',       N'Electrical'),
  (N'PROPERTYMTC',   N'SECURITY',         N'Security'),
  (N'PROPERTYMTC',   N'FIRESAFETY',       N'Fire Safety'),
  (N'PROPERTYMTC',   N'TELEPHONE',        N'Telephone'),
  (N'PROPERTYMTC',   N'INTERNET',         N'Internet'),
  (N'PROPERTYMTC',   N'PARKINGLOT',       N'Parking Lot'),      

  (N'LANDLORDIMPRV', N'LL_PREMISES',      N'Premises'),
  (N'LANDLORDIMPRV', N'LL_BUILDGLAND',    N'Building/Land'),
  
  (N'TENANTIMPROV',  N'TN_PREMISES',      N'Premises'),   
  (N'TENANTIMPROV',  N'TN_BUILDGLAND',    N'Building/Land'),
  
  (N'INCDNTISSUE',   N'TRESPASS',         N'Trespass'),
  (N'INCDNTISSUE',   N'ENCAMPMENT',       N'Encampment'),
  (N'INCDNTISSUE',   N'VANDALISM',        N'Vandalism'),
  (N'INCDNTISSUE',   N'GARBAGEDUMP',      N'Garbage/Dumping'),
  
  (N'INQUIRY',       N'GENINQUIRY',       N'General Inquiry'),
  (N'INQUIRY',       N'EXPRINTEREST',     N'Expression of Interest'),
  
  (N'INVESTRPT',     N'APPRAISAL',        N'Appraisal'),
  (N'INVESTRPT',     N'ENVIRONMENT',      N'Environmental'),
  (N'INVESTRPT',     N'GEOTECH',          N'Geotechnical'),
  (N'INVESTRPT',     N'LEGALSURVEY',      N'Legal Survey'),
  (N'INVESTRPT',     N'LANDUSEPLAN',      N'Land Use Planning'),

  (N'1STNTNCONSULT', N'STRENOFCLAIM',     N'Strength of Claim Assessment'),
  (N'1STNTNCONSULT', N'CONSULTREC',       N'Consultation Record'),
  (N'1STNTNCONSULT', N'ACCOMMAGREE',      N'Accommodation Agreement'),
  
  (N'APPLICPERMIT',  N'ZONING',           N'Zoning'),
  (N'APPLICPERMIT',  N'ACCESS',           N'Access'),
  (N'APPLICPERMIT',  N'AGRILANDRES',      N'Agricultural Land Reserves (ALR)'),
  (N'APPLICPERMIT',  N'DEVELOPMENT',      N'Development'),
  (N'APPLICPERMIT',  N'SUBDIVISION',      N'Sub-Division'),
  (N'APPLICPERMIT',  N'PERMITS',          N'Permits'),
  
  (N'UTILITYBILL',   N'ELECTRICITYBILL',  N'Electricity'),
  (N'UTILITYBILL',   N'GASBILL',          N'Gas'),
  (N'UTILITYBILL',   N'INTERNETBILL',     N'Internet'),
  (N'UTILITYBILL',   N'SEWERWATERBILL',   N'Sewer and Water'),
  (N'UTILITYBILL',   N'TELEPHONEBILL',    N'Telephone'),
  
  (N'TAXESLEVIES',   N'MUNIPROPTAX',      N'Municipal property taxes'),
  (N'TAXESLEVIES',   N'WATERTAX',         N'Water'),
  (N'TAXESLEVIES',   N'OTHERTAX',         N'Other');
GO
