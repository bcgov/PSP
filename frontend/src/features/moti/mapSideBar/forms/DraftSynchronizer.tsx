import useDraftMarkerSynchronizer from 'features/properties/hooks/useDraftMarkerSynchronizer';
import * as React from 'react';

interface IDraftSynchronizerProps {
  nameSpace?: string;
}

/**
 * Component wrapper for the useDraftMarkerSynchronizer hook, automatically displays a draft marker for the lat/lng of the current form.
 * The nameSpace must point to an object within the form that contains a lat/lng.
 * @param param0 {IDraftSynchronizerProps}
 */
export const DraftSynchronizer: React.FunctionComponent<IDraftSynchronizerProps> = ({
  nameSpace,
}) => {
  useDraftMarkerSynchronizer(nameSpace ?? '');
  return <></>;
};

export default DraftSynchronizer;
