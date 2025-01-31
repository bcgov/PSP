import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { LeaseFormModel } from '@/features/leases/models';
import { render, RenderOptions } from '@/utils/test-utils';

import { DetailNotes, IDetailNotesProps } from './DetailNotes';

const history = createMemoryHistory();

describe('DetailNotes component', () => {
  const setup = (
    renderOptions: RenderOptions & IDetailNotesProps & { lease?: LeaseFormModel } = {},
  ) => {
    // render component under test
    const component = render(
      <Formik onSubmit={noop} initialValues={renderOptions.lease ?? new LeaseFormModel()}>
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
});
