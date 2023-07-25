import { createMemoryHistory } from 'history';

import { LeaseFormModel } from '@/features/leases/models';
import { LtsaOrders } from '@/interfaces/ltsaModels';
import { mockLtsaResponse } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import { ILtsaTabViewProps, LtsaTabView } from './LtsaTabView';

const history = createMemoryHistory();

describe('LtsaTabView component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaTabViewProps & { lease?: LeaseFormModel } = {
      loading: false,
    },
  ) => {
    // render component under test
    const component = render(
      <LtsaTabView
        ltsaData={renderOptions.ltsaData}
        ltsaRequestedOn={renderOptions.ltsaRequestedOn}
        loading={renderOptions.loading}
        pid={renderOptions.pid}
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

  it('renders a spinner when the ltsa data is loading', () => {
    const {
      component: { getByTestId },
    } = setup({ loading: true });

    const spinner = getByTestId('filter-backdrop-loading');
    expect(spinner).toBeVisible();
  });

  it('renders as expected when provided valid ltsa data object and requested on datetime', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
      ltsaRequestedOn: new Date('06-Apr-2022 11:32 AM'),
      loading: false,
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not throw an exception for an invalid ltsa data object', () => {
    const {
      component: { getByText },
    } = setup({
      ltsaData: {} as LtsaOrders,
      ltsaRequestedOn: new Date(),
      loading: false,
      pid: '123',
    });
    expect(getByText('Title Details')).toBeVisible();
  });

  it('displays a warning if no pid is provided', () => {
    const {
      component: { getByText },
    } = setup({
      ltsaData: {} as LtsaOrders,
      ltsaRequestedOn: new Date(),
      loading: false,
    });
    expect(getByText('This property does not have a valid PID', { exact: false })).toBeVisible();
  });
});
