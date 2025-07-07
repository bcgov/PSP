import { createMemoryHistory } from 'history';

import { LeaseFormModel } from '@/features/leases/models';
import { SpcpOrder } from '@/interfaces/ltsaModels';
import { render, RenderOptions } from '@/utils/test-utils';

import LtsaPlanTabView, { ILtsaPlanTabViewProps } from './LtsaPlanTabView';
import { getMockLtsaSPCPResponse } from '@/mocks/ltsa.mock';

const history = createMemoryHistory();

describe('LtsaPlanTabView component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaPlanTabViewProps & { lease?: LeaseFormModel } = {
      loading: false,
    },
  ) => {
    // render component under test
    const component = render(
      <LtsaPlanTabView
        spcpData={renderOptions.spcpData}
        ltsaRequestedOn={renderOptions.ltsaRequestedOn}
        loading={renderOptions.loading}
        planNumber={renderOptions.planNumber}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };

  beforeAll(() => {
    vi.useFakeTimers();
    vi.setSystemTime(new Date('06-Apr-2022 11:00 AM').getTime());
  });

  afterAll(() => {
    vi.useRealTimers();
  });

  it('renders a spinner when the ltsa data is loading', () => {
    const {
      component: { getByTestId },
    } = setup({ loading: true });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders as expected when provided valid ltsa data object and requested on datetime', () => {
    const { component } = setup({
      spcpData: getMockLtsaSPCPResponse(),
      ltsaRequestedOn: new Date('06-Apr-2022 11:00 AM GMT'),
      loading: false,
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not throw an exception for an invalid ltsa data object', () => {
    const {
      component: { getByText },
    } = setup({
      spcpData: {} as SpcpOrder,
      ltsaRequestedOn: new Date(),
      loading: false,
      planNumber: 'VISXXXX',
    });
    expect(getByText('Strata Plan Common Property Details')).toBeVisible();
  });
});
