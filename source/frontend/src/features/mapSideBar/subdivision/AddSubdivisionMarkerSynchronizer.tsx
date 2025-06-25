import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';
import { propertyToLocationBoundaryDataset } from '@/utils/mapPropertyUtils';

import { SubdivisionFormModel } from './AddSubdivisionModel';

interface IAddSubdivisionMarkerSynchronizerProps {
  values: SubdivisionFormModel;
}

const AddSubdivisionMarkerSynchronizer: React.FunctionComponent<
  IAddSubdivisionMarkerSynchronizerProps
> = ({ values }) => {
  useDraftMarkerSynchronizer([
    ...(values.sourceProperty ? [propertyToLocationBoundaryDataset(values.sourceProperty)] : []),
    ...values.destinationProperties.map(dp => propertyToLocationBoundaryDataset(dp)),
  ]);
  return null;
};

export default AddSubdivisionMarkerSynchronizer;
