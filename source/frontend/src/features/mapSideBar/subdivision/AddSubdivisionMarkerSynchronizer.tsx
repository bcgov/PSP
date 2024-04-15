import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';

import { PropertyForm } from '../shared/models';
import { SubdivisionFormModel } from './AddSubdivisionModel';

interface IAddSubdivisionMarkerSynchronizerProps {
  values: SubdivisionFormModel;
}

const AddSubdivisionMarkerSynchronizer: React.FunctionComponent<
  IAddSubdivisionMarkerSynchronizerProps
> = ({ values }) => {
  useDraftMarkerSynchronizer([
    ...(values.sourceProperty
      ? [PropertyForm.fromPropertyApi(values.sourceProperty).toMapProperty()]
      : []),
    ...values.destinationProperties.map(dp => PropertyForm.fromPropertyApi(dp).toMapProperty()),
  ]);
  return null;
};

export default AddSubdivisionMarkerSynchronizer;
