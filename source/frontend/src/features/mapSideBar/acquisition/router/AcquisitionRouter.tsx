import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';

import Claims from '@/constants/claims';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists, stripTrailingSlash } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { UpdateChecklistForm } from '../../shared/tabs/checklist/update/UpdateChecklistForm';
import { AcquisitionFileTabs } from '../tabs/AcquisitionFileTabs';
import { UpdateAgreementsContainer } from '../tabs/agreement/update/UpdateAgreementsContainer';
import { UpdateAgreementsForm } from '../tabs/agreement/update/UpdateAgreementsForm';
import { UpdateAcquisitionChecklistContainer } from '../tabs/checklist/update/UpdateAcquisitionChecklistContainer';
import AddForm8Container from '../tabs/expropriation/form8/add/AddForm8Container';
import { UpdateForm8Container } from '../tabs/expropriation/form8/update/UpdateForm8Container';
import UpdateForm8Form from '../tabs/expropriation/form8/UpdateForm8Form';
import { UpdateAcquisitionContainer } from '../tabs/fileDetails/update/UpdateAcquisitionContainer';
import { UpdateAcquisitionForm } from '../tabs/fileDetails/update/UpdateAcquisitionForm';
import { UpdateStakeHolderContainer } from '../tabs/stakeholders/update/UpdateStakeHolderContainer';
import { UpdateStakeHolderForm } from '../tabs/stakeholders/update/UpdateStakeHolderForm';

export interface IAcquisitionRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  acquisitionFile?: ApiGen_Concepts_AcquisitionFile;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: () => void;
}

export const AcquisitionRouter: React.FC<IAcquisitionRouterProps> = props => {
  const { path, url } = useRouteMatch();

  if (!exists(props.acquisitionFile)) {
    return null;
  }

  // render edit forms
  if (props.isEditing) {
    return (
      <Switch>
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.FILE_DETAILS}`}>
          <UpdateAcquisitionContainer
            ref={props.formikRef}
            acquisitionFile={props.acquisitionFile}
            onSuccess={props.onSuccess}
            View={UpdateAcquisitionForm}
          />
        </Route>
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.CHECKLIST}`}>
          <UpdateAcquisitionChecklistContainer
            formikRef={props.formikRef}
            acquisitionFile={props.acquisitionFile}
            onSuccess={props.onSuccess}
            View={UpdateChecklistForm}
          />
        </Route>
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.AGREEMENTS}`}>
          <UpdateAgreementsContainer
            acquisitionFileId={props.acquisitionFile.id || -1}
            View={UpdateAgreementsForm}
            formikRef={props.formikRef}
            onSuccess={props.onSuccess}
          />
        </Route>
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.STAKEHOLDERS}`}>
          <UpdateStakeHolderContainer
            View={UpdateStakeHolderForm}
            formikRef={props.formikRef}
            acquisitionFile={props.acquisitionFile}
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
        <AppRoute
          exact
          path={`${stripTrailingSlash(path)}/${FileTabType.EXPROPRIATION}/add`}
          customRender={() =>
            props.acquisitionFile?.id ? (
              <AddForm8Container
                acquisitionFileId={props.acquisitionFile?.id}
                View={UpdateForm8Form}
                onSuccess={props.onSuccess}
              />
            ) : null
          }
          claim={Claims.ACQUISITION_EDIT}
          key={'expropriation'}
          title={'Add Expropriation'}
        />
        <AppRoute
          path={`${stripTrailingSlash(path)}/${FileTabType.EXPROPRIATION}/:form8Id`}
          customRender={({ match }) => (
            <UpdateForm8Container
              form8Id={+match.params.form8Id}
              View={UpdateForm8Form}
              onSuccess={props.onSuccess}
            />
          )}
          claim={Claims.ACQUISITION_EDIT}
          key={'expropriation'}
          title={'Expropriation'}
        />
        <Route path={`${stripTrailingSlash(path)}/:tab`}>
          <AcquisitionFileTabs
            acquisitionFile={props.acquisitionFile}
            defaultTab={props.defaultFileTab}
            setIsEditing={props.setIsEditing}
          />
        </Route>
        <Redirect from={`${path}`} to={`${stripTrailingSlash(url)}/${FileTabType.FILE_DETAILS}`} />
      </Switch>
    );
  }
};

export default AcquisitionRouter;
