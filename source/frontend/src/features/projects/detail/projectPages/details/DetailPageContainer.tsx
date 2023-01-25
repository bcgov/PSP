import { ProjectPageProps } from 'features/properties/map/project/ProjectContainer';
import React from 'react';

import ProjectDetailsForm from './ProjectDetailsForm';

const DetailPageContainer: React.FunctionComponent<React.PropsWithChildren<ProjectPageProps>> = ({
  isEditing,
  onEdit,
}) => {
  return <ProjectDetailsForm />;
};

export default DetailPageContainer;
