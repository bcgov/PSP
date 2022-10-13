/**
 * Roles enum, provide the available role names that a user can belong to.
 */
export enum Roles {
  SYSTEM_ADMINISTRATOR = 'System Administrator',
  ORGANIZATION_ADMINISTRATOR = 'Organization Administrator',
  REAL_ESTATE_MANAGER = 'Real Estate Manager',
  FINANCE = 'Finance',
  FUNCTIONAL = 'Functional',
  FUNCTIONAL_RESTRICTED = 'Functional (Restricted)',
  READ_ONLY = 'Read Only',
}

export default Roles;
