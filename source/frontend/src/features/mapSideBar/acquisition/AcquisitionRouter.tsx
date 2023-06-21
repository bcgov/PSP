import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';

import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { Api_AcquisitionFile } from '@/models/api/AcquisitionFile';

import { AcquisitionContainerState } from './AcquisitionContainer';
import { EditFormType } from './EditFormNames';
import { AcquisitionFileTabs } from './tabs/AcquisitionFileTabs';
import { UpdateAgreementsContainer } from './tabs/agreement/update/UpdateAgreementsContainer';
import { UpdateAgreementsForm } from './tabs/agreement/update/UpdateAgreementsForm';
import { UpdateAcquisitionChecklistContainer } from './tabs/checklist/update/UpdateAcquisitionChecklistContainer';
import { UpdateAcquisitionChecklistForm } from './tabs/checklist/update/UpdateAcquisitionChecklistForm';
import { UpdateAcquisitionContainer } from './tabs/fileDetails/update/UpdateAcquisitionContainer';
import { UpdateAcquisitionForm } from './tabs/fileDetails/update/UpdateAcquisitionForm';
import { UpdateStakeHolderContainer } from './tabs/stakeholders/update/UpdateStakeHolderContainer';
import { UpdateStakeHolderForm } from './tabs/stakeholders/update/UpdateStakeHolderForm';

export interface IAcquisitionRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  acquisitionFile?: Api_AcquisitionFile;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  activeEditForm?: EditFormType;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  setContainerState: (value: Partial<AcquisitionContainerState>) => void;
  onSuccess: () => void;
}

export const AcquisitionRouter: React.FC<IAcquisitionRouterProps> = props => {
  const { path, url } = useRouteMatch();

  if (props.acquisitionFile === undefined || props.acquisitionFile === null) {
    return null;
  }

  // render edit forms
  if (props.isEditing) {
    return (
      <Switch>
        <Route exact path={`${path}/${FileTabType.FILE_DETAILS}`}>
          <UpdateAcquisitionContainer
            ref={props.formikRef}
            acquisitionFile={props.acquisitionFile}
            onSuccess={props.onSuccess}
            View={UpdateAcquisitionForm}
          />
        </Route>
        <Route exact path={`${path}/${FileTabType.CHECKLIST}`}>
          <UpdateAcquisitionChecklistContainer
            formikRef={props.formikRef}
            acquisitionFile={props.acquisitionFile}
            onSuccess={props.onSuccess}
            View={UpdateAcquisitionChecklistForm}
          />
        </Route>
        <Route exact path={`${path}/${FileTabType.AGREEMENTS}`}>
          <UpdateAgreementsContainer
            acquisitionFileId={props.acquisitionFile.id || -1}
            View={UpdateAgreementsForm}
            formikRef={props.formikRef}
            onSuccess={() => props.setIsEditing(false)}
          />
        </Route>
        <Route exact path={`${path}/${FileTabType.STAKEHOLDERS}`}>
          <UpdateStakeHolderContainer
            View={UpdateStakeHolderForm}
            formikRef={props.formikRef}
            acquisitionFile={props.acquisitionFile}
            onSuccess={() => props.setIsEditing(false)}
          />
        </Route>
      </Switch>
    );
  } else {
    // render read-only views
    return (
      <Switch>
        {/* Ignore property-related routes (which are handled in separate FilePropertyRouter) */}
        <Route path={`${path}/property`}>
          <></>
        </Route>
        <Route path={`${path}/:tab`}>
          <AcquisitionFileTabs
            acquisitionFile={props.acquisitionFile}
            defaultTab={props.defaultFileTab}
            setIsEditing={props.setIsEditing}
            setContainerState={props.setContainerState}
          />
        </Route>
        <Redirect from={`${path}`} to={`${url}/${FileTabType.FILE_DETAILS}`} />
      </Switch>
    );
  }
};

export default AcquisitionRouter;
