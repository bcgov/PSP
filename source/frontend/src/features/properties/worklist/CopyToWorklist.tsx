import React from 'react';
import { LiaFileExportSolid } from 'react-icons/lia';

import { LinkButton } from '@/components/common/buttons';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { ParcelDataset } from '../parcelList/models';
import { useWorklistContext } from './context/WorklistContext';

export interface ICopyToWorklistProps {
  fileProperties: ApiGen_Concepts_FileProperty[];
  iconSize?: string | number;
}

// Map FileProperty fields to ParcelDataset fields as needed
const convertToParcelItem = (fileProperty: ApiGen_Concepts_FileProperty): ParcelDataset => {
  const parcelItem = ParcelDataset.fromPropertyApi(fileProperty.property);
  return parcelItem;
};

export const CopyToWorklist: React.FC<ICopyToWorklistProps> = ({ fileProperties, iconSize }) => {
  const { addRange } = useWorklistContext();

  const handleCopy = () => {
    const parcelItems = fileProperties.map(convertToParcelItem);
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
