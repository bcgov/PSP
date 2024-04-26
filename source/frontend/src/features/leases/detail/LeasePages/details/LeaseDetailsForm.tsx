import { Formik } from 'formik';
import noop from 'lodash/noop';
import React from 'react';
import styled from 'styled-components';

import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { defaultApiLease } from '@/models/defaultInitializers';

import DetailAdministration from './DetailAdministration';
import DetailConsultation from './DetailConsultation';
import DetailDocumentation from './DetailDocumentation';
import DetailTermInformation from './DetailTermInformation';
import PropertiesInformation from './PropertiesInformation';

export const LeaseDetailsForm: React.FunctionComponent<React.PropsWithChildren<unknown>> = () => {
  const { lease } = React.useContext(LeaseStateContext);
  return (
    <Formik
      initialValues={{ ...defaultApiLease(), ...lease }}
      enableReinitialize={true}
      onSubmit={noop}
    >
      <StyledDetails>
        <DetailTermInformation />
        <PropertiesInformation disabled={true} />
        <DetailAdministration disabled={true} />
        <DetailConsultation />
        <DetailDocumentation disabled={true} />
      </StyledDetails>
    </Formik>
  );
};

export const StyledDetails = styled.form`
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
