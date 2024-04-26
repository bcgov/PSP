import { createMemoryHistory } from 'history';

import {
  mockAcquisitionFileOwnersResponse,
  mockAcquisitionFileResponse,
} from '@/mocks/acquisitionFiles.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import AcquisitionOwnersSummaryContainer, {
  IAcquisitionOwnersSummaryViewProps,
} from './AcquisitionOwnersSummaryContainer';

const mockApi = {
  error: undefined,
  response: mockAcquisitionFileOwnersResponse(1),
  execute: vi.fn(),
  loading: false,
};

const mockAcquisitionFile = mockAcquisitionFileResponse();

vi.mock('@/hooks/repositories/useAcquisitionProvider', () => ({
  useAcquisitionProvider: () => {
    return {
      getAcquisitionOwners: mockApi,
    };
  },
}));

const history = createMemoryHistory();

// mock auth library

const TestView: React.FC<IAcquisitionOwnersSummaryViewProps> = () => {
  return <span>Content Rendered</span>;
};

describe('Acquisition Owners Summary container', () => {
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
    vi.resetAllMocks();
  });

  it('renders the underlying form', () => {
    const { getByText } = setup();

    expect(mockApi.execute).toHaveBeenCalled();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });
});
