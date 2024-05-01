import { createMemoryHistory } from 'history';

import { mockBcAssessmentSummary } from '@/mocks/bcAssessment.mock';
import { IBcAssessmentSummary } from '@/models/layers/bcAssesment';
import { render, RenderOptions } from '@/utils/test-utils';

import BcAssessmentTabView, { IBcAssessmentTabViewProps } from './BcAssessmentTabView';

const history = createMemoryHistory();

describe('BcAssessmentTabView component', () => {
  const setup = (
    renderOptions: RenderOptions & IBcAssessmentTabViewProps = {
      loading: false,
    },
  ) => {
    // render component under test
    const component = render(
      <BcAssessmentTabView
        summaryData={renderOptions.summaryData}
        requestedOn={renderOptions.requestedOn}
        loading={renderOptions.loading}
        pid={renderOptions.pid}
      />,
      {
        ...renderOptions,
        history,
        claims: [],
      },
    );

    return {
      component,
    };
  };

  it('renders a spinner when the bc assessment data is loading', () => {
    const {
      component: { getByTestId },
    } = setup({ loading: true, pid: '123' });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders as expected when provided valid ltsa data object and requested on datetime', () => {
    const { component } = setup({
      summaryData: mockBcAssessmentSummary,
      requestedOn: new Date('2022-04-22 11:32 AM'),
      loading: false,
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not throw an exception for an invalid bc assessment data object', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {} as IBcAssessmentSummary,
      requestedOn: new Date(),
      loading: false,
      pid: '123',
    });
    expect(getByText('Unable to determine address from BC Assessment')).toBeVisible();
  });

  it('displays a warning if no pid is provided', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {} as IBcAssessmentSummary,
      requestedOn: new Date(),
      loading: false,
    });
    expect(getByText('This property does not have a valid PID', { exact: false })).toBeVisible();
  });

  it('displays a warning if data fails to load', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: undefined,
      requestedOn: new Date(),
      loading: false,
      pid: '1234',
    });
    expect(getByText('Failed to load data from BC Assessment.', { exact: false })).toBeVisible();
  });

  it('displays primary address if there are multiple', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {
        ...mockBcAssessmentSummary,
        ADDRESSES: [
          {
            BCA_FA_SYSID: 1590820,
            ROLL_NUMBER: '06817001',
            FOLIO_ID: 'A0000HQX6G',
            FOLIO_STATUS: '01',
            FOLIO_STATUS_DESCRIPTION: 'Active',
            ADDRESS_COUNT: 1,
            ADDRESS_ID: 'D00000EY63',
            UNIT_NUMBER: '100 -',
            STREET_NUMBER: '1223',
            STREET_DIRECTION_PREFIX: '',
            STREET_NAME: 'Admirals',
            STREET_TYPE: 'rd',
            STREET_DIRECTION_SUFFIX: undefined,
            CITY: 'VICTORIA',
            POSTAL_CODE: 'V9A 0H2',
            PROVINCE: 'BC',
            PRIMARY_IND: 'true',
            MAP_REFERENCE_NUMBER: undefined,
            JURISDICTION_CODE: '401',
            JURISDICTION: 'Town of View Royal (SD61)',
            WHEN_CREATED: '2021-02-16Z',
            WHEN_UPDATED: undefined,
            EXPIRY_DATE: undefined,
            FEATURE_AREA_SQM: 99.1945,
            FEATURE_LENGTH_M: 53.6078,
          },
          {
            BCA_FA_SYSID: 1590821,
            ROLL_NUMBER: '06817001',
            FOLIO_ID: 'A0000HQX6G',
            FOLIO_STATUS: '01',
            FOLIO_STATUS_DESCRIPTION: 'Active',
            ADDRESS_COUNT: 1,
            ADDRESS_ID: 'D00000EY63',
            UNIT_NUMBER: '101 -',
            STREET_NUMBER: '1224',
            STREET_DIRECTION_PREFIX: '',
            STREET_NAME: 'Admirals',
            STREET_TYPE: 'rd',
            STREET_DIRECTION_SUFFIX: undefined,
            CITY: 'VICTORIA',
            POSTAL_CODE: 'V9A 0H3',
            PROVINCE: 'BC',
            PRIMARY_IND: 'false',
            MAP_REFERENCE_NUMBER: undefined,
            JURISDICTION_CODE: '401',
            JURISDICTION: 'Town of View Royal (SD61)',
            WHEN_CREATED: '2021-02-16Z',
            WHEN_UPDATED: undefined,
            EXPIRY_DATE: undefined,
            FEATURE_AREA_SQM: 102.1945,
            FEATURE_LENGTH_M: 54.6078,
          },
        ],
      },
      requestedOn: new Date(),
      loading: false,
      pid: '1234',
    });
    expect(getByText('100 - 1223 Admirals rd')).toBeVisible();
  });

  it('throws no errors if there are no addresses', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {
        ...mockBcAssessmentSummary,
        ADDRESSES: [],
      },
      requestedOn: new Date(),
      loading: false,
      pid: '1234',
    });
    expect(getByText('Unable to determine address from BC Assessment')).toBeVisible();
  });

  it('throws no errors if there are no values', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {
        ...mockBcAssessmentSummary,
        VALUES: [],
      },
      requestedOn: new Date(),
      loading: false,
      pid: '1234',
    });
    expect(getByText('No Value data found')).toBeVisible();
  });

  it('throws no errors if there addresses is not set', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {
        ...mockBcAssessmentSummary,
        ADDRESSES: undefined as any,
      },
      requestedOn: new Date(),
      loading: false,
      pid: '1234',
    });
    expect(getByText('Unable to determine address from BC Assessment')).toBeVisible();
  });

  it('throws no errors if values is not set', () => {
    const {
      component: { getByText },
    } = setup({
      summaryData: {
        ...mockBcAssessmentSummary,
        VALUES: undefined as any,
      },
      requestedOn: new Date(),
      loading: false,
      pid: '1234',
    });
    expect(getByText('No Value data found')).toBeVisible();
  });
});
