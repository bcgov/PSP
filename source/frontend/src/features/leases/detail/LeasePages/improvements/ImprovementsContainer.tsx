import { FormikProps } from 'formik/dist/types';
import { RefObject, useContext, useEffect } from 'react';

import ProtectedComponent from '@/components/common/ProtectedComponent';
import { Section } from '@/components/common/Section/Section';
import { Claims } from '@/constants/claims';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeasePageProps } from '@/features/mapSideBar/lease/LeaseContainer';
import { usePropertyImprovementRepository } from '@/hooks/repositories/usePropertyImprovementRepository';

import { AddImprovementsContainer } from './AddImprovementsContainer';
import { Improvements } from './Improvements';

export const ImprovementsContainer: React.FunctionComponent<
  React.PropsWithChildren<LeasePageProps<void>>
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
          lease/licence.
        </p>
      </Section>
    );
  }

  return isEditing ? (
    <ProtectedComponent claims={[Claims.LEASE_EDIT]}>
      <AddImprovementsContainer
        formikRef={formikRef as RefObject<FormikProps<LeaseFormModel>>}
        onEdit={onEdit}
        improvements={improvements ?? []}
        loading={loading}
        onSuccess={onSuccess}
      />
    </ProtectedComponent>
  ) : (
    <Improvements improvements={improvements ?? []} loading={loading} />
  );
};
