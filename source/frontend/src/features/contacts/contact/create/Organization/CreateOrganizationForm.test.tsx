import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { ContactMethodTypes } from '@/constants/contactMethodType';
import { AddressTypes } from '@/constants/index';
import useAddContact from '@/features/contacts/hooks/useAddContact';
import { getEmptyAddress } from '@/mocks/address.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { getEmptyOrganization } from '@/mocks/organization.mock';
import { ApiGen_Concepts_Organization } from '@/models/api/generated/ApiGen_Concepts_Organization';
import { ApiGen_Concepts_OrganizationAddress } from '@/models/api/generated/ApiGen_Concepts_OrganizationAddress';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, render, RenderOptions, waitFor } from '@/utils/test-utils';

import CreateOrganizationForm from './CreateOrganizationForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// Mock API service calls
vi.mock('@/features/contacts/hooks/useAddContact');

const addOrganization = vi.fn();

vi.mocked(useAddContact).mockReturnValue({
  addPerson: vi.fn(),
  addOrganization,
});

describe('CreateOrganizationForm', () => {
  const setup = (renderOptions?: RenderOptions) => {
    // render component under test
    const component = render(<CreateOrganizationForm />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    const getSaveButton = () => component.getByText('Save');
    const getCancelButton = () => component.getByText('Cancel');

    return {
      ...component,
      getSaveButton,
      getCancelButton,
    };
  };

  beforeEach(() => {
    addOrganization.mockReset();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup();
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  describe('when Cancel button is clicked', () => {
    it('should cancel the form and navigate to Contacts List view', async () => {
      const { getCancelButton } = setup();
      const cancel = getCancelButton();
      await act(async () => userEvent.click(cancel));
      await waitFor(() => expect(history.location.pathname).toBe('/contact/list'));
    });
  });

  describe('when Save button is clicked', () => {
    it('should save the form with minimal data', async () => {
      addOrganization.mockResolvedValue({ id: 1 } as ApiGen_Concepts_Organization);
      const { getSaveButton, container } = setup();
      // provide required fields
      await act(async () => {
        await fillInput(container, 'name', 'FooBarBaz Property Management');
        await fillInput(container, 'emailContactMethods.0.value', 'foo@bar.com');
        await fillInput(
          container,
          'emailContactMethods.0.contactMethodTypeCode',
          ContactMethodTypes.WorkEmail,
          'select',
        );
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      expect(addOrganization).toBeCalledWith(expectedFormData, expect.anything(), false);
      expect(history.location.pathname).toBe('/contact/O1');
    });

    it(`should save the form with address information when 'Other' country selected and no province is supplied`, async () => {
      addOrganization.mockResolvedValue({ id: 1 } as ApiGen_Concepts_Organization);
      const { getSaveButton, container } = setup();
      // provide required fields
      await act(async () => {
        await fillInput(container, 'name', 'FooBarBaz Property Management');
        await fillInput(container, 'emailContactMethods.0.value', 'foo@bar.com');
        await fillInput(
          container,
          'emailContactMethods.0.contactMethodTypeCode',
          ContactMethodTypes.WorkEmail,
          'select',
        );
        await fillInput(
          container,
          'mailingAddress.streetAddress1',
          mockAddress.address?.streetAddress1,
        );
        await fillInput(
          container,
          'mailingAddress.municipality',
          mockAddress.address?.municipality,
        );
      });

      // wait for re-render upon changing country to OTHER
      fillInput(container, 'mailingAddress.countryId', 4, 'select');

      await act(async () => {
        await fillInput(
          container,
          'mailingAddress.countryOther',
          mockAddress.address?.countryOther,
        );
        await fillInput(container, 'mailingAddress.postal', mockAddress.address?.postal);
      });

      const save = getSaveButton();
      await act(async () => userEvent.click(save));

      const formDataWithAddress: ApiGen_Concepts_Organization = {
        ...expectedFormData,
        organizationAddresses: [mockAddress],
      };

      expect(addOrganization).toBeCalledWith(formDataWithAddress, expect.anything(), false);
      expect(history.location.pathname).toBe('/contact/O1');
    });
  });
});

const expectedFormData: ApiGen_Concepts_Organization = {
  ...getEmptyOrganization(),
  id: 0,
  rowVersion: null,
  isDisabled: false,
  name: 'FooBarBaz Property Management',
  alias: '',
  incorporationNumber: '',
  comment: '',
  organizationPersons: null,
  organizationAddresses: null,
  contactMethods: [
    {
      id: 0,
      rowVersion: null,
      contactMethodType: {
        id: 'WORKEMAIL',
        description: null,
        isDisabled: false,
        displayOrder: null,
      },
      value: 'foo@bar.com',
      personId: null,
      organizationId: null,
    },
  ],
};

const mockAddress: ApiGen_Concepts_OrganizationAddress = {
  id: 0,
  organizationId: 0,
  rowVersion: null,
  addressUsageType: {
    id: AddressTypes.Mailing,
    description: null,
    isDisabled: false,
    displayOrder: null,
  },
  address: {
    ...getEmptyAddress(),
    streetAddress1: 'Test Street',
    streetAddress2: '',
    streetAddress3: '',
    municipality: 'Amsterdam',
    provinceStateId: null,
    countryId: 4,
    countryOther: 'Netherlands',
    postal: '123456',
  },
};
