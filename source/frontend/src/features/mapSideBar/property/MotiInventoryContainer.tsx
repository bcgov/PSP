import { FormikProps } from 'formik';
import React, { useEffect, useRef, useState } from 'react';
import styled from 'styled-components';

import { ReactComponent as LotSvg } from '@/assets/images/icon-lot.svg';
import GenericModal from '@/components/common/GenericModal';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import PropertyViewSelector from '@/features/mapSideBar/property/PropertyViewSelector';
import { PROPERTY_TYPES, useComposedProperties } from '@/hooks/repositories/useComposedProperties';
import { Api_Property } from '@/models/api/Property';

import MapSideBarLayout from '../layout/MapSideBarLayout';
import SidebarFooter from '../shared/SidebarFooter';
import { MotiInventoryHeader } from './MotiInventoryHeader';

export interface IMotiInventoryContainerProps {
  id?: number;
  pid?: string;
  onClose: () => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MotiInventoryContainer: React.FunctionComponent<
  React.PropsWithChildren<IMotiInventoryContainerProps>
> = props => {
  const [showCancelConfirmModal, setShowCancelConfirmModal] = useState<boolean>(false);

  const [isEditing, setIsEditing] = useState<boolean>(false);

  const mapMachine = useMapStateMachine();

  const formikRef = useRef<FormikProps<any>>(null);

  const composedPropertyState = useComposedProperties({
    id: props.id,
    pid: props?.pid === undefined || isNaN(+props.pid) ? undefined : +props.pid,
    propertyTypes: [
      PROPERTY_TYPES.ASSOCIATIONS,
      PROPERTY_TYPES.LTSA,
      PROPERTY_TYPES.PIMS_API,
      PROPERTY_TYPES.BC_ASSESSMENT,
      PROPERTY_TYPES.PARCEL_MAP,
    ],
  });

  useEffect(() => {
    setIsEditing(false);
  }, [props.pid]);

  const onSuccess = () => {
    props.id && composedPropertyState.apiWrapper?.execute(props.id);
    setIsEditing(false);
  };

  const handleSaveClick = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setShowCancelConfirmModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setIsEditing(false);
    setShowCancelConfirmModal(false);
  };

  const handleZoom = (apiProperty?: Api_Property | undefined) => {
    if (apiProperty?.longitude !== undefined && apiProperty?.latitude !== undefined) {
      mapMachine.requestFlyToLocation({ lat: apiProperty.latitude, lng: apiProperty.longitude });
    }
  };

  return (
    <MapSideBarLayout
      title="Property Information"
      header={
        <MotiInventoryHeader
          composedProperty={composedPropertyState.composedProperty}
          onZoom={handleZoom}
          isLoading={
            composedPropertyState.ltsaWrapper?.loading ||
            composedPropertyState.apiWrapper?.loading ||
            composedPropertyState.parcelMapWrapper?.loading ||
            false
          }
        />
      }
      footer={
        isEditing && (
          <SidebarFooter
            isOkDisabled={formikRef?.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancelClick}
          />
        )
      }
      icon={<LotIcon className="mr-1" />}
      showCloseButton
      onClose={props.onClose}
    >
      <>
        <PropertyViewSelector
          composedPropertyState={composedPropertyState}
          isEditMode={isEditing}
          setEditMode={setIsEditing}
          onSuccess={onSuccess}
          ref={formikRef}
        />
        <GenericModal
          display={showCancelConfirmModal}
          title={'Confirm changes'}
          message={
            <>
              <div>If you cancel now, this property information will not be saved.</div>
              <br />
              <strong>Are you sure you want to Cancel?</strong>
            </>
          }
          handleOk={handleCancelConfirm}
          handleCancel={() => setShowCancelConfirmModal(false)}
          okButtonText="Ok"
          cancelButtonText="Resume editing"
          show
        />
      </>
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;

const LotIcon = styled(LotSvg)`
  width: 3rem;
  height: 3rem;
`;
