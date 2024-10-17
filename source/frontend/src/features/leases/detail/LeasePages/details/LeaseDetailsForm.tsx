import { Formik } from 'formik';
import noop from 'lodash/noop';
import React from 'react';
import styled from 'styled-components';

import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { defaultApiLease } from '@/models/defaultInitializers';
import { exists } from '@/utils';

import DetailAdministration from './DetailAdministration';
import { DetailFeeDetermination } from './DetailFeeDetermination';
import LeaseDetailView from './LeaseDetailView';
import { LeaseRenewalsView } from './LeaseRenewalsView';
import PropertiesInformation from './PropertiesInformation';

export interface ILeaseDetailsFormProps {
  lease?: ApiGen_Concepts_Lease;
  onGenerate: (lease?: ApiGen_Concepts_Lease) => void;
}

export const LeaseDetailsForm: React.FunctionComponent<ILeaseDetailsFormProps> = ({
  lease,
  onGenerate,
}) => {
  if (!exists(lease)) {
    return <></>;
  }

  return (
    <Formik
      initialValues={{ ...defaultApiLease(), ...lease }}
      enableReinitialize={true}
      onSubmit={noop}
    >
      <StyledDetails>
        <LeaseDetailView lease={lease} onGenerate={onGenerate} />
        <LeaseRenewalsView renewals={lease.renewals} />
        <PropertiesInformation disabled={true} />
        <DetailAdministration disabled={true} />
        <DetailFeeDetermination disabled={true} />
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
