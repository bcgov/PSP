/* -----------------------------------------------------------------------------
Update the database version in the PIMS_STATIC_VARIABLE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Nov-24  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

DECLARE @CurrVer NVARCHAR(100)
SET @CurrVer = N'16.01'

UPDATE PIMS_STATIC_VARIABLE
WITH   (UPDLOCK, SERIALIZABLE) 
SET    STATIC_VARIABLE_VALUE    = @CurrVer
     , DB_LAST_UPDATE_USERID    = user_name()
     , DB_LAST_UPDATE_TIMESTAMP = getutcdate()
WHERE  STATIC_VARIABLE_NAME  = N'DBVERSION';

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_STATIC_VARIABLE (STATIC_VARIABLE_NAME, STATIC_VARIABLE_VALUE)
    VALUES (N'DBVERSION', @CurrVer);
  END

COMMIT TRANSACTION;
