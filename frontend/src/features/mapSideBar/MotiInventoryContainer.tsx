import { ReactComponent as LotSvg } from 'assets/images/icon-lot.svg';
import axios, { AxiosError } from 'axios';
import ComposedProperty from 'features/properties/map/propertyInformation/ComposedProperty';
import PropertyViewSelector from 'features/properties/map/propertyInformation/PropertyViewSelector';
import { FormikProps } from 'formik';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { usePropertyAssociations } from 'hooks/usePropertyAssociations';
import { IApiError } from 'interfaces/IApiError';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_PropertyAssociations } from 'models/api/Property';
import React, { useCallback, useEffect, useState } from 'react';
import { useHistory } from 'react-router-dom';
import styled from 'styled-components';
import { pidFormatter } from 'utils';

import { usePropertyDetails } from './hooks/usePropertyDetails';
import MapSideBarLayout from './layout/MapSideBarLayout';
import { MotiInventoryHeader } from './MotiInventoryHeader';
import { InventoryTabNames, InventoryTabs, TabInventoryView } from './tabs/InventoryTabs';
import LtsaTabView from './tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from './tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from './tabs/propertyDetails/detail/PropertyDetailsTabView';
import { UpdatePropertyDetailsContainer } from './tabs/propertyDetails/update/UpdatePropertyDetailsContainer';

export interface IMotiInventoryContainerProps {
  pid?: string;
  readOnly?: boolean;
  onClose: () => void;
  onZoom: (apiProperty?: IPropertyApiModel | undefined) => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MotiInventoryContainer: React.FunctionComponent<IMotiInventoryContainerProps> = props => {
  const isMounted = useIsMounted();

  const [formikRef, setFormikRef] = useState<React.RefObject<FormikProps<any>> | undefined>(
    undefined,
  );

  const [composedProperty, setComposedProperty] = useState<ComposedProperty>({});

  // First, fetch property information from PSP API
  const { getPropertyWithPid, getPropertyWithPidLoading: propertyLoading } = useProperties();
  useEffect(() => {
    const func = async () => {
      try {
        if (!!props.pid && !!props.readOnly) {
          const propInfo = await getPropertyWithPid(props.pid);
          if (isMounted() && propInfo.pid === pidFormatter(props.pid)) {
            let updated = composedProperty;
            updated.apiProperty = propInfo;
            setComposedProperty(property => ({ ...property, ...updated }));
          }
        }
      } catch (e) {
        // PSP-2919 Hide the property info tab for non-inventory properties
        // We get an error because PID is not on our database
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 404) {
            /*setShowPropertyInfoTab(false);
            setActiveTab(InventoryTabNames.title);*/
          }
        }
      }
    };

    func();
  }, [getPropertyWithPid, isMounted, props.pid, props.readOnly]);

  const {
    getPropertyAssociations,
    isLoading: propertyAssociationsLoading,
  } = usePropertyAssociations();

  useEffect(() => {
    async function fetchResearchFile() {
      if (props.pid !== undefined) {
        const response = await getPropertyAssociations(props.pid);
        //setPropertyAssociations(response);
        let updated = composedProperty;
        updated.propertyAssociations = response;
        setComposedProperty(property => ({ ...property, ...updated }));
      }
    }
    fetchResearchFile();
  }, [getPropertyAssociations, props.pid]);

  // After API property object has been received, we query relevant map layers to find
  // additional information which we store in a different model (IPropertyDetailsForm)
  const propertyViewForm = usePropertyDetails(composedProperty.apiProperty);

  const { getLtsaData, ltsaLoading } = useLtsa();
  useEffect(() => {
    const func = async () => {
      let updated = composedProperty;
      updated.ltsaDataRequestedOn = new Date();
      updated.ltsaData = undefined;

      setComposedProperty(property => ({ ...property, ...updated }));
      if (!!props.pid) {
        const ltsaData = await getLtsaData(pidFormatter(props.pid));
        if (
          isMounted() &&
          ltsaData?.parcelInfo?.orderedProduct?.fieldedData.parcelIdentifier ===
            pidFormatter(props.pid)
        ) {
          let updated = composedProperty;
          updated.ltsaData = ltsaData;
          setComposedProperty(property => ({ ...property, ...updated }));
        }
      }
    };
    func();
  }, [getLtsaData, props.pid, isMounted]);

  return (
    <MapSideBarLayout
      title="Property Information"
      header={
        <MotiInventoryHeader
          ltsaData={composedProperty.ltsaData}
          ltsaLoading={ltsaLoading}
          propertyLoading={propertyLoading}
          property={composedProperty.apiProperty}
          showEditButton={props.readOnly}
          onZoom={props.onZoom}
        />
      }
      icon={<LotIcon className="mr-1" />}
      showCloseButton
      onClose={props.onClose}
    >
      <PropertyViewSelector
        composedProperty={composedProperty}
        isEditMode={false}
        setEditMode={function(isEditing: boolean): void {
          throw new Error('Function not implemented.');
        }}
        setFormikRef={setFormikRef}
        onSuccess={function(): void {
          throw new Error('Function not implemented.');
        }}
      />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;

const LotIcon = styled(LotSvg)`
  width: 3rem;
  height: 3rem;
`;
