import { Formik } from 'formik';
import { noop } from 'lodash';
import { useContext, useEffect, useState } from 'react';

import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { LeaseStateContext } from '@/features/leases/context/LeaseContext';
import { LeaseFormModel } from '@/features/leases/models';
import { useSecurityDepositRepository } from '@/hooks/repositories/useSecurityDepositRepository';
import { useSecurityDepositReturnRepository } from '@/hooks/repositories/useSecurityDepositReturnRepository';
import { Api_SecurityDeposit, Api_SecurityDepositReturn } from '@/models/api/SecurityDeposit';

import DepositNotes from './components/DepositNotes/DepositNotes';
import DepositsReceivedContainer from './components/DepositsReceivedContainer/DepositsReceivedContainer';
import DepositsReturnedContainer from './components/DepositsReturnedContainer/DepositsReturnedContainer';
import ReceivedDepositModal from './modal/receivedDepositModal/ReceivedDepositModal';
import ReturnedDepositModal from './modal/returnedDepositModal/ReturnedDepositModal';
import { FormLeaseDeposit } from './models/FormLeaseDeposit';
import { FormLeaseDepositReturn } from './models/FormLeaseDepositReturn';
import { LeaseDepositForm } from './models/LeaseDepositForm';
import * as Styled from './styles';

export interface IDepositsContainerProps {}

export const DepositsContainer: React.FunctionComponent<
  React.PropsWithChildren<IDepositsContainerProps>
> = () => {
  const { lease } = useContext(LeaseStateContext);
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

  const securityDeposits: Api_SecurityDeposit[] = securityDepositsResponse ?? [];
  useEffect(() => {
    lease?.id && getSecurityDeposits(lease.id);
  }, [lease, getSecurityDeposits]);
  const depositReturns: Api_SecurityDepositReturn[] =
    securityDeposits?.flatMap((x: Api_SecurityDeposit) => x.depositReturns) ?? [];
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
    var deposit = securityDeposits.find((x: Api_SecurityDeposit) => x.id === id);
    if (deposit) {
      setEditDepositValue(FormLeaseDeposit.fromApi(deposit));
      setShowEditModal(true);
    }
  };

  const onDeleteDeposit = (id: number) => {
    var deposit = securityDeposits.find((x: Api_SecurityDeposit) => x.id === id);
    if (deposit) {
      setDepositToDelete(FormLeaseDeposit.fromApi(deposit));
      setDeleteModalWarning(true);
    }
  };

  const onReturnDeposit = (id: number) => {
    var deposit = securityDeposits.find((x: Api_SecurityDeposit) => x.id === id);
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
    }
  };

  const onDeleteDepositReturnConfirmed = async () => {
    if (lease && lease.id && lease.rowVersion && depositReturnToDelete) {
      await deleteSecurityDepositReturn(lease.id, depositReturnToDelete.toApi());
      setDepositReturnToDelete(undefined);
      setDeleteReturnModalWarning(false);
      getSecurityDeposits(lease.id);
    }
  };

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param depositForm
   */
  const onSaveDeposit = async (depositForm: FormLeaseDeposit) => {
    if (lease && lease.id) {
      const updatedSecurityDeposit = depositForm.id
        ? await updateSecurityDeposit(lease.id, depositForm.toApi())
        : await addSecurityDeposit(lease.id, depositForm.toApi());
      if (!!updatedSecurityDeposit?.id) {
        setEditDepositValue(FormLeaseDeposit.createEmpty(lease.id));
        setShowEditModal(false);
        getSecurityDeposits(lease.id);
      }
    } else {
      console.error('Lease information incomplete');
    }
  };

  const onEditReturnDeposit = (id: number) => {
    var deposit = depositReturns.find(x => x.id === id);
    if (deposit) {
      var parentDeposit = securityDeposits.find(x => x.id === deposit?.parentDepositId);
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
    var deposit = depositReturns.find(x => x.id === id);
    if (deposit) {
      var parentDeposit = securityDeposits.find(
        (x: Api_SecurityDeposit) => x.id === deposit?.parentDepositId,
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
    if (lease && lease.id) {
      let request: Api_SecurityDepositReturn = returnDepositForm.toApi();
      const securityDepositReturn = request?.id
        ? await updateSecurityDepositReturn(lease.id, request)
        : await addSecurityDepositReturn(lease.id, request);
      if (!!securityDepositReturn?.id) {
        setDepositReturnToDelete(undefined);
        setShowReturnEditModal(false);
        getSecurityDeposits(lease.id);
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
            />

            <DepositNotes
              disabled={!editNotes}
              onEdit={() => setEditNotes(true)}
              onSave={async (notes: string) => {
                lease?.id && (await updateSecurityDepositNote(lease.id, notes));
                setEditNotes(false);
              }}
              onCancel={() => {
                setEditNotes(false);
                formikProps.setFieldValue('returnNotes', lease?.returnNotes ?? '');
              }}
            />

            <GenericModal
              display={deleteModalWarning}
              title="Delete Deposit"
              message={`Are you sure you want to remove the deposit?`}
              handleOk={() => onDeleteDepositConfirmed()}
              okButtonText="OK"
              closeButton
              setDisplay={setDeleteModalWarning}
            />
            <GenericModal
              display={deleteReturnModalWarning}
              title="Delete Deposit Return"
              message={`Are you sure you want to remove this deposit return?`}
              handleOk={() => onDeleteDepositReturnConfirmed()}
              okButtonText="OK"
              closeButton
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
