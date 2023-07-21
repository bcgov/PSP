import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { LeaseFormModel } from '@/features/leases/models';
import { render, RenderOptions } from '@/utils/test-utils';

import DetailDescription, { IDetailDescriptionProps } from './DetailDescription';

const history = createMemoryHistory();

describe('DetailDescription component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailDescriptionProps & { lease?: LeaseFormModel } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
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
        ...new LeaseFormModel(),
        description: 'lease description',
      },
    });
    expect(getByDisplayValue('lease description')).toBeVisible();
  });
});
