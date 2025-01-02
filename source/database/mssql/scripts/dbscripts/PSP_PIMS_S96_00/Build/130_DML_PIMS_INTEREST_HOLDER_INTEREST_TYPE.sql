/* -----------------------------------------------------------------------------
Populate the PIMS_INTEREST_HOLDER_INTEREST_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-May-18  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_INTEREST_HOLDER_INTEREST_TYPE
GO

INSERT INTO PIMS_INTEREST_HOLDER_INTEREST_TYPE (INTEREST_HOLDER_INTEREST_TYPE_CODE, DESCRIPTION)
VALUES
  (N'NIP',  N'Non-Interest Payee'),
  (N'AC',   N'Agriculture Credit Act'),
  (N'AD',   N'Agriculture Land Develop Act'),
  (N'AL',   N'Assignment of Leases'),
  (N'AR',   N'Assignment of Rents'),
  (N'CV',   N'Covenant'),
  (N'DA',   N'Drainage Agreement'),
  (N'DT',   N'Duplicate Title'),
  (N'EA',   N'Easement'),
  (N'EA-S', N'Easement Security'),
  (N'EC',   N'Equitable Charge'),
  (N'EI',   N'Equitable Interest'),
  (N'ER',   N'Exceptions and Reservations'),
  (N'JD',   N'Judgement'),
  (N'LUC',  N'Land Use Contract'),
  (N'L',    N'Lease'),
  (N'LE',   N'Life Estate'),
  (N'LN',   N'Lien'),
  (N'LP',   N'Lis Pendens'),
  (N'LT',   N'Logging Tax Act'),
  (N'MOD',  N'Modification'),
  (N'M',    N'Mortgage'),
  (N'MC',   N'Mineral Claim'),
  (N'OL',   N'Options to Lease'),
  (N'OP',   N'Option to Purchase'),
  (N'PA',   N'Priority Agreement'),
  (N'PP',   N'Profit a Prendre'),
  (N'PW',   N'Party Wall Agreement'),
  (N'RC',   N'Restrictive Covenant'),
  (N'RAE',  N'Right to Acquire Easement'),
  (N'RA',   N'Right to Acquire SRW'),
  (N'RE',   N'Conditional Right of Entry'),
  (N'RF',   N'Right of First Refusal'),
  (N'RP',   N'Right of Purchase'),
  (N'RT',   N'Residential Tenancy'),
  (N'RW',   N'Right of Way'),
  (N'SL',   N'Sub Lease'),
  (N'SR',   N'Sub Right of Purchase'),
  (N'ST',   N'Statutory Charge'),
  (N'SW',   N'Statutory Right of Way'),
  (N'US',   N'Under Surface Rights'),
  (N'CPL',  N'Certificate of Pending Litigation');
GO