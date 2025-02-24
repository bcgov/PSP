import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';

import { Claims } from '@/constants/index';
import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiNote } from '@/mocks/noteResponses.mock';
import { ApiGen_Concepts_Note } from '@/models/api/generated/ApiGen_Concepts_Note';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { INoteDetailsFormModalProps, NoteDetailsFormModal } from './NoteDetailsFormModal';

// mock auth library

const mockAxios = new MockAdapter(axios);

const onEdit = vi.fn();
const onClose = vi.fn();

const BASIC_PROPS: INoteDetailsFormModalProps = {
  isOpened: true,
  loading: false,
  note: getMockApiNote(1),
  onCloseClick: onClose,
  onEdit,
};

describe('NoteDetailsFormModal component', () => {
  // render component under test
  const setup = (
    props: INoteDetailsFormModalProps = { ...BASIC_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(<NoteDetailsFormModal {...props} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      useMockAuthentication: true,
    });

    return {
      ...utils,
      getEditButton: () => utils.getByRole('button', { name: /edit/i }),
      getModalCloseButton: () => utils.getByTitle('ok-modal'),
    };
  };

  beforeEach(() => {
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
  });

  afterEach(() => {
    mockAxios.reset();
    vi.resetAllMocks();
  });

  it('renders as expected', () => {
    setup();
    expect(document.body).toMatchSnapshot();
  });

  it('renders a spinner while loading', () => {
    const { getByTestId } = setup({ ...BASIC_PROPS, loading: true });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders the Note field', () => {
    const { getByTitle } = setup();
    const textarea = getByTitle(/Note/i);

    expect(textarea).toBeVisible();
    expect(textarea.tagName).toBe('TEXTAREA');
    expect(textarea).toHaveAttribute('readonly');
  });

  it('should execute callback when Close button is clicked', async () => {
    const { getModalCloseButton } = setup();
    await act(async () => userEvent.click(getModalCloseButton()));

    expect(onClose).toBeCalled();
  });

  it(`should not display the Last Updated info for system-generated notes`, async () => {
    const systemNote: ApiGen_Concepts_Note = {
      ...getMockApiNote(1),
      isSystemGenerated: true,
    };
    const { queryByText } = setup({ ...BASIC_PROPS, note: systemNote }, { claims: [] });
    const lastUpdated = queryByText(/last updated/i);
    expect(lastUpdated).toBeNull();
  });

  it(`should not render Edit button when user doesn't have edit permissions`, async () => {
    const { queryByRole } = setup({ ...BASIC_PROPS }, { claims: [] });
    const editNote = queryByRole('button', { name: /edit/i });
    expect(editNote).toBeNull();
  });

  it('should render Edit button when user has edit permissions', async () => {
    const { getEditButton } = setup({ ...BASIC_PROPS }, { claims: [Claims.NOTE_EDIT] });
    expect(getEditButton()).toBeVisible();
  });

  it('should execute callback when Edit button is clicked', async () => {
    const { getEditButton } = setup({ ...BASIC_PROPS }, { claims: [Claims.NOTE_EDIT] });
    await act(async () => userEvent.click(getEditButton()));

    expect(onEdit).toBeCalledWith(getMockApiNote(1));
    expect(onClose).not.toBeCalled();
  });
});
