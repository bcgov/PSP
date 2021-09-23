import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import React from 'react';
import { Redirect, Route, RouteProps, useLocation } from 'react-router-dom';

interface IPrivateRouteProps extends RouteProps {
  component: React.ComponentType<any>;
  layout: React.ComponentType<any>;
  role?: string | Array<string>;
  claim?: string | Array<string>;
  componentProps?: any;
}

/**
 * A PrivateRoute only allows a user who is authenticated and has the appropriate role(s) or claim(s).
 * @param props - Properties to pass { component, role, claim }
 */
const PrivateRoute = (props: IPrivateRouteProps) => {
  const location = useLocation();
  const keycloak = useKeycloakWrapper();
  let { component: Component, layout: Layout, ...rest } = props;
  return (
    <Route
      {...rest}
      render={routeProps => {
        if (!!keycloak.obj?.authenticated) {
          if (
            (!rest.role && !rest.claim) ||
            keycloak.hasRole(rest.role) ||
            keycloak.hasClaim(rest.claim)
          ) {
            return (
              <Layout>
                <Component {...routeProps} {...rest.componentProps} />
              </Layout>
            );
          } else {
            return (
              <Redirect to={{ pathname: '/forbidden', state: { referer: routeProps.location } }} />
            );
          }
        } else {
          if (routeProps.location.pathname !== '/login') {
            const redirectTo = encodeURI(`${location.pathname}${location.search}`);
            return <Redirect to={`/login?redirect=${redirectTo}`} />;
          }
        }
      }}
    />
  );
};

export default PrivateRoute;
