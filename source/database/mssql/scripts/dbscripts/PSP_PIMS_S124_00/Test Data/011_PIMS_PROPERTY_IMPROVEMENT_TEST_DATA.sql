-- Add improvements for lease
INSERT INTO dbo.PIMS_PROPERTY_IMPROVEMENT (PROPERTY_ID, PROPERTY_IMPROVEMENT_TYPE_CODE, PROP_IMPRVMNT_STATUS_TYPE_CODE, IMPROVEMENT_NAME, IMPROVEMENT_DESCRIPTION, STRUCTURE_SIZE)
  VALUES (1, N'COMMBLDG', N'ACTIVE',  N'Improvement Name 1', N'This is a test description for the purpose of testing things and ensuring they are testable.', N'1234 sq ft.'),
         (2, N'OTHER',    N'ACTIVE',  N'Improvement Name 2', N'This is a test description for the purpose of testing things and ensuring they are testable.', N'111 sq ft.'),
         (3, N'RTA',      N'ARCHIVD', N'Improvement Name 3', N'This is a test description for the purpose of testing things and ensuring they are testable.', N'222 sq ft.');

