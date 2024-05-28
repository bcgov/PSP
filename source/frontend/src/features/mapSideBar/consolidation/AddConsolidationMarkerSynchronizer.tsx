import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';

import { PropertyForm } from '../shared/models';
import { ConsolidationFormModel } from './AddConsolidationModel';

interface IAddConsolidationMarkerSynchronizerProps {
  values: ConsolidationFormModel;
}

const AddConsolidationMarkerSynchronizer: React.FunctionComponent<
  IAddConsolidationMarkerSynchronizerProps
> = ({ values }) => {
  useDraftMarkerSynchronizer([
    ...values.sourceProperties.map(dp => PropertyForm.fromPropertyApi(dp).toMapProperty()),
    ...(values.destinationProperty
      ? [PropertyForm.fromPropertyApi(values.destinationProperty).toMapProperty()]
      : []),
  ]);
  return null;
};

export default AddConsolidationMarkerSynchronizer;
