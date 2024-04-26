import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyManagement } from '@/mocks/propertyManagement.mock';
import { ApiGen_Concepts_PropertyManagement } from '@/models/api/generated/ApiGen_Concepts_PropertyManagement';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import {
  IPropertyManagementUpdateContainerProps,
  PropertyManagementUpdateContainer,
} from './PropertyManagementUpdateContainer';
import { IPropertyManagementUpdateFormProps } from './PropertyManagementUpdateForm';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const mockGetApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
};

vi.mock('@/hooks/repositories/usePropertyManagementRepository', () => ({
  usePropertyManagementRepository: () => {
    return {
      getPropertyManagement: mockGetApi,
      updatePropertyManagement: mockUpdateApi,
    };
  },
}));

describe('PropertyManagementUpdateContainer component', () => {
  let viewProps: IPropertyManagementUpdateFormProps;

  const View = forwardRef<FormikProps<any>, IPropertyManagementUpdateFormProps>((props, ref) => {
    viewProps = props;
    return <></>;
  });

  const onSuccess = vi.fn();

  const setup = (
    renderOptions?: RenderOptions & { props?: Partial<IPropertyManagementUpdateContainerProps> },
  ) => {
    renderOptions = renderOptions ?? {};
    const utils = render(
      <PropertyManagementUpdateContainer
        {...renderOptions.props}
        propertyId={renderOptions.props?.propertyId ?? 1}
        View={View}
        onSuccess={onSuccess}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('fetches property management info from the api', async () => {
    mockGetApi.execute.mockResolvedValue(getMockApiPropertyManagement(1));
    await act(async () => {
      setup({ props: { propertyId: 1 } });
    });
    expect(mockGetApi.execute).toBeCalled();
  });

  it('calls onSuccess when onSave method is called', async () => {
    mockUpdateApi.execute.mockResolvedValue({ id: 1 } as ApiGen_Concepts_PropertyManagement);
    setup();
    await act(async () => await viewProps.onSave(getMockApiPropertyManagement()));
    expect(mockUpdateApi.execute).toHaveBeenCalled();
  });
});
