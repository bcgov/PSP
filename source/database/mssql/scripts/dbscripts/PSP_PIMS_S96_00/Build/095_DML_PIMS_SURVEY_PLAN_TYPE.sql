/* -----------------------------------------------------------------------------
Delete all data from the PIMS_SURVEY_PLAN_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_SURVEY_PLAN_TYPE
GO

INSERT INTO PIMS_SURVEY_PLAN_TYPE (SURVEY_PLAN_TYPE_CODE, DESCRIPTION)
VALUES
  (N'S107REFART',    N'S.107 Reference – arterial'),
  (N'S107EXPLNART',  N'S.107 Explanatory – arterial'),
  (N'S107REFNART',   N'S.107 Reference – not arterial'),
  (N'S107EXPLNNART', N'S.107 Explanatory – not arterial'),
  (N'S113REFHWYX',   N'S.113 Reference SRW – Highway closure'),
  (N'S113REFFRM12',  N'S.113 Reference SRW – Form 12'),
  (N'S113REFSRW',    N'S.113 Reference SRW – SRW'),
  (N'S99EXPLNTRY',   N'S.99(1)(e) Explanatory – SRW'),
  (N'S99SUB4TRAN',   N'S.99(1)(h)(i) Reference – Subdivision for tran purpose'),
  (N'S107S27TFAX',   N'S.107LTA/s.27(3)(b) TA/s.6 EA ref – BCTFA Expropriation'),
  (N'S107S10MOTX',   N'S.107 LTA/ s.10 TA/ s.6 EA ref – MoTI Expropriation'),
  (N'S113S10MOTX',   N'S.113 LTA/ s.10 TA/s.6 EA ref – MoTI Expropriation SRW'),
  (N'S113S27TFAX',   N'S.113 LTA/s.27(3)(b) TA/s.6 EA ref – BCTFA expropriation SRW'),
  (N'LNDACTPLCRW',   N'Land Act Plan – Crown grant'),
  (N'SKETCHSITEP',   N'Sketch/site plan'),
  (N'S1001BLTACP',   N'S.100(1)(b) LTA – Consolidation plan'),
  (N'S109REFHWYX',   N'S.109 reference plan-hwy closure'),
  (N'S109SUBHWYX',   N'S.109 subdivision plan – hwy closure'),
  (N'S219LTARCOV',   N'S.219 LTA reference – Covenant'),
  (N'S219LTAXCOV',   N'S.219 LTA explanatory – Covenant'),
  (N'S991EESMNT',    N'S.99(1)(e) – easement'),
  (N'S67LTASUBD',    N'S.67LTA – subdivision'),
  (N'BLKOTLNPST',    N'Block Outline reference'),
  (N'S69LTABLKO',    N'S.69LTA – Block Outline Posting'),
  (N'S68POSTING',    N'S.68 – Posting'),
  (N'AIRSPACE',      N'Airspace'),
  (N'S1001AREF',     N'S.100(1)(a) reference'),
  (N'S16MAPRSRV',    N'S.16 Map Reserve'),
  (N'S17MAPRSRV',    N'S.17 Map Reserve'),
  (N'OTHER',         N'Other');
GO
