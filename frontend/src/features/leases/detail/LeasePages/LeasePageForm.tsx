import { ILeasePage } from 'features/leases';
import { apiLeaseToFormLease, formLeaseToApiLease } from 'features/leases/leaseUtils';
import { Form, Formik } from 'formik';
import { defaultFormLease, IFormLease, ILease } from 'interfaces';
import * as React from 'react';
import styled from 'styled-components';

import * as Styled from '../styles';

export interface ILeasePageFormProps {
  leasePage: ILeasePage;
  lease?: ILease;
  refreshLease: () => void;
  setLease: (lease: ILease) => void;
  onUpdate: (lease: ILease, subRoute?: string | undefined) => Promise<ILease | undefined>;
}

/**
 * Wraps the lease page in a formik form
 * @param {ILeasePageFormProps} param0
 */
export const LeasePageForm: React.FunctionComponent<ILeasePageFormProps> = ({
  leasePage,
  lease,
  refreshLease,
  setLease,
  children,
  onUpdate,
}) => {
  return (
    <StyledLeasePage>
      <StyledLeasePageHeader>
        <Styled.LeaseH2>{leasePage.header ?? leasePage.title}</Styled.LeaseH2>
        {leasePage.description && <p>{leasePage.description}</p>}
      </StyledLeasePageHeader>
      <Formik<IFormLease>
        initialValues={{
          ...defaultFormLease,
          ...apiLeaseToFormLease(lease),
        }}
        enableReinitialize={true}
        onSubmit={async (values, { setSubmitting }) => {
          try {
            if (!leasePage.subRoute) {
              refreshLease();
            } else {
              const updatedLease = await onUpdate(formLeaseToApiLease(values), leasePage.subRoute);
              updatedLease && setLease(updatedLease);
            }
          } finally {
            setSubmitting(false);
          }
        }}
      >
        {formikProps => (
          <>
            <ViewEditToggleForm id="leaseForm">{children}</ViewEditToggleForm>
          </>
        )}
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
  z-index: 10;
`;

const StyledLeasePage = styled.div`
  padding: 0 2.5rem;
  height: 100%;
  overflow-y: auto;
  grid-area: leasecontent;
`;

export default LeasePageForm;
