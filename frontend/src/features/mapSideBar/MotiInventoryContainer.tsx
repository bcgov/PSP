import { ReactComponent as LotSvg } from 'assets/images/icon-lot.svg';
import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { useProperties } from 'hooks/useProperties';
import { IPropertyApiModel } from 'interfaces/IPropertyApiModel';
import { LtsaOrders } from 'interfaces/ltsaModels';
import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { pidFormatter } from 'utils';

import { usePropertyDetails } from './hooks/usePropertyDetails';
import MapSideBarLayout from './layout/MapSideBarLayout';
import { MotiInventoryHeader } from './MotiInventoryHeader';
import { InventoryTabs } from './tabs/InventoryTabs';
import LtsaTabView from './tabs/ltsa/LtsaTabView';
import { PropertyDetailsTabView } from './tabs/propertyDetails/PropertyDetailsTabView';

export interface IMotiInventoryContainerProps {
  showSideBar: boolean;
  setShowSideBar: (show: boolean) => void;
  pid?: string;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MotiInventoryContainer: React.FunctionComponent<IMotiInventoryContainerProps> = props => {
  const isMounted = useIsMounted();
  const [ltsaData, setLtsaData] = useState<LtsaOrders | undefined>(undefined);
  const [apiProperty, setApiProperty] = useState<IPropertyApiModel | undefined>(undefined);
  const [ltsaDataRequestedOn, setLtsaDataRequestedOn] = useState<Date | undefined>(undefined);

  // First, fetch property information from PSP API
  const { getPropertyWithPid } = useProperties();
  useEffect(() => {
    const func = async () => {
      if (!!props.pid) {
        const propInfo = await getPropertyWithPid(props.pid);
        if (isMounted() && propInfo.pid === pidFormatter(props.pid)) {
          setApiProperty(propInfo);
        }
      }
    };

    func();
  }, [getPropertyWithPid, isMounted, props.pid]);

  // After API property object has been received, we query relevant map layers to find
  // additional information which we store in a different model (IPropertyDetailsForm)
  const propertyViewForm = usePropertyDetails(apiProperty);

  const { getLtsaData } = useLtsa();
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

  return (
    <MapSideBarLayout
      title="Property Information"
      showSideBar={props.showSideBar}
      setShowSideBar={props.setShowSideBar}
      header={<MotiInventoryHeader ltsaData={ltsaData} property={apiProperty} />}
      icon={<LotIcon className="mr-1" />}
      showCloseButton
    >
      <InventoryTabs
        LtsaView={<LtsaTabView ltsaData={ltsaData} ltsaRequestedOn={ltsaDataRequestedOn} />}
        PropertyView={<PropertyDetailsTabView property={propertyViewForm} />}
      />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;

const LotIcon = styled(LotSvg)`
  width: 3rem;
  height: 3rem;
`;
