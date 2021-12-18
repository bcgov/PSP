-- Insert security deposit data of the three different SEC_DEP_HOLDER_TYPE_CODE
INSERT INTO [dbo].[PIMS_SECURITY_DEPOSIT]
  ([LEASE_ID], [SEC_DEP_HOLDER_TYPE_CODE], [SECURITY_DEPOSIT_TYPE_CODE], [DESCRIPTION], [AMOUNT_PAID], [DEPOSIT_DATE], [ANNUAL_INTEREST_RATE], [CONCURRENCY_CONTROL_NUMBER])
VALUES
  (
    2
	, 'MINISTRY'
	, 'PET'
	, 'This is a test description'
	, 1111
	, GETDATE()
	, 12
	, 1
  ),
  (
    3
      , 'OTHER'
      , 'SECURITY'
      , 'This is a test description.'
      , 10000
      , GETDATE()
      , 13
      , 1
  ),
  (
    4
      , 'PROPMGR'
      , 'PET'
	  , 'This is a test description.'
      , 5000
      , GETDATE()
      , 10
      , 1
  )
