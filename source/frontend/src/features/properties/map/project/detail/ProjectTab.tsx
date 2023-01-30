import { IProjectPage } from '../ProjectContainer';
import ProjectPageForm from './ProjectPages/ProjectPageForm';

export interface IProjectTabProps {
  projectPage?: IProjectPage;
  onEdit?: () => void;
  isEditing: boolean;
}

const ProjectTab: React.FC<IProjectTabProps> = ({ projectPage, onEdit, isEditing }) => {
  if (!projectPage) {
    throw Error('The requested project page does not exist');
  }

  const Component = projectPage.component;

  return (
    <ProjectPageForm isEditing={isEditing} onEdit={onEdit}>
      {/* <Component onEdit={onEdit} isEditing={isEditing} /> */}
    </ProjectPageForm>
  );
};

export default ProjectTab;
