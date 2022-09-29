import { Formik } from 'formik';
import { LtsaOrders } from 'interfaces/ltsaModels';
import { noop } from 'lodash';
import { mockLtsaResponse } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import LtsaTransferSubForm, { ILtsaTransferSubFormProps } from './LtsaTransferSubForm';

describe('LtsaTransferSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaTransferSubFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaTransferSubForm
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
  it('renders transfer sub form', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('transfer sub form does not render anything if transfer array is empty', () => {
    const testData = { ...mockLtsaResponse };
    testData.titleOrders[0].orderedProduct.fieldedData.titleTransfersOrDispositions = [];
    const {
      component: { getByText },
    } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(getByText('this title has no transfers')).toBeVisible();
  });
});
