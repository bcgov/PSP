// eslint-disable-next-line simple-import-sort/imports
import { createRef } from 'react';

import { IUpdateChecklistFormProps } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import {
  mockAcquisitionFileResponse,
  mockFileChecklistResponse,
} from '@/mocks/acquisitionFiles.mock';
import { mockLookups } from '@/mocks/index.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions, screen } from '@/utils/test-utils';

import { UpdateAcquisitionChecklistContainer } from './UpdateAcquisitionChecklistContainer';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';

// mock API service calls
jest.mock('@/hooks/repositories/useAcquisitionProvider');

type Provider = typeof useAcquisitionProvider;
const mockUpdateAcquisitionChecklist = jest.fn();

(useAcquisitionProvider as jest.MockedFunction<Provider>).mockReturnValue({
  updateAcquisitionChecklist: {
    error: undefined,
    response: undefined,
    execute: mockUpdateAcquisitionChecklist,
    loading: false,
  },
} as unknown as ReturnType<Provider>);

let viewProps: IUpdateChecklistFormProps | undefined;

const TestView: React.FC<IUpdateChecklistFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('UpdateAcquisitionChecklist container', () => {
  let acquisitionFile: ApiGen_Concepts_AcquisitionFile | undefined = undefined;
  const onSuccess = jest.fn();

  const setup = (renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateAcquisitionChecklistContainer
        formikRef={createRef()}
        acquisitionFile={acquisitionFile}
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
    acquisitionFile = mockAcquisitionFileResponse();
    acquisitionFile.fileChecklistItems = mockFileChecklistResponse();
  });

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('makes request to update the acquisition checklist and returns the response', async () => {
    setup();
    mockUpdateAcquisitionChecklist.mockResolvedValue(mockFileChecklistResponse());

    let updatedChecklist: ApiGen_Concepts_FileWithChecklist | undefined;
    await act(async () => {
      updatedChecklist = await viewProps?.onSave({} as ApiGen_Concepts_FileWithChecklist);
    });

    expect(mockUpdateAcquisitionChecklist).toHaveBeenCalled();
    expect(updatedChecklist).toStrictEqual([...mockFileChecklistResponse()]);
  });

  it('calls onSuccess when the acquisition checklist is saved successfully', async () => {
    setup();

    await act(async () => {
      viewProps?.onSuccess({} as ApiGen_Concepts_AcquisitionFile);
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
