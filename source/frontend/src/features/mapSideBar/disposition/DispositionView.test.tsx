import { createMemoryHistory } from 'history';
import React from 'react';
import { Route } from 'react-router-dom';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Claims } from '@/constants/claims';
import { useApiNotes } from '@/hooks/pims-api/useApiNotes';
import { useNoteRepository } from '@/hooks/repositories/useNoteRepository';
import {
  mockDispositionFilePropertyResponse,
  mockDispositionFileResponse,
} from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { rest, server } from '@/mocks/msw/server';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { prettyFormatUTCDate } from '@/utils';
import { act, render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import DispositionView, { IDispositionViewProps } from './DispositionView';

// mock auth library
jest.mock('@react-keycloak/web');
jest.mock('@/components/common/mapFSM/MapStateMachineContext');
jest.mock('@/hooks/repositories/useNoteRepository');
jest.mock('@/hooks/pims-api/useApiNotes');

const getNotes = jest.fn().mockResolvedValue([]);
const onClose = jest.fn();
const onSave = jest.fn();
const onCancel = jest.fn();
const onMenuChange = jest.fn();
const onSuccess = jest.fn();
const onUpdateProperties = jest.fn();
const canRemove = jest.fn();
const setIsEditing = jest.fn();
const onEditFileProperties = jest.fn();

// Need to mock this library for unit tests
jest.mock('react-visibility-sensor', () => {
  return jest.fn().mockImplementation(({ children }) => {
    if (children instanceof Function) {
      return children({ isVisible: true });
    }
    return children;
  });
});

const DEFAULT_PROPS: IDispositionViewProps = {
  onClose,
  onSave,
  onCancel,
  onMenuChange,
  onSuccess,
  onUpdateProperties,
  canRemove,
  isEditing: false,
  setIsEditing,
  onShowPropertySelector: onEditFileProperties,
  formikRef: React.createRef(),
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
    (useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);
    server.use(
      rest.get('/api/users/info/*', (req, res, ctx) =>
        res(ctx.delay(500), ctx.status(200), ctx.json(getUserMock())),
      ),
    );

    (useNoteRepository as jest.Mock).mockImplementation(() => ({
      addNote: { execute: jest.fn() },
      getNote: { execute: jest.fn() },
      updateNote: { execute: jest.fn() },
    }));
    (useApiNotes as jest.Mock).mockImplementation(() => ({
      getNotes,
    }));

    history.replace(`/mapview/sidebar/disposition/1`);
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    const testDispositionFile = mockDispositionFileResponse();

    expect(getByText('Disposition File')).toBeVisible();

    expect(getByText(/FILE_NUMBER 3A8F46B/g)).toBeVisible();
    expect(getByText(prettyFormatUTCDate(testDispositionFile.appCreateTimestamp))).toBeVisible();
  });

  it('should close the form when Close button is clicked', async () => {
    const { getCloseButton, getByText } = await setup();

    expect(getByText('Disposition File')).toBeVisible();
    await waitFor(() => userEvent.click(getCloseButton()));

    expect(onClose).toBeCalled();
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
    expect(await findByText(/Could not find property in the file/)).toBeVisible();
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
});
