import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';
import { toast } from 'react-toastify';

import { FileTypes } from '@/constants';
import { InventoryTabNames, InventoryTabs } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { UpdatePropertyDetailsContainer } from '../../property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import { TakesUpdateContainer } from '../../property/tabs/takes/update/TakesUpdateContainer';
import { TakesUpdateForm } from '../../property/tabs/takes/update/TakesUpdateForm';
import { PropertyFileContainer } from '../../shared/detail/PropertyFileContainer';

export interface IFilePropertyRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  acquisitionFile?: Api_AcquisitionFile;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  selectedMenuIndex: number;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: () => void;
}

export const FilePropertyRouter: React.FC<IFilePropertyRouterProps> = props => {
  const { path, url } = useRouteMatch();

  if (props.acquisitionFile === undefined || props.acquisitionFile === null) {
    return null;
  }

  const fileProperty = getAcquisitionFileProperty(props.acquisitionFile, props.selectedMenuIndex);

  if (fileProperty == null) {
    toast.warn('Could not find property in the file, showing file details instead', {
      autoClose: 15000,
    });
    return <Redirect to={`/mapview/sidebar/acquisition/${props.acquisitionFile.id}`} />;
  }

  // render edit forms
  if (props.isEditing) {
    return (
      <Switch>
        <Route exact path={`${path}/${InventoryTabNames.property}`}>
          {(() => {
            if (fileProperty?.property?.id === undefined) {
              throw Error('Cannot edit property without a valid id');
            }
            return (
              <UpdatePropertyDetailsContainer
                id={fileProperty?.property?.id}
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
            onSuccess={() => props.setIsEditing(false)}
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
            withRouter
            setEditFileProperty={() => props.setIsEditing(true)}
            setEditTakes={() => props.setIsEditing(true)}
            fileProperty={fileProperty}
            defaultTab={props.defaultPropertyTab}
            customTabs={[]}
            View={InventoryTabs}
            fileContext={FileTypes.Acquisition}
          />
        </Route>
        <Redirect from={`${path}`} to={`${url}/${InventoryTabNames.property}`} />
      </Switch>
    );
  }
};

const getAcquisitionFileProperty = (
  acquisitionFile: Api_AcquisitionFile,
  selectedMenuIndex: number,
) => {
  const properties = acquisitionFile?.fileProperties || [];
  const selectedPropertyIndex = selectedMenuIndex - 1;

  if (selectedPropertyIndex < 0 || selectedPropertyIndex >= properties.length) {
    return null;
  }

  const acquisitionFileProperty = properties[selectedPropertyIndex];
  if (!!acquisitionFileProperty.file) {
    acquisitionFileProperty.file = acquisitionFile;
  }
  return acquisitionFileProperty;
};

export default FilePropertyRouter;
