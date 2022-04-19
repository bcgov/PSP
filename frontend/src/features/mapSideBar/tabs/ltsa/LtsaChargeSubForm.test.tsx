import { Formik } from 'formik';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import LtsaChargeSubForm, { ILtsaChargeSubFormProps } from './LtsaChargeSubForm';
import { mockLtsaResponse } from './LtsaTabView.test';

describe('LtsaChargeSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaChargeSubFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaChargeSubForm
          nameSpace={renderOptions.nameSpace ?? 'titleOrders.0.orderedProduct.fieldedData'}
        />
      </Formik>,
      {
        ...renderOptions,
      },
    );

    return {
      component,
    };
  };
  it('renders charge sub form', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('charge sub form does not render anything if charge array is empty', () => {
    const testData = { ...mockLtsaResponse };
    testData.titleOrders[0].orderedProduct.fieldedData.chargesOnTitle = [];
    const {
      component: { getByText },
    } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(getByText('this title has no charges')).toBeVisible();
  });
});
