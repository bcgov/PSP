/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACTIVITY_TEMPLATE_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Oct-31 Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION

-- Disable Survey template type
DECLARE @CurrCd NVARCHAR(20)
SET @CurrCd = N'SURVEY'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE_TYPE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACTIVITY_TEMPLATE_TYPE (ACTIVITY_TEMPLATE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES 
    (N'SURVEY', N'Survey', CONVERT([bit],(1)));
  END
ELSE
  BEGIN
  UPDATE PIMS_ACTIVITY_TEMPLATE_TYPE
  SET    IS_DISABLED                = CONVERT([bit],(1))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;
  END

-- Disable File Document template type
SET @CurrCd = N'FILEDOC'

SELECT ACTIVITY_TEMPLATE_TYPE_CODE
FROM   PIMS_ACTIVITY_TEMPLATE_TYPE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_ACTIVITY_TEMPLATE_TYPE (ACTIVITY_TEMPLATE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES 
    (N'FILEDOC', N'File Document', CONVERT([bit],(1)));
  END
ELSE
  BEGIN
  UPDATE PIMS_ACTIVITY_TEMPLATE_TYPE
  SET    IS_DISABLED                = CONVERT([bit],(1))
       , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
  WHERE  ACTIVITY_TEMPLATE_TYPE_CODE = @CurrCd;
  END
  
-- Insert additional template types

INSERT INTO PIMS_ACTIVITY_TEMPLATE_TYPE (ACTIVITY_TEMPLATE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'NOTENTRY',  N'Notice of possible entry (H0224)', CONVERT([bit],(0))),
  (N'CONDENTRY', N'Condition of entry (H0443)',       CONVERT([bit],(0))),
  (N'RECNEGOT',  N'Record of negotiation',            CONVERT([bit],(0))),
  (N'CONSULT',   N'Consultation',                     CONVERT([bit],(0))),
  (N'RECTAKES',  N'Record takes',                     CONVERT([bit],(0))),
  (N'OFFAGREE',  N'Offer agreement (H179x)',          CONVERT([bit],(0))),
  (N'COMPREQ',   N'Compensation requisition (H120)',  CONVERT([bit],(0)));

COMMIT TRANSACTION
