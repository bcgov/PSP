import { IUpdateChecklistFormProps } from '@/features/mapSideBar/shared/tabs/checklist/update/UpdateChecklistForm';
import UpdateLeaseChecklistContainer, {
  IUpdateLeaseChecklistContainerProps,
} from './UpdateLeaseChecklistContainer';
import { act, render, RenderOptions } from '@/utils/test-utils';
import { createRef } from 'react';
import { noop } from 'lodash';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';
import { Claims } from '@/constants';
import { useLeaseRepository } from '@/hooks/repositories/useLeaseRepository';
import { mockLeaseChecklistItemsResponse } from '@/mocks/lease.mock';
import { ApiGen_Concepts_FileWithChecklist } from '@/models/api/generated/ApiGen_Concepts_FileWithChecklist';

const mockPutChecklistItemsApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/useLeaseRepository', () => ({
  useLeaseRepository: () => {
    return {
      putLeaseChecklist: mockPutChecklistItemsApi,
    };
  },
}));

let viewProps: IUpdateChecklistFormProps | undefined;
const TestView: React.FC<IUpdateChecklistFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Update Lease Checklist Item Container', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IUpdateLeaseChecklistContainerProps>;
    } = {},
  ) => {
    const component = render(
      <UpdateLeaseChecklistContainer
        formikRef={renderOptions.props?.formikRef ?? createRef()}
        View={TestView}
        onSuccess={renderOptions.props?.onSuccess ?? noop}
      />,
      {
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.LEASE_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    vi.resetAllMocks();
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders the underlying form', async () => {
    const { getByText } = await setup();
    await act(async () => {});
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('makes request to update the disposition checklist and returns the response', async () => {
    await setup();

    mockPutChecklistItemsApi.execute.mockReturnValue(mockLeaseChecklistItemsResponse());

    let updatedChecklist: ApiGen_Concepts_FileWithChecklist | undefined;
    await act(async () => {
      updatedChecklist = await viewProps?.onSave({} as ApiGen_Concepts_FileWithChecklist);
    });

    expect(mockPutChecklistItemsApi.execute).toHaveBeenCalled();
    expect(updatedChecklist).toStrictEqual([...mockLeaseChecklistItemsResponse()]);
  });
});
