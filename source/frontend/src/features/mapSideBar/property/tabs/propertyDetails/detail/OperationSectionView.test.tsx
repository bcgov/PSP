import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { createMemoryHistory } from 'history';
import { mockLookups } from '@/mocks/lookups.mock';
import { render, RenderOptions } from '@/utils/test-utils';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import {
  IOperationSectionViewProps,
  OperationSectionView,
} from './propertyOperation/OperationSectionView';
import { OperationSet } from './propertyOperation/OperationContainer';
import { getMockApiProperties } from '@/mocks/properties.mock';

const history = createMemoryHistory();
const store = { [lookupCodesSlice.name]: { lookupCodes: mockLookups } };

describe('Operation section view', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IOperationSectionViewProps> } = {},
  ) => {
    const props = renderOptions.props;
    const component = render(
      <OperationSectionView
        subdivisionOperations={props?.subdivisionOperations ?? []}
        consolidationOperations={props?.consolidationOperations ?? []}
        loading={props?.loading ?? false}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims,
        history: history,
        store: store,
      },
    );

    return { ...component };
  };

  it('matches snapshot', () => {
    const subdivisionOperations: OperationSet[] = [
      {
        sourceProperties: [getMockApiProperties()[0]],
        destinationProperties: [getMockApiProperties()[1]],
        operationDateTime: '2020-01-01',
        operationType: ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE,
      },
    ];
    const consolidationOperations: OperationSet[] = [
      {
        sourceProperties: [getMockApiProperties()[0]],
        destinationProperties: [getMockApiProperties()[1]],
        operationDateTime: '2020-01-01',
        operationType: ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE,
      },
    ];
    const { asFragment } = setup({ props: { subdivisionOperations, consolidationOperations } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('Displays the empty subdivisions message', async () => {
    const subdivisionOperations: OperationSet[] = [
      {
        sourceProperties: [],
        destinationProperties: [],
        operationDateTime: '2020-01-01',
        operationType: ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE,
      },
    ];
    const consolidationOperations: OperationSet[] = [
      {
        sourceProperties: [getMockApiProperties()[0]],
        destinationProperties: [getMockApiProperties()[1]],
        operationDateTime: '2020-01-01',
        operationType: ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE,
      },
    ];

    const { getByText, container } = setup({
      props: { subdivisionOperations, consolidationOperations },
    });

    expect(getByText('This property is not part of a subdivision')).toBeVisible();
  });

  it('Displays the empty consolidations message', async () => {
    const subdivisionOperations: OperationSet[] = [
      {
        sourceProperties: [getMockApiProperties()[0]],
        destinationProperties: [getMockApiProperties()[1]],
        operationDateTime: '2020-01-01',
        operationType: ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE,
      },
    ];
    const consolidationOperations: OperationSet[] = [
      {
        sourceProperties: [],
        destinationProperties: [],
        operationDateTime: '2020-01-01',
        operationType: ApiGen_CodeTypes_PropertyOperationTypes.CONSOLIDATE,
      },
    ];

    const { getByText, container } = setup({
      props: { subdivisionOperations, consolidationOperations },
    });

    expect(getByText('This property is not part of a consolidation')).toBeVisible();
  });
});
