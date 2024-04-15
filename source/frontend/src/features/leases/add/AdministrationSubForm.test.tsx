import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { mockLookups } from '@/mocks/lookups.mock';
import { getUserMock } from '@/mocks/user.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from '@/utils/test-utils';

import { getDefaultFormLease } from '../models';
import AdministrationSubForm, { IAdministrationSubFormProps } from './AdministrationSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

// mock auth library

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo: vi.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: getUserMock(),
});

describe('AdministrationSubForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & Partial<IAdministrationSubFormProps> = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={getDefaultFormLease()}>
        {formikProps => <AdministrationSubForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
        claims: [],
        store: storeState,
        history,
      },
    );

    return {
      component,
    };
  };
  it('renders as expected', async () => {
    const { component } = await setup({});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('does not display category type by default', async () => {
    const {
      component: { queryByText },
    } = await setup({});
    expect(queryByText('Category:')).toBeNull();
  });

  it('displays category type if correct type is selected', async () => {
    const {
      component: { container, findByText },
    } = await setup({});
    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'LSREG', 'select');
    });
    expect(await findByText('Category:')).toBeVisible();
  });

  it('displays other category text if Other is selected', async () => {
    const {
      component: { container, findByText },
    } = await setup({});

    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'LSREG', 'select');
      await fillInput(container, 'categoryTypeCode', 'OTHER', 'select');
    });
    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();
    const otherField = await container.querySelector(`input[name="otherCategoryTypeDescription"]`);
    expect(otherField).toHaveValue('');
  });

  it('resets other category type text if type is changed', async () => {
    const {
      component: { container, findByText },
    } = await setup({});
    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'LSREG', 'select');
      await fillInput(container, 'categoryTypeCode', 'OTHER', 'select');
      await fillInput(container, 'otherCategoryTypeDescription', 'other category');
    });
    await findByText('Describe other:');
    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'OTHER', 'select');
    });
    await findByText('Describe other:');
    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'LSREG', 'select');
      await fillInput(container, 'categoryTypeCode', 'OTHER', 'select');
    });

    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    const otherField = await container.querySelector(`input[name="otherCategoryTypeDescription"]`);
    expect(otherField).toHaveValue('');
  });

  it('displays other type text if Other is selected', async () => {
    const {
      component: { container, findByText },
    } = await setup({});
    let otherField = await container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(container, 'leaseTypeCode', 'OTHER', 'select');
    });
    const otherText = await findByText('Describe other:');
    expect(otherText).toBeVisible();

    otherField = await container.querySelector(`input[name="otherLeaseTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('displays other purpose text if Other is selected', async () => {
    const {
      component: { container, getByText },
    } = await setup({});
    let otherField = await container.querySelector(`input[name="otherPurposeTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(container, 'purposeTypeCode', 'OTHER', 'select');
    });
    const otherText = await getByText('Describe other:');
    expect(otherText).toBeVisible();

    otherField = await container.querySelector(`input[name="otherPurposeTypeDescription"]`);
    expect(otherField).toBeVisible();
  });

  it('displays other program text if Other is selected', async () => {
    const {
      component: { container, getByText },
    } = await setup({});
    let otherField = await container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeNull();

    await act(async () => {
      await fillInput(container, 'programTypeCode', 'OTHER', 'select');
    });
    const otherText = await getByText('Other Program:');
    expect(otherText).toBeVisible();
    otherField = await container.querySelector(`input[name="otherProgramTypeDescription"]`);
    expect(otherField).toBeVisible();
  });
});
