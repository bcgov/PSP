PRINT N'Adding [PIMS_PROPERTY_TENURE_TYPE]'

INSERT INTO PIMS_PROPERTY_TENURE_TYPE (PROPERTY_TENURE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PL', N'Payable Contract'),
  (N'TM', N'Titled Land - MoTI'),
  (N'CL', N'Crown Land Reserve'),
  (N'TT', N'Titled Land - TFA'),
  (N'RW', N'Right of Way'),
  (N'CR', N'Closed Road');
