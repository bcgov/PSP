import { Formik } from 'formik';
import { noop } from 'lodash';

import { LtsaOrders } from '@/interfaces/ltsaModels';
import { mockLtsaResponse } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import LtsaOwnershipInformationForm, {
  ILtsaOwnershipInformationFormProps,
} from './LtsaOwnershipInformationForm';

describe('LtsaOwnershipInformationForm component', () => {
  const setup = (
    renderOptions: RenderOptions &
      ILtsaOwnershipInformationFormProps & { ltsaData?: LtsaOrders } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.ltsaData ?? {}}>
        <LtsaOwnershipInformationForm
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
  it('renders ownership information form', () => {
    const { component } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('ownership information form does not render anything if ownershipGroups array is empty', () => {
    const testData = { ...mockLtsaResponse };
    testData.titleOrders[0].orderedProduct.fieldedData.ownershipGroups = [];
    const {
      component: { getByText },
    } = setup({
      ltsaData: mockLtsaResponse,
    });
    expect(getByText('None')).toBeVisible();
  });
});
