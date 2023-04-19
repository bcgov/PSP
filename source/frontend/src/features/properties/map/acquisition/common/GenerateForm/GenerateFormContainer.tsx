import { FormDocumentType } from 'constants/formDocumentTypes';

import { IGenerateFormViewProps } from './GenerateFormView';
import { useGenerateH0443 } from './hooks/useGenerateH0443';
import { useGenerateLetter } from './hooks/useGenerateLetter';

export interface IGenerateFormContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const GenerateFormContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFileId, View }) => {
  const generateLetter = useGenerateLetter();

  const generateH0443 = useGenerateH0443();

  const onGenerateClick = (formType: FormDocumentType) => {
    switch (formType) {
      case FormDocumentType.LETTER:
        generateLetter(acquisitionFileId);
        break;
      case FormDocumentType.H0443:
        generateH0443(acquisitionFileId);
        break;
      default:
        console.error('Form Document type not recognized');
    }
    console.log(formType);
  };
  return <View onGenerateClick={onGenerateClick} />;
};

export default GenerateFormContainer;
