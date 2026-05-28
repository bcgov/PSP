import { useCallback, useState } from 'react';

import PropertySelectorModal from '@/features/mapSideBar/research/PropertySelectorModal';
import NoticeGenerateContainer from '@/features/mapSideBar/shared/generateForm/NoticeGenerateContainer';
import {
  IGeneratingModal,
  useForm12Generation,
} from '@/features/mapSideBar/shared/generateForm/useForm12Generation';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';
import { firstOrNull } from '@/utils';

import { IGenerateFormViewProps } from './GenerateFormView';
import GenerateIntakeContainer from './GenerateIntakeContainer';
import GenerateItemView from './GenerateItemView';
import GenerateLetterContainer from './GenerateLetterContainer';
import { useGenerateForm12 } from './hooks/useGenerateForm12';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateIntake } from './hooks/useGenerateIntake';
import { useGenerateLetter } from './hooks/useGenerateLetter';
import { useGenerateNotice } from './hooks/useGenerateNotice';

export interface IGenerateFormContainerProps {
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const AcquisitionGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFile, View }) => {
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

  const generateH0443 = useGenerateH0443();
  const generateNotice = useGenerateNotice();
  const generateIntake = useGenerateIntake();

  const handleGenerateH0443 = useCallback(() => {
    setIsGenerating({ display: true, title: 'Generate H0443' });
    generateH0443(acquisitionFile.id).finally(() => setIsGenerating({ display: false }));
  }, [acquisitionFile.id, generateH0443]);

  const generateLetter = useGenerateLetter();
  const handleGenerateLetter = useCallback(
    (recipients: Api_GenerateOwner[]) => {
      setIsGenerating({ display: true, title: 'Generate Letter' });
      generateLetter(acquisitionFile.id, recipients).finally(() =>
        setIsGenerating({ display: false }),
      );
    },
    [acquisitionFile.id, generateLetter],
  );

  const handleSelectedNoticeEntries = (
    selectedOwners: ApiGen_Concepts_AcquisitionFileOwner[],
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
  ) => {
    setIsGenerating({ display: false, title: 'Generate Notice of Entry' });
    generateNotice(
      acquisitionFile.id,
      selectedOwners,
      responsibleTeamMember,
      signingTeamMember,
    ).finally(() => setIsGenerating({ display: false }));
  };

  const handleGeneratePropertyIntake = useCallback(
    (fileProperties: ApiGen_Concepts_FileProperty[]) => {
      setIsGenerating({ display: false, title: 'Generate Property Intake' });
      generateIntake(acquisitionFile.id, firstOrNull(fileProperties)).finally(() =>
        setIsGenerating({ display: false }),
      );
    },
    [acquisitionFile.id, generateIntake],
  );

  return (
    <View isLoading={isGenerating.display}>
      <GenerateItemView
        label="Generate Form 12"
        formType={ApiGen_CodeTypes_FormTypes.FORM12}
        onGenerate={handleGenerateForm12Click}
      />
      <PropertySelectorModal
        isOpened={isPropertyModalOpen}
        availableProperties={acquisitionFile?.fileProperties ?? []}
        onSelectOk={handleSelectedProperties}
        onCancelClick={closePropertyModal}
        title={isGenerating?.title}
      />
      <GenerateLetterContainer
        acquisitionFileId={acquisitionFile.id}
        onGenerate={handleGenerateLetter}
      />
      <GenerateItemView
        formType={ApiGen_CodeTypes_FormTypes.H0443}
        label="Conditions of Entry (H0443)"
        onGenerate={handleGenerateH0443}
      />
      <NoticeGenerateContainer
        acquisitionFileId={acquisitionFile.id}
        onGenerate={handleSelectedNoticeEntries}
      />
      <GenerateIntakeContainer
        acquisitionFileId={acquisitionFile.id}
        onGenerate={handleGeneratePropertyIntake}
      />
    </View>
  );
};

export default AcquisitionGenerateContainer;
