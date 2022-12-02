import { ReactComponent as Fence } from 'assets/images/fence.svg';
import GenericModal from 'components/common/GenericModal';
import { useMapSearch } from 'components/maps/hooks/useMapSearch';
import { MapStateActionTypes, MapStateContext } from 'components/maps/providers/MapStateContext';
import MapSideBarLayout from 'features/mapSideBar/layout/MapSideBarLayout';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { FormikProps } from 'formik';
import { Api_Lease } from 'models/api/Lease';
import * as React from 'react';
import { useMemo, useState } from 'react';
import { useEffect, useRef } from 'react';
import { useHistory } from 'react-router-dom';

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
  const { setState } = React.useContext(MapStateContext);
  const initialForm = useMemo<LeaseFormModel>(() => {
    return new LeaseFormModel();
  }, []);
  const { addLease } = useAddLease();
  const [addLeaseParams, setAddLeaseParams] = useState<
    { lease: Api_Lease; userOverride?: string } | undefined
  >();

  const { search } = useMapSearch();

  useEffect(() => {
    return () => {
      setState({
        type: MapStateActionTypes.SELECTED_LEASE_PROPERTY,
        selectedLeaseProperty: null,
      });
    };
  }, [initialForm, setState]);

  const saveLeaseFile = async (leaseFile: Api_Lease) => {
    const response = await addLease(leaseFile, (userOverrideMessage?: string) =>
      setAddLeaseParams({ lease: leaseFile, userOverride: userOverrideMessage }),
    );
    formikRef.current?.setSubmitting(false);
    if (!!response?.id) {
      formikRef.current?.resetForm();
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
      <AddLeaseForm onSubmit={saveLeaseFile} formikRef={formikRef} propertyInfo={null} />
      <GenericModal
        title="Warning"
        display={!!addLeaseParams}
        message={addLeaseParams?.userOverride}
        handleOk={async () => {
          if (!!addLeaseParams?.lease) {
            const leaseResponse = await addLease(addLeaseParams.lease, undefined, true);
            setAddLeaseParams(undefined);
            if (!!leaseResponse?.id) {
              history.push(`/lease/${leaseResponse?.id}`);
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
