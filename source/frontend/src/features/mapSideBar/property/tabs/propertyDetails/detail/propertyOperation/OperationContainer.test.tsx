import { createMemoryHistory } from 'history';
import { Claims } from '@/constants/index';
import { render, RenderOptions } from '@/utils/test-utils';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import { getEmptyPropertyOperation } from '@/mocks/propertyOperation.mock';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { IOperationSectionViewProps } from './OperationSectionView';
import { IOperationContainerProps, OperationContainer } from './OperationContainer';
import { toTypeCode } from '@/utils/formUtils';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetPropertyOperations = jest.fn<ApiGen_Concepts_PropertyOperation[], any[]>();
jest.mock('@/hooks/repositories/usePropertyOperationRepository');
(usePropertyOperationRepository as jest.Mock).mockReturnValue({
  getPropertyOperations: { execute: mockGetPropertyOperations },
});

const mockGetProperty = jest.fn<ApiGen_Concepts_Property | undefined, any[]>();
jest.mock('@/hooks/repositories/usePimsPropertyRepository');
(usePimsPropertyRepository as jest.Mock).mockReturnValue({
  getPropertyWrapper: { execute: mockGetProperty },
});

const mockView: React.FunctionComponent<IOperationSectionViewProps> = props => {
  return (
    <div>
      <span>Test view</span>
      <span>subdivisions:{props.subdivisionOperations.length}</span>
      <span>consolidations:{props.consolidationOperations.length}</span>
    </div>
  );
};

describe('OperationContainer component', () => {
  const setup = (
    renderOptions: RenderOptions & Partial<IOperationContainerProps> = { propertyId: 1 },
  ) => {
    const { propertyId, ...rest } = renderOptions;
    const component = render(<OperationContainer propertyId={propertyId ?? 0} View={mockView} />, {
      ...rest,
      store: storeState,
      claims: [Claims.PROPERTY_VIEW],
      history,
    });

    return { ...component };
  };
  beforeEach(() => {
    mockGetPropertyOperations.mockReturnValue([]);
    mockGetProperty.mockReturnValue(undefined);
  });

  it('Splits properties by operation', async () => {
    // Setup
    mockGetPropertyOperations.mockReturnValue([
      {
        ...getEmptyPropertyOperation(),
        id: 1,
        sourcePropertyId: 1,
        destinationPropertyId: 2,
        propertyOperationNo: 1,
        propertyOperationTypeCode: toTypeCode(ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE),
      },
      {
        ...getEmptyPropertyOperation(),
        id: 2,
        sourcePropertyId: 3,
        destinationPropertyId: 4,
        propertyOperationNo: 2,
        propertyOperationTypeCode: toTypeCode(ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE),
      },
    ]);
    mockGetProperty.mockReturnValue({ ...getEmptyProperty(), id: 1 });
    mockGetProperty.mockReturnValueOnce({ ...getEmptyProperty(), id: 2 });

    const { findByText } = setup();

    expect(await findByText(/subdivisions:2/i)).toBeVisible();
    expect(await findByText(/consolidations:0/i)).toBeVisible();
    expect(mockGetPropertyOperations).toHaveBeenCalledTimes(1);
    expect(mockGetProperty).toHaveBeenCalledTimes(4);
  });

  it('Groups properties by operation type', async () => {
    // Setup
    mockGetPropertyOperations.mockReturnValue([
      {
        ...getEmptyPropertyOperation(),
        id: 1,
        sourcePropertyId: 1,
        destinationPropertyId: 2,
        propertyOperationNo: 1,
        propertyOperationTypeCode: toTypeCode(ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE),
      },
      {
        ...getEmptyPropertyOperation(),
        id: 2,
        sourcePropertyId: 3,
        destinationPropertyId: 4,
        propertyOperationNo: 2,
        propertyOperationTypeCode: toTypeCode(ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE),
      },
    ]);
    mockGetProperty.mockReturnValue({ ...getEmptyProperty(), id: 1 });
    mockGetProperty.mockReturnValueOnce({ ...getEmptyProperty(), id: 2 });

    const { findByText } = setup();

    expect(await findByText(/subdivisions:1/i)).toBeVisible();
    expect(await findByText(/consolidations:1/i)).toBeVisible();
    expect(mockGetPropertyOperations).toHaveBeenCalledTimes(1);
    expect(mockGetProperty).toHaveBeenCalledTimes(4);
  });
});
