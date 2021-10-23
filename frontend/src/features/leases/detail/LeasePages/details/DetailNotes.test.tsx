import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import { DetailNotes, IDetailNotesProps } from './DetailNotes';

const history = createMemoryHistory();

describe('DetailNotes component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailNotesProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <DetailNotes disabled={renderOptions.disabled} nameSpace={renderOptions.nameSpace} />
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
  it('renders the lease notes', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        note: 'lease notes',
      },
    });
    expect(getByDisplayValue('lease notes')).toBeVisible();
  });
});
