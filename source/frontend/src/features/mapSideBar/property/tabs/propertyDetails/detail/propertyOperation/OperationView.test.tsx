import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { createMemoryHistory } from 'history';
import { mockLookups } from '@/mocks/lookups.mock';
import { render, RenderOptions } from '@/utils/test-utils';
import { EpochIsoDateTime } from '@/models/api/UtcIsoDateTime';
import { getEmptyProperty } from '@/models/defaultInitializers';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { IOperationViewProps, OperationView } from './OperationView';

const history = createMemoryHistory();
const store = { [lookupCodesSlice.name]: { lookupCodes: mockLookups } };

describe('Subdivision detail view', () => {
  const setup = (renderOptions: RenderOptions & { props?: Partial<IOperationViewProps> } = {}) => {
    const props = renderOptions.props;
    const component = render(
      <OperationView
        operationTimeStamp={props?.operationTimeStamp ?? EpochIsoDateTime}
        sourceProperties={props?.sourceProperties ?? []}
        destinationProperties={props?.destinationProperties ?? []}
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

  it('Displays the correct number of entries', async () => {
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

    console.log(container.innerHTML);
    expect(await findAllByText(/PID:/i)).toHaveLength(3);
  });

  it('Displays the checkmark on sources', async () => {
    const sourceProperties = [{ ...getEmptyProperty(), id: 1 }];
    const { queryByTestId } = setup({ props: { sourceProperties } });

    expect(queryByTestId('isSource')).toBeVisible();
  });

  it('Does not display the checkmark on destinations', async () => {
    const destinationProperties = [{ ...getEmptyProperty(), id: 1 }];
    const { queryByTestId } = setup({ props: { destinationProperties } });

    expect(queryByTestId('isSource')).not.toBeInTheDocument();
  });
});
