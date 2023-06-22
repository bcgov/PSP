import { Formik, FormikProps } from 'formik';
import noop from 'lodash/noop';
import * as React from 'react';
import { useContext } from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Section } from '@/components/common/Section/Section';
import { Claims } from '@/constants/claims';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { defaultFormLease, IFormLease } from '@/interfaces';

import { StyledDetails } from '../details/LeaseDetailsForm';
import { AddImprovementsContainer } from './AddImprovementsContainer';
import { Improvements } from './Improvements';

export const ImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps>
> = ({ isEditing, formikRef, onEdit }) => {
  const { lease } = useContext(LeaseStateContext);

  if (!lease?.improvements?.length && !isEditing) {
    return (
      <Section>
        <p>
          There are no commercial, residential, or other improvements indicated with this
          lease/license.
        </p>
      </Section>
    );
  }

  return !!isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddImprovementsContainer
        formikRef={formikRef as React.RefObject<FormikProps<IFormLease>>}
        onEdit={onEdit}
      />
    </ProtectedComponent>
  ) : (
    <Formik onSubmit={noop} enableReinitialize initialValues={{ ...defaultFormLease, ...lease }}>
      <StyledDetails>
        <Improvements disabled={true} />
      </StyledDetails>
    </Formik>
  );
};

export default ImprovementsContainer;
