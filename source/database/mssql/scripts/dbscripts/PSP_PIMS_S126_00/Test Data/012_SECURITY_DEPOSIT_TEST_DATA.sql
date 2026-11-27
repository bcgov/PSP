-- Insert security deposit data of the three different SEC_DEP_HOLDER_TYPE_CODE
INSERT INTO [dbo].[PIMS_SECURITY_DEPOSIT]
  ([LEASE_ID], [SECURITY_DEPOSIT_TYPE_CODE], [DESCRIPTION], [AMOUNT_PAID], [DEPOSIT_DATE], [CONCURRENCY_CONTROL_NUMBER])
VALUES
  (
    2
	, 'PET'
	, 'This is a test description'
	, 1111
	, GETDATE()
	, 1
  ),
  (
    3
      , 'SECURITY'
      , 'This is a test description.'
      , 10000
      , GETDATE()
      , 1
  ),
  (
    4
      , 'PET'
	  , 'This is a test description.'
      , 5000
      , GETDATE()
      , 1
  )
