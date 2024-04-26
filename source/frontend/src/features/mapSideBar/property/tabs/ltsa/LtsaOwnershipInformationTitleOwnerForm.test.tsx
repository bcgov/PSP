import { Formik } from 'formik';
import noop from 'lodash/noop';

import { LtsaOrders } from '@/interfaces/ltsaModels';
import { mockLtsaResponse } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import LtsaOwnershipInformationTitleOwnerForm, {
  ILtsaOwnershipInformationTitleOwnerFormProps,
} from './LtsaOwnershipInformationTitleOwnerForm';

describe('LtsaOwnershipInformationTitleOwnerForm component', () => {
  const setup = (
    renderOptions: RenderOptions &
      ILtsaOwnershipInformationTitleOwnerFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaOwnershipInformationTitleOwnerForm
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
  it('renders ownership information title owner form', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });
});
