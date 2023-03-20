import { FileTypes } from 'constants/fileTypes';
import { createMemoryHistory } from 'history';
import { mockLookups } from 'mocks';
import { lookupCodesSlice } from 'store/slices/lookupCodes';
import { render, RenderOptions, waitFor } from 'utils/test-utils';

import { SideBarContextProvider } from '../../context/sidebarContext';
import { IFormListViewProps } from './FormListView';
import FormListViewContainer, { IFormListViewContainerProps } from './FormListViewContainer';

const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};
jest.mock('../hooks/useFormRepository', () => ({
  useFormRepository: () => ({
    addFileForm: mockApi,
  }),
}));

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

let viewProps: IFormListViewProps;

const FormListView = (props: IFormListViewProps) => {
  viewProps = props;
  return <></>;
};

describe(' form list view container', () => {
  const setup = (renderOptions?: RenderOptions & Partial<IFormListViewContainerProps>) => {
    // render component under test
    const component = render(
      <SideBarContextProvider>
        <FormListViewContainer
          View={FormListView}
          fileId={renderOptions?.fileId ?? 0}
          fileType={renderOptions?.fileType ?? FileTypes.Acquisition}
        />
      </SideBarContextProvider>,
      {
        ...renderOptions,
        store: storeState,
        history: history,
        claims: renderOptions?.claims ?? [],
      },
    );

    return {
      ...component,
    };
  };

  it('renders as expected', async () => {
    const { asFragment } = setup({
      claims: [],
    });
    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });

  it('saveForm calls api post save', async () => {
    setup({
      claims: [],
    });
    viewProps.saveForm('h120');

    expect(mockApi.execute).toHaveBeenCalledWith('acquisition', {
      fileId: 0,
      formTypeCode: { id: 'h120' },
    });
  });
});
