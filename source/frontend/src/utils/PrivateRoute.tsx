import React from 'react';
import { Redirect, Route, RouteComponentProps, RouteProps, useLocation } from 'react-router-dom';

import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

interface BaseProtectedAppRoute extends RouteProps {
  role?: string | string[];
  claim?: string | string[];
}
interface ComponentRoute extends BaseProtectedAppRoute {
  customComponent: React.ComponentType<React.PropsWithChildren<any>>;
  layout: React.ComponentType<React.PropsWithChildren<any>>;
}

interface RenderRoute extends BaseProtectedAppRoute {
  customRender: (props: RouteComponentProps<any>) => React.ReactNode;
}

type IPrivateRouteProps = ComponentRoute | RenderRoute;

function isRenderRoute(route: RenderRoute | ComponentRoute): route is RenderRoute {
  return (route as RenderRoute).customRender !== undefined;
}

/**
 * A PrivateRoute only allows a user who is authenticated and has the appropriate role(s) or claim(s).
 * @param props - Properties to pass { component, role, claim }
 */
const PrivateRoute: React.FC<React.PropsWithChildren<IPrivateRouteProps>> = props => {
  const location = useLocation();
  const keycloak = useKeycloakWrapper();
  const { ...rest } = props;

  if (keycloak.obj?.authenticated) {
    if (
      (!rest.role && !rest.claim) ||
      keycloak.hasRole(rest.role) ||
      keycloak.hasClaim(rest.claim)
    ) {
      if (isRenderRoute(props)) {
        return <Route {...rest} render={props.customRender} />;
      } else {
        const { customComponent: Component, layout: Layout } = props;
        return (
          <Route
            {...rest}
            render={props => (
              <Layout>
                <Component {...props} />
              </Layout>
            )}
          />
        );
      }
    } else {
      return (
        <Route
          {...rest}
          render={routeProps => (
            <Redirect to={{ pathname: '/forbidden', state: { referer: routeProps.location } }} />
          )}
        />
      );
    }
  } else {
    if (location.pathname !== '/login') {
      const redirectTo = encodeURI(`${location.pathname}${location.search}`);
      return <Redirect to={`/login?redirect=${redirectTo}`} />;
    } else {
      console.error('Navigation: location was expected to be login');
      return <></>;
    }
  }
};

export default PrivateRoute;
