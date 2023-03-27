import { useState } from 'react';

import DocumentTemplateManagementView from './DocumentTemplateManagementView';

export const DocumentTemplateManagementContainer: React.FunctionComponent<
  React.PropsWithChildren<unknown>
> = () => {
  const [acitivityTemplateId, setActivityTemplateId] = useState<number | undefined>(undefined);
  // const {
  //   getActivityTemplates: { response: templateResponse, loading: templateLoading },
  // } = useActivityRepository();

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
