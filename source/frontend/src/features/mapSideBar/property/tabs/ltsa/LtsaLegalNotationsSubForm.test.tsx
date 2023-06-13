import { Formik } from 'formik';
import { noop } from 'lodash';

import { LtsaOrders } from '@/interfaces/ltsaModels';
import { mockLtsaResponse } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import LtsaLegalNotationsSubForm, {
  ILtsaLegalNotationsSubFormProps,
} from './LtsaLegalNotationsSubForm';

describe('LtsaLegalNotationsSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & ILtsaLegalNotationsSubFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaLegalNotationsSubForm
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
  it('renders legal notations form', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('legal notations sub form does not render anything if legal notations array is empty', () => {
    const testData = { ...mockLtsaResponse };
    testData.titleOrders[0].orderedProduct.fieldedData.legalNotationsOnTitle = [];
    const {
      component: { queryByLabelText },
    } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(queryByLabelText('Legal Notations:')).toBeNull();
  });
});
