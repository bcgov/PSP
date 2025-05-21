import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Switch, useHistory, useRouteMatch } from 'react-router-dom';

import { UpdatePropertyDetailsContainer } from '@/features/mapSideBar/property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import ComposedPropertyState from '@/hooks/repositories/useComposedProperties';
import { useQuery } from '@/hooks/use-query';
import { stripTrailingSlash } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { InventoryTabNames } from './InventoryTabs';
import PropertyContainer from './PropertyContainer';
import { PropertyContactEditContainer } from './tabs/propertyDetailsManagement/update/PropertyContactEditContainer';
import { PropertyContactEditForm } from './tabs/propertyDetailsManagement/update/PropertyContactEditForm';
import { PropertyManagementUpdateContainer } from './tabs/propertyDetailsManagement/update/summary/PropertyManagementUpdateContainer';
import { PropertyManagementUpdateForm } from './tabs/propertyDetailsManagement/update/summary/PropertyManagementUpdateForm';

export enum PropertyEditForms {
  UpdatePropertyDetailsContainer = 'UpdatePropertyDetailsContainer',
  UpdateManagementSummaryContainer = 'UpdateManagementSummaryContainer',
  UpdateContactContainer = 'UpdateContactContainer',
}

export interface EditManagementState {
  form: PropertyEditForms;
  childId: number | null;
}

export interface IPropertyRouterProps {
  onSuccess: () => void;
  composedPropertyState: ComposedPropertyState;
}

const PropertyRouter = React.forwardRef<FormikProps<any>, IPropertyRouterProps>(
  (props, formikRef) => {
    const query = useQuery();
    const isEditing = query.get('edit') === 'true';
    const { path } = useRouteMatch();
    const history = useHistory();

    const setIsEditing = (value: boolean) => {
      if (value) {
        query.set('edit', value.toString());
      } else {
        query.delete('edit');
      }
      history.push({ search: query.toString() });
    };

    if (isEditing && !!props?.composedPropertyState?.apiWrapper?.response?.id) {
      return (
        <Switch>
          <AppRoute
            exact
            path={`${stripTrailingSlash(path)}/${InventoryTabNames.property}`}
            customRender={({ match }) => (
              <UpdatePropertyDetailsContainer
                id={match.params.propertyId}
                onSuccess={() => {
                  setIsEditing(false);
                  props.onSuccess();
                }}
                ref={formikRef}
              />
            )}
            key={PropertyEditForms.UpdatePropertyDetailsContainer}
            title={'Update Property Details'}
          ></AppRoute>
          <AppRoute
            exact
            path={`${stripTrailingSlash(path)}/${InventoryTabNames.management}/${
              PropertyEditForms.UpdateContactContainer
            }/:contactId?`}
            customRender={({ match }) => (
              <PropertyContactEditContainer
                propertyId={match.params.propertyId}
                contactId={match.params.contactId ? +match.params.contactId : 0}
                View={PropertyContactEditForm}
                onSuccess={() => {
                  setIsEditing(false);
                  props.onSuccess();
                }}
                ref={formikRef}
              />
            )}
            key={PropertyEditForms.UpdateContactContainer}
            title="Update Contact"
          ></AppRoute>
          <AppRoute
            path={`${stripTrailingSlash(path)}/${InventoryTabNames.management}`}
            customRender={({ match }) => (
              <PropertyManagementUpdateContainer
                propertyId={match.params.propertyId}
                View={PropertyManagementUpdateForm}
                onSuccess={() => {
                  setIsEditing(false);
                  props.onSuccess();
                }}
                ref={formikRef}
              />
            )}
            key={PropertyEditForms.UpdateManagementSummaryContainer}
            title="Update Management Summary"
          ></AppRoute>
        </Switch>
      );
    } else {
      return (
        <Switch>
          <AppRoute
            path={`${stripTrailingSlash(path)}/:tab`}
            customRender={() => (
              <PropertyContainer
                composedPropertyState={props.composedPropertyState}
                onChildSuccess={props.onSuccess}
              />
            )}
            key={'property_tabs'}
            title={'Property Tabs'}
          ></AppRoute>

          <Redirect
            from={`${path}`}
            to={`${stripTrailingSlash(path)}/${
              path.includes('non-inventory-property')
                ? InventoryTabNames.title
                : InventoryTabNames.property
            }`}
          />
        </Switch>
      );
    }
  },
);

export default PropertyRouter;
