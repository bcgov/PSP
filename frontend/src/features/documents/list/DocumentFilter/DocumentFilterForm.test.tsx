import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultDocumentFilter } from 'interfaces/IDocumentResults';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { mockDocumentTypesResponse } from 'mocks/mockDocuments';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { DocumentFilterForm, IDocumentFilterFormProps } from './DocumentFilterForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSetFilter = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

describe('DocumentFilterForm component', () => {
  const setup = async (
    renderOptions: RenderOptions &
      Partial<IDocumentFilterFormProps> & {
        initialValues?: any;
      } = {},
  ) => {
    // render component under test
    const component = await renderAsync(
      <Formik initialValues={renderOptions.initialValues ?? {}} onSubmit={noop}>
        <DocumentFilterForm onSetFilter={onSetFilter} documentFilter={defaultDocumentFilter} />
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

  beforeEach(() => {
    mockAxios.reset();
    mockAxios.onGet(`documents/document-types`).reply(200, mockDocumentTypesResponse());
  });

  it('renders as expected', async () => {
    const { component } = await setup({});

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders with data as expected', async () => {
    const { component } = await setup({
      initialValues: { ...defaultDocumentFilter },
    });

    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders documentType with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultDocumentFilter },
    });

    await fillInput(container, 'documentType', 'Registered', 'select');

    expect(getByTestId('document-type')).not.toBeNull();
  });

  it('renders documentStatus with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultDocumentFilter },
    });

    await fillInput(container, 'documentStatus', 'Draft', 'select');

    expect(getByTestId('document-status')).not.toBeNull();
  });
});
