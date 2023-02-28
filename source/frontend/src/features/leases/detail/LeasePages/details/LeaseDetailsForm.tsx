import {
  DetailAdministration,
  DetailTermInformation,
  PropertiesInformation,
} from 'features/leases';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { Formik } from 'formik';
import { defaultLease } from 'interfaces';
import { noop } from 'lodash';
import * as React from 'react';
import styled from 'styled-components';

import DetailDocumentation from './DetailDocumentation';

export interface IDetailsProps {}

export const LeaseDetailsForm: React.FunctionComponent<
  React.PropsWithChildren<IDetailsProps>
> = () => {
  const { lease } = React.useContext(LeaseStateContext);
  return (
    <Formik initialValues={{ ...defaultLease, ...lease }} enableReinitialize={true} onSubmit={noop}>
      <StyledDetails>
        <DetailTermInformation />
        <PropertiesInformation disabled={true} />
        <DetailAdministration disabled={true} />
        <DetailDocumentation disabled={true} />
      </StyledDetails>
    </Formik>
  );
};

export const StyledDetails = styled.form`
  margin-top: 4rem;
  text-align: left;
  input:disabled,
  select:disabled,
  textarea:disabled {
    background: none;
    border: none;
    resize: none;
    height: fit-content;
    padding: 0;
  }
  textarea:disabled {
    padding-left: 0;
  }
  .input-group.disabled {
    .input-group-text {
      background: none;
      border: none;
      font-size: 1.6rem;
      padding: 0;
    }
    .input {
      height: 100%;
      input {
        width: fit-content;
        height: 100%;
      }
    }
  }
`;

export default LeaseDetailsForm;
