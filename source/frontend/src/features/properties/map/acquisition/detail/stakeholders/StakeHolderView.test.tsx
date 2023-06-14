import { createMemoryHistory } from 'history';

import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

import { InterestHolderViewForm } from '../../update/stakeholders/models';
import StakeHolderView, { IStakeHolderViewProps } from './StakeHolderView';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onEdit = jest.fn();

describe('StakeHolderView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<IStakeHolderViewProps> }) => {
    const utils = render(
      <StakeHolderView
        {...renderOptions.props}
        groupedInterestProperties={
          renderOptions.props?.groupedInterestProperties ??
          getMockApiInterestHolders()
            .flatMap(i => i.interestHolderProperties)
            .map(i => InterestHolderViewForm.fromApi(i))
        }
        groupedNonInterestProperties={
          renderOptions.props?.groupedNonInterestProperties ??
          getMockApiInterestHolders()
            .flatMap(i => i.interestHolderProperties)
            .map(i => InterestHolderViewForm.fromApi(i))
        }
        loading={renderOptions.props?.loading ?? false}
        onEdit={onEdit}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
        useMockAuthentication: true,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays a loading spinner when loading', () => {
    const { getByTestId } = setup({ props: { loading: true } });
    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('displays empty warning messages when no values passed', () => {
    const { getByText } = setup({
      props: { loading: true, groupedInterestProperties: [], groupedNonInterestProperties: [] },
    });

    expect(getByText('There are no interest holders associated with this file.')).toBeVisible();
    expect(getByText('There are no non-interest payees associated with this file.')).toBeVisible();
  });

  it('displays table with grouped property values', () => {
    const model = InterestHolderViewForm.fromApi(
      getMockApiInterestHolders()[0].interestHolderProperties[0],
    );

    model.identifier = 'PID: 025-196-375';
    const { getByText } = setup({
      props: {
        groupedInterestProperties: [model],
        groupedNonInterestProperties: [],
      },
    });

    expect(getByText('PID: 025-196-375')).toBeVisible();
  });

  it('displays table with grouped non-interest property values', () => {
    const model = InterestHolderViewForm.fromApi(
      getMockApiInterestHolders()[0].interestHolderProperties[0],
    );

    model.identifier = 'PID: 025-196-375';
    const { getByText } = setup({
      props: {
        groupedInterestProperties: [],
        groupedNonInterestProperties: [model],
      },
    });

    expect(getByText('PID: 025-196-375')).toBeVisible();
  });
});
