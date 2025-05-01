/**
 * Roles enum, provide the available role names that a user can belong to.
 */
export enum Roles {
  UNDETERMINED = 'Undetermined',
  SYSTEM_ADMINISTRATOR = 'System Administrator',
  ACQUISITION_FUNCTIONAL = 'Acquisition functional',
  ACQUISITION_READ_ONLY = 'Acquisition read-only',
  LEASE_FUNCTIONAL = 'Lease License functional',
  LEASE_READ_ONLY = 'Lease License read-only',
  PROJECT_FUNCTIONAL = 'Project functional',
  PROJECT_READ_ONLY = 'Project read-only',
  RESEARCH_FUNCTIONAL = 'Research functional',
  RESEARCH_READ_ONLY = 'Research read-only',
  MANAGEMENT_FUNCTIONAL = 'Management functional',
  MANAGEMENT_READ_ONLY = 'Management read-only',
  DISPOSITION_FUNCTIONAL = 'Disposition functional',
  DISPOSITION_READ_ONLY = 'Disposition read-only',
}

export default Roles;
