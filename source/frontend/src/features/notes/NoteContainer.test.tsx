import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Claims, NoteTypes } from 'constants/index';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { mockNoteResponse } from 'mocks/mockNoteResponses';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import {
  render,
  RenderOptions,
  userEvent,
  waitFor,
  waitForElementToBeRemoved,
} from 'utils/test-utils';

import { INotesDetailContainerProps, NoteContainer } from './NoteContainer';

// mock auth library
jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const openModal = jest.fn();
const closeModal = jest.fn();
const onSuccess = jest.fn();

const BASIC_PROPS: INotesDetailContainerProps = {
  type: NoteTypes.Activity,
  noteId: 1,
  isOpened: true,
  editMode: false,
  openModal,
  closeModal,
  onSuccess,
};

describe('NoteContainer component', () => {
  // render component under test
  const setup = (
    props: INotesDetailContainerProps = { ...BASIC_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(<NoteContainer {...props} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      history,
      useMockAuthentication: true,
      claims: [Claims.NOTE_EDIT],
    });

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
      getEditButton: () => utils.getByRole('button', { name: /edit/i }),
      getCloseButton: () => utils.getByTitle('ok-modal'),
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios.onGet(new RegExp('notes/activity/*')).reply(200, mockNoteResponse(1));
  });

  afterEach(() => {
    mockAxios.resetHistory();
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(document.body).toMatchSnapshot();
  });

  it('renders a spinner while loading', async () => {
    const { getByTestId } = setup({ ...BASIC_PROPS });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
    await waitForElementToBeRemoved(spinner);
  });

  it('renders read-only note details by default', async () => {
    const { findByTitle, getByTestId, getSaveButton, getCancelButton, getEditButton } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    const textarea = await findByTitle(/Note/i);

    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
    expect(textarea).toHaveAttribute('readonly');
    expect(getEditButton()).toBeVisible();

    // These buttons should NOT be in the DOM
    expect(() => getSaveButton()).toThrow();
    expect(() => getCancelButton()).toThrow();
  });

  describe('When in Read-only mode', () => {
    it('closes the modal when Close button is clicked', async () => {
      const { getCloseButton, getByText, getByTestId } = setup();

      const spinner = getByTestId('filter-backdrop-loading');
      await waitForElementToBeRemoved(spinner);

      const modalTitle = getByText(/Notes/i);
      expect(modalTitle).toBeVisible();

      userEvent.click(getCloseButton());
      await waitFor(() => expect(closeModal).toBeCalled());
    });

    it('changes to edit mode when Edit button is clicked', async () => {
      const {
        getEditButton,
        getSaveButton,
        getCancelButton,
        findByText,
        findByLabelText,
      } = setup();
      const modalTitle = await findByText(/Notes/i);
      expect(modalTitle).toBeVisible();

      userEvent.click(getEditButton());
      const textarea = await findByLabelText(/Type a note/i);

      expect(modalTitle).toBeVisible();
      expect(textarea).toBeVisible();
      expect(textarea.tagName).toBe('TEXTAREA');
      expect(textarea).not.toHaveAttribute('readonly');
      expect(getSaveButton()).toBeVisible();
      expect(getCancelButton()).toBeVisible();

      // These buttons should NOT be in the DOM
      expect(() => getEditButton()).toThrow();
    });
  });

  describe('When in Edit mode', () => {
    it('allows updating notes', async () => {
      const {
        getByLabelText,
        getByText,
        getByTestId,
        getSaveButton,
        getCancelButton,
        getEditButton,
      } = setup({ ...BASIC_PROPS, editMode: true });

      const spinner = getByTestId('filter-backdrop-loading');
      await waitForElementToBeRemoved(spinner);

      const modalTitle = getByText(/Notes/i);
      const textarea = getByLabelText(/Type a note/i);

      expect(modalTitle).toBeVisible();
      expect(textarea).toBeVisible();
      expect(textarea.tagName).toBe('TEXTAREA');
      expect(textarea).not.toHaveAttribute('readonly');
      expect(getSaveButton()).toBeVisible();
      expect(getCancelButton()).toBeVisible();

      // These buttons should NOT be in the DOM
      expect(() => getEditButton()).toThrow();
    });

    it('closes the modal when Cancel button is clicked', async () => {
      const { getCancelButton, getByText, getByTestId } = setup({ ...BASIC_PROPS, editMode: true });

      const spinner = getByTestId('filter-backdrop-loading');
      await waitForElementToBeRemoved(spinner);

      const modalTitle = getByText(/Notes/i);
      expect(modalTitle).toBeVisible();

      userEvent.click(getCancelButton());
      await waitFor(() => expect(closeModal).toBeCalled());
    });

    it('closes the modal and submits form when Save button is clicked', async () => {
      const { getSaveButton, findByLabelText, getByTestId } = setup({
        ...BASIC_PROPS,
        editMode: true,
      });

      const spinner = getByTestId('filter-backdrop-loading');
      await waitForElementToBeRemoved(spinner);

      const textarea = await findByLabelText(/Type a note/i);
      userEvent.clear(textarea);
      userEvent.type(textarea, 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.');

      mockAxios.onPut().reply(200, mockNoteResponse(1));
      await waitFor(() => userEvent.click(getSaveButton()));

      expect(closeModal).toBeCalled();
      expect(onSuccess).toBeCalled();
    });
  });
});
