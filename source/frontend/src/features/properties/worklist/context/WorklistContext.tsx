import { createContext, ReactNode, useCallback, useContext, useState } from 'react';
import { toast } from 'react-toastify';

import { LocationFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { areFeatureDatasetsEqual, areLocationFeatureDatasetsEqual, latLngToKey } from '@/utils';

export interface IWorklistNotifier {
  error: (msg: string) => void;
  success: (msg: string) => void;
  warn: (msg: string) => void;
}

export interface IWorklistContext {
  parcels: LocationFeatureDataset[];
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
  parcels?: LocationFeatureDataset[];
  /** Override the default react‑toastify notifier in tests or other environments */
  notifier?: IWorklistNotifier;
}

export function WorklistContextProvider({
  children,
  parcels: initialParcels,
  notifier = toast, // default is react‑toastify’s toast object
}: IWorklistContextProviderProps) {
  const [parcels, setParcels] = useState<LocationFeatureDataset[]>(initialParcels ?? []);
  const [selectedId, setSelectedId] = useState<string | null>(null);

  const select = useCallback((id: string) => setSelectedId(id), []);
  const remove = useCallback(
    (id: string) => setParcels(prev => prev.filter(p => latLngToKey(p.location) !== id)),
    [],
  );

  // The worklist should not allow duplicate property (using pid/pin/globalUID, lat/lng)
  const add = useCallback(
    (parcel: LocationFeatureDataset) => {
      setParcels(prev => {
        const alreadyExists = prev.some(p => areFeatureDatasetsEqual(p, parcel));
        if (alreadyExists) {
          notifier.error('Duplicate parcel detected. Add to worklist skipped.');
          return prev;
        }
        return [...prev, parcel];
      });
    },
    [notifier],
  );

  const addRange = useCallback(
    (newParcels: LocationFeatureDataset[]) => {
      setParcels(prev => {
        const uniqueParcels = newParcels.filter(newParcel => {
          return !prev.some(existingParcel =>
            areLocationFeatureDatasetsEqual(existingParcel, newParcel),
          );
        });

        const duplicatesSkipped = newParcels.length - uniqueParcels.length;

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
