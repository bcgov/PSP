import { createMemoryHistory } from 'history';

import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_ExportProjectFilter } from '@/models/api/ProjectFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import ProjectExportContainer, { ISideProjectContainerProps } from './ProjectExportContainer';
import { IProjectExportFormProps } from './ProjectExportForm';

const history = createMemoryHistory();

jest.mock('@react-keycloak/web');

let viewProps: IProjectExportFormProps = {} as any;
const ProjectExportContainerView = (props: IProjectExportFormProps) => {
  viewProps = props;
  return <></>;
};

jest.mock('@/hooks/repositories/useAcquisitionProvider');
const getAllAcquisitionFileTeamMembers = jest.fn();
const getAgreementsReport = jest.fn();
(useAcquisitionProvider as jest.MockedFunction<typeof useAcquisitionProvider>).mockReturnValue({
  getAllAcquisitionFileTeamMembers: {
    execute: getAllAcquisitionFileTeamMembers as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
  getAgreementsReport: {
    execute: getAgreementsReport as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
} as ReturnType<typeof useAcquisitionProvider>);

jest.mock('@/hooks/repositories/useProjectProvider');
const getAllProjects = jest.fn();
(useProjectProvider as jest.MockedFunction<typeof useProjectProvider>).mockReturnValue({
  getAllProjects: {
    execute: getAllProjects as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
} as ReturnType<typeof useProjectProvider>);

describe('ProjectExportForm component', () => {
  // render component under test
  const setup = (props: ISideProjectContainerProps, renderOptions: RenderOptions = {}) => {
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
    jest.clearAllMocks();
  });

  it('does not load project and team member information by default', () => {
    setup({} as any);

    expect(getAllAcquisitionFileTeamMembers).not.toHaveBeenCalled();
    expect(getAllProjects).not.toHaveBeenCalled();
  });

  it('loads project and team member information when onExportTypeSelected is called', () => {
    setup({} as any);
    act(() => viewProps.onExportTypeSelected());

    expect(getAllAcquisitionFileTeamMembers).toHaveBeenCalled();
    expect(getAllProjects).toHaveBeenCalled();
  });

  it('requests project export form when export function called', () => {
    setup({} as any);
    act(() => viewProps.onExport({ type: 'AGREEMENT' } as Api_ExportProjectFilter));

    expect(getAgreementsReport).toHaveBeenCalled();
  });
});
