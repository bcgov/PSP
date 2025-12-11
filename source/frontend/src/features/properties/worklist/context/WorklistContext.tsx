import { createContext, ReactNode, useCallback, useContext, useState } from 'react';
import { toast } from 'react-toastify';
import { v4 as uuidv4 } from 'uuid';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { areLocationFeatureDatasetsEqual } from '@/utils';

export interface LocationDatasetWithId extends LocationFeatureDataset {
  id;
}
export interface IWorklistNotifier {
  error: (msg: string) => void;
  success: (msg: string) => void;
  warn: (msg: string) => void;
}

export interface IWorklistContext {
  parcels: LocationDatasetWithId[];
  selectedId: string | null;
  select: (id: string) => void;
  remove: (id: string) => void;
  add: (parcel: LocationFeatureDataset) => void;
  addRange: (parcel: LocationFeatureDataset[]) => void;
  clearAll: () => void;
}

const WorklistContext = createContext<IWorklistContext | undefined>(undefined);

export function useWorklistContext() {
  const context = useContext(WorklistContext);
  if (!context) {
    throw new Error('useWorklistContext must be used within WorklistProvider');
  }
  return context;
}

export interface IWorklistContextProviderProps {
  children: ReactNode;
  initialParcels?: LocationDatasetWithId[]; //Only used for Testing
  /** Override the default react‑toastify notifier in tests or other environments */
  notifier?: IWorklistNotifier;
}

export function WorklistContextProvider({
  children,
  initialParcels,
  notifier = toast, // default is react‑toastify’s toast object
}: IWorklistContextProviderProps) {
  const [parcels, setParcels] = useState<LocationDatasetWithId[]>(initialParcels ?? []);
  const [selectedId, setSelectedId] = useState<string | null>(null);

  const select = useCallback((id: string) => setSelectedId(id), []);
  const remove = useCallback((id: string) => setParcels(prev => prev.filter(p => p.id !== id)), []);

  // The worklist should not allow duplicate property (using pid/pin/globalUID, lat/lng)
  const add = useCallback(
    (parcel: LocationFeatureDataset) => {
      const newParcelWithId = generateId(parcel);
      setParcels(prev => {
        const alreadyExists = prev.some(p => areLocationFeatureDatasetsEqual(p, newParcelWithId));
        if (alreadyExists) {
          notifier.error('Duplicate parcel detected. Add to worklist skipped.');
          return prev;
        }
        return [...prev, newParcelWithId];
      });
    },
    [notifier],
  );

  const addRange = useCallback(
    (newParcels: LocationFeatureDataset[]) => {
      const newParcelsWithId = newParcels.map(generateId);
      setParcels(prev => {
        const uniqueParcels = newParcelsWithId.filter(newParcel => {
          return !prev.some(existingParcel =>
            areLocationFeatureDatasetsEqual(existingParcel, newParcel),
          );
        });

        const duplicatesSkipped = newParcelsWithId.length - uniqueParcels.length;

        if (uniqueParcels.length > 0) {
          notifier.success(`Added ${uniqueParcels.length} new parcel(s).`);
        }

        if (duplicatesSkipped > 0) {
          notifier.warn(`${duplicatesSkipped} duplicate parcel(s) were skipped.`);
        }

        return [...prev, ...uniqueParcels];
      });
    },
    [notifier],
  );

  const clearAll = useCallback(() => setParcels([]), []);

  return (
    <WorklistContext.Provider
      value={{ parcels, selectedId, select, remove, add, addRange, clearAll }}
    >
      {children}
    </WorklistContext.Provider>
  );
}

const generateId = (dataset: LocationFeatureDataset): LocationDatasetWithId => {
  return {
    ...dataset,
    id: uuidv4(),
  };
};
