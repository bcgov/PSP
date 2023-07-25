import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { LeaseFormModel } from '@/features/leases/models';
import { mockParcel } from '@/mocks/filterData.mock';
import { render, RenderOptions } from '@/utils/test-utils';

import PropertyInformation, { IPropertyInformationProps } from './PropertyInformation';

const history = createMemoryHistory();

describe('PropertyInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertyInformationProps & { lease?: LeaseFormModel } = {
      nameSpace: 'properties',
    },
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
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
        ...new LeaseFormModel(),
        properties: [{ ...mockParcel, areaUnitTypeCode: 'test', landArea: '123', leaseId: null }],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: {
        ...new LeaseFormModel(),
        properties: [{ ...mockParcel, areaUnitTypeCode: 'test', landArea: '123', leaseId: null }],
        amount: 1,
        description: 'a test description',
        programName: 'A program',
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
        ...new LeaseFormModel(),
        properties: [{ ...mockParcel, landArea: '1', areaUnitTypeCode: 'test', leaseId: null }],
        amount: 1,
        description: 'a test description',
        programName: 'A program',
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
        ...new LeaseFormModel(),
        properties: [{ ...mockParcel, landArea: '123', areaUnitTypeCode: 'test', leaseId: null }],
        amount: 1,
        description: 'a test description',
        programName: 'A program',
        lFileNo: '222',
        tfaFileNumber: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        startDate: '2020-01-01',
      },
    });
    expect(component.getByText('123.00')).toBeVisible();
    expect(component.queryByDisplayValue('undefined')).toBeNull();
  });
});
