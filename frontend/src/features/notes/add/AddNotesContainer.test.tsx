import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks/mockLookups';
import { Api_EntityNote } from 'models/api/Note';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { AddNotesContainer, IAddNotesContainerProps } from './AddNotesContainer';
import { EntityNoteForm } from './models';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const openModal = jest.fn();
const closeModal = jest.fn();

const BASIC_PROPS: IAddNotesContainerProps = {
  isOpened: true,
  openModal,
  closeModal,
  parentId: 1,
  parentType: 'activity',
};

describe('AddNotesContainer component', () => {
  // render component under test
  const setup = (
    props: IAddNotesContainerProps = BASIC_PROPS,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(<AddNotesContainer {...props} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      history,
    });

    return {
      ...utils,
      getSaveButton: () => utils.getByText(/Save/i),
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  beforeEach(() => {
    mockAxios.resetHistory();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    setup();
    expect(document.body).toMatchSnapshot();
  });

  it('renders the underlying form', () => {
    const { getByLabelText, getByText } = setup();

    const modalTitle = getByText(/Notes/);
    const textarea = getByLabelText(/Type a note/i);

    expect(modalTitle).toBeVisible();
    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
  });

  it('should cancel form when Cancel button is clicked', async () => {
    const { getCancelButton, getByText } = setup();

    expect(getByText(/Notes/)).toBeVisible();
    userEvent.click(getCancelButton());

    expect(closeModal).toBeCalled();
  });

  it('should save the form and close the modal when Submit button is clicked', async () => {
    const form = new EntityNoteForm();
    form.parentId = BASIC_PROPS.parentId;
    form.note.note = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByLabelText } = setup(BASIC_PROPS);

    const textarea = await findByLabelText(/Type a note/i);
    userEvent.type(textarea, form.note.note);

    mockAxios.onPost().reply(200, { id: 1 } as Api_EntityNote);
    userEvent.click(getSaveButton());

    expect(closeModal).toBeCalled();

    // TODO: navigate to Notes LIST VIEW - route not implemented yet
    await waitFor(() => expect(history.location.pathname).toBe('/mapview'));

    await waitFor(() => {
      const axiosData: Api_EntityNote = JSON.parse(mockAxios.history.post[0].data);
      const expectedValues = form.toApi();

      expect(mockAxios.history.post[0].url).toBe('/notes/activity');
      expect(axiosData.parent).toEqual(expectedValues.parent);
      expect({ ...axiosData.note, id: undefined, rowVersion: undefined }).toEqual(
        expectedValues.note,
      );
    });
  });

  it('should support adding notes to other entity types', async () => {
    const form = new EntityNoteForm();
    form.parentId = BASIC_PROPS.parentId;
    form.note.note = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByLabelText } = setup({ ...BASIC_PROPS, parentType: 'file' });

    const textarea = await findByLabelText(/Type a note/i);
    userEvent.type(textarea, form.note.note);

    mockAxios.onPost().reply(200, { id: 1 } as Api_EntityNote);
    userEvent.click(getSaveButton());

    expect(closeModal).toBeCalled();

    // TODO: navigate to Notes LIST VIEW - route not implemented yet
    await waitFor(() => expect(history.location.pathname).toBe('/mapview'));

    await waitFor(() => {
      const axiosData: Api_EntityNote = JSON.parse(mockAxios.history.post[0].data);
      const expectedValues = form.toApi();

      expect(mockAxios.history.post[0].url).toBe('/notes/file');
      expect(axiosData.parent).toEqual(expectedValues.parent);
      expect({ ...axiosData.note, id: undefined, rowVersion: undefined }).toEqual(
        expectedValues.note,
      );
    });
  });
});
