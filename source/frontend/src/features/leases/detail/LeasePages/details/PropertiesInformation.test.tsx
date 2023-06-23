import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { defaultFormLease, IFormLease } from '@/interfaces';
import { mockParcel } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertiesInformation, { IPropertiesInformationProps } from './PropertiesInformation';

const history = createMemoryHistory();

describe('PropertiesInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertiesInformationProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <PropertiesInformation nameSpace={renderOptions.nameSpace} />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', () => {
    const { component } = setup({ lease: { ...defaultFormLease, properties: [mockParcel] } });
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('renders one Property Information section per property', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, properties: [mockParcel, mockParcel] },
    });
    const { getAllByText } = component;
    const propertyHeaders = getAllByText('Property Information');

    expect(propertyHeaders).toHaveLength(1);
  });

  it('renders no property information section if there are no properties', () => {
    const { component } = setup({
      lease: { ...defaultFormLease, properties: [] },
    });
    const { queryByText } = component;
    const propertyHeader = queryByText('Property Information');

    expect(propertyHeader).toBeNull();
  });
});
