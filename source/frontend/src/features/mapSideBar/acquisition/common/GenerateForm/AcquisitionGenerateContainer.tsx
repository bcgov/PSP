import { useCallback, useState } from 'react';

import NoticeGenerateContainer from '@/features/mapSideBar/shared/generateForm/NoticeGenerateContainer';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFileOwner } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileOwner';
import { ApiGen_Concepts_AcquisitionFileTeam } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFileTeam';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

import { IGenerateFormViewProps } from './GenerateFormView';
import GenerateItemView from './GenerateItemView';
import GenerateLetterContainer from './GenerateLetterContainer';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';
import { useGenerateNotice } from './hooks/useGenerateNotice';

export interface IGenerateFormContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const AcquisitionGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [isGenerating, setIsGenerating] = useState(false);

  const generateH0443 = useGenerateH0443();
  const generateNotice = useGenerateNotice();

  const handleGenerateH0443 = useCallback(() => {
    setIsGenerating(true);
    generateH0443(acquisitionFileId).finally(() => setIsGenerating(false));
  }, [acquisitionFileId, generateH0443]);

  const generateLetter = useGenerateLetter();
  const handleGenerateLetter = useCallback(
    (recipients: Api_GenerateOwner[]) => {
      setIsGenerating(true);
      generateLetter(acquisitionFileId, recipients).finally(() => setIsGenerating(false));
    },
    [acquisitionFileId, generateLetter],
  );

  const handleSelectedNoticeEntries = (
    selectedOwners: ApiGen_Concepts_AcquisitionFileOwner[],
    responsibleTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
    signingTeamMember: ApiGen_Concepts_AcquisitionFileTeam | null,
  ) => {
    generateNotice(
      acquisitionFileId,
      selectedOwners,
      responsibleTeamMember,
      signingTeamMember,
    ).finally(() => setIsGenerating(false));
  };

  return (
    <View isLoading={isGenerating}>
      <GenerateLetterContainer
        acquisitionFileId={acquisitionFileId}
        onGenerate={handleGenerateLetter}
      />
      <GenerateItemView
        formType={ApiGen_CodeTypes_FormTypes.H0443}
        label="Conditions of Entry (H0443)"
        onGenerate={handleGenerateH0443}
      />
      <NoticeGenerateContainer
        acquisitionFileId={acquisitionFileId}
        onGenerate={handleSelectedNoticeEntries}
      />
    </View>
  );
};

export default AcquisitionGenerateContainer;
