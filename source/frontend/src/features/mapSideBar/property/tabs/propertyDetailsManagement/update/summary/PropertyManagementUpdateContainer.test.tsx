import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockApiPropertyManagement } from '@/mocks/propertyManagement.mock';
import { Api_PropertyManagement } from '@/models/api/Property';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { render, RenderOptions } from '@/utils/test-utils';

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
  execute: jest.fn(),
  loading: false,
};

const mockUpdateApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};

jest.mock('@/hooks/repositories/usePropertyManagementRepository', () => ({
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

  const onSuccess = jest.fn();

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
    jest.clearAllMocks();
  });

  it('fetches property management info from the api', () => {
    mockGetApi.execute.mockResolvedValue(getMockApiPropertyManagement(1));
    setup({ props: { propertyId: 1 } });
    expect(mockGetApi.execute).toBeCalled();
  });

  it('calls onSuccess when onSave method is called', async () => {
    mockUpdateApi.execute.mockResolvedValue({ id: 1 } as Api_PropertyManagement);
    setup();
    await viewProps.onSave(getMockApiPropertyManagement());
    expect(mockUpdateApi.execute).toHaveBeenCalled();
  });
});
