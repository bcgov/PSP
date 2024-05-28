import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { useDocumentGenerationRepository } from '@/features/documents/hooks/useDocumentGenerationRepository';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { mockLastUpdatedBy } from '@/mocks/lastUpdatedBy.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { mockNotesResponse } from '@/mocks/noteResponses.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  render,
  RenderOptions,
  screen,
  waitFor,
  waitForElementToBeRemoved,
} from '@/utils/test-utils';

import { SideBarContextProvider } from '../context/sidebarContext';
import { AcquisitionContainer, IAcquisitionContainerProps } from './AcquisitionContainer';
import { IAcquisitionViewProps } from './AcquisitionView';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const generateFn = vi.fn();

// mock auth library

vi.mock('@/features/documents/hooks/useDocumentGenerationRepository');
vi.mocked(useDocumentGenerationRepository).mockImplementation(
  () =>
    ({
      generateDocumentDownloadWrappedRequest: generateFn,
    } as unknown as ReturnType<typeof useDocumentGenerationRepository>),
);

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

let viewProps: IAcquisitionViewProps = {} as any;
const AcquisitionContainerView = (props: IAcquisitionViewProps) => {
  viewProps = props;
  return (
    <Formik innerRef={props.formikRef} onSubmit={noop} initialValues={{ value: 0 }}>
      {({ values }) => <>{values.value}</>}
    </Formik>
  );
};
const DEFAULT_PROPS: IAcquisitionContainerProps = {
  acquisitionFileId: 1,
  onClose,
  View: AcquisitionContainerView,
};

describe('AcquisitionContainer component', () => {
  // render component under test
  const setup = (
    props: IAcquisitionContainerProps = { ...DEFAULT_PROPS },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <SideBarContextProvider>
        <AcquisitionContainer {...props} View={AcquisitionContainerView} />
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
      .onGet(new RegExp('acquisitionfiles/1/properties'))
      .reply(200, mockAcquisitionFileResponse().fileProperties);
    mockAxios.onGet(new RegExp('acquisitionfiles/1/updateInfo')).reply(200, mockLastUpdatedBy(1));
    mockAxios.onGet(new RegExp('acquisitionfiles/*')).reply(200, mockAcquisitionFileResponse());
    mockAxios.onGet(new RegExp('notes/*')).reply(200, mockNotesResponse());
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

    mockAxios.onGet(new RegExp('acquisitionfiles/1/properties')).timeout();
    await act(async () => viewProps.onShowPropertySelector());
    await act(async () => {
      viewProps.canRemove(1);
    });
    expect(spinner).not.toBeVisible();
  });

  it('canRemove returns true if file property has no associated entities', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    mockAxios.onGet(new RegExp('acquisitionfiles/1/properties')).reply(200, [
      {
        id: 1,
        isDisabled: false,
        property: {
          id: 1,
        },
      },
    ]);
    await act(async () => viewProps.onShowPropertySelector());
    const canRemoveResponse = await viewProps.canRemove(1);
    expect(canRemoveResponse).toBe(true);
  });

  it('should change menu index when not editing', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.onMenuChange(1));
    expect(history.location.pathname).toBe('/property/1');
  });

  it('displays a warning if form is dirty and menu index changes', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.setIsEditing(true));
    await act(async () => (viewProps.formikRef.current as any).setFieldValue('value', 1));
    await screen.findByText('1');
    await act(async () => viewProps.onMenuChange(1));

    expect(history.location.pathname).toBe('/property/1');
    const params = new URLSearchParams(history.location.search);
    expect(params.has('edit')).toBe(true);
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

  it('on success function refetches acq file', async () => {
    const { getByTestId } = setup(undefined, { claims: [] });
    vi.spyOn(global, 'confirm' as any).mockReturnValueOnce(true);

    const spinner = getByTestId('filter-backdrop-loading');
    await waitForElementToBeRemoved(spinner);

    await act(async () => viewProps.onSuccess());

    expect(mockAxios.history.get.filter(x => x.url === '/acquisitionfiles/1')).toHaveLength(2);
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
