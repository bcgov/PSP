import { ILeasePage } from 'features/leases';
import {
  addFormLeaseToApiLease,
  apiLeaseToAddFormLease,
  apiLeaseToFormLease,
  formLeaseToApiLease,
} from 'features/leases/leaseUtils';
import { Form, Formik } from 'formik';
import {
  defaultAddFormLease,
  defaultFormLease,
  IAddFormLease,
  IFormLease,
  ILease,
} from 'interfaces';
import queryString from 'query-string';
import * as React from 'react';
import { useLocation } from 'react-router-dom';
import styled from 'styled-components';

import { LeasePageNames, leasePages } from '../LeaseContainer';
import * as Styled from '../styles';

export interface ILeasePageFormProps {
  leasePage: ILeasePage;
  lease?: ILease;
  refreshLease: () => void;
  setLease: (lease: ILease) => void;
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
}) => {
  const location = useLocation();
  const { edit } = queryString.parse(location.search);
  return (
    <StyledLeasePage>
      <StyledLeasePageHeader>
        <Styled.LeaseH2>{leasePage.header ?? leasePage.title}</Styled.LeaseH2>
        {leasePage.description && <p>{leasePage.description}</p>}
      </StyledLeasePageHeader>
      {!edit ? (
        <Formik<IFormLease | IAddFormLease>
          initialValues={
            leasePage.title !== leasePages.get(LeasePageNames.DETAILS)?.title
              ? ({
                  ...defaultFormLease,
                  ...apiLeaseToFormLease(lease),
                } as IFormLease)
              : ({ ...defaultAddFormLease, ...apiLeaseToAddFormLease(lease) } as IAddFormLease)
          }
          enableReinitialize={true}
          validationSchema={leasePage?.validation}
          onSubmit={async (values, { setSubmitting }) => {
            try {
              refreshLease();
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
      ) : (
        <>{children}</>
      )}
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
  flex-direction: column;
  display: flex;
  text-align: left;
`;

export default LeasePageForm;
