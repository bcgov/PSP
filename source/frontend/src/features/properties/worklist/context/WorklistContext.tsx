import { createContext, ReactNode, useCallback, useContext, useState } from 'react';
import { toast } from 'react-toastify';

import {
  exists,
  pidFromFullyAttributedFeature,
  pinFromFullyAttributedFeature,
  planFromFullyAttributedFeature,
} from '@/utils';

import { ParcelFeature } from '../models';

interface IWorklistContext {
  parcels: ParcelFeature[];
  selectedId: string | null;
  select: (id: string) => void;
  remove: (id: string) => void;
  add: (parcel: ParcelFeature) => void;
  addRange: (parcel: ParcelFeature[]) => void;
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
  parcels?: ParcelFeature[];
}

export function WorklistContextProvider({
  children,
  parcels: initialParcels,
}: IWorklistContextProviderProps) {
  const [parcels, setParcels] = useState<ParcelFeature[]>(initialParcels ?? []);
  const [selectedId, setSelectedId] = useState<string | null>(null);

  const select = useCallback((id: string) => setSelectedId(id), []);
  const remove = useCallback((id: string) => setParcels(prev => prev.filter(p => p.id !== id)), []);

  // The worklist should not allow duplicate property (using pid/pin/globalUID, lat/lng)
  const add = useCallback((parcel: ParcelFeature) => {
    setParcels(prev => {
      const alreadyExists = prev.some(p => areParcelsEqual(p, parcel));
      if (alreadyExists) {
        toast.error('Duplicate parcel detected. Add to worklist skipped.');
        return prev;
      }
      return [...prev, parcel];
    });
  }, []);

  const addRange = useCallback((newParcels: ParcelFeature[]) => {
    setParcels(prev => {
      const uniqueParcels = newParcels.filter(newParcel => {
        return !prev.some(existingParcel => areParcelsEqual(existingParcel, newParcel));
      });

      const duplicatesSkipped = newParcels.length - uniqueParcels.length;

      if (uniqueParcels.length > 0) {
        toast.success(`Added ${uniqueParcels.length} new parcel(s).`);
      }

      if (duplicatesSkipped > 0) {
        toast.warn(`${duplicatesSkipped} duplicate parcel(s) were skipped.`);
      }

      return [...prev, ...uniqueParcels];
    });
  }, []);

  const clearAll = useCallback(() => {
    setParcels([]);
  }, []);

  return (
    <WorklistContext.Provider
      value={{ parcels, selectedId, select, remove, add, addRange, clearAll }}
    >
      {children}
    </WorklistContext.Provider>
  );
}

function areParcelsEqual(p1: ParcelFeature, p2: ParcelFeature): boolean {
  if (!exists(p1) || !exists(p2)) {
    return false;
  }

  if (p1.id === p2.id) {
    return true;
  }

  const pid1 = pidFromFullyAttributedFeature(p1.feature) ?? null;
  const pid2 = pidFromFullyAttributedFeature(p2.feature) ?? null;
  if (exists(pid1) && pid1 === pid2) {
    return true;
  }

  const pin1 = pinFromFullyAttributedFeature(p1.feature) ?? null;
  const pin2 = pinFromFullyAttributedFeature(p2.feature) ?? null;
  if (exists(pin1) && pin1 === pin2) {
    return true;
  }

  // Some parcels are only identified by their plan-number (e.g. common strata, parks)
  // Only consider plan-number as an identifier when there are no PID/PIN
  const planNumber1 = planFromFullyAttributedFeature(p1.feature) ?? null;
  const planNumber2 = planFromFullyAttributedFeature(p2.feature) ?? null;
  if (
    exists(planNumber1) &&
    !exists(pid1) &&
    !exists(pin1) &&
    !exists(pid2) &&
    !exists(pin2) &&
    planNumber1 === planNumber2
  ) {
    return true;
  }

  // Only consider lat/long when there are no PID/PIN
  const location1 = p1.location;
  const location2 = p2.location;
  if (
    exists(location1) &&
    exists(location2) &&
    !exists(pid1) &&
    !exists(pin1) &&
    !exists(pid2) &&
    !exists(pin2) &&
    location1.lat === location2.lat &&
    location1.lng === location2.lng
  ) {
    return true;
  }

  return false;
}
