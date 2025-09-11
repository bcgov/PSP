import { useCallback, useState } from 'react';

import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { IGenerateFormViewProps } from '../acquisition/common/GenerateForm/GenerateFormView';
import GenerateItemView from '../acquisition/common/GenerateForm/GenerateItemView';
import { useGenerateForm12 } from '../acquisition/common/GenerateForm/hooks/useGenerateForm12';
import PropertySelectorModal from './PropertySelectorModal';

export interface IResearchGenerateContainerProps {
  researchFile: ApiGen_Concepts_ResearchFile;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const ResearchGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<IResearchGenerateContainerProps>
> = ({ researchFile, View }) => {
  const [isModalOpen, openModal, closeModal] = useModalManagement();
  const [isGenerating, setIsGenerating] = useState(false);
  const generateForm12 = useGenerateForm12();

  const onGenerate = useCallback(
    (properties: ApiGen_Concepts_FileProperty[]) => {
      setIsGenerating(true);
      generateForm12(properties.map(fp => fp.property)).finally(() => setIsGenerating(false));
    },
    [generateForm12],
  );

  const handleGenerateClick = () => {
    openModal();
  };

  const handleSelectedProperties = (selectedProperties: ApiGen_Concepts_FileProperty[]) => {
    onGenerate(selectedProperties);
    closeModal();
  };

  return (
    <View isLoading={isGenerating}>
      <GenerateItemView
        label="Generate Form 12"
        formType={ApiGen_CodeTypes_FormTypes.FORM12}
        onGenerate={handleGenerateClick}
      />
      <PropertySelectorModal
        isOpened={isModalOpen}
        availiableProperties={researchFile?.fileProperties ?? []}
        onSelectOk={handleSelectedProperties}
        onCancelClick={closeModal}
      />
    </View>
  );
};

export default ResearchGenerateContainer;
