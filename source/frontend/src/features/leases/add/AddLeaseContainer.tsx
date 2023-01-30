import { ReactComponent as Fence } from 'assets/images/fence.svg';
import GenericModal from 'components/common/GenericModal';
import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import { MapStateContext } from 'components/maps/providers/MapStateContext';
import { IMapProperty } from 'components/propertySelector/models';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { FormikHelpers, FormikProps } from 'formik';
import { Api_Lease } from 'models/api/Lease';
import * as React from 'react';
import { useMemo, useState } from 'react';
import { useRef } from 'react';
import { useHistory } from 'react-router-dom';
import { toast } from 'react-toastify';
import { mapFeatureToProperty } from 'utils/mapPropertyUtils';

import { useAddLease } from '../hooks/useAddLease';
import { LeaseFormModel } from '../models';
import AddLeaseForm from './AddLeaseForm';

export interface IAddLeaseContainerProps {
  onClose: () => void;
}

export const AddLeaseContainer: React.FunctionComponent<
  React.PropsWithChildren<IAddLeaseContainerProps>
> = props => {
  const history = useHistory();
  const formikRef = useRef<FormikProps<LeaseFormModel>>(null);
  const { selectedFileFeature } = React.useContext(MapStateContext);

  const { addLease } = useAddLease();
  const [addLeaseParams, setAddLeaseParams] = useState<
    { lease: Api_Lease; userOverride?: string } | undefined
  >();

  const { search } = useMapSearch();

  const initialProperty = useMemo<IMapProperty | null>(() => {
    if (selectedFileFeature) {
      return mapFeatureToProperty(selectedFileFeature);
    }
    return null;
  }, [selectedFileFeature]);

  const saveLeaseFile = async (
    leaseFormModel: LeaseFormModel,
    formikHelpers: FormikHelpers<LeaseFormModel>,
  ) => {
    const leaseApi = leaseFormModel.toApi();
    const response = await addLease(leaseFormModel.toApi(), (userOverrideMessage?: string) =>
      setAddLeaseParams({ lease: leaseApi, userOverride: userOverrideMessage }),
    );
    formikHelpers.setSubmitting(false);
    if (!!response?.id) {
      if (leaseApi.properties?.find(p => !p.property?.address && !p.property?.id)) {
        toast.warn(
          'Address could not be retrieved for this property, it will have to be provided manually in property details tab',
          { autoClose: 15000 },
        );
      }
      formikHelpers.resetForm();
      await search();
      history.replace(`/mapview/sidebar/lease/${response.id}`);
    }
  };

  const handleSave = () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancel = () => {
    props.onClose();
  };

  return (
    <MapSideBarLayout
      title="Create Lease/License"
      icon={<Fence />}
      footer={
        <SidebarFooter
          isOkDisabled={formikRef.current?.isSubmitting}
          onSave={handleSave}
          onCancel={handleCancel}
        />
      }
      showCloseButton
      onClose={handleCancel}
    >
      <AddLeaseForm onSubmit={saveLeaseFile} formikRef={formikRef} propertyInfo={initialProperty} />
      <GenericModal
        title="Warning"
        display={!!addLeaseParams}
        message={addLeaseParams?.userOverride}
        handleOk={async () => {
          if (!!addLeaseParams?.lease) {
            const leaseResponse = await addLease(addLeaseParams.lease, undefined, true);
            setAddLeaseParams(undefined);
            if (!!leaseResponse?.id) {
              history.push(`/mapview/sidebar/lease/${leaseResponse?.id}`);
            }
          }
        }}
        handleCancel={() => setAddLeaseParams(undefined)}
        okButtonText="Save Anyways"
        okButtonVariant="warning"
        cancelButtonText="Cancel"
      />
    </MapSideBarLayout>
  );
};

export default AddLeaseContainer;
