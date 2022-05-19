import axios, { AxiosError } from 'axios';
import { usePropertyDetails } from 'features/mapSideBar/hooks/usePropertyDetails';
import {
  InventoryTabNames,
  InventoryTabs,
  TabInventoryView,
} from 'features/mapSideBar/tabs/InventoryTabs';
import LtsaTabView from 'features/mapSideBar/tabs/ltsa/LtsaTabView';
import PropertyAssociationTabView from 'features/mapSideBar/tabs/propertyAssociations/PropertyAssociationTabView';
import { PropertyDetailsTabView } from 'features/mapSideBar/tabs/propertyDetails/PropertyDetailsTabView';
import PropertyResearchTabView from 'features/mapSideBar/tabs/propertyResearch/PropertyResearchTabView';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { usePropertyAssociations } from 'hooks/usePropertyAssociations';
import { IApiError } from 'interfaces/IApiError';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { Api_PropertyAssociations } from 'models/api/Property';
import { Api_ResearchFileProperty } from 'models/api/ResearchFile';
import * as React from 'react';
import { useEffect, useState } from 'react';
import { pidFormatter } from 'utils';

export interface IPropertyResearchContainerProps {
  researchFileProperty: Api_ResearchFileProperty;
  setEditMode: (isEditing: boolean) => void;
}

const PropertyResearchContainer: React.FunctionComponent<IPropertyResearchContainerProps> = props => {
  const isMounted = useIsMounted();
  const [ltsaData, setLtsaData] = useState<LtsaOrders | undefined>(undefined);
  const [apiProperty, setApiProperty] = useState<IPropertyApiModel | undefined>(undefined);
  const [ltsaDataRequestedOn, setLtsaDataRequestedOn] = useState<Date | undefined>(undefined);
  const [propertyAssociations, setPropertyAssociations] = useState<
    Api_PropertyAssociations | undefined
  >(undefined);
  const [showPropertyInfoTab, setShowPropertyInfoTab] = useState(true);
  const [activeTab, setActiveTab] = useState<InventoryTabNames>(InventoryTabNames.property);

  const pid = props.researchFileProperty?.property?.pid?.toString();

  // First, fetch property information from PSP API
  const { getPropertyWithPid, getPropertyWithPidLoading: propertyLoading } = useProperties();
  useEffect(() => {
    const func = async () => {
      try {
        if (!!pid) {
          const propInfo = await getPropertyWithPid(pid);
          if (isMounted() && propInfo.pid === pidFormatter(pid)) {
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
          }
        }
      }
    };

    func();
  }, [getPropertyWithPid, isMounted, pid]);

  // After API property object has been received, we query relevant map layers to find
  // additional information which we store in a different model (IPropertyDetailsForm)
  const propertyViewForm = usePropertyDetails(apiProperty);

  const { getLtsaData, ltsaLoading } = useLtsa();
  useEffect(() => {
    const func = async () => {
      setLtsaDataRequestedOn(new Date());
      setLtsaData(undefined);
      if (!!pid) {
        const ltsaData = await getLtsaData(pidFormatter(pid));
        if (
          isMounted() &&
          ltsaData?.parcelInfo?.orderedProduct?.fieldedData.parcelIdentifier === pidFormatter(pid)
        ) {
          setLtsaData(ltsaData);
        }
      }
    };
    func();
  }, [getLtsaData, pid, isMounted]);

  const {
    getPropertyAssociations,
    isLoading: propertyAssociationsLoading,
  } = usePropertyAssociations();

  useEffect(() => {
    const func = async () => {
      if (pid !== undefined) {
        const response = await getPropertyAssociations(pid);
        if (response?.id !== undefined) {
          setPropertyAssociations(response);
        }
      }
    };

    func();
  }, [getPropertyAssociations, pid]);

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

  tabViews.push({
    content: (
      <PropertyResearchTabView
        researchFile={props.researchFileProperty}
        setEditMode={props.setEditMode}
      />
    ),
    key: InventoryTabNames.research,
    name: 'Property Research',
  });

  const defaultTab = InventoryTabNames.research;

  if (showPropertyInfoTab) {
    tabViews.push({
      content: <PropertyDetailsTabView property={propertyViewForm} loading={propertyLoading} />,
      key: InventoryTabNames.property,
      name: 'Property Details',
    });
  }

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

  return (
    <InventoryTabs
      tabViews={tabViews}
      defaultTabKey={defaultTab}
      activeTab={activeTab}
      setActiveTab={setActiveTab}
    />
  );
};

export default PropertyResearchContainer;
