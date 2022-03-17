import { Formik } from 'formik';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import LtsaChargeOwnerSubForm, { ILtsaChargeOwnerSubFormProps } from './LtsaChargeOwnerSubForm';
import { mockLtsaResponse } from './LtsaTabView.test';

describe('LtsaChargeOwnerSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaChargeOwnerSubFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaChargeOwnerSubForm
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
  it('renders charge owner sub form', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse as any,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('charge owner sub form does not render anything if charge ownership groups is empty', () => {
    const testData = { ...mockLtsaResponse };
    testData.titleOrders[0].orderedProduct.fieldedData.chargesOnTitle = [
      { charge: { chargeOwnershipGroups: [] } } as any,
    ];
    const {
      component: { queryByLabelText },
    } = setup({
      ltsaData: mockLtsaResponse as any,
    });
    expect(queryByLabelText('Registered owner:')).toBeNull();
  });
});
