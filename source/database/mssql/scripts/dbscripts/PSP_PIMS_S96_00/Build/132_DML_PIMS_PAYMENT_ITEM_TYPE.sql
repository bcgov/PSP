/* -----------------------------------------------------------------------------
Populate the PIMS_PAYMENT_ITEM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jun-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PAYMENT_ITEM_TYPE
GO

INSERT INTO PIMS_PAYMENT_ITEM_TYPE (PAYMENT_ITEM_TYPE_CODE, DESCRIPTION, DISPLAY_ORDER)
VALUES
  (N'MARKETVALUE',   N'Market Value',                                                1),
  (N'ADVNCTAKNGTTL', N'Total Amount of Advance Taking',                              2),
  (N'NOMINALADVPMT', N'Nominal Advance Payment Fee',                                 3),
  (N'TEMPSRW',       N'Temporary SRW',                                               4),
  (N'PERMSRW',       N'Permanent SRW',                                               5),
  (N'DISTURBDAMAGE', N'Disturbance Damages',                                         6),
  (N'LESSREMEDCSTS', N'Less Remediation Costs (in accordance with attached report)', 7),
  (N'RELEASEMNTPRT', N'Release of a part of Easement XXXXXXXX',                      8),
  (N'TMPRLESMNTPRT', N'Temporary Release of part of Easement XXXXXXXX',              9),
  (N'LOSSSITEIMPRV', N'Loss of Site Improvements',                                  10),
  (N'INJUREAFFECTN', N'Injurious Affection to Remainder',                           11);
GO
