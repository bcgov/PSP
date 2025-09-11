import { useCallback, useState } from 'react';

import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { Api_GenerateOwner } from '@/models/generate/GenerateOwner';

import { IGenerateFormViewProps } from './GenerateFormView';
import GenerateItemView from './GenerateItemView';
import GenerateLetterContainer from './GenerateLetterContainer';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';

export interface IGenerateFormContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const AcquisitionGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFileId, View }) => {
  const [isGenerating, setIsGenerating] = useState(false);

  const generateH0443 = useGenerateH0443();

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
    </View>
  );
};

export default AcquisitionGenerateContainer;
