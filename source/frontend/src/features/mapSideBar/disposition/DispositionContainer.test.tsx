import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import {
  mockDispositionFilePropertyResponse,
  mockDispositionFileResponse,
} from '@/mocks/dispositionFiles.mock';
import { mockLastUpdatedBy } from '@/mocks/lastUpdatedBy.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fireEvent,
  render,
  RenderOptions,
  screen,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import DispositionContainer, { IDispositionContainerProps } from './DispositionContainer';
import { IDispositionViewProps } from './DispositionView';
import { ApiGen_Concepts_File } from '@/models/api/generated/ApiGen_Concepts_File';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const mockDispositionFileApi = mockDispositionFileResponse();

// mock auth library

const onClose = vi.fn();

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

let viewProps!: IDispositionViewProps;
const DispositionContainerView = (props: IDispositionViewProps) => {
  viewProps = props;
  return (
    <Formik innerRef={props.formikRef} onSubmit={noop} initialValues={{ value: 0 }}>
      {({ values }) => <>{values.value}</>}
    </Formik>
  );
};
const DEFAULT_PROPS: IDispositionContainerProps = {
  dispositionFileId: 1,
  onClose,
  View: DispositionContainerView,
};

describe('DispositionContainer component', () => {
  // render component under test
  const setup = (
    props: IDispositionContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <SideBarContextProvider>
        <DispositionContainer {...props} View={DispositionContainerView} />
      </SideBarContextProvider>,
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
    mockAxios.onGet(new RegExp('users/info/*')).reply(200, {});
    mockAxios
      .onGet(new RegExp('dispositionfiles/1/properties'))
      .reply(200, mockDispositionFilePropertyResponse());
    mockAxios
      .onPut(new RegExp('dispositionfiles/1/properties'))
      .reply(200, mockDispositionFilePropertyResponse());
    mockAxios.onGet(new RegExp('dispositionfiles/1/updateInfo')).reply(200, mockLastUpdatedBy(1));
    mockAxios.onGet(new RegExp('dispositionfiles/1')).reply(200, mockDispositionFileApi);
  });

  afterEach(() => {
    mockAxios.resetHistory();
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment, getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    expect(asFragment()).toMatchSnapshot();
  });

  it('renders a spinner while loading', async () => {
    const { getByTestId } = setup();

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();

    await waitForElementToBeRemoved(spinner);
  });

  it('should not display the spinner when properties are loading and the property selector is being displayed', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    mockAxios.onGet(new RegExp('dispositionfiles/1/properties')).timeout();
    await act(async () => viewProps.onShowPropertySelector());
    await act(async () => {
      viewProps.canRemove(1);
    });
    expect(spinner).not.toBeVisible();
  });

  it('should call the onUpdateProperty when related function is called', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => {
      await viewProps.onUpdateProperties(mockDispositionFileApi);
    });
    expect(spinner).not.toBeVisible();
    expect(
      mockAxios.history.put.filter(x => x.url === '/dispositionfiles/1/properties?'),
    ).toHaveLength(1);
  });

  it('should show error popup when user adds a property outside of the user account regions', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    const errorMessage = 'You cannot add a property that is outside of your user account region';
    mockAxios
      .onPut(new RegExp('dispositionfiles/1/properties'))
      .reply(400, { error: errorMessage });
    await act(async () => {
      await viewProps.onUpdateProperties(mockDispositionFileApi);
    });
    expect(spinner).not.toBeVisible();
    expect(await screen.findByText(errorMessage)).toBeVisible();
  });

  it('should change menu index when not editing', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.onMenuChange(1));
    expect(history.location.pathname).toBe('/property/1');
  });

  it('displays a warning if form is dirty and menu index changes', async () => {
    const { getByTestId, getByText } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.setIsEditing(true));
    await act(async () => (viewProps.formikRef.current as any).setFieldValue('value', 1));
    await screen.findByText('1');
    await act(async () => viewProps.onMenuChange(1));

    await waitFor(() => {
      const warning = getByText(/Confirm Changes/i);
      expect(warning).toBeVisible();
    });
  });

  it('Cancels edit if user confirms modal', async () => {
    const { getByTestId, getByText } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.setIsEditing(true));
    await act(async () => (viewProps.formikRef.current as any).setFieldValue('value', 1));
    await screen.findByText('1');
    await act(async () => viewProps.onMenuChange(1));

    expect(history.location.pathname).toBe('/property/1');

    const yesButton = getByText('Yes');
    await act(async () => {
      fireEvent.click(yesButton);
    });
    const params = new URLSearchParams(history.location.search);
    await waitFor(async () => expect(params.has('edit')).toBe(false));
  });

  it('cancels edit if form is not dirty and menu index changes', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.onMenuChange(1));

    const params = new URLSearchParams(history.location.search);
    expect(params.has('edit')).toBe(false);
  });

  it('on success function refetches disposition file', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.onSuccess(true, true));

    expect(mockAxios.history.get.filter(x => x.url === '/dispositionfiles/1')).toHaveLength(2);
  });

  it('on success function cancels edit', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.onSuccess());

    const params = new URLSearchParams(history.location.search);
    expect(params.has('edit')).toBe(false);
  });

  it('on save function submits the form', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.onSave());

    await waitFor(async () => viewProps.formikRef.current?.submitCount === 1);
  });
});
