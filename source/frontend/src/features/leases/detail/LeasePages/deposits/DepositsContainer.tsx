import { Formik } from 'formik';
import noop from 'lodash/noop';
import { useContext, useEffect, useState } from 'react';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { LeaseStatusUpdateSolver } from '@/features/leases/models/LeaseStatusUpdateSolver';
import { useSecurityDepositRepository } from '@/hooks/repositories/useSecurityDepositRepository';
import { useSecurityDepositReturnRepository } from '@/hooks/repositories/useSecurityDepositReturnRepository';
import { ApiGen_Concepts_SecurityDeposit } from '@/models/api/generated/ApiGen_Concepts_SecurityDeposit';
import { ApiGen_Concepts_SecurityDepositReturn } from '@/models/api/generated/ApiGen_Concepts_SecurityDepositReturn';
import { exists, isValidId } from '@/utils/utils';

import DepositNotes from './components/DepositNotes/DepositNotes';
import DepositsReceivedContainer from './components/DepositsReceivedContainer/DepositsReceivedContainer';
import DepositsReturnedContainer from './components/DepositsReturnedContainer/DepositsReturnedContainer';
import ReceivedDepositModal from './modal/receivedDepositModal/ReceivedDepositModal';
import ReturnedDepositModal from './modal/returnedDepositModal/ReturnedDepositModal';
import { FormLeaseDeposit } from './models/FormLeaseDeposit';
import { FormLeaseDepositReturn } from './models/FormLeaseDepositReturn';
import { LeaseDepositForm } from './models/LeaseDepositForm';
import * as Styled from './styles';

export interface IDepositsContainerProps {
  onSuccess: () => void;
}

export const DepositsContainer: React.FunctionComponent<
  React.PropsWithChildren<IDepositsContainerProps>
> = props => {
  const { lease } = useContext(LeaseStateContext);
  const statusSolver = new LeaseStatusUpdateSolver(lease?.fileStatusTypeCode);
  const {
    getSecurityDeposits: {
      execute: getSecurityDeposits,
      loading,
      response: securityDepositsResponse,
    },
    deleteSecurityDeposit: { execute: deleteSecurityDeposit },
    updateSecurityDeposit: { execute: updateSecurityDeposit },
    addSecurityDeposit: { execute: addSecurityDeposit },
    updateSecurityDepositNote: { execute: updateSecurityDepositNote },
  } = useSecurityDepositRepository();

  const {
    updateSecurityDepositReturn: { execute: updateSecurityDepositReturn },
    addSecurityDepositReturn: { execute: addSecurityDepositReturn },
    deleteSecurityDepositReturn: { execute: deleteSecurityDepositReturn },
  } = useSecurityDepositReturnRepository();

  const securityDeposits: ApiGen_Concepts_SecurityDeposit[] = securityDepositsResponse ?? [];
  useEffect(() => {
    lease?.id && getSecurityDeposits(lease.id);
  }, [lease, getSecurityDeposits]);
  const depositReturns: ApiGen_Concepts_SecurityDepositReturn[] =
    securityDeposits
      ?.flatMap((x: ApiGen_Concepts_SecurityDeposit) => x.depositReturns)
      .filter(exists) ?? [];
  const [editNotes, setEditNotes] = useState<boolean>(false);

  const [showDepositEditModal, setShowEditModal] = useState<boolean>(false);
  const [deleteModalWarning, setDeleteModalWarning] = useState<boolean>(false);
  const [deleteReturnModalWarning, setDeleteReturnModalWarning] = useState<boolean>(false);

  const [showReturnEditModal, setShowReturnEditModal] = useState<boolean>(false);

  const [depositToDelete, setDepositToDelete] = useState<FormLeaseDeposit | undefined>(undefined);
  const [editDepositValue, setEditDepositValue] = useState<FormLeaseDeposit>(
    FormLeaseDeposit.createEmpty(lease?.id ?? 0),
  );

  const [depositReturnToDelete, setDepositReturnToDelete] = useState<
    FormLeaseDepositReturn | undefined
  >(undefined);
  const [editReturnValue, setEditReturnValue] = useState<FormLeaseDepositReturn | undefined>(
    undefined,
  );

  const onAddDeposit = () => {
    lease?.id && setEditDepositValue(FormLeaseDeposit.createEmpty(lease?.id));
    setShowEditModal(true);
  };

  const onEditDeposit = (id: number) => {
    const deposit = securityDeposits.find((x: ApiGen_Concepts_SecurityDeposit) => x.id === id);
    if (deposit) {
      setEditDepositValue(FormLeaseDeposit.fromApi(deposit));
      setShowEditModal(true);
    }
  };

  const onDeleteDeposit = (id: number) => {
    const deposit = securityDeposits.find((x: ApiGen_Concepts_SecurityDeposit) => x.id === id);
    if (deposit) {
      setDepositToDelete(FormLeaseDeposit.fromApi(deposit));
      setDeleteModalWarning(true);
    }
  };

  const onReturnDeposit = (id: number) => {
    const deposit = securityDeposits.find((x: ApiGen_Concepts_SecurityDeposit) => x.id === id);
    if (deposit) {
      setEditReturnValue(FormLeaseDepositReturn.createEmpty(deposit));
      setShowReturnEditModal(true);
    }
  };

  const onDeleteDepositConfirmed = async () => {
    if (lease && lease.id && lease.rowVersion && depositToDelete) {
      await deleteSecurityDeposit(lease.id, depositToDelete.toApi());
      setDepositToDelete(undefined);
      setDeleteModalWarning(false);
      getSecurityDeposits(lease.id);
      props.onSuccess();
    }
  };

  const onDeleteDepositReturnConfirmed = async () => {
    if (lease && lease.id && lease.rowVersion && depositReturnToDelete) {
      await deleteSecurityDepositReturn(lease.id, depositReturnToDelete.toApi());
      setDepositReturnToDelete(undefined);
      setDeleteReturnModalWarning(false);
      getSecurityDeposits(lease.id);
      props.onSuccess();
    }
  };

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param depositForm
   */
  const onSaveDeposit = async (depositForm: FormLeaseDeposit) => {
    if (exists(lease) && isValidId(lease.id)) {
      const updatedSecurityDeposit = depositForm.id
        ? await updateSecurityDeposit(lease.id, depositForm.toApi())
        : await addSecurityDeposit(lease.id, depositForm.toApi());
      if (isValidId(updatedSecurityDeposit?.id)) {
        setEditDepositValue(FormLeaseDeposit.createEmpty(lease.id));
        setShowEditModal(false);
        getSecurityDeposits(lease.id);
        props.onSuccess();
      }
    } else {
      console.error('Lease information incomplete');
    }
  };

  const onEditReturnDeposit = (id: number) => {
    const deposit = depositReturns.find(x => x.id === id);
    if (deposit) {
      const parentDeposit = securityDeposits.find(x => x.id === deposit?.parentDepositId);
      if (parentDeposit) {
        setEditReturnValue(FormLeaseDepositReturn.fromApi(deposit, parentDeposit));
        setShowReturnEditModal(true);
        lease?.id && getSecurityDeposits(lease.id);
      } else {
        console.error('Parent deposit incomplete');
      }
    }
  };

  const onDeleteDepositReturn = (id: number) => {
    const deposit = depositReturns.find(x => x.id === id);
    if (deposit) {
      const parentDeposit = securityDeposits.find(
        (x: ApiGen_Concepts_SecurityDeposit) => x.id === deposit?.parentDepositId,
      );
      if (parentDeposit) {
        setDepositReturnToDelete(FormLeaseDepositReturn.fromApi(deposit, parentDeposit));
        setDeleteReturnModalWarning(true);
      } else {
        console.error('Parent deposit incomplete');
      }
    }
  };

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param returnDepositForm
   */
  const onSaveReturnDeposit = async (returnDepositForm: FormLeaseDepositReturn) => {
    if (exists(lease) && isValidId(lease.id)) {
      const request: ApiGen_Concepts_SecurityDepositReturn = returnDepositForm.toApi();
      const securityDepositReturn = request?.id
        ? await updateSecurityDepositReturn(lease.id, request)
        : await addSecurityDepositReturn(lease.id, request);
      if (isValidId(securityDepositReturn?.id)) {
        setDepositReturnToDelete(undefined);
        setShowReturnEditModal(false);
        getSecurityDeposits(lease.id);
        props.onSuccess();
      }
    } else {
      console.error('Lease information incomplete');
    }
  };

  const initialValues = LeaseFormModel.fromApi(lease);

  return (
    <>
      <LoadingBackdrop show={loading} parentScreen />
      <Formik initialValues={{ ...new LeaseDepositForm(), ...initialValues }} onSubmit={noop}>
        {formikProps => (
          <Styled.DepositsContainer>
            <DepositsReceivedContainer
              statusSolver={statusSolver}
              securityDeposits={securityDeposits}
              onAdd={onAddDeposit}
              onEdit={onEditDeposit}
              onDelete={onDeleteDeposit}
              onReturn={onReturnDeposit}
            />

            <DepositsReturnedContainer
              securityDeposits={securityDeposits}
              depositReturns={depositReturns}
              onEdit={onEditReturnDeposit}
              onDelete={onDeleteDepositReturn}
              statusSolver={statusSolver}
            />

            <DepositNotes
              disabled={!editNotes}
              onEdit={() => setEditNotes(true)}
              isFileFinalStatus={!statusSolver?.canEditDeposits()}
              onSave={async (notes: string) => {
                lease?.id && (await updateSecurityDepositNote(lease.id, notes));
                setEditNotes(false);
                props.onSuccess();
              }}
              onCancel={() => {
                setEditNotes(false);
                formikProps.setFieldValue('returnNotes', lease?.returnNotes ?? '');
              }}
            />

            <GenericModal
              variant="warning"
              display={deleteModalWarning}
              title="Delete Deposit"
              message={`Are you sure you want to remove the deposit?`}
              handleOk={() => onDeleteDepositConfirmed()}
              okButtonText="OK"
              setDisplay={setDeleteModalWarning}
            />
            <GenericModal
              variant="warning"
              display={deleteReturnModalWarning}
              title="Delete Deposit Return"
              message={`Are you sure you want to remove this deposit return?`}
              handleOk={() => onDeleteDepositReturnConfirmed()}
              okButtonText="OK"
              setDisplay={setDeleteReturnModalWarning}
            />

            <ReceivedDepositModal
              display={showDepositEditModal}
              initialValues={editDepositValue}
              onCancel={() => {
                lease?.id && setEditDepositValue(FormLeaseDeposit.createEmpty(lease.id));
                setShowEditModal(false);
              }}
              onSave={onSaveDeposit}
            />

            <ReturnedDepositModal
              display={showReturnEditModal}
              initialValues={editReturnValue}
              onCancel={() => {
                setEditReturnValue(undefined);
                setShowReturnEditModal(false);
              }}
              onSave={onSaveReturnDeposit}
            />
          </Styled.DepositsContainer>
        )}
      </Formik>
    </>
  );
};

export default DepositsContainer;
