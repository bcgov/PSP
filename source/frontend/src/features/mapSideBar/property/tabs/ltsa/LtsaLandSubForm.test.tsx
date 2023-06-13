import { Formik } from 'formik';
import { noop } from 'lodash';

import { LtsaOrders } from '@/interfaces/ltsaModels';
import { mockLtsaResponse } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import LtsaLandSubForm, { ILtsaLandSubFormProps } from './LtsaLandSubForm';

describe('LtsaLandSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaLandSubFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaLandSubForm
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
    testData.titleOrders[0].orderedProduct.fieldedData.descriptionsOfLand = [];
    const {
      component: { getByText },
    } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(getByText('None')).toBeVisible();
  });
});
