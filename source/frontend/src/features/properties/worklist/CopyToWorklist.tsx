import React from 'react';

import AddAllWorkListIcon from '@/assets/images/add-all-wl-icon.svg?react';
import { LinkButton } from '@/components/common/buttons';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

import { ParcelDataset } from '../parcelList/models';
import { useWorklistContext } from './context/WorklistContext';

export interface ICopyToWorklistProps {
  fileProperties: ApiGen_Concepts_FileProperty[];
  iconSize?: string | number;
}

export const CopyToWorklist: React.FC<ICopyToWorklistProps> = ({ fileProperties, iconSize }) => {
  const { addRange } = useWorklistContext();

  const handleCopy = () => {
    const parcelItems = fileProperties.map(fp => {
      return ParcelDataset.fromPropertyApi(fp.property);
    });
    addRange(parcelItems);
  };

  return (
    <LinkButton
      title="Copy to worklist"
      onClick={handleCopy}
      disabled={fileProperties.length === 0}
    >
      <AddAllWorkListIcon width={iconSize ?? 24} height={iconSize ?? 24} />
    </LinkButton>
  );
};

export default CopyToWorklist;
