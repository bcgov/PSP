import { ReactComponent as LotSvg } from 'assets/images/icon-lot.svg';
import axios, { AxiosError } from 'axios';
import GenericModal from 'components/common/GenericModal';
import ComposedProperty from 'features/properties/map/propertyInformation/ComposedProperty';
import PropertyViewSelector from 'features/properties/map/propertyInformation/PropertyViewSelector';
import SidebarFooter from 'features/properties/map/shared/SidebarFooter';
import { FormikProps } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { usePropertyAssociations } from 'hooks/usePropertyAssociations';
import { IApiError } from 'interfaces/IApiError';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { pidFormatter } from 'utils';

import MapSideBarLayout from './layout/MapSideBarLayout';
import { MotiInventoryHeader } from './MotiInventoryHeader';

export interface IMotiInventoryContainerProps {
  id?: number;
  pid?: string;
  onClose: () => void;
  onZoom: (apiProperty?: IPropertyApiModel | undefined) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MotiInventoryContainer: React.FunctionComponent<IMotiInventoryContainerProps> = props => {
  const isMounted = useIsMounted();

  const [showCancelConfirmModal, setShowCancelConfirmModal] = useState<boolean>(false);

  const [isEditing, setIsEditing] = useState<boolean>(false);

  const [formikRef, setFormikRef] = useState<React.RefObject<FormikProps<any>> | undefined>(
    undefined,
  );
    !!props.id ? InventoryTabNames.property : InventoryTabNames.title,
  );
  const pid = props.pid ? props.pid : apiProperty?.pid;

  // First, fetch property information from PSP API
  const { getPropertyLoading: propertyLoading, getProperty } = useProperties();

  const {
    getPropertyAssociations,
    isLoading: propertyAssociationsLoading,
  } = usePropertyAssociations();

  const { getLtsaData, ltsaLoading } = useLtsa();

  const [composedProperty, setComposedProperty] = useState<ComposedProperty>({
    apiPropertyLoading: ltsaLoading,
    ltsaLoading: propertyLoading,
    propertyAssociationsLoading: propertyAssociationsLoading,
  });

  useEffect(() => {
    setComposedProperty(property => ({
      ...property,
      pid: props.pid,
    }));
    setIsEditing(false);
  }, [props.pid]);

  useEffect(() => {
    setComposedProperty(property => ({
      ...property,
      ltsaLoading: ltsaLoading,
      apiPropertyLoading: propertyLoading,
      propertyAssociationsLoading: propertyAssociationsLoading,
    }));
  }, [propertyLoading, ltsaLoading, propertyAssociationsLoading]);

  const fetchPimsProperty = React.useCallback(async () => {
    try {
        if (!!props.id) {
          const propInfo = await getProperty(props.id);
          if (isMounted() && propInfo?.id === props.id) {
          setComposedProperty(property => ({ ...property, apiProperty: propInfo }));
        }
      }
    } catch (e) {
      // PSP-2919 Hide the property info tab for non-inventory properties
      // We get an error because PID is not on our database
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        if (axiosError?.response?.status === 404) {
          setComposedProperty(property => ({ ...property, apiProperty: undefined }));
        }
      }
    }
  }, [getProperty, isMounted, props.id]);

  useEffect(() => {
    fetchPimsProperty();
  }, [fetchPimsProperty]);

  useEffect(() => {
    const getAssociations = async () => {
      if (props?.id !== undefined) {
        const response = await getPropertyAssociations(props.id);
        if (response !== undefined) {
          setComposedProperty(property => ({ ...property, propertyAssociations: response }));
        }
      }
    };
    getAssociations();
  }, [getPropertyAssociations, props?.id]);

  useEffect(() => {
    const func = async () => {
      setComposedProperty(property => ({
        ...property,
        ltsaDataRequestedOn: new Date(),
        ltsaData: undefined,
      }));

        if (
          isMounted() &&
          ltsaData?.parcelInfo?.orderedProduct?.fieldedData.parcelIdentifier === pidFormatter(pid)
        ) {
          setComposedProperty(property => ({ ...property, ltsaData: ltsaData }));
        }
      }
    };
    func();
  }, [getLtsaData, pid, isMounted]);

  const onSuccess = () => {
    fetchPimsProperty();
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

  return (
    <MapSideBarLayout
      title="Property Information"
      header={<MotiInventoryHeader composedProperty={composedProperty} onZoom={props.onZoom} />}
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
          composedProperty={composedProperty}
          isEditMode={isEditing}
          setEditMode={setIsEditing}
          setFormikRef={setFormikRef}
          onSuccess={onSuccess}
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
