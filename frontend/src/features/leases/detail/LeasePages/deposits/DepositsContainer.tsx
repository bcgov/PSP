import { FormSection } from 'components/common/form/styles';
import GenericModal from 'components/common/GenericModal';
import { LeaseStateContext } from 'features/leases/context/LeaseContext';
import { getIn, useFormikContext } from 'formik';
import {
  FormLeaseDeposit,
  FormLeaseDepositReturn,
  IFormLease,
  ILeaseSecurityDeposit,
  ILeaseSecurityDepositReturn,
} from 'interfaces';
import { IParentConcurrencyGuard } from 'interfaces/IParentConcurrencyGuard';
import { useContext, useState } from 'react';

import DepositNotes from './components/DepositNotes/DepositNotes';
import DepositsReceivedContainer from './components/DepositsReceivedContainer/DepositsReceivedContainer';
import DepositsReturnedContainer from './components/DepositsReturnedContainer/DepositsReturnedContainer';
import { useLeaseDepositReturns } from './hooks/useDepositReturns';
import { useLeaseDeposits } from './hooks/useDeposits';
import ReceivedDepositModal from './modal/receivedDepositModal/ReceivedDepositModal';
import ReturnedDepositModal from './modal/returnedDepositModal/ReturnedDepositModal';
import * as Styled from './styles';

export interface IDepositsContainerProps {}

export const DepositsContainer: React.FunctionComponent<IDepositsContainerProps> = () => {
  const { lease, setLease } = useContext(LeaseStateContext);
  const { values } = useFormikContext<IFormLease>();
  const securityDeposits: ILeaseSecurityDeposit[] = getIn(values, 'securityDeposits') ?? [];
  const depositReturns: ILeaseSecurityDepositReturn[] =
    getIn(values, 'securityDepositReturns') ?? [];

  const [showDepositEditModal, setShowEditModal] = useState<boolean>(false);
  const [deleteModalWarning, setDeleteModalWarning] = useState<boolean>(false);
  const [deleteReturnModalWarning, setDeleteReturnModalWarning] = useState<boolean>(false);

  const [showReturnEditModal, setShowReturnEditModal] = useState<boolean>(false);

  const [depositToDelete, setDepositToDelete] = useState<FormLeaseDeposit | undefined>(undefined);
  const [editDepositValue, setEditDepositValue] = useState<FormLeaseDeposit>(
    FormLeaseDeposit.createEmpty(),
  );

  const [depositReturnToDelete, setDepositReturnToDelete] = useState<
    FormLeaseDepositReturn | undefined
  >(undefined);
  const [editReturnValue, setEditReturnValue] = useState<FormLeaseDepositReturn | undefined>(
    undefined,
  );

  const { updateLeaseDeposit, removeLeaseDeposit } = useLeaseDeposits();
  const { updateLeaseDepositReturn, removeLeaseDepositReturn } = useLeaseDepositReturns();

  const onAddDeposit = () => {
    setEditDepositValue(FormLeaseDeposit.createEmpty());
    setShowEditModal(true);
  };

  const onEditDeposit = (id: number) => {
    var deposit = securityDeposits.find(x => x.id === id);
    if (deposit) {
      setEditDepositValue(FormLeaseDeposit.createFromModel(deposit));
      setShowEditModal(true);
    }
  };

  const onDeleteDeposit = (id: number) => {
    var deposit = securityDeposits.find(x => x.id === id);
    if (deposit) {
      setDepositToDelete(FormLeaseDeposit.createFromModel(deposit));
      setDeleteModalWarning(true);
    }
  };

  const onReturnDeposit = (id: number) => {
    var deposit = securityDeposits.find(x => x.id === id);
    if (deposit) {
      setEditReturnValue(FormLeaseDepositReturn.createEmpty(deposit));
      setShowReturnEditModal(true);
    }
  };

  const onDeleteDepositConfirmed = async () => {
    if (lease && lease.id && lease.rowVersion && depositToDelete) {
      const updatedLease = await removeLeaseDeposit({
        parentId: lease.id,
        parentRowVersion: lease.rowVersion,
        payload: depositToDelete.toInterfaceModel(),
      });
      if (!!updatedLease?.id) {
        setDepositToDelete(undefined);
        setDeleteModalWarning(false);
        setLease(updatedLease);
      }
    }
  };

  const onDeleteDepositReturnConfirmed = async () => {
    if (lease && lease.id && lease.rowVersion && depositReturnToDelete) {
      const updatedLease = await removeLeaseDepositReturn({
        parentId: lease.id,
        parentRowVersion: lease.rowVersion,
        payload: depositReturnToDelete.toInterfaceModel(),
      });
      if (!!updatedLease?.id) {
        setDepositReturnToDelete(undefined);
        setDeleteReturnModalWarning(false);
        setLease(updatedLease);
      }
    }
  };

  /**
   * Send the save request (either an update or an add). Use the response to update the parent lease.
   * @param depositForm
   */
  const onSaveDeposit = async (depositForm: FormLeaseDeposit) => {
    if (lease && lease.id && lease.rowVersion) {
      let request: IParentConcurrencyGuard<ILeaseSecurityDeposit> = {
        parentId: lease.id,
        parentRowVersion: lease.rowVersion,
        payload: depositForm.toInterfaceModel(),
      };
      const updatedLease = await updateLeaseDeposit(request);
      if (!!updatedLease?.id) {
        setLease(updatedLease);
        setEditDepositValue(FormLeaseDeposit.createEmpty());
        setShowEditModal(false);
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
        setEditReturnValue(FormLeaseDepositReturn.createFromModel(deposit, parentDeposit));
        setShowReturnEditModal(true);
      } else {
        console.error('Parent deposit incomplete');
      }
    }
  };

  const onDeleteDepositReturn = (id: number) => {
    var deposit = depositReturns.find(x => x.id === id);
    if (deposit) {
      var parentDeposit = securityDeposits.find(x => x.id === deposit?.parentDepositId);
      if (parentDeposit) {
        setDepositReturnToDelete(FormLeaseDepositReturn.createFromModel(deposit, parentDeposit));
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
    if (lease && lease.id && lease.rowVersion) {
      let request: IParentConcurrencyGuard<ILeaseSecurityDepositReturn> = {
        parentId: lease.id,
        parentRowVersion: lease.rowVersion,
        payload: returnDepositForm.toInterfaceModel(),
      };
      const updatedLease = await updateLeaseDepositReturn(request);
      if (!!updatedLease?.id) {
        setLease(updatedLease);
        setDepositReturnToDelete(undefined);
        setShowReturnEditModal(false);
      }
    } else {
      console.error('Lease information incomplete');
    }
  };

  return (
    <Styled.DepositsContainer>
      <DepositsReceivedContainer
        securityDeposits={securityDeposits}
        depositReturns={depositReturns}
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

      <FormSection>
        <DepositNotes disabled={true} />
      </FormSection>

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
          setEditDepositValue(FormLeaseDeposit.createEmpty());
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
  );
};

export default DepositsContainer;
