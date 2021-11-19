import { Formik } from 'formik';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import { DepositNotes, IDepositNotesProps } from './DepositNotes';

const setup = (renderOptions: RenderOptions & IDepositNotesProps & { lease?: IFormLease } = {}) => {
  // render component under test
  const result = render(
    <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
      <DepositNotes disabled={renderOptions.disabled} />
    </Formik>,
    {
      ...renderOptions,
    },
  );

  return { ...result };
};

describe('DepositNotes component', () => {
  it('renders as expected', () => {
    const { asFragment } = setup({
      lease: { ...defaultFormLease, returnNotes: 'security deposit notes' },
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders the lease notes', () => {
    const { getByDisplayValue } = setup({
      lease: { ...defaultFormLease, returnNotes: 'security deposit notes' },
    });
    expect(getByDisplayValue('security deposit notes')).toBeVisible();
  });
});
