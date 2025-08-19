import { ParcelDataset } from './models';
import { IParcelListViewProps } from './ParcelListView';

export interface IParcelListContainwerProps {
  View: React.FC<IParcelListViewProps>;
  parcels: ParcelDataset[];
}

export const ParcelListContainer: React.FC<IParcelListContainwerProps> = ({ parcels, View }) => {
  // TODO: Add the FIT BOUNDARY to all parcels
  return <View parcels={parcels ?? []} />;
};
