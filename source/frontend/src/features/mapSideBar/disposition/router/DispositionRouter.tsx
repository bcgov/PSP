import { FormikProps } from 'formik';
import React from 'react';
import { Redirect, Route, Switch, useRouteMatch } from 'react-router-dom';

import Claims from '@/constants/claims';
import { InventoryTabNames } from '@/features/mapSideBar/property/InventoryTabs';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { Api_DispositionFile } from '@/models/api/DispositionFile';
import { stripTrailingSlash } from '@/utils';
import AppRoute from '@/utils/AppRoute';

import { UpdateChecklistForm } from '../../shared/tabs/checklist/update/UpdateChecklistForm';
import { UpdateDispositionChecklistContainer } from '../tabs/checklist/update/UpdateDispositionChecklistContainer';
import DispositionFileTabs from '../tabs/DispositionFileTabs';
import AddDispositionOfferContainer from '../tabs/offersAndSale/dispositionOffer/add/AddDispositionOfferContainer';
import DispositionOfferForm from '../tabs/offersAndSale/dispositionOffer/form/DispositionOfferForm';
import UpdateDispositionOfferContainer from '../tabs/offersAndSale/dispositionOffer/update/UpdateDispositionOfferContainer';

export interface IDispositionRouterProps {
  formikRef: React.Ref<FormikProps<any>>;
  dispositionFile?: Api_DispositionFile;
  isEditing: boolean;
  setIsEditing: (value: boolean) => void;
  defaultFileTab: FileTabType;
  defaultPropertyTab: InventoryTabNames;
  onSuccess: () => void;
}

export const DispositionRouter: React.FC<IDispositionRouterProps> = props => {
  const { path, url } = useRouteMatch();

  if (props.dispositionFile === undefined || props.dispositionFile === null) {
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
        <Route exact path={`${stripTrailingSlash(path)}/${FileTabType.CHECKLIST}`}>
          <UpdateDispositionChecklistContainer
            formikRef={props.formikRef}
            dispositionFile={props.dispositionFile}
            onSuccess={props.onSuccess}
            View={UpdateChecklistForm}
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
        <AppRoute
          exact
          path={`${stripTrailingSlash(path)}/${FileTabType.OFFERS_AND_SALE}/offers/new`}
          customRender={() => (
            <AddDispositionOfferContainer
              dispositionFileId={props.dispositionFile?.id ?? 0}
              View={DispositionOfferForm}
            ></AddDispositionOfferContainer>
          )}
          claim={Claims.DISPOSITION_EDIT}
          key={'disposition'}
          title={'Add Disposition Offer'}
        />
        <AppRoute
          exact
          path={`${stripTrailingSlash(path)}/${FileTabType.OFFERS_AND_SALE}/offers/:offerId/update`}
          customRender={({ match }) => (
            <UpdateDispositionOfferContainer
              dispositionFileId={props.dispositionFile?.id ?? 0}
              dispositionOfferId={match.params.offerId}
              View={DispositionOfferForm}
            ></UpdateDispositionOfferContainer>
          )}
          claim={Claims.DISPOSITION_EDIT}
          key={'disposition'}
          title={'Add Disposition Offer'}
        />
        <Route path={`${stripTrailingSlash(path)}/:tab`}>
          <DispositionFileTabs
            dispositionFile={props.dispositionFile}
            defaultTab={props.defaultFileTab}
            setIsEditing={props.setIsEditing}
          />
        </Route>
        <Redirect from={`${path}`} to={`${stripTrailingSlash(url)}/${FileTabType.FILE_DETAILS}`} />
      </Switch>
    );
  }
};

export default DispositionRouter;
