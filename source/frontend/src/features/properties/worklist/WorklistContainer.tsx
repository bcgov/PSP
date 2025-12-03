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
  const { requestLocationFeatureAddition: prepareForCreation, isEditPropertiesMode } =
    useMapStateMachine();
  const pathGenerator = usePathGenerator();

  // Handle creating a research file from the worklist
  const handleCreateResearchFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    prepareForCreation(parcels);
    pathGenerator.newFile('research');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a acquisition file from the worklist
  const handleCreateAcquisitionFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    prepareForCreation(parcels);
    pathGenerator.newFile('acquisition');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a disposition file from the worklist
  const handleCreateDispositionFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    prepareForCreation(parcels);
    pathGenerator.newFile('disposition');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a lease file from the worklist
  const handleCreateLeaseFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    prepareForCreation(parcels);
    pathGenerator.newFile('lease');
  }, [parcels, pathGenerator, prepareForCreation]);

  // Handle creating a management file from the worklist
  const handleCreateManagementFile = useCallback(() => {
    if (parcels.length === 0) {
      return;
    }
    prepareForCreation(parcels);
    pathGenerator.newFile('management');
  }, [parcels, pathGenerator, prepareForCreation]);

  const handleAddToOpenFile = useCallback(() => {
    // If in edit properties mode, prepare the parcels for addition to an open file
    if (parcels.length > 0 && isEditPropertiesMode) {
      prepareForCreation(parcels);
    }
  }, [isEditPropertiesMode, parcels, prepareForCreation]);

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
