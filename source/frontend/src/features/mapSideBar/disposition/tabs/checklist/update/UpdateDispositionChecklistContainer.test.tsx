import { createRef } from 'react';

import { IUpdateChecklistFormProps } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';
import { useDispositionProvider } from '@/hooks/repositories/useDispositionProvider';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockFileChecklistResponse, mockLookups } from '@/mocks/index.mock';
import { ApiGen_Concepts_DispositionFile } from '@/models/api/generated/ApiGen_Concepts_DispositionFile';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions, screen } from '@/utils/test-utils';

import { UpdateDispositionChecklistContainer } from './UpdateDispositionChecklistContainer';

// mock API service calls
jest.mock('@/hooks/repositories/useDispositionProvider');

type Provider = typeof useDispositionProvider;
const mockUpdateDispositionChecklist = jest.fn();

(useDispositionProvider as jest.MockedFunction<Provider>).mockReturnValue({
  putDispositionChecklist: {
    error: undefined,
    response: undefined,
    execute: mockUpdateDispositionChecklist,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

let viewProps: IUpdateChecklistFormProps | undefined;

const TestView: React.FC<IUpdateChecklistFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('UpdateDispositionChecklist container', () => {
  let dispositionFile: ApiGen_Concepts_DispositionFile | undefined = undefined;
  const onSuccess = jest.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateDispositionChecklistContainer
        formikRef={createRef()}
        dispositionFile={dispositionFile}
        onSuccess={onSuccess}
        View={TestView}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [],
        ...renderOptions,
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    dispositionFile = mockDispositionFileResponse();
    dispositionFile.fileChecklistItems = mockFileChecklistResponse();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('makes request to update the disposition checklist and returns the response', async () => {
    setup();
    mockUpdateDispositionChecklist.mockResolvedValue(mockFileChecklistResponse());

    let updatedChecklist: ApiGen_Concepts_FileWithChecklist | undefined;
    await act(async () => {
      updatedChecklist = await viewProps?.onSave({} as ApiGen_Concepts_FileWithChecklist);
    });

    expect(mockUpdateDispositionChecklist).toHaveBeenCalled();
    expect(updatedChecklist).toStrictEqual([...mockFileChecklistResponse()]);
  });

  it('calls onSuccess when the disposition checklist is saved successfully', async () => {
    setup();

    await act(async () => {
      viewProps?.onSuccess({} as ApiGen_Concepts_DispositionFile);
    });

    expect(onSuccess).toHaveBeenCalled();
  });

  it('displays a toast with server-returned error responses', async () => {
    setup();

    await act(async () => {
      const error400 = createAxiosError(400, 'Lorem ipsum error');
      viewProps?.onError(error400);
    });

    expect(await screen.findByText('Lorem ipsum error')).toBeVisible();
  });

  it('displays a toast for generic server errors', async () => {
    setup();

    await act(async () => {
      const error500 = createAxiosError(500);
      viewProps?.onError(error500);
    });

    expect(await screen.findByText('Unable to save. Please try again.')).toBeVisible();
  });
});
