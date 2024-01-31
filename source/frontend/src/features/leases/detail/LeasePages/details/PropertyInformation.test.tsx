import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { mockParcel, mockProperties } from '@/mocks/filterData.mock';
import { getMockApiLease } from '@/mocks/lease.mock';
import { getMockApiProperty } from '@/mocks/properties.mock';
import { Api_Lease } from '@/models/api/Lease';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyInformation, { IPropertyInformationProps } from './PropertyInformation';

const history = createMemoryHistory();

describe('PropertyInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertyInformationProps & { lease?: Api_Lease } = {
      nameSpace: 'properties',
    },
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getMockApiLease()}>
        <PropertyInformation
          disabled={renderOptions.disabled}
          nameSpace={renderOptions.nameSpace}
        />
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
  it('renders minimally as expected', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: {
        ...getMockApiLease(),
        properties: [
          {
            ...getMockApiProperty(),
            areaUnitType: { id: 'test' },
            leaseArea: 123,
            leaseId: null,
            lease: null,
          },
        ],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: {
        ...getMockApiLease(),
        properties: [
          {
            ...getMockApiProperty(),
            areaUnitType: { id: 'test' },
            leaseArea: 123,
            leaseId: null,
            lease: null,
          },
        ],
        amount: 1,
        description: 'a test description',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not render the area if the value is not set', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: {
        ...getMockApiLease(),
        properties: [
          {
            ...getMockApiProperty(),
            leaseArea: 1,
            areaUnitType: { id: 'test' },
            leaseId: null,
            lease: null,
          },
        ],
        amount: 1,
        description: 'a test description',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        startDate: '2020-01-01',
      },
    });
    expect(component.queryByText('Area')).toBeNull();
  });

  it('will render the land area if no area unit is set', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: {
        ...getMockApiLease(),
        properties: [
          {
            ...getMockApiProperty(),
            leaseArea: 123,
            areaUnitType: null,
            leaseId: null,
            lease: null,
          },
        ],
        amount: 1,
        description: 'a test description',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        startDate: '2020-01-01',
      },
    });
    expect(component.getByText(/123.00/i)).toBeVisible();
    expect(component.queryByDisplayValue('undefined')).toBeNull();
  });
});
