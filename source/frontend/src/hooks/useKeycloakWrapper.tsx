import { useKeycloak } from '@react-keycloak/web';

import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import { Roles } from '@/constants/roles';
import { IProperty } from '@/interfaces';

import useLookupCodeHelpers from './useLookupCodeHelpers';

/**
 * IUserInfo interface, represents the userinfo provided by keycloak.
 */
export interface IUserInfo {
  displayName?: string;
  businessIdentifierValue: string;
  name?: string;
  preferred_businessIdentifier?: string;
  firstName?: string;
  surname?: string;
  email: string;
  groups: string[];
  client_roles: string[];
  given_name?: string;
  family_name?: string;
  organizations: number[];
}

/**
 * IKeycloak interface, represents the keycloak object for the authenticated user.
 */
export interface IKeycloak {
  obj: any;
  displayName?: string;
  businessIdentifierValue: string;
  name?: string;
  preferred_businessIdentifier?: string;
  idir_user_guid?: string;
  firstName?: string;
  surname?: string;
  email?: string;
  roles: string[];
  organizationId?: number;
  isAdmin: boolean;
  hasRole(role?: string | Array<string>): boolean;
  hasClaim(claim?: string | Array<string>): boolean;
  hasOrganization(organization?: number): boolean;
  organizationIds: number[];
  canUserEditProperty: (property: IProperty | null) => boolean;
  canUserViewProperty: (property: IProperty | null) => boolean;
  canUserDeleteProperty: (property: IProperty | null) => boolean;
}

/**
 * Provides extension methods to interact with the `keycloak` object.
 */
export function useKeycloakWrapper(): IKeycloak {
  const { keycloak } = useKeycloak();
  const userInfo = keycloak?.userInfo as IUserInfo;
  const { getByType } = useLookupCodeHelpers();

  /**
   * Determine if the user has the specified 'claim'
   * @param claim - The name of the claim
   */
  const hasClaim = (claim?: string | Array<string>): boolean => {
    return (
      claim !== undefined &&
      claim !== null &&
      (typeof claim === 'string'
        ? userInfo?.client_roles?.includes(claim)
        : claim.some(c => userInfo?.client_roles?.includes(c)))
    );
  };

  /**
   * Determine if the user belongs to the specified 'role'
   * @param role - The role name or an array of role name
   */
  const hasRole = (role?: string | Array<string>): boolean => {
    return (
      role !== undefined &&
      role !== null &&
      (typeof role === 'string'
        ? userInfo?.client_roles?.includes(role)
        : role.some(r => userInfo?.client_roles?.includes(r)))
    );
  };

  /**
   * Determine if the user belongs to the specified 'organization'
   * @param organization - The organization name
   */
  const hasOrganization = (organization?: number): boolean => {
    return (
      organization !== undefined &&
      organization !== null &&
      userInfo?.organizations?.includes(organization)
    );
  };

  /**
   * Return an array of roles the user belongs to
   */
  const roles = (): Array<string> => {
    const pimsRoleNames = getByType(API.ROLE_TYPES).map(r => r.name);
    return userInfo?.client_roles
      ? [...userInfo?.client_roles.filter(r => pimsRoleNames.includes(r))]
      : [];
  };

  /**
   * Return the user's businessIdentifier
   */
  const businessIdentifier = (): string => {
    return userInfo?.businessIdentifierValue;
  };

  /**
   * Return the user's display name
   */
  const displayName = (): string | undefined => {
    return userInfo?.name ?? userInfo?.preferred_businessIdentifier;
  };

  /**
   * Return the user's first name
   */
  const firstName = (): string | undefined => {
    return userInfo?.firstName ?? userInfo?.given_name;
  };

  /**
   * Return the user's last name
   */
  const surname = (): string | undefined => {
    return userInfo?.surname ?? userInfo?.family_name;
  };

  /**
   * Return the user's email
   */
  const email = (): string | undefined => {
    return userInfo?.email;
  };

  const isAdmin = hasClaim(Claims.ADMIN_PROPERTIES);
  const canEdit = hasClaim(Claims.PROPERTY_EDIT);
  const canDelete = hasClaim(Claims.PROPERTY_DELETE);

  /**
   * Return true if the user has permissions to edit this property
   * NOTE: this function will be true for MOST of PIMS, but there may be exceptions for certain cases.
   */
  const canUserEditProperty = (property: IProperty | null): boolean => {
    return !!property && (isAdmin || canEdit);
  };

  /**
   * Return true if the user has permissions to delete this property
   * NOTE: this function will be true for MOST of PIMS, but there may be exceptions for certain cases.
   */
  const canUserDeleteProperty = (property: IProperty | null): boolean => {
    return !!property && (isAdmin || canDelete);
  };

  /**
   * Return true if the user has permissions to edit this property
   * NOTE: this function will be true for MOST of PIMS, but there may be exceptions for certain cases.
   */
  const canUserViewProperty = (property: IProperty | null): boolean => {
    return (!!property && hasClaim(Claims.ADMIN_PROPERTIES)) || hasClaim(Claims.PROPERTY_VIEW);
  };

  return {
    obj: keycloak,
    businessIdentifierValue: businessIdentifier(),
    displayName: displayName(),
    firstName: firstName(),
    surname: surname(),
    email: email(),
    isAdmin: hasRole(Roles.SYSTEM_ADMINISTRATOR) || hasRole(Roles.ORGANIZATION_ADMINISTRATOR),
    roles: roles(),
    organizationId: userInfo?.organizations?.find(x => x),
    hasRole: hasRole,
    hasClaim: hasClaim,
    hasOrganization: hasOrganization,
    organizationIds: userInfo?.organizations,
    canUserEditProperty,
    canUserDeleteProperty,
    canUserViewProperty,
  };
}

export default useKeycloakWrapper;
