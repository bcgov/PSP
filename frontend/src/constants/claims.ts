/**
 * Claims enum, provides a list of permissions that govern what actions are available to an authenticated user.
 */
export enum Claims {
  LEASE_VIEW = 'lease-view',
  LEASE_ADD = 'lease-add',
  LEASE_EDIT = 'lease-edit',
  LEASE_DELETE = 'lease-delete',
  CONTACT_EDIT = 'contact-edit',
  CONTACT_VIEW = 'contact-view',
  CONTACT_ADD = 'contact-add',
  PROPERTY_VIEW = 'property-view',
  PROPERTY_EDIT = 'property-edit',
  PROPERTY_ADD = 'property-add',
  PROPERTY_DELETE = 'property-delete',
  ADMIN_USERS = 'admin-users',
  ADMIN_ROLES = 'admin-roles',
  ADMIN_PROPERTIES = 'admin-properties',
  ADMIN_PROJECTS = 'admin-projects',
}

export default Claims;
