import React from 'react';
import { Route, Switch } from 'react-router-dom';

import CreateOrganizationForm from './Organization/CreateOrganizationForm';
import CreatePersonForm from './Person/CreatePersonForm';

export const ContactRouter: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  return (
    <Switch>
      <Route exact path={`/contact/new/P`}>
        <CreatePersonForm />
      </Route>
      <Route exact path={`/contact/new/O`}>
        <CreateOrganizationForm />
      </Route>
    </Switch>
  );
};

export default ContactRouter;
