import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useExpropriationEventRepository } from '@/hooks/repositories/useExpropriationEventRepository';
import { useInterestHolderRepository } from '@/hooks/repositories/useInterestHolderRepository';
import { getMockApiAcquisitionFileOwnerPerson } from '@/mocks/acquisitionFiles.mock';
import { getMockExpropriationEvent } from '@/mocks/expropriationEvents.mock';
import { getMockApiInterestHolderPerson } from '@/mocks/interestHolders.mock';
import { ApiGen_Concepts_ExpropriationEvent } from '@/models/api/generated/ApiGen_Concepts_ExpropriationEvent';
import { act, getMockRepositoryObj, render, screen, userEvent } from '@/utils/test-utils';

import {
  ExpropriationEventHistoryContainer,
  IExpropriationEventHistoryContainerProps,
} from './ExpropriationEventHistoryContainer';
import { IExpropriationEventHistoryViewProps } from './ExpropriationEventHistoryView';
import { IExpropriationEventModalProps } from './modal/ExpropriationEventModal';
import { ExpropriationEventFormModel } from './models';

vi.mock('@/hooks/repositories/useExpropriationEventRepository');
const mockGetExpropriationEventsApi = getMockRepositoryObj([]);
const mockAddExpropriationEventsApi = getMockRepositoryObj();
const mockUpdateExpropriationEventsApi = getMockRepositoryObj();
const mockDeleteExpropriationEventsApi = getMockRepositoryObj();

vi.mock('@/hooks/repositories/useAcquisitionProvider');
const mockGetAcquisitionOwnersApi = getMockRepositoryObj([]);

vi.mock('@/hooks/repositories/useInterestHolderRepository');
const mockGetAcquisitionInterestHoldersApi = getMockRepositoryObj([]);

describe('ExpropriationEventHistoryContainer', () => {
  let viewProps: IExpropriationEventHistoryViewProps;

  const ViewMock = vi.fn((props: IExpropriationEventHistoryViewProps) => {
    viewProps = props;
    return (
      <div>
        <button data-testid="add-button" onClick={() => props.onAdd()}>
          Add
        </button>
        <button data-testid="update-button" onClick={() => props.onUpdate(1)}>
          Update
        </button>
        <button data-testid="delete-button" onClick={() => props.onDelete(1)}>
          Delete
        </button>
        {props.isLoading && <div data-testid="loading-indicator">Loading...</div>}
      </div>
    );
  });

  let modalProps: IExpropriationEventModalProps;

  const ModalMock = vi.fn((props: IExpropriationEventModalProps) => {
    modalProps = props;
    return (
      <div>
        <button
          data-testid="save-button"
          onClick={() =>
            props.onSave(ExpropriationEventFormModel.createEmpty(props.acquisitionFileId))
          }
        >
          Save
        </button>
        <button data-testid="cancel-button" onClick={() => props.onCancel()}>
          Cancel
        </button>
      </div>
    );
  });

  const setup = async (props: Partial<IExpropriationEventHistoryContainerProps> = {}) => {
    const defaultProps: IExpropriationEventHistoryContainerProps = {
      acquisitionFileId: 1,
      View: ViewMock,
      ModalView: ModalMock,
    };

    const rendered = render(<ExpropriationEventHistoryContainer {...defaultProps} {...props} />);
    await act(async () => {});

    return rendered;
  };

  beforeEach(() => {
    vi.mocked(useExpropriationEventRepository, { partial: true }).mockReturnValue({
      getExpropriationEvents: mockGetExpropriationEventsApi,
      addExpropriationEvent: mockAddExpropriationEventsApi,
      updateExpropriationEvent: mockUpdateExpropriationEventsApi,
      deleteExpropriationEvent: mockDeleteExpropriationEventsApi,
    });

    vi.mocked(useAcquisitionProvider, { partial: true }).mockReturnValue({
      getAcquisitionOwners: mockGetAcquisitionOwnersApi,
    });
    vi.mocked(useInterestHolderRepository, { partial: true }).mockReturnValue({
      getAcquisitionInterestHolders: mockGetAcquisitionInterestHoldersApi,
    });
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the View component', async () => {
    await setup();

    expect(ViewMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventHistoryViewProps>>({
        isLoading: false,
        onAdd: expect.any(Function),
        onUpdate: expect.any(Function),
        onDelete: expect.any(Function),
      }),
      {},
    );
  });

  it('loads existing expropriation events from the API', async () => {
    const apiEvent = getMockExpropriationEvent();

    mockGetExpropriationEventsApi.execute.mockImplementationOnce(async () => {
      mockGetExpropriationEventsApi.response = [apiEvent];
      return [apiEvent];
    });

    await setup();

    expect(ViewMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventHistoryViewProps>>({
        isLoading: false,
        expropriationEvents: [apiEvent],
        onAdd: expect.any(Function),
        onUpdate: expect.any(Function),
        onDelete: expect.any(Function),
      }),
      {},
    );
  });

  it('loads acquisition owners and interest holders from the API', async () => {
    const apiOwner = getMockApiAcquisitionFileOwnerPerson();
    const apiInterestHolder = getMockApiInterestHolderPerson();
    const expectedPayees: PayeeOption[] = [
      PayeeOption.createOwner(apiOwner, null),
      PayeeOption.createInterestHolder(apiInterestHolder, null),
    ];

    mockGetAcquisitionOwnersApi.execute.mockResolvedValueOnce([apiOwner]);
    mockGetAcquisitionInterestHoldersApi.execute.mockResolvedValueOnce([apiInterestHolder]);

    await setup();

    expect(ModalMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventModalProps>>({
        display: false,
        payeeOptions: expectedPayees,
        onSave: expect.any(Function),
        onCancel: expect.any(Function),
      }),
      {},
    );
  });

  it('calls the API when onSave is triggered', async () => {
    mockAddExpropriationEventsApi.execute.mockResolvedValueOnce(getMockExpropriationEvent());
    mockUpdateExpropriationEventsApi.execute.mockResolvedValueOnce(getMockExpropriationEvent());

    await setup();

    // add
    await act(async () => {
      modalProps.onSave(ExpropriationEventFormModel.createEmpty(1));
    });

    expect(mockAddExpropriationEventsApi.execute).toHaveBeenCalledWith(
      1,
      expect.objectContaining<Partial<ApiGen_Concepts_ExpropriationEvent>>({
        acquisitionFileId: 1,
      }),
    );

    // update
    await act(async () => {
      const values = ExpropriationEventFormModel.createEmpty(1);
      values.id = 1;
      values.eventDate = '2025-01-15';
      modalProps.onSave(values);
    });

    expect(mockUpdateExpropriationEventsApi.execute).toHaveBeenCalledWith(
      1,
      expect.objectContaining<Partial<ApiGen_Concepts_ExpropriationEvent>>({
        acquisitionFileId: 1,
        eventDate: '2025-01-15',
      }),
    );

    // expropriation history list should be refreshed after add/update
    expect(mockGetExpropriationEventsApi.execute).toHaveBeenCalledTimes(3);
  });

  it('passes the loading state to the View component', async () => {
    vi.mocked(useExpropriationEventRepository, { partial: true }).mockReturnValue({
      getExpropriationEvents: { ...mockGetExpropriationEventsApi, loading: true },
      addExpropriationEvent: mockAddExpropriationEventsApi,
      updateExpropriationEvent: mockUpdateExpropriationEventsApi,
      deleteExpropriationEvent: mockDeleteExpropriationEventsApi,
    });

    await setup();

    expect(screen.getByTestId('loading-indicator')).toBeVisible();
  });

  it('hides the modal form by default', async () => {
    await setup();

    expect(ModalMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventModalProps>>({
        display: false,
        onSave: expect.any(Function),
        onCancel: expect.any(Function),
      }),
      {},
    );
  });

  it('shows the modal when onAdd is triggered', async () => {
    await setup();
    await act(async () => viewProps.onAdd());

    expect(ModalMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventModalProps>>({
        display: true,
        initialValues: ExpropriationEventFormModel.createEmpty(1),
        onSave: expect.any(Function),
        onCancel: expect.any(Function),
      }),
      {},
    );
  });

  it('dismisses the modal when onCancel is triggered', async () => {
    await setup();
    await act(async () => viewProps.onAdd());

    expect(ModalMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventModalProps>>({
        display: true,
        initialValues: ExpropriationEventFormModel.createEmpty(1),
        onSave: expect.any(Function),
        onCancel: expect.any(Function),
      }),
      {},
    );

    await act(async () => modalProps.onCancel());

    expect(ModalMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventModalProps>>({
        display: false,
      }),
      {},
    );
  });

  it('populates the modal with existing values when onUpdate is triggered', async () => {
    const apiEvent = getMockExpropriationEvent();

    mockGetExpropriationEventsApi.execute.mockImplementationOnce(async () => {
      mockGetExpropriationEventsApi.response = [apiEvent];
      return [apiEvent];
    });

    await setup();
    await act(async () => viewProps.onUpdate(apiEvent.id));

    expect(ModalMock).toHaveBeenCalledWith(
      expect.objectContaining<Partial<IExpropriationEventModalProps>>({
        display: true,
        initialValues: ExpropriationEventFormModel.fromApi(apiEvent),
        onSave: expect.any(Function),
        onCancel: expect.any(Function),
      }),
      {},
    );
  });

  it('shows a confirmation popup when onDelete is triggered', async () => {
    const apiEvent = getMockExpropriationEvent();

    mockGetExpropriationEventsApi.execute.mockImplementationOnce(async () => {
      mockGetExpropriationEventsApi.response = [apiEvent];
      return [apiEvent];
    });

    await setup();
    await act(async () => viewProps.onDelete(apiEvent.id));

    expect(screen.getByText(/Delete Expropriation Event/i)).toBeVisible();
  });

  it('calls the API when the user confirms the removal', async () => {
    const apiEvent = getMockExpropriationEvent();

    mockDeleteExpropriationEventsApi.execute.mockResolvedValueOnce(true);
    mockGetExpropriationEventsApi.execute.mockImplementationOnce(async () => {
      mockGetExpropriationEventsApi.response = [apiEvent];
      return [apiEvent];
    });

    await setup();
    await act(async () => viewProps.onDelete(apiEvent.id));

    expect(screen.getByText(/Delete Expropriation Event/i)).toBeVisible();

    await act(async () => userEvent.click(screen.getByTitle('ok-modal')));

    expect(mockDeleteExpropriationEventsApi.execute).toHaveBeenCalled();
    expect(mockGetExpropriationEventsApi.execute).toHaveBeenCalled();
  });

  it('dismisses the popup and does not call the API when the user cancels the removal', async () => {
    const apiEvent = getMockExpropriationEvent();

    mockDeleteExpropriationEventsApi.execute.mockResolvedValueOnce(true);
    mockGetExpropriationEventsApi.execute.mockImplementationOnce(async () => {
      mockGetExpropriationEventsApi.response = [apiEvent];
      return [apiEvent];
    });

    await setup();
    await act(async () => viewProps.onDelete(apiEvent.id));

    expect(screen.getByText(/Delete Expropriation Event/i)).toBeVisible();

    await act(async () => userEvent.click(screen.getByTitle('cancel-modal')));

    expect(mockDeleteExpropriationEventsApi.execute).not.toHaveBeenCalled();
  });
});
