import axios, { AxiosError } from 'axios';
import { usePropertyDetails } from 'features/mapSideBar/hooks/usePropertyDetails';
import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from 'features/mapSideBar/tabs/InventoryTabs';
import LtsaTabView from 'features/mapSideBar/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from 'features/mapSideBar/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from 'features/mapSideBar/tabs/propertyDetails/detail/PropertyDetailsTabView';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { usePropertyAssociations } from 'hooks/usePropertyAssociations';
import { IApiError } from 'interfaces/IApiError';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_PropertyAssociations } from 'models/api/Property';
import React, { useEffect, useState } from 'react';
import { pidFormatter } from 'utils';

export interface IPropertyContainerProps {
  pid: string;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const PropertyContainer: React.FunctionComponent<IPropertyContainerProps> = props => {
  const isMounted = useIsMounted();
  const [ltsaData, setLtsaData] = useState<LtsaOrders | undefined>(undefined);
  const [apiProperty, setApiProperty] = useState<IPropertyApiModel | undefined>(undefined);
  const [propertyAssociations, setPropertyAssociations] = useState<
    Api_PropertyAssociations | undefined
  >(undefined);
  const [ltsaDataRequestedOn, setLtsaDataRequestedOn] = useState<Date | undefined>(undefined);
  const [showPropertyInfoTab, setShowPropertyInfoTab] = useState(true);
  const [activeTab, setActiveTab] = useState<InventoryTabNames>(InventoryTabNames.property);

  // First, fetch property information from PSP API
  const { getPropertyWithPid, getPropertyWithPidLoading: propertyLoading } = useProperties();
  useEffect(() => {
    const func = async () => {
      try {
        if (!!props.pid) {
          const propInfo = await getPropertyWithPid(props.pid);
          if (isMounted() && propInfo.pid === pidFormatter(props.pid)) {
            setApiProperty(propInfo);
            setShowPropertyInfoTab(true);
          }
        }
      } catch (e) {
        // PSP-2919 Hide the property info tab for non-inventory properties
        // We get an error because PID is not on our database
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError?.response?.status === 404) {
            setShowPropertyInfoTab(false);
            setActiveTab(InventoryTabNames.title);
          }
        }
      }
    };

    func();
  }, [getPropertyWithPid, isMounted, props.pid]);

  const {
    getPropertyAssociations,
    isLoading: propertyAssociationsLoading,
  } = usePropertyAssociations();

  useEffect(() => {
    async function fetchResearchFile() {
      if (props.pid !== undefined) {
        const response = await getPropertyAssociations(props.pid);
        setPropertyAssociations(response);
      }
    }
    fetchResearchFile();
  }, [getPropertyAssociations, props.pid]);

  // After API property object has been received, we query relevant map layers to find
  // additional information which we store in a different model (IPropertyDetailsForm)
  const propertyViewForm = usePropertyDetails(apiProperty);

  const { getLtsaData, ltsaLoading } = useLtsa();
  useEffect(() => {
    const func = async () => {
      setLtsaDataRequestedOn(new Date());
      setLtsaData(undefined);
      if (!!props.pid) {
        const ltsaData = await getLtsaData(pidFormatter(props.pid));
        if (
          isMounted() &&
          ltsaData?.parcelInfo?.orderedProduct?.fieldedData.parcelIdentifier ===
            pidFormatter(props.pid)
        ) {
          setLtsaData(ltsaData);
        }
      }
    };
    func();
  }, [getLtsaData, props.pid, isMounted]);

  const tabViews: TabInventoryView[] = [];

  tabViews.push({
    content: (
      <LtsaTabView
        ltsaData={ltsaData}
        ltsaRequestedOn={ltsaDataRequestedOn}
        loading={ltsaLoading}
      />
    ),
    key: InventoryTabNames.title,
    name: 'Title',
  });

  tabViews.push({
    content: <></>,
    key: InventoryTabNames.value,
    name: 'Value',
  });

  var defaultTab = InventoryTabNames.title;

  if (showPropertyInfoTab) {
    tabViews.push({
      content: <PropertyDetailsTabView property={propertyViewForm} loading={propertyLoading} />,
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
    defaultTab = InventoryTabNames.property;
  }

  if (propertyAssociations?.id !== undefined) {
    tabViews.push({
      content: (
        <PropertyAssociationTabView
          isLoading={propertyAssociationsLoading}
          associations={propertyAssociations}
        />
      ),
      key: InventoryTabNames.pims,
      name: 'PIMS Files',
    });
  }

  return (
    <InventoryTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default PropertyContainer;
