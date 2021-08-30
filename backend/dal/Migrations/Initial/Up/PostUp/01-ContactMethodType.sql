PRINT N'Adding [PIMS_CONTACT_METHOD_TYPE]'

INSERT INTO PIMS_CONTACT_METHOD_TYPE (CONTACT_METHOD_TYPE_CODE, DESCRIPTION)
VALUES
  (N'FAX', N'Facsimile machine'),
  (N'PERSPHONE', N'Personal phone'),
  (N'WORKPHONE', N'Work phone'),
  (N'PERSEMAIL', N'Personal email'),
  (N'WORKEMAIL', N'Work email');
