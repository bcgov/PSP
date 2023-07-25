import * as React from 'react';
import { useHistory } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { Roles } from '@/constants/roles';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

interface IProtectedComponentProps {
  /** a list of roles that allow access to this component */
  roles?: Roles[];
  /** a list of claims that allow access to this component */
  claims?: Claims[];
  /** */
  hideIfNotAuthorized?: boolean;
}
/**
 * If the user does not have one of the roles or claims passed into this component, they will be redirected to the 401 page for the app.
 * @param param0
 */
export const ProtectedComponent: React.FunctionComponent<
  React.PropsWithChildren<IProtectedComponentProps>
> = ({ roles, claims, hideIfNotAuthorized, children }) => {
  const history = useHistory();
  const { hasRole, hasClaim } = useKeycloakWrapper();
  const isAuthorized = hasRole(roles) || hasClaim(claims);
  if (!hideIfNotAuthorized && !isAuthorized) {
    history.push('/forbidden');
  }
  return isAuthorized ? <>{children}</> : <></>;
};

export default ProtectedComponent;
