import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultAddFormLease } from 'interfaces';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import AdministrationSubForm, { IAdministrationSubFormProps } from './AdministrationSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('AdministrationSubForm component', () => {
  const setup = async (
    renderOptions: RenderOptions & Partial<IAdministrationSubFormProps> = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={defaultAddFormLease}>
        {formikProps => <AdministrationSubForm formikProps={formikProps} />}
      </Formik>,
      {
        ...renderOptions,
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
    await fillInput(container, 'type', 'LSREG', 'select');
    expect(await findByText('Category:')).toBeVisible();
  });

  it('displays other category text if "Other" is selected', async () => {
    const {
      component: { container, findByLabelText },
    } = await setup({});
    await fillInput(container, 'type', 'LSREG', 'select');
    await fillInput(container, 'categoryType', 'OTHER', 'select');
    const otherText = await findByLabelText('Describe other:');
    expect(otherText).toBeVisible();
    expect(otherText).toHaveAttribute('name', 'otherCategoryType');
  });
  it('resets other category type text if type is changed', async () => {
    const {
      component: { container, findByLabelText },
    } = await setup({});
    await fillInput(container, 'type', 'LSREG', 'select');
    await fillInput(container, 'categoryType', 'OTHER', 'select');
    await fillInput(container, 'otherCategory', 'other category');
    await findByLabelText('Describe other:');
    await fillInput(container, 'type', 'OTHER', 'select');
    await findByLabelText('Describe other:');
    await fillInput(container, 'type', 'LSREG', 'select');
    await fillInput(container, 'categoryType', 'OTHER', 'select');
    const otherText = await findByLabelText('Describe other:');

    expect(otherText).toBeVisible();
    expect(otherText).toHaveValue('');
  });
  it('displays other type text if "Other" is selected', async () => {
    const {
      component: { container, findByLabelText },
    } = await setup({});
    await fillInput(container, 'type', 'OTHER', 'select');
    const otherText = await findByLabelText('Describe other:');
    expect(otherText).toBeVisible();
    expect(otherText).toHaveAttribute('name', 'otherType');
  });
  it('displays other purpose text if "Other" is selected', async () => {
    const {
      component: { container, findByLabelText },
    } = await setup({});
    await fillInput(container, 'purposeType', 'OTHER', 'select');
    const otherText = await findByLabelText('Describe other:');
    expect(otherText).toBeVisible();
    expect(otherText).toHaveAttribute('name', 'otherPurposeType');
  });
  it('displays other program text if "Other" is selected', async () => {
    const {
      component: { container, findByLabelText },
    } = await setup({});
    await fillInput(container, 'programType', 'OTHER', 'select');
    const otherText = await findByLabelText('Other Program:');
    expect(otherText).toBeVisible();
    expect(otherText).toHaveAttribute('name', 'otherProgramType');
  });
});
