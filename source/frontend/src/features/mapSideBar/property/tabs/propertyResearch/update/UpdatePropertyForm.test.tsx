import { Formik, FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { getMockResearchFileProperty } from '@/mocks/researchFile.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fakeText,
  fireEvent,
  getByName,
  render,
  RenderOptions,
  screen,
  userEvent,
} from '@/utils/test-utils';

import { UpdatePropertyFormModel } from './models';
import UpdatePropertyForm, { IUpdatePropertyResearchFormProps } from './UpdatePropertyForm';
import { UpdatePropertyYupSchema } from './UpdatePropertyYupSchema';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();
const validationSchema = vi.fn().mockReturnValue(UpdatePropertyYupSchema);

describe('UpdatePropertyForm component', () => {
  // render component under test
  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IUpdatePropertyResearchFormProps> } = {},
  ) => {
    const formikRef = createRef<FormikProps<UpdatePropertyFormModel>>();

    const utils = render(
      <UpdatePropertyForm
        formikRef={formikRef}
        initialValues={renderOptions?.props?.initialValues ?? new UpdatePropertyFormModel()}
        validationSchema={renderOptions?.props?.validationSchema ?? validationSchema}
        onSubmit={renderOptions?.props?.onSubmit ?? onSubmit}
      />,
      {
        ...renderOptions,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
      formikRef,
    };
  };

  let initialValues: UpdatePropertyFormModel;

  beforeEach(() => {
    initialValues = UpdatePropertyFormModel.fromApi(getMockResearchFileProperty());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ props: { initialValues } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays property research purposes - multiselect', () => {
    setup({ props: { initialValues } });

    expect(screen.getByText('Form 12')).toBeVisible();
    expect(screen.getByText('District Other')).toBeVisible();
  });

  it('validates max length for research summary', async () => {
    setup({ props: { initialValues } });

    const summary = getByName('researchSummary') as HTMLTextAreaElement;

    await act(async () => {
      userEvent.paste(summary, fakeText(4001));
    });
    await act(async () => {
      fireEvent.blur(summary);
    });

    expect(
      await screen.findByText(/Summary comments must be less than 4000 characters/i),
    ).toBeVisible();
    expect(validationSchema).toHaveBeenCalled();
  });

  it('calls onSubmit when form is submitted', async () => {
    const { formikRef } = setup({ props: { initialValues } });

    const summary = getByName('researchSummary') as HTMLTextAreaElement;

    await act(async () => {
      userEvent.paste(summary, fakeText(100));
    });
    await act(async () => {
      formikRef.current?.submitForm();
    });

    expect(validationSchema).toHaveBeenCalled();
    expect(onSubmit).toHaveBeenCalled();
  });
});
