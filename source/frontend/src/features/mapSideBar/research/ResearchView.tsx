import { FormikProps } from 'formik';
import * as React from 'react';
import { matchPath, Route, useHistory, useRouteMatch } from 'react-router-dom';

import { FileTypes } from '@/constants';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';
import { stripTrailingSlash } from '@/utils';

import FilePropertyRouter from '../router/FilePropertyRouter';
import { FileTabType } from '../shared/detail/FileTabs';
import ResearchRouter from './ResearchRouter';

export interface IViewSelectorProps {
  researchFile?: ApiGen_Concepts_ResearchFile;
  setEditMode: (isEditing: boolean) => void;
  isEditing: boolean;
  onSuccess: () => void;
}

const ResearchView = React.forwardRef<FormikProps<any>, IViewSelectorProps>((props, formikRef) => {
  const match = useRouteMatch();

  const { location } = useHistory();
  const propertiesMatch = matchPath<Record<string, string>>(
    location.pathname,
    `${stripTrailingSlash(match.path)}/property/:menuIndex/:tab?`,
  );

  const selectedMenuIndex = propertiesMatch !== null ? Number(propertiesMatch.params.menuIndex) : 0;

  return (
    <>
      <ResearchRouter
        formikRef={formikRef}
        isEditing={props.isEditing}
        setIsEditing={props.setEditMode}
        defaultFileTab={FileTabType.FILE_DETAILS}
        defaultPropertyTab={InventoryTabNames.research}
        onSuccess={props.onSuccess}
        researchFile={props.researchFile}
      />
      <Route
        path={`${stripTrailingSlash(match.path)}/property/:menuIndex`}
        render={() => (
          <FilePropertyRouter
            formikRef={formikRef}
            selectedMenuIndex={selectedMenuIndex}
            file={props.researchFile}
            fileType={FileTypes.Research}
            isEditing={props.isEditing}
            setIsEditing={props.setEditMode}
            defaultFileTab={FileTabType.FILE_DETAILS}
            defaultPropertyTab={InventoryTabNames.research}
            onSuccess={props.onSuccess}
          />
        )}
      />
    </>
  );
});

export default ResearchView;
