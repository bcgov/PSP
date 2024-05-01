import { createMemoryHistory } from 'history';
import { Claims } from '@/constants/index';
import { render, RenderOptions } from '@/utils/test-utils';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';
import OperationFileAssociationsContainer, {
  IOperationFileAssociationsContainerProps,
} from './OperationFileAssocationsContainer';
import { PropertyOperationResult } from './OperationView';
import { usePropertyAssociations } from '@/hooks/repositories/usePropertyAssociations';
import { ApiGen_Concepts_PropertyAssociations } from '@/models/api/generated/ApiGen_Concepts_PropertyAssociations';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetPropertyAssociations = vi.fn();
vi.mock('@/hooks/repositories/usePropertyAssociations');
vi.mocked(usePropertyAssociations).mockReturnValue({
  execute: mockGetPropertyAssociations,
  response: {} as unknown as ApiGen_Concepts_PropertyAssociations,
} as unknown as ReturnType<typeof usePropertyAssociations>);

describe('OperationContainer component', () => {
  const setup = (
    renderOptions: RenderOptions & Partial<IOperationFileAssociationsContainerProps> = {
      operation: { id: 1 } as unknown as PropertyOperationResult,
    },
  ) => {
    const component = render(
      <OperationFileAssociationsContainer operation={renderOptions.operation} />,
      {
        ...renderOptions,
        store: storeState,
        claims: [Claims.PROPERTY_VIEW],
        history,
      },
    );

    return { ...component };
  };
  beforeEach(() => {
    mockGetPropertyAssociations.mockResolvedValue(undefined);
  });

  it('displays without error when no associations present', async () => {
    mockGetPropertyAssociations.mockResolvedValue({
      id: 1,
      pid: '',
      acquisitionAssociations: [],
      researchAssociations: [],
      dispositionAssociations: [],
      leaseAssociations: [],
    });
    const { findByText } = setup();
    await findByText('There are no files associated to the property.');
  });

  it('displays without error when all associations present', async () => {
    vi.mocked(usePropertyAssociations).mockReturnValue({
      execute: mockGetPropertyAssociations,
      response: {
        id: 1,
        pid: '',
        acquisitionAssociations: [
          {
            ...getEmptyAssociation(),
            fileNumber: 'A-1',
          },
        ],
        researchAssociations: [
          {
            ...getEmptyAssociation(),
            fileNumber: 'R-1',
          },
        ],
        dispositionAssociations: [
          {
            ...getEmptyAssociation(),
            fileNumber: 'D-1',
          },
        ],
        leaseAssociations: [
          {
            ...getEmptyAssociation(),
            fileNumber: 'L-1',
          },
        ],
      },
    } as unknown as ReturnType<typeof usePropertyAssociations>);
    const { findByText } = setup();
    await findByText('A-1');
    await findByText('R-1');
    await findByText('D-1');
    await findByText('L-1');
  });
});

const getEmptyAssociation = () => ({
  id: 1,
  fileNumber: '1',
  fileName: 'acq file',
  createdBy: '',
  createdByGuid: '',
  createdDateTime: '',
  status: 'Active',
});
