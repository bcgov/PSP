import useDraftMarkerSynchronizer from '@/hooks/useDraftMarkerSynchronizer';

import { ConsolidationFormModel } from './AddConsolidationModel';

interface IAddConsolidationMarkerSynchronizerProps {
  values: ConsolidationFormModel;
}

const AddConsolidationMarkerSynchronizer: React.FunctionComponent<
  IAddConsolidationMarkerSynchronizerProps
> = ({ values }) => {
  useDraftMarkerSynchronizer([...values.sourceProperties, ...[values.destinationProperty]]);
  return null;
};

export default AddConsolidationMarkerSynchronizer;
