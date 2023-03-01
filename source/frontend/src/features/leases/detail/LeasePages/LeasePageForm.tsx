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
      <StyledEditWrapper className="mr-3 my-1">
        <LeaseEditButton onEdit={onEdit} isEditing={isEditing} />
      </StyledEditWrapper>
      <>{children}</>
    </StyledLeasePage>
  );
};

const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};
  text-align: right;
`;

const StyledLeasePage = styled.div`
  height: 100%;
  overflow-y: auto;
  grid-area: leasecontent;
  flex-direction: column;
  display: flex;
  text-align: left;
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

export default LeaseViewPageForm;
