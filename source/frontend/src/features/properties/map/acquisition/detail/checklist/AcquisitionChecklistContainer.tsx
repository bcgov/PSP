import * as API from 'constants/API';
import { useLookupCodeHelpers } from 'hooks/useLookupCodeHelpers';
import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import React from 'react';

import { IAcquisitionChecklistViewProps } from './AcquisitionChecklistView';

export interface IAcquisitionChecklistContainerProps {
  acquisitionFile?: Api_AcquisitionFile;
  onEdit: () => void;
  View: React.FC<IAcquisitionChecklistViewProps>;
}

export const AcquisitionChecklistContainer: React.FC<IAcquisitionChecklistContainerProps> = ({
  acquisitionFile,
  onEdit,
  View,
}) => {
  const { getByType } = useLookupCodeHelpers();
  const checklistSectionTypes = getByType(API.ACQUISITION_CHECKLIST_SECTION_TYPES);
  const checklistItemTypes = getByType(API.ACQUISITION_CHECKLIST_ITEM_TYPES);
  const checklistItemStatusTypes = getByType(API.ACQUISITION_CHECKLIST_ITEM_STATUS_TYPES);

  return (
    <View
      acquisitionFile={acquisitionFile}
      onEdit={onEdit}
      sectionTypes={checklistSectionTypes}
      itemTypes={checklistItemTypes}
      itemStatusTypes={checklistItemStatusTypes}
    />
  );
};
