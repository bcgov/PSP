import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import noop from 'lodash/noop';

import { defaultDocumentFilter } from '@/interfaces/IDocumentResults';
import { mockDocumentTypesResponse } from '@/mocks/documents.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from '@/utils/test-utils';

import { DocumentFilterForm, IDocumentFilterFormProps } from './DocumentFilterForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSetFilter = vi.fn();
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
        <DocumentFilterForm
          onSetFilter={onSetFilter}
          documentFilter={defaultDocumentFilter}
          documentTypes={mockDocumentTypesResponse()}
        />
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
    mockAxios.onGet(`documents/types`).reply(200, mockDocumentTypesResponse());
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

    await act(async () => {
      fillInput(container, 'documentType', 'Registered', 'select');
    });

    expect(getByTestId('document-type')).not.toBeNull();
  });

  it('renders documentStatus with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultDocumentFilter },
    });

    await act(async () => {
      fillInput(container, 'documentStatus', 'Draft', 'select');
    });

    expect(getByTestId('document-status')).not.toBeNull();
  });

  it('renders filename with data as expected', async () => {
    const {
      component: { getByTestId, container },
    } = await setup({
      initialValues: { ...defaultDocumentFilter },
    });

    await act(async () => {
      fillInput(container, 'filename', 'someDocName', 'input');
    });

    expect(getByTestId('document-filename')).not.toBeNull();
  });
});
