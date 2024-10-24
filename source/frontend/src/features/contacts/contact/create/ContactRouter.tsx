import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';

import { ContactTypes } from '../../interfaces';
import CreateOrganizationForm from './Organization/CreateOrganizationForm';
import CreatePersonForm from './Person/CreatePersonForm';

export const ContactRouter: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { path, url } = useRouteMatch();
  return (
    <Switch>
      <Redirect exact from={path} to={`${url}/${ContactTypes.INDIVIDUAL}`} />
      <Route exact path={`${path}/${ContactTypes.INDIVIDUAL}`}>
        <CreatePersonForm />
      </Route>
      <Route exact path={`${path}/${ContactTypes.ORGANIZATION}`}>
        <CreateOrganizationForm />
      </Route>
    </Switch>
  );
};

export default ContactRouter;
