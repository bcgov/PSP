import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

import { Claims } from '@/constants/claims';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import {
  mockDispositionFilePropertyResponse,
  mockDispositionFileResponse,
} from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { rest, server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { prettyFormatUTCDate } from '@/utils';
import { act, cleanup, render, RenderOptions, userEvent, screen } from '@/utils/test-utils';

import DispositionView, { IDispositionViewProps } from './DispositionView';
import { useApiProperties } from '@/hooks/pims-api/useApiProperties';
import { ApiGen_Base_Page } from '@/models/api/generated/ApiGen_Base_Page';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { vi } from 'vitest';
import { useLtsa } from '@/hooks/useLtsa';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { createRef } from 'react';

// mock auth library

vi.mock('@/hooks/repositories/useNoteRepository');
vi.mock('@/hooks/pims-api/useApiNotes');

const getNotes = vi.fn().mockResolvedValue([]);
const onClose = vi.fn();
const onSave = vi.fn();
const onCancel = vi.fn();
const onMenuChange = vi.fn();
const onSuccess = vi.fn();
const onUpdateProperties = vi.fn();
const confirmBeforeAdd = vi.fn();
const canRemove = vi.fn();
const setIsEditing = vi.fn();
const onEditFileProperties = vi.fn();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

vi.mock('@/hooks/repositories/useComposedProperties', () => {
  return {
    useComposedProperties: vi.fn().mockResolvedValue({ apiWrapper: { response: {} } }),
    PROPERTY_TYPES: {},
  };
});

vi.mock('@/hooks/pims-api/useApiProperties');
vi.mocked(useApiProperties).mockReturnValue({
  getPropertiesViewPagedApi: vi
    .fn()
    .mockResolvedValue({ data: {} as ApiGen_Base_Page<ApiGen_Concepts_Property> }),
  getMatchingPropertiesApi: vi.fn(),
  getPropertyAssociationsApi: vi.fn(),
  exportPropertiesApi: vi.fn(),
  getPropertiesApi: vi.fn(),
  getPropertyConceptWithIdApi: vi.fn(),
  putPropertyConceptApi: vi.fn(),
  getPropertyConceptWithPidApi: vi.fn(),
  getPropertyConceptWithPinApi: vi.fn(),
});

vi.mock('@/hooks/useLtsa');
vi.mocked(useLtsa).mockImplementation(
  () =>
    ({
      execute: vi.fn(),
    } as unknown as ReturnType<typeof useLtsa>),
);

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider).mockReturnValue({
  retrieveProjectProducts: vi.fn(),
} as unknown as ReturnType<typeof useProjectProvider>);

const DEFAULT_PROPS: IDispositionViewProps = {
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onSuccess,
  onUpdateProperties,
  confirmBeforeAdd,
  canRemove,
  isEditing: false,
  setIsEditing,
  onShowPropertySelector: onEditFileProperties,
  formikRef: createRef(),
  isFormValid: true,
  error: undefined,
};

const history = createMemoryHistory();

describe('DispositionView component', () => {
  // render component under test
  const setup = async (
    props: IDispositionViewProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path="/mapview/sidebar/disposition/:id">
        <DispositionView
          {...{
            ...props,
            dispositionFile: {
              ...mockDispositionFileResponse(),
              fileProperties: mockDispositionFilePropertyResponse() as any,
            },
          }}
        />
      </Route>,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        history,
        ...renderOptions,
      },
    );

    return {
      ...utils,
      getCloseButton: () => utils.getByTitle('close'),
    };
  };

  beforeEach(() => {
    server.use(
      rest.get('/api/users/info/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock())),
      ),
    );

    vi.mocked(useNoteRepository).mockImplementation(
      () =>
        ({
          addNote: { execute: vi.fn() },
          getNote: { execute: vi.fn() },
          updateNote: { execute: vi.fn() },
        } as unknown as ReturnType<typeof useNoteRepository>),
    );
    vi.mocked(useApiNotes).mockImplementation(
      () =>
        ({
          getNotes,
        } as unknown as ReturnType<typeof useApiNotes>),
    );

    history.replace(`/mapview/sidebar/disposition/1`);
  });

  afterEach(() => {
    vi.clearAllMocks();
    cleanup();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    const testDispositionFile = mockDispositionFileResponse();

    expect(getByText('Disposition File')).toBeVisible();

    expect(getByText(/FILE_NUMBER 3A8F46B/i)).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))).toBeVisible();
  });

  it('should display the Edit Properties button if the user has permissions', async () => {
    const { getByTitle } = await setup(undefined, { claims: [Claims.DISPOSITION_EDIT] });
    expect(getByTitle(/Change properties/)).toBeVisible();
  });

  it('should not display the Edit Properties button if the user does not have permissions', async () => {
    const { queryByTitle } = await setup(undefined, { claims: [] });
    expect(queryByTitle('Change properties')).toBeNull();
  });

  it('should display the notes tab if the user has permissions', async () => {
    const { getAllByText } = await setup(undefined, { claims: [Claims.NOTE_VIEW] });
    expect(getAllByText(/Notes/)[0]).toBeVisible();
  });

  it('should not display the notes tab if the user does not have permissions', async () => {
    const { queryByText } = await setup(undefined, { claims: [] });
    expect(queryByText('Notes')).toBeNull();
  });

  it('should display the File Details tab by default', async () => {
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should show a toast and redirect to the File Details page when accessing a non-existing property index`, async () => {
    history.replace(`/mapview/sidebar/disposition/1/property/99999`);
    const { getByRole, findByText } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
    // toast
    expect(await screen.findByText(/Could not find property in the file/i)).toBeVisible();
  });

  it('should display the Property Selector according to routing', async () => {
    history.replace(`/mapview/sidebar/disposition/1/property/selector`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Locate on Map/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it('should display the Property Details tab according to routing', async () => {
    history.replace(`/mapview/sidebar/disposition/1/property/1`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the File Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/disposition/1/blahblahtab?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /File details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display the Property Details tab when we are editing and the path doesn't match any route`, async () => {
    history.replace(`/mapview/sidebar/disposition/1/property/1/unknownTabWhatIsThis?edit=true`);
    const { getByRole } = await setup();
    const tab = getByRole('tab', { name: /Property Details/i });
    expect(tab).toBeVisible();
    expect(tab).toHaveClass('active');
  });

  it(`should display an error message when the error prop is set.`, async () => {
    const { getByText } = await setup({ ...DEFAULT_PROPS, error: {} } as any);
    expect(
      getByText(
        'Failed to load Disposition File. Check the detailed error in the top right for more details.',
      ),
    ).toBeVisible();
  });

  it(`should display property edit title when editing`, async () => {
    history.replace(`/mapview/sidebar/disposition/1?edit=true`);
    const { getByText } = await setup({ ...DEFAULT_PROPS, isEditing: true } as any);
    expect(getByText('Update Disposition File')).toBeVisible();
  });

  it(`should display property edit title when editing properties`, async () => {
    history.replace(`/mapview/sidebar/disposition/4/property/selector`);
    const { getByText } = await setup({ ...DEFAULT_PROPS, isEditing: true } as any);
    expect(getByText('Property selection')).toBeVisible();
  });

  it(`should display property edit title when editing and on property tab`, async () => {
    history.replace(`/mapview/sidebar/disposition/1/property/1?edit=true`);
    const { getByText } = await setup({ ...DEFAULT_PROPS, isEditing: true } as any);
    expect(getByText('Update Property File Data')).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText } = await setup();

    expect(getByText('Disposition File')).toBeVisible();
    await act(async () => userEvent.click(getCloseButton()));

    expect(onClose).toBeCalled();
  });
});
