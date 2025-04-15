import orderBy from 'lodash/orderBy';
import React, { useCallback, useEffect, useMemo, useReducer, useState } from 'react';

import { TableSort } from '@/components/Table/TableSort';
import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';
import { exists, isValidId } from '@/utils';

import { IExpropriationEventHistoryViewProps } from './ExpropriationEventHistoryView';
import { IExpropriationEventModalProps } from './modal/ExpropriationEventModal';
import { ExpropriationEventFormModel, ExpropriationEventRow } from './models';

export interface IExpropriationEventHistoryContainerProps {
  acquisitionFileId: number;
  View: React.FunctionComponent<IExpropriationEventHistoryViewProps>;
  ModalView: React.FunctionComponent<IExpropriationEventModalProps>;
}

// internal container state vars
interface ContainerState {
  editExpropriationEventValue: ExpropriationEventFormModel;
  showExpropriationEditModal: boolean;
  sort: TableSort<ExpropriationEventRow>;
}

export const ExpropriationEventHistoryContainer: React.FunctionComponent<
  IExpropriationEventHistoryContainerProps
> = ({ acquisitionFileId, View, ModalView }) => {
  const [payeeOptions, setPayeeOptions] = useState<PayeeOption[]>([]);

  const [containerState, setContainerState] = useReducer(
    (prevState: ContainerState, newState: Partial<ContainerState>) => ({
      ...prevState,
      ...newState,
    }),
    {
      showExpropriationEditModal: false,
      editExpropriationEventValue: ExpropriationEventFormModel.createEmpty(acquisitionFileId),
      sort: {},
    },
  );

  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    getAcquisitionOwners: { execute: retrieveAcquisitionOwners, loading: loadingAcquisitionOwners },
  } = useAcquisitionProvider();
  const {
    getAcquisitionInterestHolders: {
      execute: fetchInterestHolders,
      loading: loadingInterestHolders,
    },
  } = useInterestHolderRepository();
  const {
    getExpropriationEvents: {
      execute: getExpropriationEvents,
      loading: loadingExpropriationEvents,
      response: expropriationEventsResponse,
    },
    addExpropriationEvent: { execute: addExpropriationEvent },
    updateExpropriationEvent: { execute: updateExpropriationEvent },
    deleteExpropriationEvent: { execute: deleteExpropriationEvent },
  } = useExpropriationEventRepository();

  const expropriationEvents: ApiGen_Concepts_ExpropriationEvent[] = useMemo(
    () => expropriationEventsResponse ?? [],
    [expropriationEventsResponse],
  );

  const fetchPayeeOptions = useCallback(
    async (acquisitionFileId: number) => {
      const acquisitionOwnersCall = retrieveAcquisitionOwners(acquisitionFileId);
      const interestHoldersCall = fetchInterestHolders(acquisitionFileId);

      await Promise.all([acquisitionOwnersCall, interestHoldersCall]).then(
        ([acquisitionOwners, interestHolders]) => {
          const options = [];

          if (exists(acquisitionOwners)) {
            const ownersOptions: PayeeOption[] = acquisitionOwners.map(x =>
              PayeeOption.createOwner(x, null),
            );
            options.push(...ownersOptions);
          }

          if (exists(interestHolders)) {
            const interestHolderOptions: PayeeOption[] = interestHolders.map(x =>
              PayeeOption.createInterestHolder(x, null),
            );
            options.push(...interestHolderOptions);
          }

          setPayeeOptions(options);
        },
      );
    },
    [retrieveAcquisitionOwners, fetchInterestHolders],
  );

  // Load the expropriation event history (and payee options) upon rendering this component
  useEffect(() => {
    if (isValidId(acquisitionFileId)) {
      getExpropriationEvents(acquisitionFileId);
      fetchPayeeOptions(acquisitionFileId);
    }
  }, [acquisitionFileId, fetchPayeeOptions, getExpropriationEvents]);

  // Launch the expropriation modal in response to the user clicking the "Add" button
  const onAddExpropriation = () => {
    setContainerState({
      editExpropriationEventValue: ExpropriationEventFormModel.createEmpty(acquisitionFileId),
      showExpropriationEditModal: true,
    });
  };

  // Launch the expropriation modal in response to the user clicking the "Update" icon-button on a given row in the history table
  const onUpdateExpropriation = (id: number) => {
    const event = expropriationEvents.find((x: ApiGen_Concepts_ExpropriationEvent) => x.id === id);
    if (exists(event)) {
      setContainerState({
        editExpropriationEventValue: ExpropriationEventFormModel.fromApi(event),
        showExpropriationEditModal: true,
      });
    } else {
      console.error('Invalid expropriation event');
    }
  };

  // Called when the SAVE button is clicked on the expropriation event modal.
  // Sends the save request (either an update or an add). Uses the response to update the parent lease.
  const onSaveExpropriationEvent = async (values: ExpropriationEventFormModel) => {
    if (exists(values)) {
      const updatedEvent = isValidId(values.id)
        ? await updateExpropriationEvent(acquisitionFileId, values.toApi(payeeOptions))
        : await addExpropriationEvent(acquisitionFileId, values.toApi(payeeOptions));

      if (isValidId(updatedEvent?.id)) {
        setContainerState({
          editExpropriationEventValue: ExpropriationEventFormModel.createEmpty(acquisitionFileId),
          showExpropriationEditModal: false,
        });
        await getExpropriationEvents(acquisitionFileId);
      }
    } else {
      console.error('Invalid expropriation event');
    }
  };

  const onDeleteExpropriation = async (id: number) => {
    const event = expropriationEvents.find((x: ApiGen_Concepts_ExpropriationEvent) => x.id === id);

    // show confirmation popup before actually removing the Expropriation Event
    setModalContent({
      ...getDeleteModalProps(),
      variant: 'error',
      title: 'Delete Expropriation Event',
      message: `You have selected to delete this Event from the history.
                                            Do you want to proceed?`,
      okButtonText: 'Yes',
      cancelButtonText: 'No',
      handleOk: async () => {
        if (isValidId(event?.id)) {
          const result = await deleteExpropriationEvent(acquisitionFileId, event.id);
          if (result === false) {
            console.error('Unable to delete expropriation event');
          }
          await getExpropriationEvents(acquisitionFileId);
        } else {
          console.error('Invalid expropriation event');
        }
        setDisplayModal(false);
      },
      handleCancel: () => {
        setDisplayModal(false);
      },
    });

    setDisplayModal(true);
  };

  // client-side sorting of table rows
  const eventRows = useMemo(
    () => (expropriationEvents ?? []).map(x => ExpropriationEventRow.fromApi(x)),
    [expropriationEvents],
  );

  const sortedEventRows = useMemo(() => {
    if (exists(containerState.sort) && eventRows.length > 0) {
      const sortFields = Object.keys(containerState.sort);
      if (sortFields?.length > 0) {
        const keyName = containerState.sort[sortFields[0]];
        return orderBy(eventRows, sortFields[0], keyName);
      }
      return eventRows;
    }
    return [];
  }, [containerState.sort, eventRows]);

  return (
    <>
      <View
        isLoading={loadingExpropriationEvents || loadingAcquisitionOwners || loadingInterestHolders}
        eventRows={sortedEventRows}
        sort={containerState.sort}
        setSort={value => {
          setContainerState({ sort: value });
        }}
        onAdd={onAddExpropriation}
        onUpdate={onUpdateExpropriation}
        onDelete={onDeleteExpropriation}
      />
      <ModalView
        acquisitionFileId={acquisitionFileId}
        display={containerState.showExpropriationEditModal}
        initialValues={containerState.editExpropriationEventValue}
        payeeOptions={payeeOptions}
        onCancel={() => {
          setContainerState({
            editExpropriationEventValue: ExpropriationEventFormModel.createEmpty(acquisitionFileId),
            showExpropriationEditModal: false,
          });
        }}
        onSave={onSaveExpropriationEvent}
      />
    </>
  );
};

export default ExpropriationEventHistoryContainer;
