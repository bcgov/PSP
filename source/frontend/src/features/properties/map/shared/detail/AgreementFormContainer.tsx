import * as React from 'react';
import { toast } from 'react-toastify';

import { showFile } from '@/features/documents/DownloadDocumentButton';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';

import { IAgreementFormProps } from './AgreementForm';
import { IAgreementFormData } from './models';

interface IAgreementFormContainerProps {
  View: React.FunctionComponent<React.PropsWithChildren<IAgreementFormProps>>;
}

//TODO: POC implementation only.
export const AgreementFormContainer: React.FunctionComponent<IAgreementFormContainerProps> = ({
  View,
}) => {
  const { generateDocumentDownloadWrappedRequest } = useDocumentGenerationRepository();
  const generateAgreement = async (values: IAgreementFormData) => {
    const file = await generateDocumentDownloadWrappedRequest({
      templateData: values,
      templateType: 'placeholder',
      convertToType: null,
    });
    if (file?.payload) {
      showFile(file?.payload);
    } else {
      toast.error('Failed to generate document');
    }
  };

  return <View onSubmit={generateAgreement} />;
};

export default AgreementFormContainer;
