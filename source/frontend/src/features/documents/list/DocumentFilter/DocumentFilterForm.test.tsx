import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { Formik } from 'formik';
import { createMemoryHistory } from 'history';
import { defaultDocumentFilter } from 'interfaces/IDocumentResults';
import { noop } from 'lodash';
import { mockLookups } from 'mocks';
import { mockDocumentTypesResponse } from 'mocks/mockDocuments';
import { Api_DocumentType } from 'models/api/Document';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { act, fillInput, renderAsync, RenderOptions } from 'utils/test-utils';

import { DocumentFilterForm, IDocumentFilterFormProps } from './DocumentFilterForm';

const history = createMemoryHistory();
const mockAxios = new MockAdapter(axios);
const onSetFilter = jest.fn();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const documentTypes: Api_DocumentType[] = [
  {
    id: 1,
    documentType: 'BC Assessment Search',
    mayanId: 17,
  },
  {
    id: 2,
    documentType: 'Privy Council',
    mayanId: 7,
  },
];

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
          documentTypes={documentTypes}
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

    act(() => {
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

    act(() => {
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

    act(() => {
      fillInput(container, 'filename', 'someDocName', 'input');
    });

    expect(getByTestId('document-filename')).not.toBeNull();
  });
});
