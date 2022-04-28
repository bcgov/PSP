import { Feature, GeoJsonProperties, Geometry } from 'geojson';
import { IProperty } from 'interfaces';
import { noop } from 'lodash';
import React from 'react';

import { PointFeature } from '../types';

export enum MapCursors {
  DRAFT = 'draft-cursor',
}
export interface ISelectedPropertyContext {
  propertyInfo: IProperty | null;
  setPropertyInfo: (propertyInfo: IProperty | null) => void;
  selectedFeature: Feature<Geometry, GeoJsonProperties> | null;
  setSelectedFeature: (feature: Feature<Geometry, GeoJsonProperties> | null) => void;
  selectedResearchFeature: Feature<Geometry, GeoJsonProperties> | null;
  setSelectedResearchFeature: (feature: Feature<Geometry, GeoJsonProperties> | null) => void;
  draftProperties: PointFeature[];
  setDraftProperties: (draftProperties: PointFeature[]) => void;
  loading: boolean;
  setLoading: (loading: boolean) => void;
  cursor?: MapCursors;
  setCursor: (cursor?: MapCursors) => void;
  isSelecting: boolean;
}

export const SelectedPropertyContext = React.createContext<ISelectedPropertyContext>({
  propertyInfo: null,
  setPropertyInfo: noop,
  selectedFeature: null,
  setSelectedFeature: noop,
  selectedResearchFeature: null,
  setSelectedResearchFeature: noop,
  draftProperties: [],
  setDraftProperties: noop,
  loading: false,
  setLoading: noop,
  setCursor: noop,
  isSelecting: false,
});

interface ISelectedPropertyContextComponent {
  values?: Partial<ISelectedPropertyContext>;
}

/**
 * Allows for the property information to be sent to the map
 * when the user clicks on a marker
 */
export const SelectedPropertyContextProvider: React.FC<ISelectedPropertyContextComponent> = ({
  children,
  values,
}) => {
  const [propertyInfo, setPropertyInfo] = React.useState<IProperty | null>(
    values?.propertyInfo ?? null,
  );
  const [selectedFeature, setSelectedFeature] = React.useState<Feature<
    Geometry,
    GeoJsonProperties
  > | null>(values?.selectedFeature ?? null);
  const [selectedResearchFeature, setSelectedResearchFeature] = React.useState<Feature<
    Geometry,
    GeoJsonProperties
  > | null>(values?.selectedResearchFeature ?? null);
  const [draftProperties, setDraftProperties] = React.useState<PointFeature[]>(
    values?.draftProperties ?? [],
  );
  const [loading, setLoading] = React.useState<boolean>(values?.loading ?? false);
  const [cursor, setCursor] = React.useState<MapCursors | undefined>(undefined);
  return (
    <SelectedPropertyContext.Provider
      value={{
        propertyInfo,
        setPropertyInfo,
        selectedFeature,
        setSelectedFeature,
        selectedResearchFeature,
        setSelectedResearchFeature:
          values?.setSelectedResearchFeature ?? setSelectedResearchFeature,
        draftProperties,
        setDraftProperties: values?.setDraftProperties ?? setDraftProperties,
        loading,
        setLoading,
        cursor,
        setCursor,
        isSelecting: cursor === MapCursors.DRAFT,
      }}
    >
      {children}
    </SelectedPropertyContext.Provider>
  );
};
