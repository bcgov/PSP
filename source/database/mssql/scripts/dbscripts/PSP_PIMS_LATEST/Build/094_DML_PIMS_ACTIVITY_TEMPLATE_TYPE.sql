/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACTIVITY_TEMPLATE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACTIVITY_TEMPLATE_TYPE
GO

INSERT INTO PIMS_ACTIVITY_TEMPLATE_TYPE (ACTIVITY_TEMPLATE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'GENERAL',   N'General',                          CONVERT([bit],(0))),
  (N'SURVEY',    N'Survey',                           CONVERT([bit],(1))),
  (N'SITEVIS',   N'Site Visit',                       CONVERT([bit],(0))),
  (N'GENLTR',    N'Generate Letter',                  CONVERT([bit],(0))),
  (N'FILEDOC',   N'File Document',                    CONVERT([bit],(1))),
  (N'NOTENTRY',  N'Notice of possible entry (H0224)', CONVERT([bit],(0))),
  (N'CONDENTRY', N'Condition of entry (H0443)',       CONVERT([bit],(0))),
  (N'RECNEGOT',  N'Record of negotiation',            CONVERT([bit],(0))),
  (N'CONSULT',   N'Consultation',                     CONVERT([bit],(0))),
  (N'RECTAKES',  N'Record takes',                     CONVERT([bit],(0))),
  (N'OFFAGREE',  N'Offer agreement (H179x)',          CONVERT([bit],(0))),
  (N'COMPREQ',   N'Compensation requisition (H120)',  CONVERT([bit],(0)));
GO
