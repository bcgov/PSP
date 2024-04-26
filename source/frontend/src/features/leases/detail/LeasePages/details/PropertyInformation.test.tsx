import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { mockApiProperty } from '@/mocks/filterData.mock';
import { getEmptyPropertyLease } from '@/mocks/properties.mock';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { toTypeCodeNullable } from '@/utils/formUtils';
import { getMockApiLease } from '@/mocks/lease.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyInformation, { IPropertyInformationProps } from './PropertyInformation';

const history = createMemoryHistory();

describe('PropertyInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertyInformationProps & { lease?: ApiGen_Concepts_Lease } = {
      nameSpace: 'fileProperties',
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
      nameSpace: 'fileProperties.0',
      lease: {
        ...getMockApiLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            ...mockApiProperty,
            areaUnitType: toTypeCodeNullable('test'),
            leaseArea: 123,
            fileId: 0,
            file: null,
          },
        ],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0',
      lease: {
        ...getMockApiLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            ...mockApiProperty,
            areaUnitType: toTypeCodeNullable('test'),
            leaseArea: 123,
            fileId: 0,
            file: null,
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
      nameSpace: 'fileProperties.0',
      lease: {
        ...getMockApiLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            ...mockApiProperty,
            leaseArea: 1,
            areaUnitType: toTypeCodeNullable('test'),
            fileId: 0,
            file: null,
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
      nameSpace: 'fileProperties.0',
      lease: {
        ...getMockApiLease(),
        fileProperties: [
          {
            ...getEmptyPropertyLease(),
            ...mockApiProperty,
            leaseArea: 123,
            areaUnitType: null,
            fileId: 0,
            file: null,
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
