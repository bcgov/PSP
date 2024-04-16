import { FormikProps } from 'formik';
import * as React from 'react';
import { useContext, useEffect } from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Section } from '@/components/common/Section/Section';
import { Claims } from '@/constants/claims';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';

import { StyledDetails } from '../details/LeaseDetailsForm';
import { AddImprovementsContainer } from './AddImprovementsContainer';
import { Improvements } from './Improvements';

export const ImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps>
> = ({ isEditing, formikRef, onEdit, onSuccess }) => {
  const { lease } = useContext(LeaseStateContext);
  const {
    getPropertyImprovements: { execute: getPropertyImprovements, loading, response: improvements },
  } = usePropertyImprovementRepository();

  useEffect(() => {
    lease?.id && getPropertyImprovements(lease.id);
  }, [getPropertyImprovements, lease]);

  if (!improvements?.length && !isEditing) {
    return (
      <Section>
        <p>
          There are no commercial, residential, or other improvements indicated with this
          lease/license.
        </p>
      </Section>
    );
  }

  return isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddImprovementsContainer
        formikRef={formikRef as React.RefObject<FormikProps<LeaseFormModel>>}
        onEdit={onEdit}
        improvements={improvements ?? []}
        loading={loading}
        onSuccess={onSuccess}
      />
    </ProtectedComponent>
  ) : (
    <StyledDetails>
      <Improvements improvements={improvements ?? []} loading={loading} />
    </StyledDetails>
  );
};
