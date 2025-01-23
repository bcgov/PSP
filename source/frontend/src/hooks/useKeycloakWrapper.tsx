import { useKeycloak } from '@react-keycloak/web';
import { useCallback, useMemo } from 'react';

import * as API from '@/constants/API';
import { exists } from '@/utils';

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
  hasRole(role?: string | Array<string>): boolean;
  hasClaim(claim?: string | Array<string>): boolean;
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
  const hasClaim = useCallback(
    (claim?: string | Array<string>): boolean => {
      return (
        exists(claim) &&
        (typeof claim === 'string'
          ? userInfo?.client_roles?.includes(claim)
          : claim.some(c => userInfo?.client_roles?.includes(c)))
      );
    },
    [userInfo?.client_roles],
  );

  /**
   * Determine if the user belongs to the specified 'role'
   * @param role - The role name or an array of role name
   */
  const hasRole = useCallback(
    (role?: string | Array<string>): boolean => {
      return (
        exists(role) &&
        (typeof role === 'string'
          ? userInfo?.client_roles?.includes(role)
          : role.some(r => userInfo?.client_roles?.includes(r)))
      );
    },
    [userInfo?.client_roles],
  );

  /**
   * Return an array of roles the user belongs to
   */
  const roles = useCallback((): Array<string> => {
    const pimsRoleNames = getByType(API.ROLE_TYPES).map(r => r.name);
    return userInfo?.client_roles
      ? [...(userInfo?.client_roles.filter(r => pimsRoleNames.includes(r)) ?? [])]
      : [];
  }, [getByType, userInfo?.client_roles]);

  /**
   * Return the user's businessIdentifier
   */
  const businessIdentifier = useCallback((): string => {
    return userInfo?.businessIdentifierValue;
  }, [userInfo?.businessIdentifierValue]);

  /**
   * Return the user's display name
   */
  const displayName = useCallback((): string | undefined => {
    return userInfo?.name ?? userInfo?.preferred_businessIdentifier;
  }, [userInfo?.name, userInfo?.preferred_businessIdentifier]);

  /**
   * Return the user's first name
   */
  const firstName = useCallback((): string | undefined => {
    return userInfo?.firstName ?? userInfo?.given_name;
  }, [userInfo?.firstName, userInfo?.given_name]);

  /**
   * Return the user's last name
   */
  const surname = useCallback((): string | undefined => {
    return userInfo?.surname ?? userInfo?.family_name;
  }, [userInfo?.family_name, userInfo?.surname]);

  /**
   * Return the user's email
   */
  const email = useCallback((): string | undefined => {
    return userInfo?.email;
  }, [userInfo?.email]);

  return useMemo(
    () => ({
      obj: keycloak,
      businessIdentifierValue: businessIdentifier(),
      displayName: displayName(),
      firstName: firstName(),
      surname: surname(),
      email: email(),
      roles: roles(),
      hasRole: hasRole,
      hasClaim: hasClaim,
    }),
    [
      businessIdentifier,
      displayName,
      email,
      firstName,
      hasClaim,
      hasRole,
      keycloak,
      roles,
      surname,
    ],
  );
}

export default useKeycloakWrapper;
