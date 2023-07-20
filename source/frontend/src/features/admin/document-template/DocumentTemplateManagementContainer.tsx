import { useState } from 'react';

import { useFormDocumentRepository } from '@/hooks/repositories/useFormDocumentRepository';

import DocumentTemplateManagementView from './DocumentTemplateManagementView';

export const DocumentTemplateManagementContainer: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const [selectedFormDocumentTypeCode, setSelectedFormDocumentTypeCode] = useState<
    string | undefined
  >(undefined);
  const {
    getFormDocumentTypes: { response: templateResponse, loading: templateLoading },
  } = useFormDocumentRepository();

  return (
    <DocumentTemplateManagementView
      isLoading={templateLoading}
      formDocumentTypes={templateResponse}
      selectedFormDocumentTypeCode={selectedFormDocumentTypeCode}
      setSelectedFormDocumentTypeCode={setSelectedFormDocumentTypeCode}
    />
  );
};

export default DocumentTemplateManagementContainer;
