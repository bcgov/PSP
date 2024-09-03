import { FormikHelpers, FormikProps } from 'formik';
import { useCallback, useMemo, useRef, useState } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';

import LeaseIcon from '@/assets/images/lease-icon.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { IMapProperty } from '@/components/propertySelector/models';
import MapSideBarLayout from '@/features/mapSideBar/layout/MapSideBarLayout';
import SidebarFooter from '@/features/mapSideBar/shared/SidebarFooter';
import useApiUserOverride from '@/hooks/useApiUserOverride';
import { useInitialMapSelectorProperties } from '@/hooks/useInitialMapSelectorProperties';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { exists, isValidId } from '@/utils';
import { featuresetToMapProperty } from '@/utils/mapPropertyUtils';

import { useAddLease } from '../hooks/useAddLease';
import { LeaseFormModel } from '../models';
import AddLeaseForm from './AddLeaseForm';

export interface IAddLeaseContainerProps {
  onClose: () => void;
  onSuccess: (newLeaseId: number) => void;
}

export const AddLeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseContainerProps>
> = props => {
  const { onClose } = props;
  const history = useHistory();
  const formikRef = useRef<FormikProps<LeaseFormModel>>(null);
  const mapMachine = useMapStateMachine();

  const selectedFeatureDataset = mapMachine.selectedFeatureDataset;

  const withUserOverride = useApiUserOverride('Failed to save Lease File');
  const { addLease } = useAddLease();
  const [isValid, setIsValid] = useState<boolean>(true);

  const initialProperty = useMemo<IMapProperty | null>(() => {
    if (selectedFeatureDataset) {
      return featuresetToMapProperty(selectedFeatureDataset);
    }
    return null;
  }, [selectedFeatureDataset]);

  const { initialProperty: initialFormProperty, bcaLoading } =
    useInitialMapSelectorProperties(selectedFeatureDataset);
  if (!!initialProperty && !!initialFormProperty) {
    initialProperty.address = initialFormProperty?.formattedAddress;
  }

  const saveLeaseFile = async (
    leaseFormModel: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
    userOverrideCodes: UserOverrideCode[],
  ) => {
    const leaseApi = LeaseFormModel.toApi(leaseFormModel);
    const response = await addLease.execute(leaseApi, userOverrideCodes);
    formikHelpers.setSubmitting(false);

    if (exists(response) && isValidId(response?.id)) {
      if (leaseApi.fileProperties?.find(p => !p.property?.address && !p.property?.id)) {
        toast.warn(
          'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
          { autoClose: 15000 },
        );
      }
      mapMachine.refreshMapProperties();
      props.onSuccess(response.id);
    }
  };

  const handleSave = async () => {
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancel = () => {
    onClose();
  };

  const checkState = useCallback(() => {
    return formikRef?.current?.dirty && !formikRef?.current?.isSubmitting;
  }, [formikRef]);

  return (
    <MapSideBarLayout
      title="Create Lease/Licence"
      icon={
        <LeaseIcon
          title="Lease and Licence Icon"
          width="2.6rem"
          height="2.6rem"
          fill="currentColor"
          className="mr-2"
        />
      }
      footer={
        <SidebarFooter
          isOkDisabled={formikRef.current?.isSubmitting || bcaLoading}
          onSave={handleSave}
          onCancel={handleCancel}
          displayRequiredFieldError={isValid === false}
        />
      }
      showCloseButton
      onClose={handleCancel}
    >
      <AddLeaseForm
        onSubmit={(values: LeaseFormModel, formikHelpers: FormikHelpers<LeaseFormModel>) =>
          withUserOverride((useOverrideCodes: UserOverrideCode[]) =>
            saveLeaseFile(values, formikHelpers, useOverrideCodes),
          )
        }
        formikRef={formikRef}
        propertyInfo={initialProperty}
      />
      <ConfirmNavigation navigate={history.push} shouldBlockNavigation={checkState} />
    </MapSideBarLayout>
  );
};

export default AddLeaseContainer;
