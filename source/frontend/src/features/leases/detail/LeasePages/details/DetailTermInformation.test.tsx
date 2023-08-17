import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { noop } from 'lodash';

import { LeaseFormModel } from '@/features/leases/models';
import { render, RenderOptions } from '@/utils/test-utils';

import DetailTermInformation, { IDetailTermInformationProps } from './DetailTermInformation';

const history = createMemoryHistory();

describe('DetailTermInformation component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailTermInformationProps & { lease?: LeaseFormModel } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
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
      lease: new LeaseFormModel(),
    });
    expect(getByText('Lease / License')).toBeVisible();
  });

  it('renders the start date in the expected format', () => {
    const { getByText } = setup({
      lease: { ...new LeaseFormModel(), startDate: '2000-01-01T18:00' },
    });
    expect(getByText('Jan 1, 2000')).toBeVisible();
  });

  it('renders the end date in the expected format', () => {
    const { getByText } = setup({
      lease: { ...new LeaseFormModel(), startDate: '2001-01-01T18:00' },
    });
    expect(getByText('Jan 1, 2001')).toBeVisible();
  });

  it('renders the project name', () => {
    const { getByText } = setup({
      lease: {
        ...new LeaseFormModel(),
        project: { code: '0000', description: 'MOCK PROJECT' },
      } as any,
    });
    expect(getByText('0000 - MOCK PROJECT')).toBeVisible();
  });
});
