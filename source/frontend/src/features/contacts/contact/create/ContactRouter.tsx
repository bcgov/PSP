import React from 'react';
import { Redirect, Switch, useRouteMatch } from 'react-router-dom';

import { ContactTypes } from '@/features/contacts/interfaces';
import AppRoute from '@/utils/AppRoute';

import CreateOrganizationForm from './Organization/CreateOrganizationForm';
import CreatePersonForm from './Person/CreatePersonForm';

export interface IContactPage {
  component: any;
  title: string;
}

export const pages = new Map<ContactTypes, IContactPage>([
  [
    ContactTypes.INDIVIDUAL,
    {
      component: CreatePersonForm,
      title: 'Create Person',
    },
  ],
  [
    ContactTypes.ORGANIZATION,
    {
      component: CreateOrganizationForm,
      title: 'Create Organization',
    },
  ],
]);

export const ContactRouter: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  // The `path` lets us build <Route> paths that are relative to the parent route, while
  // the `url` lets us build relative links.
  const { path, url } = useRouteMatch();
  return (
    <Switch>
      <Redirect exact from={path} to={`${url}/${ContactTypes.INDIVIDUAL}`} />
      {Array.from(pages.entries()).map(([pageName, page]) => (
        <AppRoute
          path={`${path}/${pageName}`}
          customComponent={page.component ?? React.Fragment}
          title={`${page.title}`}
          exact
          key={pageName}
        ></AppRoute>
      ))}
    </Switch>
  );
};

export default ContactRouter;
