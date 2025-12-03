import { renderHook } from '@testing-library/react-hooks';
import { FormikProps } from 'formik';
import React from 'react';

import * as MapStateMachineContext from '@/components/common/mapFSM/MapStateMachineContext';
import * as FeatureDatasetsHook from './useLocationFeatureDatasetsWithAddresses';
import * as EditPropertiesModeHook from './useEditPropertiesMode';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { toast } from 'react-toastify';
import { LocationFeatureDatasetWithAddress } from './useLocationFeatureDatasetsWithAddresses';
import { vi, Mock } from 'vitest';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { usePropertyFormSyncronizer } from './usePropertyFormSyncronizer';

vi.mock('./useEditPropertiesMode', () => ({
  useEditPropertiesMode: vi.fn(),
}));
vi.mock('./useFeatureDatasetsWithAddresses', () => ({
  useFeatureDatasetsWithAddresses: vi.fn(),
}));

const toastSuccessSpy = vi.spyOn(toast, 'success');
const toastWarnSpy = vi.spyOn(toast, 'warn');

describe('usePropertyFormSyncronizer', () => {
  let mockProcessCreation: Mock;
  let mockFormikRef: React.RefObject<FormikProps<any>>;

  beforeEach(() => {
    vi.clearAllMocks();
    mockProcessCreation = vi.fn();

    mockFormikRef = {
      current: {
        setFieldValue: vi.fn(),
        setFieldTouched: vi.fn(),
        values: {},
      },
    } as unknown as React.RefObject<FormikProps<any>>;

    vi.spyOn(MapStateMachineContext, 'useMapStateMachine').mockReturnValue({
      selectedFeatures: [getMockSelectedFeatureDataset()],
      processCreation: mockProcessCreation,
    } as any);

    (FeatureDatasetsHook.useLocationFeatureDatasetsWithAddresses as any).mockReturnValue({
      featuresWithAddresses: [
        { feature: getMockSelectedFeatureDataset() } as LocationFeatureDatasetWithAddress,
      ],
      bcaLoading: false,
    });
  });

  it('calls useEditPropertiesMode and returns featuresWithAddresses and bcaLoading', () => {
    const { result } = renderHook(() => usePropertyFormSyncronizer(mockFormikRef, 'properties'));
    expect(EditPropertiesModeHook.useEditPropertiesMode).toHaveBeenCalled();
    expect(result.current.featuresWithAddresses).toEqual([
      { feature: getMockSelectedFeatureDataset() },
    ]);
    expect(result.current.isLoading).toBe(false);
  });

  it('uses overrideFeatures if provided', () => {
    const overrideFeatures = [{ feature: { id: 2 }, address: '456 Side St' }];
    renderHook(() =>
      usePropertyFormSyncronizer(mockFormikRef, 'properties', overrideFeatures as any),
    );
    expect(FeatureDatasetsHook.useLocationFeatureDatasetsWithAddresses).toHaveBeenCalledWith(
      overrideFeatures,
    );
  });

  it('calls setFieldValue, setFieldTouched, and shows success toast when unique properties are added', () => {
    mockFormikRef.current.values = { properties: [] };
    (FeatureDatasetsHook.useLocationFeatureDatasetsWithAddresses as any).mockReturnValue({
      featuresWithAddresses: [
        { feature: { id: 1 }, address: 'A' },
        { feature: { id: 2 }, address: 'B' },
      ],
      bcaLoading: false,
    });

    renderHook(() => usePropertyFormSyncronizer(mockFormikRef, 'properties'));

    expect(mockFormikRef.current.setFieldValue).toHaveBeenCalledWith('properties', [
      expect.objectContaining({ address: 'A' }),
      expect.objectContaining({ address: 'B' }),
    ]);
    expect(mockFormikRef.current.setFieldTouched).toHaveBeenCalledWith('properties', true);
    expect(toastSuccessSpy).toHaveBeenCalledWith('Added 2 new property(s) to the file.');
    expect(toastWarnSpy).not.toHaveBeenCalled();
    expect(mockProcessCreation).toHaveBeenCalled();
  });

  it('shows warning toast when duplicates are skipped', () => {
    const mockPropertyForms = [PropertyForm.fromLocationFeatureDataset(getMockSelectedFeatureDataset())];
    mockFormikRef.current.values = { properties: mockPropertyForms };

    (FeatureDatasetsHook.useLocationFeatureDatasetsWithAddresses as any).mockReturnValue({
      featuresWithAddresses: [
        { feature: getMockSelectedFeatureDataset() } as LocationFeatureDatasetWithAddress,
        { feature: { id: 999 }, address: 'Unique' } as unknown as LocationFeatureDatasetWithAddress,
      ],
      bcaLoading: false,
    });

    renderHook(() => usePropertyFormSyncronizer(mockFormikRef, 'properties'));

    expect(mockFormikRef.current.setFieldValue).toHaveBeenCalled();
    expect(mockFormikRef.current.setFieldTouched).toHaveBeenCalledWith('properties', true);
    expect(toastSuccessSpy).toHaveBeenCalledWith('Added 1 new property(s) to the file.');
    expect(toastWarnSpy).toHaveBeenCalledWith('Skipped 1 duplicate property(s).');
    expect(mockProcessCreation).toHaveBeenCalled();
  });

  it('shows only warning toast if all properties are duplicates', () => {
    mockFormikRef.current.values = {
      properties: [PropertyForm.fromLocationFeatureDataset(getMockSelectedFeatureDataset())],
    };
    (FeatureDatasetsHook.useLocationFeatureDatasetsWithAddresses as any).mockReturnValue({
      featuresWithAddresses: [
        { feature: getMockSelectedFeatureDataset() } as LocationFeatureDatasetWithAddress,
      ],
      bcaLoading: false,
    });

    renderHook(() => usePropertyFormSyncronizer(mockFormikRef, 'properties'));

    expect(mockFormikRef.current.setFieldValue).not.toHaveBeenCalled();
    expect(mockFormikRef.current.setFieldTouched).not.toHaveBeenCalled();
    expect(toastSuccessSpy).not.toHaveBeenCalled();
    expect(toastWarnSpy).toHaveBeenCalledWith('Skipped 1 duplicate property(s).');
    expect(mockProcessCreation).toHaveBeenCalled();
  });

  it('handles empty propertyForms gracefully', () => {
    mockFormikRef.current.values = { properties: [{ id: 1 }] };

    (FeatureDatasetsHook.useLocationFeatureDatasetsWithAddresses as any).mockReturnValue({
      featuresWithAddresses: [],
      bcaLoading: false,
    });

    renderHook(() => usePropertyFormSyncronizer(mockFormikRef, 'properties'));

    expect(mockFormikRef.current.setFieldValue).not.toHaveBeenCalled();
    expect(mockFormikRef.current.setFieldTouched).not.toHaveBeenCalled();
    expect(toastSuccessSpy).not.toHaveBeenCalled();
    expect(toastWarnSpy).not.toHaveBeenCalled();
  });

  it('handles undefined formikRef.current gracefully', () => {
    const emptyRef = { current: undefined } as React.RefObject<FormikProps<any>>;

    renderHook(() => usePropertyFormSyncronizer(emptyRef, 'properties'));

    expect(toastSuccessSpy).not.toHaveBeenCalled();
    expect(toastWarnSpy).not.toHaveBeenCalled();
  });
});
