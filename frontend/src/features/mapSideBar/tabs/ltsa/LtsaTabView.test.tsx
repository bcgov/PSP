import { createMemoryHistory } from 'history';
import { IFormLease } from 'interfaces';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { mockLtsaResponse } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import LtsaTabView, { ILtsaTabViewProps } from './LtsaTabView';

const history = createMemoryHistory();

describe('LtsaTabView component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaTabViewProps & { lease?: IFormLease } = { loading: false },
  ) => {
    // render component under test
    const component = render(
      <LtsaTabView
        ltsaData={renderOptions.ltsaData}
        ltsaRequestedOn={renderOptions.ltsaRequestedOn}
        loading={renderOptions.loading}
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
    } = setup({ ltsaData: {} as LtsaOrders, ltsaRequestedOn: new Date(), loading: false });
    expect(getByText('Title Details')).toBeVisible();
  });
});
