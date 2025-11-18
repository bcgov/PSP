-- Add improvements for lease
INSERT INTO dbo.PIMS_PROPERTY_IMPROVEMENT (PROPERTY_ID, PROPERTY_IMPROVEMENT_TYPE_CODE, IMPROVEMENT_DESCRIPTION, STRUCTURE_SIZE)
  VALUES (1, N'COMMBLDG', N'This is a test description for the purpose of testing things and ensuring they are testable.', N'1234 sq ft.'),
         (2, N'OTHER',    N'This is a test description for the purpose of testing things and ensuring they are testable.', N'111 sq ft.'),
         (3, N'RTA',      N'This is a test description for the purpose of testing things and ensuring they are testable.', N'222 sq ft.');

