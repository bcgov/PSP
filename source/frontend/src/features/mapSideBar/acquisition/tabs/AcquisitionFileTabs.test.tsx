import { createMemoryHistory } from 'history';
import { Route } from 'react-router-dom';

import Claims from '@/constants/claims';
import { FileTabType } from '@/features/mapSideBar/shared/detail/FileTabs';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import AcquisitionFileTabs, { IAcquisitionFileTabsProps } from './AcquisitionFileTabs';

const history = createMemoryHistory();
const setIsEditing = vi.fn();

vi.mock('@/hooks/pims-api/useApiContacts');
const getPersonConceptFn = vi.fn();
vi.mocked(useApiContacts).mockImplementation(
  () =>
    ({
      getPersonConcept: getPersonConceptFn,
    } as any),
);

vi.mock('@/hooks/repositories/useAcquisitionProvider');
const getAcquisitionFileOwnersFn = vi.fn();
vi.mocked(useAcquisitionProvider).mockReturnValue({
  getAcquisitionOwners: {
    execute: getAcquisitionFileOwnersFn as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
  getAcquisitionSubFiles: {
    execute: vi.fn() as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
  getAcquisitionFile: {
    execute: vi.fn() as any,
    error: undefined,
    loading: false,
    response: undefined,
  },
} as ReturnType<typeof useAcquisitionProvider>);

vi.mock('@/features/documents/hooks/useDocumentRelationshipProvider', () => ({
  useDocumentRelationshipProvider: () => {
    return {
      retrieveDocumentRelationship: vi.fn(),
      retrieveDocumentRelationshipLoading: false,
    };
  },
}));

vi.mock('@/features/documents/hooks/useDocumentProvider', () => ({
  useDocumentProvider: () => {
    return {
      getDocumentRelationshipTypes: vi.fn(),
      getDocumentRelationshipTypesLoading: false,
      getDocumentTypes: vi.fn(),
      getDocumentTypesLoading: false,
    };
  },
}));

describe('AcquisitionFileTabs component', () => {
  // render component under test
  const setup = (
    props: Omit<IAcquisitionFileTabsProps, 'setIsEditing'>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Route path="/blah/:tab">
        <AcquisitionFileTabs
          acquisitionFile={props.acquisitionFile}
          defaultTab={props.defaultTab}
          setIsEditing={setIsEditing}
        />
      </Route>,
      {
        useMockAuthentication: true,
        history,
        ...renderOptions,
      },
    );

    return { ...utils };
  };

  beforeEach(() => {
    history.replace(`/blah/${FileTabType.FILE_DETAILS}`);
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('has a documents tab', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );
    await act(async () => {});

    const tab = getByText('Documents');
    expect(tab).toBeVisible();
  });

  it('documents tab can be changed to', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW] },
    );

    const tab = getByText('Documents');
    await act(async () => userEvent.click(tab));

    expect(getByText('Documents')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.DOCUMENTS}`);
  });

  it('hides the expropriation tab when the Acquisition file type is "Consensual Agreement"', async () => {
    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFileResponse(),
      defaultTab: FileTabType.FILE_DETAILS,
    });
    await act(async () => {});

    const expropriationButton = queryByText('Expropriation');
    expect(expropriationButton).not.toBeInTheDocument();
  });

  it('shows the expropriation tab when the Acquisition file type is "Section 3"', async () => {
    const mockAcquisitionFile = mockAcquisitionFileResponse();
    mockAcquisitionFile.acquisitionTypeCode = {
      id: 'SECTN3',
      description: 'Section 3 Agreement',
      isDisabled: false,
      displayOrder: null,
    };

    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFile,
      defaultTab: FileTabType.FILE_DETAILS,
    });
    await act(async () => {});

    const editButton = queryByText('Expropriation');
    expect(editButton).toBeInTheDocument();
  });

  it('shows the expropriation tab when the Acquisition file type is "Section 6"', async () => {
    const mockAcquisitionFile = mockAcquisitionFileResponse();
    mockAcquisitionFile.acquisitionTypeCode = {
      id: 'SECTN6',
      description: 'Section 6 Expropriation',
      isDisabled: false,
      displayOrder: null,
    };

    const { queryByText } = setup({
      acquisitionFile: mockAcquisitionFile,
      defaultTab: FileTabType.FILE_DETAILS,
    });
    await act(async () => {});

    const editButton = queryByText('Expropriation');
    expect(editButton).toBeInTheDocument();
  });

  it('has a sub-files tab', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW, Claims.ACQUISITION_ADD] },
    );
    await act(async () => {});

    const tab = getByText('Sub-Files');
    expect(tab).toBeVisible();
  });

  it('sub-files tab can be changed to', async () => {
    const { getByText } = setup(
      {
        acquisitionFile: mockAcquisitionFileResponse(),
        defaultTab: FileTabType.FILE_DETAILS,
      },
      { claims: [Claims.DOCUMENT_VIEW, Claims.ACQUISITION_ADD] },
    );

    const tab = getByText('Sub-Files');
    await act(async () => userEvent.click(tab));

    expect(getByText('Sub-Files')).toHaveClass('active');
    expect(history.location.pathname).toBe(`/blah/${FileTabType.SUB_FILES}`);
  });
});
