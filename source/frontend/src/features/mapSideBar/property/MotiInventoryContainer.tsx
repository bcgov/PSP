import { FormikProps } from 'formik';
import React, { useEffect, useRef, useState } from 'react';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import { ReactComponent as LotSvg } from '@/assets/images/icon-lot.svg';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PROPERTY_TYPES, useComposedProperties } from '@/hooks/repositories/useComposedProperties';
import { useQuery } from '@/hooks/use-query';
import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { exists } from '@/utils/utils';

import MapSideBarLayout from '../layout/MapSideBarLayout';
import SidebarFooter from '../shared/SidebarFooter';
import { MotiInventoryHeader } from './MotiInventoryHeader';
import PropertyRouter from './PropertyRouter';

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
  const query = useQuery();
  const { push } = useHistory();
  const match = useRouteMatch();
  const tabMatch = useRouteMatch<{ tab: string; propertyId: string }>(`${match.path}/:tab`);
  const isEditing = query.get('edit') === 'true';
  const [isValid, setIsValid] = useState<boolean>(true);

  const { setModalContent, setDisplayModal } = useModalContext();
  const mapMachine = useMapStateMachine();

  const formikRef = useRef<FormikProps<any>>(null);

  const composedPropertyState = useComposedProperties({
    id: props.id,
    pid:
      props?.pid === undefined || props?.pid === '' || isNaN(+props.pid) ? undefined : +props.pid,
    propertyTypes: [
      PROPERTY_TYPES.ASSOCIATIONS,
      PROPERTY_TYPES.LTSA,
      PROPERTY_TYPES.PIMS_API,
      PROPERTY_TYPES.BC_ASSESSMENT,
      PROPERTY_TYPES.PARCEL_MAP,
    ],
  });

  useEffect(() => {
    push({ search: '' });
  }, [props.pid, push]);

  const onSuccess = () => {
    props.id && composedPropertyState.apiWrapper?.execute(props.id);
    stripEditFromPath();
  };

  const handleSaveClick = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      await formikRef.current?.submitForm();
      setIsValid(formikRef.current?.isValid ?? false);
    }
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => {
            handleCancelConfirm();
            setDisplayModal(false);
          },
          handleCancel: () => setDisplayModal(false),
        });
        setDisplayModal(true);
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
    stripEditFromPath();
  };

  const stripEditFromPath = () => {
    if (!tabMatch) {
      return;
    }
    const path = generatePath('/mapview/sidebar/property/:propertyId/:tab', {
      propertyId: tabMatch?.params.propertyId,
      tab: tabMatch?.params.tab,
    });
    push(path, { search: query.toString() });
  };

  const handleZoom = (apiProperty: ApiGen_Concepts_Property | undefined) => {
    // todo:the method 'exists' here should allow the compiler to validate the child property. this works correctly in typescropt 5.3 +
    if (exists(apiProperty?.longitude) && exists(apiProperty?.latitude)) {
      mapMachine.requestFlyToLocation({ lat: apiProperty!.latitude, lng: apiProperty!.longitude });
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
            displayRequiredFieldError={!isValid}
          />
        )
      }
      icon={<LotIcon className="mr-1" />}
      showCloseButton
      onClose={props.onClose}
    >
      <PropertyRouter
        composedPropertyState={composedPropertyState}
        onSuccess={onSuccess}
        ref={formikRef}
      />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;

const LotIcon = styled(LotSvg)`
  width: 3rem;
  height: 3rem;
`;
