import { FormikProps } from 'formik';
import React, { useContext } from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';
import { toast } from 'react-toastify';

import { InventoryTabNames, InventoryTabs } from '@/features/mapSideBar/property/InventoryTabs';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';
import { ApiGen_Concepts_ResearchFileProperty } from '@/models/api/generated/ApiGen_Concepts_ResearchFileProperty';
import { exists, isValidId } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { SideBarContext } from '../context/sidebarContext';
import { PropertyEditForms } from '../property/PropertyRouter';
import { UpdatePropertyDetailsContainer } from '../property/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import { PropertyContactEditContainer } from '../property/tabs/propertyDetailsManagement/update/PropertyContactEditContainer';
import { PropertyContactEditForm } from '../property/tabs/propertyDetailsManagement/update/PropertyContactEditForm';
import { PropertyManagementUpdateContainer } from '../property/tabs/propertyDetailsManagement/update/summary/PropertyManagementUpdateContainer';
import { PropertyManagementUpdateForm } from '../property/tabs/propertyDetailsManagement/update/summary/PropertyManagementUpdateForm';
import UpdatePropertyForm from '../property/tabs/propertyResearch/update/UpdatePropertyForm';
import UpdatePropertyResearchContainer from '../property/tabs/propertyResearch/update/UpdatePropertyResearchContainer';
import TakesAddContainer from '../property/tabs/takes/add/TakesAddContainer';
import { TakeForm } from '../property/tabs/takes/update/TakeForm';
import { TakesUpdateContainer } from '../property/tabs/takes/update/TakeUpdateContainer';
import ResearchStatusUpdateSolver from '../research/tabs/fileDetails/ResearchStatusUpdateSolver';
import { PropertyFileContainer } from '../shared/detail/PropertyFileContainer';

export interface IFilePropertyRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  file?: ApiGen_Concepts_File;
  fileType: ApiGen_CodeTypes_FileTypes;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  selectedFilePropertyId: number;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: () => void;
}

export const FilePropertyRouter: React.FC<IFilePropertyRouterProps> = props => {
  const { path, url } = useRouteMatch();

  const { setStaleLastUpdatedBy } = useContext(SideBarContext);
  const statusSolver = new ResearchStatusUpdateSolver(props?.file);

  const onChildSuccess = () => {
    props.setIsEditing(false);
    setStaleLastUpdatedBy(true);
    props.onSuccess();
  };

  if (!exists(props.file)) {
    return null;
  }

  const fileProperty = getFileProperty(props.file, props.selectedFilePropertyId);
  if (!exists(fileProperty)) {
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
                id={fileProperty.property.id}
                onSuccess={props.onSuccess}
                ref={props.formikRef}
              />
            );
          })()}
        </Route>
        <Route exact path={`${path}/${InventoryTabNames.takes}/:takeId`}>
          <TakesUpdateContainer
            fileProperty={fileProperty}
            View={TakeForm}
            ref={props.formikRef}
            onSuccess={onChildSuccess}
          />
        </Route>
        <Route exact path={`${path}/${InventoryTabNames.takes}`}>
          <TakesAddContainer
            fileProperty={fileProperty}
            View={TakeForm}
            ref={props.formikRef}
            onSuccess={onChildSuccess}
          />
        </Route>
        <Route exact path={`${path}/${InventoryTabNames.research}`}>
          <UpdatePropertyResearchContainer
            researchFileProperty={fileProperty as ApiGen_Concepts_ResearchFileProperty}
            onSuccess={props.onSuccess}
            ref={props.formikRef}
            View={UpdatePropertyForm}
          />
        </Route>
        <Route exact path={`${path}/${InventoryTabNames.management}`}>
          <PropertyManagementUpdateContainer
            onSuccess={onChildSuccess}
            propertyId={fileProperty?.propertyId}
            View={PropertyManagementUpdateForm}
            ref={props.formikRef}
          />
        </Route>
        <AppRoute
          exact
          path={`${path}/${InventoryTabNames.management}/${PropertyEditForms.UpdateContactContainer}/:contactId?`}
          customRender={({ match }) => (
            <PropertyContactEditContainer
              propertyId={fileProperty?.propertyId}
              contactId={match.params.contactId ? +match.params.contactId : 0}
              View={PropertyContactEditForm}
              onSuccess={onChildSuccess}
              ref={props.formikRef}
            />
          )}
          key={PropertyEditForms.UpdateContactContainer}
          title="Update Contact"
        ></AppRoute>
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
            statusSolver={statusSolver}
            onChildSuccess={props.onSuccess}
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

const getFileProperty = (file: ApiGen_Concepts_File, selectedFilePropertyId: number) => {
  const properties = file?.fileProperties || [];

  const fileProperty = properties.find(p => p.id === selectedFilePropertyId);
  if (exists(fileProperty) && !exists(fileProperty.file)) {
    fileProperty.file = file;
  }
  return fileProperty;
};

export default FilePropertyRouter;
