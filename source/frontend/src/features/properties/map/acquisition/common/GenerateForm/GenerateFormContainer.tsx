import { FormDocumentType } from 'constants/formDocumentTypes';

import { IGenerateFormViewProps } from './GenerateFormView';
import { useGenerateLetter } from './hooks/useGenerateLetter';

export interface IGenerateFormContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const GenerateFormContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateFormContainerProps>
> = ({ acquisitionFileId, View }) => {
  const generateLetter = useGenerateLetter();

  const onGenerateClick = (formType: FormDocumentType) => {
    switch (formType) {
      case FormDocumentType.LETTER:
        generateLetter(acquisitionFileId);
        break;
      case FormDocumentType.H0443:
        break;
      default:
        console.error('Form Document type not recognized');
    }
    console.log(formType);
  };
  return <View onGenerateClick={onGenerateClick} />;
};

export default GenerateFormContainer;
