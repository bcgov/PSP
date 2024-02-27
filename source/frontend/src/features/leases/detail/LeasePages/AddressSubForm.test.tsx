import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { getEmptyAddress } from '@/mocks/address.mock';
import { mockLeaseProperty } from '@/mocks/filterData.mock';
import { ApiGen_Concepts_Address } from '@/models/api/generated/ApiGen_Concepts_Address';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';
import { getEmptyLease, getEmptyProperty } from '@/models/defaultInitializers';
import { toTypeCode, toTypeCodeConcept } from '@/utils/formUtils';
import { render, RenderOptions } from '@/utils/test-utils';

import AddressSubForm, { IAddressSubFormProps } from './AddressSubForm';

const history = createMemoryHistory();

const defaultLeaseWithPropertyAddress = (
  address?: Partial<ApiGen_Concepts_Address>,
): ApiGen_Concepts_Lease => {
  return {
    ...getEmptyLease(),
    fileProperties: [
      {
        ...mockLeaseProperty(),
        areaUnitType: toTypeCode('test'),
        leaseArea: 0,
        property: {
          ...getEmptyProperty(),
          address:
            address !== undefined
              ? { ...getEmptyAddress(), ...mockLeaseProperty().property!.address, ...address }
              : null,
        },
        fileId: 0,
      },
    ],
  };
};

describe('AddressSubForm component', () => {
  const setup = (
    renderOptions: RenderOptions & IAddressSubFormProps & { lease?: ApiGen_Concepts_Lease } = {
      nameSpace: 'property.address',
    },
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? getEmptyLease()}>
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
      nameSpace: 'fileProperties.0.property.address',
      lease: {
        ...getEmptyLease(),
        fileProperties: [
          {
            ...mockLeaseProperty(),
            areaUnitType: toTypeCode('test'),
            leaseArea: 0,
            fileId: 0,
          },
        ],
      },
    });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not render the street address 1 field if not present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress1: '' }),
    });
    const { container } = component;
    const streetAddress1 = container.querySelector(
      `input[name="fileProperties.0.property.address.streetAddress1"]`,
    );

    expect(streetAddress1).toBeNull();
  });

  it('renders the street address 1 field if present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress1: 'street address 1' }),
    });
    const { container } = component;
    const streetAddress1 = container.querySelector(
      `input[name="fileProperties.0.property.address.streetAddress1"]`,
    );

    expect(streetAddress1).toBeVisible();
  });

  it('does not render the street address 2 field if not present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress2: '' }),
    });
    const { container } = component;
    const streetAddress2 = container.querySelector(
      `input[name="fileProperties.0.property.address.streetAddress2"]`,
    );

    expect(streetAddress2).toBeNull();
  });

  it('renders the street address 2 field if present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress2: 'street address 2' }),
    });
    const { container } = component;
    const streetAddress2 = container.querySelector(
      `input[name="fileProperties.0.property.address.streetAddress2"]`,
    );

    expect(streetAddress2).toBeVisible();
  });

  it('does not render the street address 3 field if not present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress3: '' }),
    });
    const { container } = component;
    const streetAddress3 = container.querySelector(
      `input[name="fileProperties.0.property.address.streetAddress3"]`,
    );

    expect(streetAddress3).toBeNull();
  });

  it('renders the street address 3 field if present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ streetAddress3: 'street address 3' }),
    });
    const { container } = component;
    const streetAddress3 = container.querySelector(
      `input[name="fileProperties.0.property.address.streetAddress3"]`,
    );

    expect(streetAddress3).toBeVisible();
  });

  it('does not render the municipality field if not present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ municipality: '' }),
    });
    const { container } = component;
    const municipality = container.querySelector(
      `input[name="fileProperties.0.property.address.municipality"]`,
    );

    expect(municipality).toBeNull();
  });

  it('renders the municipality field if present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ municipality: 'municipality' }),
    });
    const { container } = component;
    const municipality = container.querySelector(
      `input[name="fileProperties.0.property.address.municipality"]`,
    );

    expect(municipality).toBeVisible();
  });

  it('does not render the postal field if not present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ postal: '' }),
    });
    const { container } = component;
    const postal = container.querySelector(
      `input[name="fileProperties.0.property.address.postal"]`,
    );

    expect(postal).toBeNull();
  });

  it('renders the postal field if present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ postal: 'postal' }),
    });
    const { container } = component;
    const postal = container.querySelector(
      `input[name="fileProperties.0.property.address.postal"]`,
    );

    expect(postal).toBeVisible();
  });

  it('does not render the country field if not present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({ country: toTypeCodeConcept(1) }),
    });
    const { container } = component;
    const country = container.querySelector(
      `input[name="fileProperties.0.property.address.country"]`,
    );

    expect(country).toBeNull();
  });

  it('renders the country field if present', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress({
        country: {
          id: 1337,
          code: 'MD',
          description: 'Madagascar',
          displayOrder: 1337,
        },
      }),
    });
    const { container } = component;
    const country = container.querySelector(
      `input[name="fileProperties.0.property.address.country.description"]`,
    );

    expect(country).toBeVisible();
  });

  it('renders the warning text if the address is not defined', () => {
    const { component } = setup({
      nameSpace: 'fileProperties.0.property.address',
      lease: defaultLeaseWithPropertyAddress(undefined),
    });
    const { getByText } = component;
    expect(getByText('Address not available in PIMS')).toBeVisible();
  });
});
