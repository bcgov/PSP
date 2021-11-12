/**
 * Claims enum, provides a list of permissions that govern what actions are available to an authenticated user.
 */
export enum Claims {
  CONTACT_VIEW = 'contact-view',
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
