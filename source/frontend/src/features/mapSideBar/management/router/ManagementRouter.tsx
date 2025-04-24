import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';

import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { exists, stripTrailingSlash } from '@/utils';

import ManagementFileTabs from '../tabs/ManagementFileTabs';
import UpdateManagementContainer from '../update/UpdateManagementContainer';
import UpdateManagementForm from '../update/UpdateManagementForm';

export interface IManagementRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  managementFile?: ApiGen_Concepts_ManagementFile;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: (updateProperties?: boolean, updateFile?: boolean) => void;
}

export const ManagementRouter: React.FC<IManagementRouterProps> = props => {
  const { path, url } = useRouteMatch();

  if (!exists(props.managementFile)) {
    return null;
  }

  // render edit forms
  if (props.isEditing) {
    return (
      <Switch>
        {/* Ignore property-related routes (which are handled in separate FilePropertyRouter) */}
        <Route path={`${stripTrailingSlash(path)}/property`}>
          <></>
        </Route>
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.FILE_DETAILS}`}>
          <UpdateManagementContainer
            ref={props.formikRef}
            managementFile={props.managementFile}
            onSuccess={props.onSuccess}
            View={UpdateManagementForm}
          />
        </Route>
        <Redirect
          from={`${path}`}
          to={`${stripTrailingSlash(url)}/${FileTabType.FILE_DETAILS}?edit=true`}
        />
      </Switch>
    );
  } else {
    // render read-only views
    return (
      <Switch>
        {/* Ignore property-related routes (which are handled in separate FilePropertyRouter) */}
        <Route path={`${stripTrailingSlash(path)}/property`}>
          <></>
        </Route>
        <Route path={`${stripTrailingSlash(path)}/:tab`}>
          <ManagementFileTabs
            managementFile={props.managementFile}
            defaultTab={props.defaultFileTab}
            setIsEditing={props.setIsEditing}
          />
        </Route>
        <Redirect from={`${path}`} to={`${stripTrailingSlash(url)}/${FileTabType.FILE_DETAILS}`} />
      </Switch>
    );
  }
};

export default ManagementRouter;
