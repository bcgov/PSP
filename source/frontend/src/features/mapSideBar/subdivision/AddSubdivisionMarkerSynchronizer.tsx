import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';

import { SubdivisionFormModel } from './AddSubdivisionModel';

interface IAddSubdivisionMarkerSynchronizerProps {
  values: SubdivisionFormModel;
}

const AddSubdivisionMarkerSynchronizer: React.FunctionComponent<
  IAddSubdivisionMarkerSynchronizerProps
> = ({ values }) => {
  useDraftMarkerSynchronizer([
    ...(values.sourceProperty ? [values.sourceProperty] : []),
    ...values.destinationProperties,
  ]);
  return null;
};

export default AddSubdivisionMarkerSynchronizer;
