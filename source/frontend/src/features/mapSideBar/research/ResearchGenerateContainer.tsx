import { useState } from 'react';

import {
  IGeneratingModal,
  useForm12Generation,
} from '@/features/mapSideBar/shared/generateForm/useForm12Generation';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_ResearchFile } from '@/models/api/generated/ApiGen_Concepts_ResearchFile';

import { IGenerateFormViewProps } from '../acquisition/common/GenerateForm/GenerateFormView';
import GenerateItemView from '../acquisition/common/GenerateForm/GenerateItemView';
import { useGenerateForm12 } from '../acquisition/common/GenerateForm/hooks/useGenerateForm12';
import { useGenerateResearchNotice } from '../acquisition/common/GenerateForm/hooks/useGenerateResearchNotice';
import NoticeGenerateContainer from '../shared/generateForm/NoticeGenerateContainer';
import PropertySelectorModal from './PropertySelectorModal';

export interface IResearchGenerateContainerProps {
  researchFile: ApiGen_Concepts_ResearchFile;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const ResearchGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<IResearchGenerateContainerProps>
> = ({ researchFile, View }) => {
  const [isGenerating, setIsGenerating] = useState<IGeneratingModal>({
    display: false,
    title: null,
  });
  const generateForm12 = useGenerateForm12();
  const {
    isPropertyModalOpen,
    closePropertyModal,
    handleGenerateForm12Click,
    handleSelectedProperties,
  } = useForm12Generation(generateForm12, setIsGenerating);
  const generateNotice = useGenerateResearchNotice();
  const handleSelectedNoticeEntries = () => {
    generateNotice(researchFile?.id).finally(() => setIsGenerating({ display: false }));
  };

  return (
    <View isLoading={isGenerating.display}>
      <GenerateItemView
        label="Generate Form 12"
        formType={ApiGen_CodeTypes_FormTypes.FORM12}
        onGenerate={handleGenerateForm12Click}
      />
      <PropertySelectorModal
        isOpened={isPropertyModalOpen}
        availableProperties={researchFile?.fileProperties ?? []}
        onSelectOk={handleSelectedProperties}
        onCancelClick={closePropertyModal}
      />
      <NoticeGenerateContainer onGenerate={handleSelectedNoticeEntries} />
    </View>
  );
};

export default ResearchGenerateContainer;
