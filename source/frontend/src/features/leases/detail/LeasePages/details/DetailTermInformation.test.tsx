import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultFormLease, IFormLease } from 'interfaces';
import { noop } from 'lodash';
import { render, RenderOptions } from 'utils/test-utils';

import DetailTermInformation, { IDetailTermInformationProps } from './DetailTermInformation';

const history = createMemoryHistory();

describe('DetailTermInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailTermInformationProps & { lease?: IFormLease } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? defaultFormLease}>
        <DetailTermInformation nameSpace={renderOptions.nameSpace} />
      </Formik>,
      {
        ...renderOptions,
        history,
      },
    );

    return { ...component };
  };
  it('renders a subcomponent with the expected title', () => {
    const { getByText } = setup({
      lease: defaultFormLease,
    });
    expect(getByText('Lease / License')).toBeVisible();
  });

  it('renders the start date in the expected format', () => {
    const { getByText } = setup({
      lease: { ...defaultFormLease, startDate: '2000-01-01' },
    });
    expect(getByText('Jan 1, 2000')).toBeVisible();
  });

  it('renders the end date in the expected format', () => {
    const { getByText } = setup({
      lease: { ...defaultFormLease, startDate: '2001-01-01' },
    });
    expect(getByText('Jan 1, 2001')).toBeVisible();
  });

  it('renders the project name', () => {
    const { getByText } = setup({
      lease: { ...defaultFormLease, project: { code: '0000', description: 'MOCK PROJECT' } } as any,
    });
    expect(getByText('0000 - MOCK PROJECT')).toBeVisible();
  });
});
