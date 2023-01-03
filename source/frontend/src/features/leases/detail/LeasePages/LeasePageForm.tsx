import * as React from 'react';
import styled from 'styled-components';

import LeaseEditButton from '../LeaseEditButton';

export interface ILeasePageFormProps {
  isEditing: boolean;
  onEdit?: () => void;
}

/**
 * Wraps the lease page in a formik form
 * @param {ILeasePageFormProps} param0
 */
export const LeaseViewPageForm: React.FunctionComponent<
  React.PropsWithChildren<ILeasePageFormProps>
> = ({ children, isEditing, onEdit }) => {
  return (
    <StyledLeasePage>
      <StyledLeasePageHeader>
        <LeaseEditButton onEdit={onEdit} isEditing={isEditing} />
      </StyledLeasePageHeader>
      <>{children}</>
    </StyledLeasePage>
  );
};

const StyledLeasePageHeader = styled.div`
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

const StyledLeasePage = styled.div`
  height: 100%;
  overflow-y: auto;
  grid-area: leasecontent;
  flex-direction: column;
  display: flex;
  text-align: left;
  background-color: #f2f2f2;
`;

export default LeaseViewPageForm;
