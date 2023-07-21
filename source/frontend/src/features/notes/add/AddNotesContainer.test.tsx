import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { createMemoryHistory } from 'history';

import { NoteTypes } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockEntityNote } from '@/mocks/noteResponses.mock';
import { Api_EntityNote } from '@/models/api/Note';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { AddNotesContainer, IAddNotesContainerProps } from './AddNotesContainer';
import { EntityNoteForm } from './models';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);

const openModal = jest.fn();
const closeModal = jest.fn();
const onSuccess = jest.fn();

const BASIC_PROPS: IAddNotesContainerProps = {
  isOpened: true,
  openModal,
  closeModal,
  parentId: 1,
  type: NoteTypes.Activity,
  onSuccess,
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

    const modalTitle = getByText(/Notes/i);
    const textarea = getByLabelText(/Type a note/i);

    expect(modalTitle).toBeVisible();
    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
  });

  it('should cancel form when Cancel button is clicked', async () => {
    const { getCancelButton, getByText } = setup();

    expect(getByText(/Notes/i)).toBeVisible();
    await act(async () => {
      userEvent.click(getCancelButton());
    });

    expect(closeModal).toBeCalled();
  });

  it('should save the form and close the modal when Submit button is clicked', async () => {
    const formValues = new EntityNoteForm();
    formValues.parentId = BASIC_PROPS.parentId;
    formValues.note.note = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByLabelText } = setup(BASIC_PROPS);

    const textarea = await findByLabelText(/Type a note/i);
    act(() => {
      userEvent.type(textarea, formValues.note.note as string);
    });

    mockAxios.onPost().reply(200, mockEntityNote(1));
    await act(async () => {
      userEvent.click(getSaveButton());
    });

    expect(closeModal).toBeCalled();
    expect(onSuccess).toBeCalled();

    const axiosData: Api_EntityNote = JSON.parse(mockAxios.history.post[0].data);
    const expectedValues = formValues.toApi();

    expect(mockAxios.history.post[0].url).toBe('/notes/activity');
    expect(axiosData.parent).toEqual(expectedValues.parent);
    expect({ ...axiosData.note, id: undefined, rowVersion: undefined }).toEqual(
      expectedValues.note,
    );
  });

  it('should support adding notes to other entity types', async () => {
    const formValues = new EntityNoteForm();
    formValues.parentId = BASIC_PROPS.parentId;
    formValues.note.note = 'Lorem ipsum dolor sit amet, consectetur adipiscing elit.';

    const { getSaveButton, findByLabelText } = setup({
      ...BASIC_PROPS,
      type: NoteTypes.Acquisition_File,
    });

    const textarea = await findByLabelText(/Type a note/i);
    act(() => {
      userEvent.type(textarea, formValues.note.note as string);
    });

    mockAxios.onPost().reply(200, mockEntityNote(1));
    await act(async () => {
      userEvent.click(getSaveButton());
    });

    expect(closeModal).toBeCalled();
    expect(onSuccess).toBeCalled();
    expect(mockAxios.history.post[0].url).toBe('/notes/acquisition_file');
  });
});
