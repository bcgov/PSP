import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';

import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { exists, stripTrailingSlash } from '@/utils';

import UpdateResearchContainer from './tabs/fileDetails/update/UpdateSummaryContainer';
import ResearchTabsContainer from './tabs/ResearchTabsContainer';

export interface IResearchRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  researchFile?: ApiGen_Concepts_ResearchFile;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: () => void;
}

export const ResearchRouter: React.FC<IResearchRouterProps> = props => {
  const { path, url } = useRouteMatch();

  if (!exists(props.researchFile)) {
    return null;
  }

  // render edit forms
  if (props.isEditing) {
    return (
      <Switch>
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.FILE_DETAILS}`}>
          <UpdateResearchContainer
            researchFile={props.researchFile}
            ref={props.formikRef}
            onSuccess={props.onSuccess}
          />
        </Route>
        {/* Ignore property-related routes (which are handled in separate FilePropertyRouter) */}
        <Route path={`${stripTrailingSlash(path)}/property`}>
          <></>
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
          <ResearchTabsContainer
            researchFile={props.researchFile}
            setIsEditing={props.setIsEditing}
          />
        </Route>
        <Redirect from={`${path}`} to={`${stripTrailingSlash(url)}/${FileTabType.FILE_DETAILS}`} />
      </Switch>
    );
  }
};

export default ResearchRouter;
