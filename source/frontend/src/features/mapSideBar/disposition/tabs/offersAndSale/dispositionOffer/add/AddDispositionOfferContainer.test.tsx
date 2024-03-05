import { createMemoryHistory } from 'history';
import React from 'react';

import { Claims } from '@/constants/claims';
import { mockDispositionFileOfferApi } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { Api_DispositionFileOffer } from '@/models/api/DispositionFile';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, createAxiosError, render, RenderOptions } from '@/utils/test-utils';

import { IDispositionOfferFormProps } from '../form/DispositionOfferForm';
import AddDispositionOfferContainer, {
  IAddDispositionOfferContainerProps,
} from './AddDispositionOfferContainer';

const history = createMemoryHistory();

const mockPostApi = {
  error: undefined,
  response: undefined,
  execute: jest.fn(),
  loading: false,
};
const onSuccess = jest.fn();

jest.mock('@/hooks/repositories/useDispositionProvider', () => ({
  useDispositionProvider: () => {
    return {
      postDispositionFileOffer: mockPostApi,
    };
  },
}));

// eslint-disable-next-line @typescript-eslint/no-unused-vars
let viewProps: IDispositionOfferFormProps | undefined;
const TestView: React.FC<IDispositionOfferFormProps> = props => {
  viewProps = props;
  return <span>Content Rendered</span>;
};

describe('Add Disposition Offer Container component', () => {
  const setup = async (
    renderOptions: RenderOptions & {
      props?: Partial<IAddDispositionOfferContainerProps>;
    } = {},
  ) => {
    const component = render(
      <AddDispositionOfferContainer dispositionFileId={1} View={TestView} onSuccess={onSuccess} />,
      {
        history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_VIEW, Claims.DISPOSITION_EDIT],
        ...renderOptions,
      },
    );

    return {
      ...component,
    };
  };

  beforeEach(() => {
    viewProps = undefined;
    jest.resetAllMocks();
  });

  it('Renders the underlying form', async () => {
    const { getByText } = await setup();
    expect(getByText(/Content Rendered/)).toBeVisible();
  });

  it('Loads props with the initial values', async () => {
    await setup({ props: { dispositionFileId: 1 } });

    expect(viewProps?.initialValues?.id).toBe(null);
    expect(viewProps?.initialValues?.dispositionFileId).toBe(1);
    expect(viewProps?.initialValues?.dispositionOfferStatusTypeCode).toBe(null);
    expect(viewProps?.initialValues?.offerName).toBe(null);
    expect(viewProps?.initialValues?.offerDate).toBe(null);
    expect(viewProps?.initialValues?.offerExpiryDate).toBe(null);
    expect(viewProps?.initialValues?.offerAmount).toBe(null);
    expect(viewProps?.initialValues?.offerNote).toBe(null);
  });

  it('makes request to create a new Offer and returns the response', async () => {
    await setup();
    const offerMock = mockDispositionFileOfferApi();
    mockPostApi.execute.mockReturnValue(offerMock);

    let createdOffer: Api_DispositionFileOffer | undefined;
    await act(async () => {
      createdOffer = await viewProps?.onSave({} as Api_DispositionFileOffer);
    });

    expect(mockPostApi.execute).toHaveBeenCalled();
    expect(createdOffer).toStrictEqual({ ...offerMock });

    expect(history.location.pathname).toBe('/');
  });

  it('navigates back to Offers and Sale tab when form is cancelled', async () => {
    await setup();
    act(() => {
      viewProps?.onCancel();
    });

    expect(history.location.pathname).toBe('/');
    expect(mockPostApi.execute).not.toHaveBeenCalled();
  });

  it('displays error message for duplicate accepted status', async () => {
    await setup();

    await act(async () => {
      const error409 = createAxiosError(409, 'Duplicate');
      viewProps?.onError(error409);
    });

    expect(viewProps?.showOfferStatusError).toBe(true);
  });
});
