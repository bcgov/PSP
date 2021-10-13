import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IAddress, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockParcel } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import PropertyInformation, { IPropertyInformationProps } from './PropertyInformation';

const history = createMemoryHistory();

const defaultLeaseWithPropertyAddress = (address: Partial<IAddress>) => {
  return {
    ...defaultFormLease,
    properties: [{ ...mockParcel, address: { ...mockParcel.address, ...address } }],
  };
};

describe('PropertyInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IPropertyInformationProps & { lease?: IFormLease } = {
      nameSpace: 'properties',
    },
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
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
      nameSpace: 'properties',
      lease: { ...defaultFormLease, properties: [mockParcel] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders a complete lease as expected', () => {
    const { component } = setup({
      nameSpace: 'properties',
      lease: {
        ...defaultFormLease,
        properties: [mockParcel],
        amount: 1,
        description: 'a test description',
        landArea: 111,
        areaUnit: 'Hectares',
        programName: 'A program',
        lFileNo: '222',
        tfaFileNo: '333',
        psFileNo: '444',
        motiName: 'test moti name',
        note: 'a test note',
        expiryDate: '2022-01-01',
        startDate: '2020-01-01',
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not render the street address 2 field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: defaultLeaseWithPropertyAddress({ streetAddress2: '' }),
    });
    const { container } = component;
    const streetAddress2 = container.querySelector(
      `input[name="properties.0.address.streetAddress2"]`,
    );

    expect(streetAddress2).toBeNull();
  });

  it('renders the street address 2 field if present', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: defaultLeaseWithPropertyAddress({ streetAddress2: 'street address 2' }),
    });
    const { container } = component;
    const streetAddress2 = container.querySelector(
      `input[name="properties.0.address.streetAddress2"]`,
    );

    expect(streetAddress2).toBeVisible();
  });

  it('does not render the street address 3 field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: defaultLeaseWithPropertyAddress({ streetAddress3: '' }),
    });
    const { container } = component;
    const streetAddress3 = container.querySelector(
      `input[name="properties.0.address.streetAddress3"]`,
    );

    expect(streetAddress3).toBeNull();
  });

  it('renders the street address 3 field if present', () => {
    const { component } = setup({
      nameSpace: 'properties.0',
      lease: defaultLeaseWithPropertyAddress({ streetAddress3: 'street address 3' }),
    });
    const { container } = component;
    const streetAddress3 = container.querySelector(
      `input[name="properties.0.address.streetAddress3"]`,
    );

    expect(streetAddress3).toBeVisible();
  });
});
