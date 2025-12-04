import { useState } from 'react';
import { ColProps } from 'react-bootstrap';

import { usePropertyTenureCleanupRepository } from '@/hooks/repositories/usePropertyTenureCleanupRepository';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { ApiGen_Concepts_PropertyTenureCleanup } from '@/models/api/generated/ApiGen_Concepts_PropertyTenureCleanup';

import { ITenureCleanupViewProps } from './PropertyTenureCleanupFieldView';

export interface ITenureCleanupContainerProps {
  propertyIds: number[];
  labelWidth?: ColProps;
  contentWidth?: ColProps;
  View: React.FunctionComponent<ITenureCleanupViewProps>;
}
const TenureCleanupContainer: React.FC<ITenureCleanupContainerProps> = ({
  propertyIds,
  labelWidth,
  contentWidth,
  View,
}) => {
  const [tenureCleanups, setTenureCleanups] = useState<ApiGen_Concepts_PropertyTenureCleanup[]>([]);

  const { getPropertyTenureCleanups } = usePropertyTenureCleanupRepository();

  const getTenureCleanupsExecute = getPropertyTenureCleanups.execute;

  useDeepCompareEffect(() => {
    const tasks: Promise<ApiGen_Concepts_PropertyTenureCleanup[]>[] = [];

    for (let i = 0; i < propertyIds.length; i++) {
      tasks.push(getTenureCleanupsExecute(propertyIds[i]));
    }

    Promise.all(tasks).then(results => {
      const flatted = results.flat();
      setTenureCleanups(flatted);
    });
  }, [propertyIds, getTenureCleanupsExecute]);

  return (
    <View tenureCleanups={tenureCleanups} labelWidth={labelWidth} contentWidth={contentWidth} />
  );
};
export default TenureCleanupContainer;
