import { useCallback, useEffect, useMemo, useState } from 'react';
import { useHistory, useRouteMatch } from 'react-router-dom';

import { useConsultationProvider } from '@/hooks/repositories/useConsultationProvider';
import { ApiGen_Concepts_ConsultationLease } from '@/models/api/generated/ApiGen_Concepts_ConsultationLease';
import { isValidId } from '@/utils';

import { LeasePageNames } from '../../../LeaseContainer';
import { IConsultationListViewProps } from './ConsultationListView';

export interface IConsultationListProps {
  leaseId: number;
  View: React.FunctionComponent<React.PropsWithChildren<IConsultationListViewProps>>;
}

export const ConsultationListContainer: React.FunctionComponent<
  React.PropsWithChildren<IConsultationListProps>
> = ({ leaseId, View }) => {
  const [leaseConsultations, setLeaseConsultations] = useState<ApiGen_Concepts_ConsultationLease[]>(
    [],
  );

  const {
    getLeaseConsultations: { execute: getConsultations, loading: loadingConsultations },
    deleteLeaseConsultation: { execute: deleteConsultation, loading: deletingConsultation },
  } = useConsultationProvider();

  if (!isValidId(leaseId)) {
    throw new Error('Unable to determine id of current file.');
  }

  const fetchData = useCallback(async () => {
    const consultations = await getConsultations(leaseId);

    if (consultations) {
      setLeaseConsultations(consultations);
    }
  }, [leaseId, getConsultations]);

  const history = useHistory();
  const match = useRouteMatch();

  const handleConsultationAdd = async () => {
    history.push(`${match.url}/${LeasePageNames.CONSULTATIONS}/add`);
  };

  const handleConsultationEdit = async (consultationId: number) => {
    history.push(`${match.url}/${LeasePageNames.CONSULTATIONS}/${consultationId}/edit`);
  };

  const handleConsultationDeleted = async (consultationId: number) => {
    if (isValidId(consultationId)) {
      await deleteConsultation(leaseId, consultationId);
      const updatedConsultations = await getConsultations(leaseId);
      if (updatedConsultations) {
        setLeaseConsultations(updatedConsultations);
      }
    }
  };

  useEffect(() => {
    fetchData();
  }, [fetchData]);

  const isLoading = useMemo(() => {
    return loadingConsultations || deletingConsultation;
  }, [deletingConsultation, loadingConsultations]);

  return leaseId ? (
    <View
      loading={isLoading}
      consultations={leaseConsultations}
      onAdd={handleConsultationAdd}
      onEdit={handleConsultationEdit}
      onDelete={handleConsultationDeleted}
    />
  ) : null;
};

export default ConsultationListContainer;
