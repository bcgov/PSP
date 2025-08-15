import { geoJSON } from 'leaflet';
import { useCallback } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import { exists } from '@/utils';

import { useWorklistContext } from './context/WorklistContext';
import { ParcelDataset } from './models';
import { IWorklistViewProps } from './WorklistView';

export interface IWorklistContainerProps {
  View: React.FC<IWorklistViewProps>;
}

export const WorklistContainer: React.FC<IWorklistContainerProps> = ({ View }) => {
  const { parcels, selectedId, select, remove, clearAll } = useWorklistContext();
  const { requestFlyToBounds, requestFlyToLocation, prepareForCreation, isEditPropertiesMode } =
    useMapStateMachine();
  const pathGenerator = usePathGenerator();

  // A worklist parcel points to either a parcel-map boundary/shape or a lat/long (if no parcels were found at that location)
  const onZoomToBounds = useCallback(
    (parcel: ParcelDataset) => {
      if (exists(parcel.pmbcFeature)) {
        const bounds = geoJSON(parcel.pmbcFeature).getBounds();
        if (exists(bounds) && bounds.isValid()) {
          requestFlyToBounds(bounds);
        } else if (exists(parcel.location)) {
          requestFlyToLocation(parcel.location);
        }
      } else if (exists(parcel.location)) {
        requestFlyToLocation(parcel.location);
      }
    },
    [requestFlyToBounds, requestFlyToLocation],
  );

  // Handle creating a research file from the worklist
  const handleCreateResearchFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
    prepareForCreation(featuresSets);
    pathGenerator.newFile('research');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a acquisition file from the worklist
  const handleCreateAcquisitionFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
    prepareForCreation(featuresSets);
    pathGenerator.newFile('acquisition');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a disposition file from the worklist
  const handleCreateDispositionFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
    prepareForCreation(featuresSets);
    pathGenerator.newFile('disposition');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a lease file from the worklist
  const handleCreateLeaseFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
    prepareForCreation(featuresSets);
    pathGenerator.newFile('lease');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a management file from the worklist
  const handleCreateManagementFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
    prepareForCreation(featuresSets);
    pathGenerator.newFile('management');
  }, [parcels, pathGenerator, prepareForCreation]);

  const handleAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcels for addition to an open file
    if (parcels.length > 0 && isEditPropertiesMode) {
      const featuresSets = parcels.map(p => p.toSelectedFeatureDataset());
      prepareForCreation(featuresSets);
    }
  }, [isEditPropertiesMode, parcels, prepareForCreation]);

  return (
    <View
      parcels={parcels ?? []}
      selectedId={selectedId}
      onSelect={select}
      onRemove={remove}
      onZoomToParcel={onZoomToBounds}
      onClearAll={clearAll}
      onCreateResearchFile={handleCreateResearchFile}
      onCreateAcquisitionFile={handleCreateAcquisitionFile}
      onCreateDispositionFile={handleCreateDispositionFile}
      onCreateLeaseFile={handleCreateLeaseFile}
      onCreateManagementFile={handleCreateManagementFile}
      canAddToOpenFile={isEditPropertiesMode}
      onAddToOpenFile={handleAddToOpenFile}
    />
  );
};
