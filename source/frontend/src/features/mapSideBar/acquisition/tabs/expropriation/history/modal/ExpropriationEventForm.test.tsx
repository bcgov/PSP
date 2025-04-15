import { FormikProps } from 'formik';
import { createRef } from 'react';

import { PayeeOption } from '@/features/mapSideBar/acquisition/models/PayeeOptionModel';
import { getMockApiAcquisitionFileOwnerPerson } from '@/mocks/acquisitionFiles.mock';
import { getMockExpropriationEvent } from '@/mocks/expropriationEvents.mock';
import { getMockApiInterestHolderPerson } from '@/mocks/interestHolders.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, getByName, render, RenderOptions, screen } from '@/utils/test-utils';

import { ExpropriationEventFormModel } from '../models';
import { ExpropriationEventForm, IExpropriationEventFormProps } from './ExpropriationEventForm';

describe('ExpropriationEventForm component', () => {
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IExpropriationEventFormProps> } = {},
  ) => {
    const formikRef = createRef<FormikProps<ExpropriationEventFormModel>>();
    const defaultProps: IExpropriationEventFormProps = {
      formikRef,
      initialValues: ExpropriationEventFormModel.createEmpty(1),
      payeeOptions: [],
      onSave: vi.fn(),
    };

    const rendered = render(<ExpropriationEventForm {...defaultProps} {...renderOptions.props} />, {
      ...renderOptions,
      store: {
        [lookupCodesSlice.name]: { lookupCodes: mockLookups },
      },
    });

    return {
      ...rendered,
      formikRef,
      getPayeeOptionSelect: () => getByName('payeeKey') as HTMLSelectElement,
    };
  };

  afterEach(() => {
    vi.clearAllMocks();
  });

  it(`renders new Form with default values`, async () => {
    const { getPayeeOptionSelect } = setup();

    expect(getPayeeOptionSelect()).toHaveValue('');
    expect(getByName('eventTypeCode')).toHaveValue('');
    expect(getByName('eventDate')).toHaveValue('');
  });

  it('displays the Form values from API', async () => {
    const apiOwner = getMockApiAcquisitionFileOwnerPerson();
    const apiInterestHolder = getMockApiInterestHolderPerson();

    const { getPayeeOptionSelect } = setup({
      props: {
        initialValues: ExpropriationEventFormModel.fromApi({
          ...getMockExpropriationEvent(),
          eventDate: '2025-01-20',
          acquisitionOwnerId: apiOwner.id,
          acquisitionOwner: apiOwner,
        }),
        payeeOptions: [PayeeOption.createOwner(apiOwner, null)],
      },
    });

    expect(getPayeeOptionSelect()).toHaveValue('OWNER-1');
    expect(getByName('eventTypeCode')).toHaveValue('ADVPMTSRVDDT');
    expect(getByName('eventDate')).toHaveValue('Jan 20, 2025');
  });

  it('validates the Form before submitting values to the API', async () => {
    const { formikRef } = await setup();

    await act(async () => formikRef.current?.submitForm());

    const error = await screen.findByText(/Event is required/i);
    expect(error).toBeVisible();
  });
});
