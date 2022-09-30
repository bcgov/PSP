/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_PURPOSE_SUBTYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_PURPOSE_SUBTYPE
GO

INSERT INTO PIMS_LEASE_PURPOSE_SUBTYPE (LEASE_PURPOSE_SUBTYPE_CODE, DESCRIPTION)
VALUES
  (1, N'Parking Lot'),
  (2, N'Ferry Terminal'),
  (3, N'Trail'),
  (4, N'Park'),
  (5, N'Weigh Scale'),
  (6, N'Railway'),
  (7, N'Agriculture'),
  (8, N'Grazing'),
  (9, N'Utilities'),
  (10, N'Storage'),
  (11, N'Access'),
  (12, N'Community'),
  (13, N'Transmission Sites'),
  (14, N'Landscaping'),
  (15, N'Encroachment'),
  (16, N'Others'),
  (17, N'Retail'),
  (18, N'Billboard'),
  (19, N'Apartment'),
  (20, N'Oil and Gas'),
  (21, N'Oil and Gas'),
  (22, N'Overhead Construction Crane permit'),
  (23, N'Overhead Crane permit'),
  (24, N'Public Building - Firehall'),
  (25, N'Relocation - Office'),
  (26, N'Park N Ride'),
  (27, N'Hwy Maintenance Yard');