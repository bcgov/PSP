import styled from 'styled-components';

export interface IProjectPageFormProps {
  isEditing: boolean;
  onEdit?: () => void;
}

/**
 * Wraps the lease page in a formik form
 * @param {IProjectPageFormProps} param0
 */
export const ProjectViewPageForm: React.FunctionComponent<
  React.PropsWithChildren<IProjectPageFormProps>
> = ({ children, isEditing, onEdit }) => {
  return (
    <StyledPage>
      <StyledPageHeader>
        {/* <ProjectEditButton onEdit={onEdit} isEditing={isEditing} /> */}
      </StyledPageHeader>
      <>{children}</>
    </StyledPage>
  );
};

export default ProjectViewPageForm;

const StyledPageHeader = styled.div`
  font-family: 'BCSans-Bold';
  font-size: 3.2rem;
  line-height: 4.2rem;
  text-align: left;
  color: ${props => props.theme.css.textColor};
  position: sticky;
  top: 0;
  left: 0;
  height: 1rem;
  padding-bottom: 1rem;
  background-color: #f2f2f2;

  z-index: 10;
`;

const StyledPage = styled.div`
  height: 100%;
  overflow-y: auto;
  grid-area: leasecontent;
  flex-direction: column;
  display: flex;
  text-align: left;
  background-color: #f2f2f2;
`;
