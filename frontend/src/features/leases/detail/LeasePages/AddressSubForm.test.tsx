import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IAddress, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockParcel } from 'mocks/filterDataMock';
import { render, RenderOptions } from 'utils/test-utils';

import AddressSubForm, { IAddressSubFormProps } from './AddressSubForm';

const history = createMemoryHistory();

const defaultLeaseWithPropertyAddress = (address: Partial<IAddress>) => {
  return {
    ...defaultFormLease,
    properties: [{ ...mockParcel, address: { ...mockParcel.address, ...address } }],
  };
};

describe('AddressSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & IAddressSubFormProps & { lease?: IFormLease } = {
      nameSpace: 'address',
    },
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <AddressSubForm disabled={renderOptions.disabled} nameSpace={renderOptions.nameSpace} />
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
      nameSpace: 'properties.0.address',
      lease: { ...defaultFormLease, properties: [mockParcel] },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not render the street address 1 field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress1: '' }),
    });
    const { container } = component;
    const streetAddress1 = container.querySelector(
      `input[name="properties.0.address.streetAddress1"]`,
    );

    expect(streetAddress1).toBeNull();
  });

  it('renders the street address 1 field if present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress1: 'street address 1' }),
    });
    const { container } = component;
    const streetAddress1 = container.querySelector(
      `input[name="properties.0.address.streetAddress1"]`,
    );

    expect(streetAddress1).toBeVisible();
  });

  it('does not render the street address 2 field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
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
      nameSpace: 'properties.0.address',
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
      nameSpace: 'properties.0.address',
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
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress3: 'street address 3' }),
    });
    const { container } = component;
    const streetAddress3 = container.querySelector(
      `input[name="properties.0.address.streetAddress3"]`,
    );

    expect(streetAddress3).toBeVisible();
  });

  it('does not render the municipality field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ municipality: '' }),
    });
    const { container } = component;
    const municipality = container.querySelector(`input[name="properties.0.address.municipality"]`);

    expect(municipality).toBeNull();
  });

  it('renders the municipality field if present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ municipality: 'municipality' }),
    });
    const { container } = component;
    const municipality = container.querySelector(`input[name="properties.0.address.municipality"]`);

    expect(municipality).toBeVisible();
  });

  it('does not render the postal field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ postal: '' }),
    });
    const { container } = component;
    const postal = container.querySelector(`input[name="properties.0.address.postal"]`);

    expect(postal).toBeNull();
  });

  it('renders the postal field if present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ postal: 'postal' }),
    });
    const { container } = component;
    const postal = container.querySelector(`input[name="properties.0.address.postal"]`);

    expect(postal).toBeVisible();
  });

  it('does not render the country field if not present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ country: '' }),
    });
    const { container } = component;
    const country = container.querySelector(`input[name="properties.0.address.country"]`);

    expect(country).toBeNull();
  });

  it('renders the country field if present', () => {
    const { component } = setup({
      nameSpace: 'properties.0.address',
      lease: defaultLeaseWithPropertyAddress({ country: 'country' }),
    });
    const { container } = component;
    const country = container.querySelector(`input[name="properties.0.address.country"]`);

    expect(country).toBeVisible();
  });
});
