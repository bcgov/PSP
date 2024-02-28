
import { createMemoryHistory } from 'history';
import { Claims } from '@/constants/index';
import { render, RenderOptions } from '@/utils/test-utils';
import { ISubdivisionContainerProps, SubdivisionContainer } from "./SubdivisionContainer";
import { ISubdivisionViewProps } from './SubdivisionView';
import { usePropertyOperationRepository } from '@/hooks/repositories/usePropertyOperationRepository';
import { getEmptyPropertyOperation } from '@/mocks/propertyOperation.mock';
import { ApiGen_Concepts_PropertyOperation } from '@/models/api/generated/ApiGen_Concepts_PropertyOperation';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { getEmptyProperty } from '@/models/defaultInitializers';

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

let subdivisionViewsProps: ISubdivisionViewProps[] = [];
const mockView: React.FunctionComponent<ISubdivisionViewProps> = props => {
  subdivisionViewsProps.push(props);
  return <div><span>Test view</span><span>source:{props.sourceProperties.length}</span><span>destination:{props.destinationProperties.length}</span></div>;
};

describe('SubdivisionContainer component', () => {
  const setup = (renderOptions: RenderOptions & Partial<ISubdivisionContainerProps> = { propertyId: 1 }) => {

    const { propertyId, ...rest } = renderOptions;
    const component = render(<SubdivisionContainer propertyId={propertyId ?? 0} View={mockView} />, { ...rest, store: storeState, claims: [Claims.PROPERTY_VIEW], history, });

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
      },
      {
        ...getEmptyPropertyOperation(),
        id: 2,
        sourcePropertyId: 3,
        destinationPropertyId: 4,
        propertyOperationNo: 2,
      }
    ]);
    mockGetProperty.mockReturnValue({ ...getEmptyProperty(), id: 1 });
    mockGetProperty.mockReturnValueOnce({ ...getEmptyProperty(), id: 2 });

    const { findAllByText } = setup();

    expect(await findAllByText(/Test view/i)).toHaveLength(2);
    expect(mockGetPropertyOperations).toHaveBeenCalledTimes(1);
    expect(mockGetProperty).toHaveBeenCalledTimes(4);
  });

  it('Groups properties by operation', async () => {
    // Setup
    mockGetPropertyOperations.mockReturnValue([
      {
        ...getEmptyPropertyOperation(),
        id: 1,
        sourcePropertyId: 1,
        destinationPropertyId: 2,
        propertyOperationNo: 1,
      },
      {
        ...getEmptyPropertyOperation(),
        id: 2,
        sourcePropertyId: 3,
        destinationPropertyId: 4,
        propertyOperationNo: 1,
      }
    ]);
    mockGetProperty.mockReturnValue({ ...getEmptyProperty(), id: 1 });
    mockGetProperty.mockReturnValueOnce({ ...getEmptyProperty(), id: 2 });

    const { findAllByText } = setup();

    expect(await findAllByText(/Test view/i)).toHaveLength(1);
    expect(mockGetPropertyOperations).toHaveBeenCalledTimes(1);
    expect(mockGetProperty).toHaveBeenCalledTimes(4);
  });
});

