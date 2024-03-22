import * as React from 'react';

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
    ...(values.destinationProperty
      ? [PropertyForm.fromPropertyApi(values.destinationProperty).toMapProperty()]
      : []),
    ...values.sourceProperties.map(dp => PropertyForm.fromPropertyApi(dp).toMapProperty()),
  ]);
  return null;
};

export default AddConsolidationMarkerSynchronizer;
