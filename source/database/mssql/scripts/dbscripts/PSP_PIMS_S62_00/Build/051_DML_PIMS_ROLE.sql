DELETE FROM PIMS_ROLE
GO

INSERT INTO PIMS_ROLE (ROLE_UID, NAME, DESCRIPTION, IS_PUBLIC, IS_DISABLED, SORT_ORDER, APP_CREATE_TIMESTAMP, APP_CREATE_USERID, APP_CREATE_USER_DIRECTORY, APP_LAST_UPDATE_TIMESTAMP, APP_LAST_UPDATE_USERID, APP_LAST_UPDATE_USER_DIRECTORY)
VALUES
<<<<<<< HEAD
  (NEWID(), N'System Administrator', N'System Administrator of the PIMS solution.', CONVERT([bit],(0)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Organization Administrator', N'Administrator of the user''s organization.', CONVERT([bit],(0)), CONVERT([bit],(1)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Undetermined', N'The user has an undetermined role with their organization.', CONVERT([bit],(0)), CONVERT([bit],(1)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Real Estate Manager', N'Real Estate Manager can manage properties within their agency.', CONVERT([bit],(1)), CONVERT([bit],(1)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Real Estate Analyst', N'Real Estate Analyst can manage properties within their organization.', CONVERT([bit],(1)), CONVERT([bit],(1)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Finance', N'Finance team members.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Functional', N'PLMB staff (includes team members from HQ, regions and districts).', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Functional (Restricted)', N'Contractors, Internal ministry staff.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Read Only', N'Other ministries (e.g. Attorney General).', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');
=======
  (NEWID(), N'System Administrator', N'System Administrator of the PIMS solution.',                CONVERT([bit],(0)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Undetermined', N'The user has an undetermined role with their organization.',        CONVERT([bit],(0)), CONVERT([bit],(1)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Acquisition functional',   N'Access to create, read, update Acquisition files.',     CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Acquisition read-only',    N'Access to read Acquisition files.',                     CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Lease/License functional', N'Access to create, read, update Leases/Licenses files.', CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Lease/License read-only',  N'Access to read lease/license.',                         CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Project functional',       N'Access to create, read, update projects.',              CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Project read-only',        N'Access to read Projects.',                              CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Research functional',      N'Access to create, read, update Research files.',        CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data'),
  (NEWID(), N'Research read-only',       N'Access to read Research files.',                        CONVERT([bit],(1)), CONVERT([bit],(0)), 0, CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data', CURRENT_TIMESTAMP, N'Seed Data', N'Seed Data');
>>>>>>> upstream/dev
