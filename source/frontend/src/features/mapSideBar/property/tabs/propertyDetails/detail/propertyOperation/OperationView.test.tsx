import { mockLookups } from '@/mocks/lookups.mock';
import { ApiGen_CodeTypes_PropertyOperationTypes } from '@/models/api/generated/ApiGen_CodeTypes_PropertyOperationTypes';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';
import { createMemoryHistory } from 'history';
import { IOperationViewProps, OperationView } from './OperationView';

const history = createMemoryHistory();
const store = { [lookupCodesSlice.name]: { lookupCodes: mockLookups } };

describe('Subdivision detail view', () => {
  const setup = (renderOptions: RenderOptions & { props?: Partial<IOperationViewProps> } = {}) => {
    const props = renderOptions.props;
    const component = render(
      <OperationView
        operationType={props?.operationType ?? ApiGen_CodeTypes_PropertyOperationTypes.SUBDIVIDE}
        operationTimeStamp={props?.operationTimeStamp ?? EpochIsoDateTime}
        sourceProperties={props?.sourceProperties ?? []}
        destinationProperties={props?.destinationProperties ?? []}
        ExpandedRowComponent={() => <></>}
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
    const sourceProperties: ApiGen_Concepts_Property[] = [
      { ...getEmptyProperty(), id: 2, pid: 1111 },
    ];
    const destinationProperties: ApiGen_Concepts_Property[] = [
      { ...getEmptyProperty(), id: 1, pid: 2222 },
    ];
    const { asFragment } = setup({ props: { sourceProperties, destinationProperties } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays the correct number of entries', async () => {
    const sourceProperties: ApiGen_Concepts_Property[] = [
      { ...getEmptyProperty(), id: 1, pid: 1111 },
    ];
    const destinationProperties: ApiGen_Concepts_Property[] = [
      { ...getEmptyProperty(), id: 2, pid: 2222 },
      { ...getEmptyProperty(), id: 3, pid: 333 },
    ];

    const { findAllByText, container } = setup({
      props: { sourceProperties, destinationProperties },
    });

    expect(await findAllByText(/PID:/i)).toHaveLength(3);
  });

  it('displays Lat/Long for properties with no other identifier', async () => {
    const sourceProperties: ApiGen_Concepts_Property[] = [
      {
        ...getEmptyProperty(),
        id: 1,
        pid: null,
        pin: null,
        latitude: 48.43,
        longitude: -123.49,
      },
    ];
    const destinationProperties: ApiGen_Concepts_Property[] = [
      { ...getEmptyProperty(), id: 2, pid: 2222 },
      { ...getEmptyProperty(), id: 3, pid: 333 },
    ];

    const { findAllByText, findByText } = setup({
      props: { sourceProperties, destinationProperties },
    });

    expect(await findAllByText(/PID:/i)).toHaveLength(2);
    expect(await findByText(/Location: 48.430000, -123.490000/i)).toBeInTheDocument();
  });

  it('displays the checkmark on sources', async () => {
    const sourceProperties = [{ ...getEmptyProperty(), id: 1 }];
    const { queryByTestId } = setup({ props: { sourceProperties } });

    expect(queryByTestId('isSource')).toBeVisible();
  });

  it('does not display the checkmark on destinations', async () => {
    const destinationProperties = [{ ...getEmptyProperty(), id: 1 }];
    const { queryByTestId } = setup({ props: { destinationProperties } });

    expect(queryByTestId('isSource')).not.toBeInTheDocument();
  });
});
