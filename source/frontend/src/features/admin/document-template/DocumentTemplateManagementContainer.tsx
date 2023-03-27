import { useState } from 'react';

import DocumentTemplateManagementView from './DocumentTemplateManagementView';

export const DocumentTemplateManagementContainer: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const [acitivityTemplateId, setActivityTemplateId] = useState<number | undefined>(undefined);

  return (
    <DocumentTemplateManagementView
      isLoading={false}
      activityTypes={[]}
      activityTypeId={acitivityTemplateId}
      setActivityTemplateId={setActivityTemplateId}
    />
  );
};

export default DocumentTemplateManagementContainer;
