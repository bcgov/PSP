import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { useLeaseDetail } from '@/features/leases';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { mockLastUpdatedBy } from '@/mocks/lastUpdatedBy.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { getMockApiPropertyFiles } from '@/mocks/properties.mock';
import { ApiGen_CodeTypes_FileTypes } from '@/models/api/generated/ApiGen_CodeTypes_FileTypes';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  getMockRepositoryObj,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import {
  ISideBarContextProviderProps,
  SideBarContextProvider,
  TypedFile,
} from '../context/sidebarContext';
import LeaseContainer, { ILeaseContainerProps } from './LeaseContainer';
import { ILeaseViewProps } from './LeaseView';
import { LeaseFileTabNames } from './detail/LeaseFileTabs';

const getLeaseLastUpdatedByMock = getMockRepositoryObj(mockLastUpdatedBy(1));

vi.mock('@/hooks/repositories/useLeaseRepository');
vi.mocked(useLeaseRepository, { partial: true }).mockReturnValue({
  getLastUpdatedBy: getLeaseLastUpdatedByMock,
});

vi.mock('@/features/leases/hooks/useLeaseDetail');
let useLeaseDetailMock: ReturnType<typeof useLeaseDetail>;

const history = createMemoryHistory();
const onClose = vi.fn();

describe('LeaseContainer component', () => {
  let viewProps: ILeaseViewProps;

  const TestView = (props: ILeaseViewProps) => {
    viewProps = props;
    return (
      <Formik innerRef={props.formikRef} onSubmit={vi.fn()} initialValues={{ value: 0 }}>
        {({ values }) => <>{values.value}</>}
      </Formik>
    );
  };

  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<ILeaseContainerProps> } & {
      ctx?: Partial<ISideBarContextProviderProps>;
    } = {},
  ) => {
    const rendered = render(
      <SideBarContextProvider
        file={
          renderOptions.ctx?.file ?? {
            ...getMockApiLease(),
            fileProperties: getMockApiPropertyFiles(),
            fileType: ApiGen_CodeTypes_FileTypes.Lease,
          }
        }
        lastUpdatedBy={renderOptions.ctx?.lastUpdatedBy ?? undefined}
        project={renderOptions.ctx?.project ?? undefined}
      >
        <LeaseContainer
          View={TestView}
          leaseId={renderOptions.props?.leaseId ?? 1}
          onClose={renderOptions.props?.onClose ?? onClose}
        />
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

    // wait for useEffects to complete
    await act(() => {});

    return { ...rendered };
  };

  beforeEach(() => {
    viewProps = undefined;

    useLeaseDetailMock = {
      lease: getMockApiLease(),
      setLease: vi.fn(),
      getCompleteLease: vi.fn().mockResolvedValue(getMockApiLease()),
      refresh: vi.fn(),
      loading: false,
    };
    vi.mocked(useLeaseDetail).mockReturnValue(useLeaseDetailMock);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders a spinner while loading', async () => {
    useLeaseDetailMock.loading = true;
    await setup();
    const spinner = screen.getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders the "draft" markers when the file is opened', async () => {
    const testMockMachine: IMapStateMachineContext = {
      ...mapMachineBaseMock,
    };
    await setup({ mockMapMachine: testMockMachine });
    expect(testMockMachine.setFilePropertyLocations).toHaveBeenCalled();
  });

  it('should refresh the "last-update" info when the file is opened', async () => {
    await setup();
    expect(useLeaseRepository().getLastUpdatedBy.execute).toHaveBeenCalled();
  });

  it('should refresh the lease when loading a different file', async () => {
    const currentFile: TypedFile = {
      ...getMockApiLease(),
      id: 1, // lease with id = 1 is in context
      fileType: ApiGen_CodeTypes_FileTypes.Lease,
    };

    // the container is re-rendered for lease with id = 500 (different than the one in context)
    await setup({
      props: { leaseId: 500 },
      ctx: { file: currentFile },
    });

    expect(useLeaseDetail().getCompleteLease).toHaveBeenCalled();
  });

  it('should change menu index when not editing', async () => {
    await setup();
    await act(async () => viewProps.onSelectProperty(1));
    expect(history.location.pathname).toBe('/mapview/sidebar/lease/1/property/1');
  });

  it('displays a warning if form is dirty and menu index changes', async () => {
    await setup();

    // make changes to the form
    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.formikRef.current.setFieldValue('value', 1));
    expect(await screen.findByText('1')).toBeVisible();
    // try to navigate away
    await act(async () => viewProps.onSelectProperty(1));

    const warning = await screen.findByText(/Confirm Changes/i);
    expect(warning).toBeVisible();
  });

  it('displays a warning if form is dirty and user cancels the form', async () => {
    await setup();

    // make changes to the form
    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.formikRef.current.setFieldValue('value', 1));
    expect(await screen.findByText('1')).toBeVisible();
    // cancel the form
    await act(async () => viewProps.onCancel());

    const warning = await screen.findByText(/Confirm Changes/i);
    expect(warning).toBeVisible();
  });

  it('cancels edit if user confirms modal', async () => {
    await setup();

    // make changes to the form
    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.formikRef.current.setFieldValue('value', 1));
    expect(await screen.findByText('1')).toBeVisible();
    // try to navigate away
    await act(async () => viewProps.onSelectProperty(1));

    const warning = await screen.findByText(/Confirm Changes/i);
    expect(warning).toBeVisible();

    // confirm changes
    const yesButton = await screen.findByTitle('ok-modal');
    await act(async () => userEvent.click(yesButton));

    const params = new URLSearchParams(history.location.search);
    expect(history.location.pathname).toBe('/mapview/sidebar/lease/1/property/1');
    expect(params.has('edit')).toBe(false);
  });

  it('cancels edit if form is not dirty and menu index changes', async () => {
    await setup();

    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.onSelectProperty(1));

    const params = new URLSearchParams(history.location.search);
    expect(history.location.pathname).toBe('/mapview/sidebar/lease/1/property/1');
    expect(params.has('edit')).toBe(false);
  });

  it('displays a warning if form is dirty and user clicks to go back to file summary', async () => {
    await setup();

    // go to property, then edit it
    await act(async () => viewProps.onSelectProperty(1));
    await act(async () => viewProps.setIsEditing(true));
    await act(async () => viewProps.formikRef.current.setFieldValue('value', 1));
    expect(await screen.findByText('1')).toBeVisible();

    // go back to file summary
    await act(async () => viewProps.onSelectFileSummary());

    const warning = await screen.findByText(/Confirm Changes/i);
    expect(warning).toBeVisible();
  });

  it('submits the form when onSave function is called', async () => {
    await setup();
    await act(async () => viewProps.onSave());
    expect(viewProps.formikRef.current?.submitCount).toBe(1);
  });

  it('refetches the lease when onPropertyUpdateSuccess function is called', async () => {
    await setup();
    await act(async () => viewProps.onPropertyUpdateSuccess());
    expect(viewProps.containerState.isEditing).toBe(false);
    expect(useLeaseDetail().refresh).toHaveBeenCalled();
  });

  it('navigates to the property selector route when onEditProperties function is called', async () => {
    await setup();
    await act(async () => viewProps.onEditProperties());
    expect(history.location.pathname).toBe('/mapview/sidebar/lease/1/property/selector');
  });

  it.each([
    [LeaseFileTabNames.notes, true],
    [LeaseFileTabNames.documents, true],
    [LeaseFileTabNames.deposit, true],
    [LeaseFileTabNames.payments, true],
    [LeaseFileTabNames.fileDetails, false],
    [LeaseFileTabNames.checklist, false],
    [LeaseFileTabNames.compensation, false],
    [LeaseFileTabNames.insurance, false],
    [LeaseFileTabNames.surplusDeclaration, false],
  ])(
    'expands the sidebar to full-width for specific tabs - %s',
    async (tabName: LeaseFileTabNames, isFullWidth: boolean) => {
      const testMockMachine: IMapStateMachineContext = {
        ...mapMachineBaseMock,
      };
      await setup({ mockMapMachine: testMockMachine });
      await act(async () => viewProps.setContainerState({ activeTab: tabName }));
      expect(testMockMachine.setFullWidthSideBar).toHaveBeenCalledWith(isFullWidth);
    },
  );
});
