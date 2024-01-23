import { createMemoryHistory } from 'history';

import { mockAcquisitionFileResponse } from '@/mocks/acquisitionFiles.mock';
import { getMockApiInterestHolders } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';
import { exists } from '@/utils/utils';

import { InterestHolderViewForm, InterestHolderViewRow } from '../update/models';
import StakeholderOrganizer from './stakeholderOrganizer';
import StakeHolderView, { IStakeHolderViewProps } from './StakeHolderView';

jest.mock('@react-keycloak/web');

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

jest.mock('./stakeholderOrganizer');

export const organizerMock = {
  getInterestProperties: jest.fn(),
  getNonInterestProperties: jest.fn(),
};

const onEdit = jest.fn();

describe('StakeHolderView component', () => {
  // render component under test
  const setup = (renderOptions: RenderOptions & { props?: Partial<IStakeHolderViewProps> }) => {
    const utils = render(
      <StakeHolderView
        {...renderOptions.props}
        acquisitionFile={renderOptions.props?.acquisitionFile ?? mockAcquisitionFileResponse()}
        interestHolders={renderOptions.props?.interestHolders ?? getMockApiInterestHolders()}
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

  beforeEach(() => {
    jest.resetAllMocks();

    const groupedInterestProperties = getMockApiInterestHolders()
      .flatMap(i => i.interestHolderProperties)
      .filter(exists)
      .map(i => InterestHolderViewForm.fromApi(i));

    const groupedNonInterestProperties = getMockApiInterestHolders()
      .flatMap(i => i.interestHolderProperties)
      .filter(exists)
      .map(i => InterestHolderViewForm.fromApi(i));

    organizerMock.getInterestProperties.mockReturnValue(groupedInterestProperties);
    organizerMock.getNonInterestProperties.mockReturnValue(groupedNonInterestProperties);
    (StakeholderOrganizer as jest.Mock).mockImplementation(() => organizerMock);
  });

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
    organizerMock.getInterestProperties.mockReturnValue([]);
    organizerMock.getNonInterestProperties.mockReturnValue([]);

    const { getByText } = setup({
      props: { loading: true, acquisitionFile: undefined, interestHolders: [] },
    });

    expect(getByText('There are no interest holders associated with this file.')).toBeVisible();
    expect(getByText('There are no non-interest payees associated with this file.')).toBeVisible();
  });

  it('displays table with grouped property values', () => {
    const model = InterestHolderViewForm.fromApi(
      getMockApiInterestHolders()![0].interestHolderProperties![0],
    );

    organizerMock.getInterestProperties.mockReturnValue([model]);

    model.identifier = 'PID: 025-196-375';
    const { getByText } = setup({
      props: {},
    });

    expect(getByText('PID: 025-196-375')).toBeVisible();
  });

  it('displays table with grouped property values where there are multiple interest type codes', () => {
    const interestHolder = getMockApiInterestHolders()[0];
    const interestHolderProperty = interestHolder.interestHolderProperties![0];
    interestHolderProperty.propertyInterestTypes = [
      { id: 'O', description: 'Ordinal', displayOrder: null, isDisabled: false },
      { id: 'R', description: 'Registered', displayOrder: null, isDisabled: false },
    ];
    const model = InterestHolderViewForm.fromApi(interestHolderProperty);
    model.identifier = 'PID: 025-196-375';
    model.groupedPropertyInterests = [
      InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder, {
        id: 'O',
        description: 'Ordinal',
        displayOrder: null,
        isDisabled: false,
      }),
      InterestHolderViewRow.fromApi(interestHolderProperty, interestHolder, {
        id: 'R',
        description: 'Registered',
        displayOrder: null,
        isDisabled: false,
      }),
    ];

    organizerMock.getInterestProperties.mockReturnValue([model]);

    const { getByText } = setup({
      props: {},
    });

    expect(getByText('PID: 025-196-375')).toBeVisible();
    expect(getByText('Ordinal')).toBeVisible();
    expect(getByText('Registered')).toBeVisible();
  });

  it('displays table with grouped non-interest property values', () => {
    const model = InterestHolderViewForm.fromApi(
      getMockApiInterestHolders()![0].interestHolderProperties![0],
    );

    organizerMock.getInterestProperties.mockReturnValue([model]);

    model.identifier = 'PID: 025-196-375';
    const { getByText } = setup({
      props: {},
    });

    expect(getByText('PID: 025-196-375')).toBeVisible();
  });

  it('it hides the legacy stakeholders', () => {
    const { queryByTestId } = setup({});

    expect(queryByTestId('acq-file-legacy-stakeholders')).not.toBeInTheDocument();
  });

  it('it displays the legacy stakeholders', () => {
    const { queryByTestId } = setup({
      props: {
        acquisitionFile: {
          ...mockAcquisitionFileResponse(),
          legacyStakeholders: ['John,Doe'],
        },
      },
    });
    expect(queryByTestId('acq-file-legacy-stakeholders')).toBeInTheDocument();
    expect(queryByTestId('acq-file-legacy-stakeholders')).toHaveTextContent('John,Doe');
  });
});
