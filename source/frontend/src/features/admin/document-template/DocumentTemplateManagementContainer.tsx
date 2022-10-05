import { useActivityRepository } from 'features/properties/map/activity/hooks/useActivityRepository';
import { useState } from 'react';

import DocumentTemplateManagementView from './DocumentTemplateManagementView';

export const DocumentTemplateManagementContainer: React.FunctionComponent = () => {
  const [acitivityTemplateId, setActivityTemplateId] = useState<number>(0);
  const {
    getActivityTemplates: { response: templateResponse, loading: templateLoading },
  } = useActivityRepository();

  return (
    <DocumentTemplateManagementView
      isLoading={templateLoading}
      activityTypes={templateResponse}
      activityTypeId={acitivityTemplateId}
      setActivityTemplateId={setActivityTemplateId}
    />
  );
};

export default DocumentTemplateManagementContainer;
