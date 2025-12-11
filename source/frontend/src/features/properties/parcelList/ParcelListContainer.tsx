import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';

import { ISearchItemListViewProps } from './ParcelListView';

export interface IParcelListContainwerProps {
  View: React.FC<ISearchItemListViewProps>;
  parcels: LocationFeatureDataset[];
}

export const ParcelListContainer: React.FC<IParcelListContainwerProps> = ({ parcels, View }) => {
  // TODO: Add the FIT BOUNDARY to all parcels
  return <View parcels={parcels ?? []} />;
};
