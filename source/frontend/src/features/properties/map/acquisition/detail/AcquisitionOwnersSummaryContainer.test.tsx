import { createMemoryHistory } from 'history';
import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from 'mocks/mockAcquisitionFiles';
import { render, RenderOptions } from 'utils/test-utils';

import AcquisitionOwnersSummaryContainer, {
  IAcquisitionOwnersSummaryViewProps,
} from './AcquisitionOwnersSummaryContainer';

const mockApi = {
  error: undefined,
  response: mockAcquisitionFileOwnersResponse(1),
  execute: jest.fn(),
  loading: false,
};

const mockAcquisitionFile = mockAcquisitionFileResponse();

jest.mock('../hooks/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: mockApi,
    };
  },
}));

const history = createMemoryHistory();

// mock auth library
jest.mock('@react-keycloak/web');
const TestView: React.FC<IAcquisitionOwnersSummaryViewProps> = () => {
  return <span>Content Rendered</span>;
};

describe('Acquistion Owners Summary container', () => {
  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <AcquisitionOwnersSummaryContainer
        acquisitionFileId={mockAcquisitionFile.id!}
        View={TestView}
      />,
      {
        ...renderOptions,
        history,
      },
    );
    return { ...utils };
  };

  beforeEach(() => {
    jest.resetAllMocks();
  });

  it('renders the underlying form', () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });
});
