import { useCallback } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';

import { useWorklistContext } from './context/WorklistContext';
import { IWorklistViewProps } from './WorklistView';

export interface IWorklistContainerProps {
  View: React.FC<IWorklistViewProps>;
}

export const WorklistContainer: React.FC<IWorklistContainerProps> = ({ View }) => {
  const { parcels, remove, clearAll } = useWorklistContext();
  const { requestLocationFeatureAddition, isEditPropertiesMode } = useMapStateMachine();
  const pathGenerator = usePathGenerator();

  // Handle creating a research file from the worklist
  const handleCreateResearchFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    requestLocationFeatureAddition(parcels);
    pathGenerator.newFile('research');
  }, [parcels, pathGenerator, requestLocationFeatureAddition]);

  // Handle creating a acquisition file from the worklist
  const handleCreateAcquisitionFile = useCallback(() => {
    debugger;
    if (parcels.length === 0) {
      return;
    }
    requestLocationFeatureAddition(parcels);
    pathGenerator.newFile('acquisition');
  }, [parcels, pathGenerator, requestLocationFeatureAddition]);

  // Handle creating a disposition file from the worklist
  const handleCreateDispositionFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    requestLocationFeatureAddition(parcels);
    pathGenerator.newFile('disposition');
  }, [parcels, pathGenerator, requestLocationFeatureAddition]);

  // Handle creating a lease file from the worklist
  const handleCreateLeaseFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    requestLocationFeatureAddition(parcels);
    pathGenerator.newFile('lease');
  }, [parcels, pathGenerator, requestLocationFeatureAddition]);

  // Handle creating a management file from the worklist
  const handleCreateManagementFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    requestLocationFeatureAddition(parcels);
    pathGenerator.newFile('management');
  }, [parcels, pathGenerator, requestLocationFeatureAddition]);

  const handleAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcels for addition to an open file
    if (parcels.length > 0 && isEditPropertiesMode) {
      requestLocationFeatureAddition(parcels);
    }
  }, [isEditPropertiesMode, parcels, requestLocationFeatureAddition]);

  return (
    <View
      parcels={parcels ?? []}
      onRemove={remove}
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
