import { useContext } from 'react';
import styled from 'styled-components';

import { LeasePageNames } from '@/features/mapSideBar/lease/LeaseContainer';
import { ApiGen_CodeTypes_LeaseStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseStatusTypes';

import { LeaseStateContext } from '../../context/LeaseContext';
import LeaseEditButton from '../LeaseEditButton';

export interface ILeasePageFormProps {
  leasePageName: LeasePageNames;
  isEditing: boolean;
  onEdit?: () => void;
}

/**
 * Wraps the lease page in a formik form
 * @param {ILeasePageFormProps} param0
 */
export const LeaseViewPageForm: React.FunctionComponent<
  React.PropsWithChildren<ILeasePageFormProps>
> = ({ children, leasePageName, isEditing, onEdit }) => {
  const { lease } = useContext(LeaseStateContext);

  const displayLeaseTerminationMessage = () => {
    return (
      lease &&
      leasePageName === LeasePageNames.DETAILS &&
      (lease.fileStatusTypeCode.id === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD ||
        lease.fileStatusTypeCode.id === ApiGen_CodeTypes_LeaseStatusTypes.TERMINATED)
    );
  };

  const getTerminationMessage = (): string => {
    if (lease.fileStatusTypeCode.id === ApiGen_CodeTypes_LeaseStatusTypes.DISCARD) {
      return lease.cancellationReason;
    } else {
      return lease.terminationReason;
    }
  };

  return (
    <StyledLeasePage>
      <StyledEditWrapper className="mr-3 my-1">
        <StyledTerminationWrapper>
          {displayLeaseTerminationMessage() && (
            <StyledTerminationMessage>{getTerminationMessage()}</StyledTerminationMessage>
          )}
          <LeaseEditButton onEdit={onEdit} isEditing={isEditing} />
        </StyledTerminationWrapper>
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
  background-color: ${props => props.theme.css.highlightBackgroundColor};
`;

const StyledTerminationWrapper = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
`;

const StyledTerminationMessage = styled.div`
  flex-grow: 1;
  margin-top: 1.5rem;
  margin-left: 1.5rem;
  margin-right: 1.5rem;
  padding: 0.5rem;
  background-color: white;
  border-radius: 0.5rem;
  align-content: center;
  text-align: center;
  font-style: italic;
`;

export default LeaseViewPageForm;
