import { createMemoryHistory } from 'history';
import fileDownload from 'js-file-download';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, screen } from '@/utils/test-utils';

import ProjectExportContainer from './ProjectExportContainer';
import { IProjectExportFormProps } from './ProjectExportForm';

const history = createMemoryHistory();

let viewProps: IProjectExportFormProps = {} as any;
const ProjectExportContainerView = (props: IProjectExportFormProps) => {
  viewProps = props;
  return <></>;
};

const defaultRepositoryResponse = () => ({
  execute: vi.fn(),
  response: {} as any,
  error: undefined,
  status: undefined,
  loading: false,
});

vi.mock('@/hooks/repositories/useAcquisitionProvider');
vi.mocked(useAcquisitionProvider).mockReturnValue({
  getAllAcquisitionFileTeamMembers: { ...defaultRepositoryResponse() },
  getAgreementsReport: { ...defaultRepositoryResponse() },
  getCompensationReport: { ...defaultRepositoryResponse() },
} as unknown as ReturnType<typeof useAcquisitionProvider>);

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider).mockReturnValue({
  getAllProjects: { ...defaultRepositoryResponse() },
} as unknown as ReturnType<typeof useProjectProvider>);

vi.mock('js-file-download', () => {
  return {
    __esModule: true,
    default: vi.fn(),
  };
});

describe('ProjectExportContainer component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(<ProjectExportContainer View={ProjectExportContainerView} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
      claims: [],
      history,
    });

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('does not load project and team member information by default', () => {
    setup();

    const { getAllAcquisitionFileTeamMembers } = useAcquisitionProvider();
    const { getAllProjects } = useProjectProvider();
    expect(getAllAcquisitionFileTeamMembers.execute).not.toHaveBeenCalled();
    expect(getAllProjects.execute).not.toHaveBeenCalled();
  });

  it('loads project and team member information when onExportTypeSelected is called', () => {
    setup();
    act(() => viewProps.onExportTypeSelected());

    const { getAllAcquisitionFileTeamMembers } = useAcquisitionProvider();
    const { getAllProjects } = useProjectProvider();
    expect(getAllAcquisitionFileTeamMembers.execute).toHaveBeenCalled();
    expect(getAllProjects.execute).toHaveBeenCalled();
  });

  it('requests agreement export when export function called', async () => {
    setup();
    await act(async () => viewProps.onExport({ type: 'AGREEMENT' } as Api_ExportProjectFilter));

    const { getAgreementsReport } = useAcquisitionProvider();
    expect(getAgreementsReport.execute).toHaveBeenCalled();
  });

  it('displays warning when there are no records to be returned by agreement export function', async () => {
    vi.mocked(useAcquisitionProvider).mockReturnValue({
      ...useAcquisitionProvider(),
      getAgreementsReport: {
        ...defaultRepositoryResponse(),
        status: 204, // API returns 204 No Content when no records were found
      },
    } as unknown as ReturnType<typeof useAcquisitionProvider>);

    setup();
    await act(async () => viewProps.onExport({ type: 'AGREEMENT' } as Api_ExportProjectFilter));

    const { getAgreementsReport } = useAcquisitionProvider();
    expect(getAgreementsReport.execute).toHaveBeenCalled();
    expect(
      await screen.findByText(/There is no data for the input parameters you entered/i),
    ).toBeVisible();
  });

  it('requests compensation export when export function called', async () => {
    setup();
    await act(async () => viewProps.onExport({ type: 'COMPENSATION' } as Api_ExportProjectFilter));

    const { getCompensationReport } = useAcquisitionProvider();
    expect(getCompensationReport.execute).toHaveBeenCalled();
  });

  it('displays warning when there are no records to be returned by export function', async () => {
    vi.mocked(useAcquisitionProvider).mockReturnValue({
      ...useAcquisitionProvider(),
      getCompensationReport: {
        ...defaultRepositoryResponse(),
        status: 204, // API returns 204 No Content when no records were found
      },
    } as unknown as ReturnType<typeof useAcquisitionProvider>);

    setup();
    await act(async () => viewProps.onExport({ type: 'COMPENSATION' } as Api_ExportProjectFilter));

    const { getCompensationReport } = useAcquisitionProvider();
    expect(getCompensationReport.execute).toHaveBeenCalled();
    expect(
      await screen.findByText(/There is no data for the input parameters you entered/i),
    ).toBeVisible();
  });

  it('triggers a file download of generated excel file', async () => {
    vi.mocked(useAcquisitionProvider).mockReturnValue({
      ...useAcquisitionProvider(),
      getCompensationReport: {
        ...defaultRepositoryResponse(),
        status: 200,
      },
    } as unknown as ReturnType<typeof useAcquisitionProvider>);

    setup();
    await act(async () => viewProps.onExport({ type: 'COMPENSATION' } as Api_ExportProjectFilter));

    const { getCompensationReport } = useAcquisitionProvider();
    expect(getCompensationReport.execute).toHaveBeenCalled();
    expect(fileDownload).toHaveBeenCalled();
  });
});
