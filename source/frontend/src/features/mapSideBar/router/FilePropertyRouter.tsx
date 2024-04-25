import { FormikProps } from 'formik';
import React, { useContext } from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';
import { toast } from 'react-toastify';

import { FileTypes } from '@/constants';
import { InventoryTabNames, InventoryTabs } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { exists, isValidId } from '@/utils';

import { SideBarContext } from '../context/sidebarContext';
import { UpdatePropertyDetailsContainer } from '../property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import UpdatePropertyResearchContainer from '../property/tabs/propertyResearch/update/UpdatePropertyResearchContainer';
import { TakesUpdateContainer } from '../property/tabs/takes/update/TakesUpdateContainer';
import { TakesUpdateForm } from '../property/tabs/takes/update/TakesUpdateForm';
import { PropertyFileContainer } from '../shared/detail/PropertyFileContainer';

export interface IFilePropertyRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  file?: ApiGen_Concepts_File;
  fileType: FileTypes;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  selectedMenuIndex: number;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: () => void;
}

export const FilePropertyRouter: React.FC<IFilePropertyRouterProps> = props => {
  const { path, url } = useRouteMatch();

  const { setStaleLastUpdatedBy } = useContext(SideBarContext);

  const onChildSuccess = () => {
    props.setIsEditing(false);
    setStaleLastUpdatedBy(true);
    props.onSuccess();
  };

  if (!exists(props.file)) {
    return null;
  }

  const fileProperty = getFileProperty(props.file, props.selectedMenuIndex);
  if (fileProperty == null) {
    toast.warn('Could not find property in the file, showing file details instead', {
      autoClose: 15000,
    });
    return <Redirect to={`/mapview/sidebar/${props.fileType}/${props.file.id}`} />;
  }

  // render edit forms
  if (props.isEditing) {
    return (
      <Switch>
        <Route exact path={`${path}/${InventoryTabNames.property}`}>
          {(() => {
            if (!isValidId(fileProperty?.property?.id)) {
              throw Error('Cannot edit property without a valid id');
            }
            return (
              <UpdatePropertyDetailsContainer
                id={fileProperty!.property!.id}
                onSuccess={props.onSuccess}
                ref={props.formikRef}
              />
            );
          })()}
        </Route>
        <Route exact path={`${path}/${InventoryTabNames.takes}`}>
          <TakesUpdateContainer
            fileProperty={fileProperty}
            View={TakesUpdateForm}
            ref={props.formikRef}
            onSuccess={onChildSuccess}
          />
        </Route>
        <Route exact path={`${path}/${InventoryTabNames.research}`}>
          <UpdatePropertyResearchContainer
            researchFileProperty={fileProperty as ApiGen_Concepts_ResearchFileProperty}
            onSuccess={props.onSuccess}
            ref={props.formikRef}
          />
        </Route>

        <Redirect from={`${path}`} to={`${url}/${InventoryTabNames.property}?edit=true`} />
      </Switch>
    );
  } else {
    // render read-only views
    return (
      <Switch>
        <Route path={`${path}/:tab`}>
          <PropertyFileContainer
            setEditing={() => props.setIsEditing(true)}
            fileProperty={fileProperty}
            defaultTab={props.defaultPropertyTab}
            customTabs={[]}
            View={InventoryTabs}
            fileContext={props.fileType}
          />
        </Route>
        <Redirect
          from={`${path}`}
          to={`${url}/${props.defaultPropertyTab ?? InventoryTabNames.property}`}
        />
      </Switch>
    );
  }
};

const getFileProperty = (file: ApiGen_Concepts_File, selectedMenuIndex: number) => {
  const properties = file?.fileProperties || [];
  const selectedPropertyIndex = selectedMenuIndex - 1;

  if (selectedPropertyIndex < 0 || selectedPropertyIndex >= properties.length) {
    return null;
  }

  const fileProperty = properties[selectedPropertyIndex];
  if (exists(fileProperty)) {
    fileProperty.file = file;
  }
  return fileProperty;
};

export default FilePropertyRouter;
