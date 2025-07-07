import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { propertyToLocationBoundaryDataset } from '@/utils/mapPropertyUtils';

import { ConsolidationFormModel } from './AddConsolidationModel';

interface IAddConsolidationMarkerSynchronizerProps {
  values: ConsolidationFormModel;
}

const AddConsolidationMarkerSynchronizer: React.FunctionComponent<
  IAddConsolidationMarkerSynchronizerProps
> = ({ values }) => {
  useDraftMarkerSynchronizer([
    ...values.sourceProperties.map(dp => propertyToLocationBoundaryDataset(dp)),
    ...(values.destinationProperty
      ? [propertyToLocationBoundaryDataset(values.destinationProperty)]
      : []),
  ]);
  return null;
};

export default AddConsolidationMarkerSynchronizer;
