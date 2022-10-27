import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import DetailDescription, { IDetailDescriptionProps } from './DetailDescription';

const history = createMemoryHistory();

describe('DetailDescription component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailDescriptionProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <DetailDescription disabled={renderOptions.disabled} nameSpace={renderOptions.nameSpace} />
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
  it('renders the lease description', () => {
    const {
      component: { getByDisplayValue },
    } = setup({
      lease: {
        ...defaultFormLease,
        description: 'lease description',
      },
    });
    expect(getByDisplayValue('lease description')).toBeVisible();
  });
});
