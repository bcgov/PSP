import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';

import { IParcelListViewProps } from './ParcelListView';

export interface IParcelListContainwerProps {
  View: React.FC<IParcelListViewProps>;
  parcels: LocationFeatureDataset[];
}

export const ParcelListContainer: React.FC<IParcelListContainwerProps> = ({ parcels, View }) => {
  // TODO: Add the FIT BOUNDARY to all parcels
  return <View parcels={parcels ?? []} />;
};
