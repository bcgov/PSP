import { createMemoryHistory } from 'history';
import { filter, sortBy } from 'lodash';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { FileTypes } from '@/constants/fileTypes';
import { SideBarContextProvider } from '@/features/mapSideBar/context/sidebarContext';
import { useFormDocumentRepository } from '@/hooks/repositories/useFormDocumentRepository';
import { getMockApiFileForms } from '@/mocks/form.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { act, render, RenderOptions, screen, userEvent, waitFor } from '@/utils/test-utils';

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
const mockGetApi = {
  error: undefined,
  response: getMockApiFileForms(),
  execute: jest.fn(),
  loading: false,
};
jest.mock('@/hooks/repositories/useFormDocumentRepository');

jest.mock('@/components/common/mapFSM/MapStateMachineContext');
(useMapStateMachine as jest.Mock).mockImplementation(() => mapMachineBaseMock);

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

let viewProps: IFormListViewProps;

const FormListView = (props: IFormListViewProps) => {
  viewProps = props;
  return <></>;
};

describe('form list view container', () => {
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

  beforeEach(() => {
    (useFormDocumentRepository as jest.Mock).mockImplementation(() => ({
      addFilesForm: mockApi,
      deleteFileForm: mockApi,
      getFileForms: mockGetApi,
    }));
  });

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
      id: null,
      formDocumentType: {
        description: '',
        displayOrder: null,
        documentId: null,
        formTypeCode: 'h120',
      },
    });
  });

  it('Delete form calls displays delete modal', async () => {
    setup({
      claims: [],
    });
    viewProps.onDelete(1);
    const modal = await screen.findByText('Confirm Delete');

    expect(modal).toBeVisible();
  });

  it('confirming delete modal sends delete call', async () => {
    setup({
      claims: [],
    });
    viewProps.onDelete(1);
    const continueButton = await screen.findByText('Continue');
    act(() => userEvent.click(continueButton));

    expect(mockApi.execute).toHaveBeenCalledWith('acquisition', 1);
  });

  it('fetchs data when no data is currently available in container', async () => {
    (useFormDocumentRepository as jest.Mock).mockImplementation(() => ({
      addFilesForm: mockApi,
      deleteFileForm: mockApi,
      getFileForms: mockApi,
    }));

    setup({
      claims: [],
    });

    expect(mockApi.execute).toHaveBeenCalledWith('acquisition', 0);
  });

  it('fetchs data when no data is currently available in container', async () => {
    (useFormDocumentRepository as jest.Mock).mockImplementation(() => ({
      addFilesForm: mockApi,
      deleteFileForm: mockApi,
      getFileForms: mockApi,
    }));

    setup({
      claims: [],
    });

    expect(mockApi.execute).toHaveBeenCalledWith('acquisition', 0);
  });

  it('sorts as expected when setSort is called', async () => {
    setup({
      claims: [],
    });
    act(() => viewProps.setSort({ formDocumentType: 'asc' }));

    await waitFor(() => {
      expect(viewProps.forms).toStrictEqual(
        sortBy(getMockApiFileForms(), form => form.formDocumentType.formTypeCode),
      );
    });
  });

  it('sorts as expected when an invalid sort key is included', async () => {
    setup({
      claims: [],
    });
    act(() => viewProps.setSort({ blah: 'asc' } as any));

    await waitFor(() => {
      expect(viewProps.forms).toStrictEqual(getMockApiFileForms());
    });
  });

  it('makes a filter call to api when setFilter is called', async () => {
    setup({
      claims: [],
    });
    act(() => viewProps.setFormFilter({ formTypeId: 'h120' }));

    expect(viewProps.forms).toStrictEqual(
      filter(getMockApiFileForms(), form => form.formDocumentType.formTypeCode === 'h120'),
    );
  });
});
