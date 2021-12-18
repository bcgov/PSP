import { ILeasePage } from 'features/leases';
import { Form, Formik } from 'formik';
import { defaultFormLease, ILease } from 'interfaces';
import * as React from 'react';
import styled from 'styled-components';

import * as Styled from '../styles';

export interface ILeasePageFormProps {
  leasePage: ILeasePage;
  lease?: ILease;
  refreshLease: () => void;
}

/**
 * Wraps the lease page in a formik form
 * @param {ILeasePageFormProps} param0
 */
export const LeasePageForm: React.FunctionComponent<ILeasePageFormProps> = ({
  leasePage,
  lease,
  refreshLease,
}) => {
  return (
    <StyledLeasePage>
      <StyledLeasePageHeader>
        <Styled.LeaseH2>{leasePage.title}</Styled.LeaseH2>
        {leasePage.description && <p>{leasePage.description}</p>}
      </StyledLeasePageHeader>
      <Formik
        initialValues={{
          ...defaultFormLease,
          ...lease,
        }}
        enableReinitialize={true}
        onSubmit={() => {
          refreshLease();
        }}
      >
        <ViewEditToggleForm id="leaseForm">{leasePage.component}</ViewEditToggleForm>
      </Formik>
    </StyledLeasePage>
  );
};

export const ViewEditToggleForm = styled(Form)`
  &#leaseForm {
    text-align: left;
    input:disabled,
    select:disabled,
    textarea:disabled {
      background: none;
      border: none;
      resize: none;
      height: fit-content;
    }
    textarea:disabled {
      padding-left: 0;
    }
    .input-group.disabled {
      .input-group-text {
        background: none;
        border: none;
      }
    }
  }
`;

const StyledLeasePageHeader = styled.div`
  position: sticky;
  top: 0;
  left: 0;
  padding-bottom: 1rem;
  background-color: white;
  p {
    height: 1rem;
    text-align: left;
  }
  z-index: 1;
`;

const StyledLeasePage = styled.div`
  padding: 0 2.5rem;
  height: 100%;
  overflow-y: auto;
  grid-area: leasecontent;
`;

export default LeasePageForm;
