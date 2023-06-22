import { Claims } from '@/constants/index';
import { useApiUsers } from '@/hooks/pims-api/useApiUsers';
import {
  mockAcquisitionFileChecklistResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import {
  AcquisitionChecklistView,
  IAcquisitionChecklistViewProps,
} from './AcquisitionChecklistView';

// mock auth library
jest.mock('@react-keycloak/web');

// mock API service calls
jest.mock('@/hooks/pims-api/useApiUsers');

(useApiUsers as jest.MockedFunction<typeof useApiUsers>).mockReturnValue({
  getUserInfo: jest.fn().mockResolvedValue({}),
} as any);

const mockViewProps: IAcquisitionChecklistViewProps = {
  acquisitionFile: undefined,
  onEdit: jest.fn(),
};

describe('AcquisitionChecklistView component', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionChecklistView
        acquisitionFile={mockViewProps.acquisitionFile}
        onEdit={mockViewProps.onEdit}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    mockViewProps.acquisitionFile = mockAcquisitionFileResponse();
    mockViewProps.acquisitionFile.acquisitionFileChecklist = mockAcquisitionFileChecklistResponse();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the edit button for users with acquisition edit permissions', () => {
    const { getByTitle } = setup({ claims: [Claims.ACQUISITION_EDIT] });
    const editButton = getByTitle('Edit acquisition checklist');
    expect(editButton).toBeVisible();
    userEvent.click(editButton);
    expect(mockViewProps.onEdit).toHaveBeenCalled();
  });

  it('does not render the edit button for users that do not have acquisition edit permissions', () => {
    const { queryByTitle } = setup({ claims: [] });
    const editResearchFile = queryByTitle('Edit acquisition checklist');
    expect(editResearchFile).toBeNull();
  });

  it('renders last updated by and last updated on for the overall checklist', () => {
    const { getByText } = setup();
    expect(getByText(/This checklist was last updated Mar 17, 2023 by/i)).toBeVisible();
  });
});
