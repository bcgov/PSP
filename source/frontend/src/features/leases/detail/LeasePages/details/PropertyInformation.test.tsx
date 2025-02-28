import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockApiProperty } from '@/mocks/filterData.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyInformation, { IPropertyInformationProps } from './PropertyInformation';
import { getEmptyLease } from '@/models/defaultInitializers';

const history = createMemoryHistory();

describe('PropertyInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertyInformationProps = {
      property: getEmptyPropertyLease(),
    },
  ) => {
    // render component under test
    const component = render(
      
        <PropertyInformation
          hideAddress={renderOptions.hideAddress}
          property={renderOptions.property}
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
  it('renders minimally as expected', () => {
    const { component } = setup({
     property: 
          {
            ...getEmptyPropertyLease(),
            property: { ...mockApiProperty },
            areaUnitType: toTypeCodeNullable('test'),
            leaseArea: 123,
            fileId: 0,
            file: null,
          },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      property:
          {
            ...getEmptyPropertyLease(),
            property: { ...mockApiProperty },
            areaUnitType: toTypeCodeNullable('test'),
            leaseArea: 123,
            fileId: 0,
            file: null,
          },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not render the area if the value is not set', () => {
    const { component } = setup({
          property:{
            ...getEmptyPropertyLease(),
            property: { ...mockApiProperty },
            leaseArea: 1,
            areaUnitType: toTypeCodeNullable('test'),
            fileId: 0,
            file: null,
          },
    });
    expect(component.queryByText('Area')).toBeNull();
  });

  it('will render the land area if no area unit is set', () => {
    const { component } = setup({
          property:{
            ...getEmptyPropertyLease(),
            property: { ...mockApiProperty },
            leaseArea: 1230.09,
            areaUnitType: null,
            fileId: 0,
            file: null,
          },
    });
    expect(component.getByText(/1230/i)).toBeVisible();
    expect(component.queryByDisplayValue('undefined')).toBeNull();
  });
});
