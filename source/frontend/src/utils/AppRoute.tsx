import React from 'react';
import { Route, RouteComponentProps, RouteProps } from 'react-router-dom';

import PrivateRoute from '@/utils/PrivateRoute';

interface BaseAppRoute extends RouteProps {
  protected?: boolean;
  role?: string | string[];
  claim?: string | string[];
  title: string;
}
interface ComponentRoute extends BaseAppRoute {
  customComponent: React.ComponentType<React.PropsWithChildren<any>>;
  layout?: React.ComponentType<React.PropsWithChildren<any>>;
}

interface RenderRoute extends BaseAppRoute {
  customRender: (props: RouteComponentProps<any>) => React.ReactNode;
}
type IAppRouteProps = ComponentRoute | RenderRoute;

function isRenderRoute(route: RenderRoute | ComponentRoute): route is RenderRoute {
  return (route as RenderRoute).customRender !== undefined;
}

const AppRoute: React.FC<React.PropsWithChildren<IAppRouteProps>> = props => {
  document.title = props.title;

  if (props.protected) {
    if (isRenderRoute(props)) {
      return (
        <PrivateRoute
          {...props}
          customRender={props.customRender}
          role={props.role}
          claim={props.claim}
        />
      );
    } else {
      const Layout =
        props.layout === undefined ? (props: any) => <>{props.children}</> : props.layout;
      return (
        <PrivateRoute
          {...props}
          customComponent={props.customComponent}
          layout={Layout}
          role={props.role}
          claim={props.claim}
        />
      );
    }
  }

  if (isRenderRoute(props)) {
    return <Route {...props} render={props.customRender} />;
  } else {
    const Component = props.customComponent;
    const Layout =
      props.layout === undefined ? (props: any) => <>{props.children}</> : props.layout;
    return (
      <Route
        {...props}
        render={props => (
          <Layout>
            <Component {...props} />
          </Layout>
        )}
      />
    );
  }
};

export default AppRoute;
