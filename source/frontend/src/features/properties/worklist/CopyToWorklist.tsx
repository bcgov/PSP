import React from 'react';
import { LiaFileExportSolid } from 'react-icons/lia';

import { LinkButton } from '@/components/common/buttons';
import {
  emptyFeatureDataset,
  LocationFeatureDataset,
} from '@/components/common/mapFSM/useLocationFeatureLoader';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { apiFilePropertyToPimsFeature, getLatLng } from '@/utils';

import { useWorklistContext } from './context/WorklistContext';

export interface ICopyToWorklistProps {
  fileProperties: ApiGen_Concepts_FileProperty[];
  iconSize?: string | number;
}

export const CopyToWorklist: React.FC<ICopyToWorklistProps> = ({ fileProperties, iconSize }) => {
  const { addRange } = useWorklistContext();

  const handleCopy = () => {
    const parcelItems = fileProperties.map(fp => {
      const featureSet: LocationFeatureDataset = {
        ...emptyFeatureDataset(),
        pimsFeatures: [apiFilePropertyToPimsFeature(fp)],
        location: getLatLng(fp?.location),
      };
      return featureSet;
    });
    addRange(parcelItems);
  };
  return (
    <LinkButton
      title="Copy to worklist"
      onClick={handleCopy}
      disabled={fileProperties.length === 0}
    >
      <LiaFileExportSolid size={iconSize ?? 24} />
    </LinkButton>
  );
};

export default CopyToWorklist;
