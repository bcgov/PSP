import useIsMounted from 'hooks/useIsMounted';
import { useLtsa } from 'hooks/useLtsa';
import { LtsaOrders } from 'interfaces/ltsaModels';
import * as React from 'react';
import { useState } from 'react';
import { useEffect } from 'react';
import { pidFormatter } from 'utils';

import { InventoryTabs } from './tabs/InventoryTabs';
import LtsaTabView from './tabs/ltsa/LtsaTabView';

export interface IMapPropertySideBarContainer {
  pid?: string;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MapPropertySideBarContainer: React.FunctionComponent<IMapPropertySideBarContainer> = ({
  pid,
}) => {
  const [ltsaData, setLtsaData] = useState<LtsaOrders | undefined>(undefined);

  const { getLtsaData } = useLtsa();
  const isMounted = useIsMounted();
  useEffect(() => {
    const func = async () => {
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

  return <InventoryTabs PropertyView={<></>} LtsaView={<LtsaTabView ltsaData={ltsaData} />} />;
};

export default MapPropertySideBarContainer;
