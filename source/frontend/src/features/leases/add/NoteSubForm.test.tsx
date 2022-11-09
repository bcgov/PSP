import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';
import { mockLookups } from 'mocks/mockLookups';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { renderAsync, RenderOptions } from 'utils/test-utils';

import { defaultFormLease } from '../models';
import NoteSubForm, { INoteSubFormProps } from './NoteSubForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('NoteSubForm component', () => {
  const setup = async (renderOptions: RenderOptions & Partial<INoteSubFormProps> = {}) => {
    // render component under test
    const component = await renderAsync(
      <Formik onSubmit={noop} initialValues={defaultFormLease}>
        {formikProps => <NoteSubForm />}
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
});
