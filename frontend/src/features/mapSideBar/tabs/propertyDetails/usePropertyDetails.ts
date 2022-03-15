import { useProperties } from 'hooks';
import useIsMounted from 'hooks/useIsMounted';
import { IProperty } from 'interfaces';
import { useEffect, useState } from 'react';
import { pidFormatter } from 'utils';

export function usePropertyDetails(pid?: string) {
  const { getPropertyWithPid } = useProperties();
  const isMounted = useIsMounted();
  const [propertyDetails, setPropertyDetails] = useState<IProperty | undefined>(undefined);

  useEffect(() => {
    const func = async () => {
      if (!!pid) {
        const propInfo = await getPropertyWithPid(pid);
        if (isMounted() && propInfo.pid === pidFormatter(pid)) {
          setPropertyDetails(propInfo);
        }
      }
    };
    func();
  }, [getPropertyWithPid, isMounted, pid]);

  return { propertyDetails };
}
