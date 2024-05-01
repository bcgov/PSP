import { Formik, FormikProps } from 'formik';
import noop from 'lodash/noop';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { ContactInputContainer, IContactInputContainerProps } from './ContactInputContainer';
import { IContactInputViewProps } from './ContactInputView';

let viewProps = {} as IContactInputViewProps;
let testFormikProps = {} as FormikProps<any>;

const ContactInputView = (props: IContactInputViewProps) => {
  viewProps = props;
  return <></>;
};

describe('ContactInputContainer component', () => {
  // render component under test
  const setup = (
    props: IContactInputContainerProps = {} as any,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <Formik onSubmit={noop} initialValues={{} as any}>
        {formikProps => {
          testFormikProps = formikProps;
          return <ContactInputContainer {...{ ...props, View: ContactInputView }} />;
        }}
      </Formik>,
      {
        ...renderOptions,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
        claims: [],
      },
    );

    return {
      ...utils,
    };
  };

  beforeEach(() => {});

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup();
    expect(asFragment()).toMatchSnapshot();
  });

  it('shows the contact manager if setShowContactManager called', async () => {
    setup();
    await act(async () => viewProps.setShowContactManager(true));
    expect(viewProps.contactManagerProps.display).toBe(true);
  });

  it('modal ok function hides the contact manager', async () => {
    setup({ field: 'test' });
    await act(
      () =>
        viewProps.contactManagerProps?.handleModalOk &&
        viewProps.contactManagerProps.handleModalOk(),
    );
    expect(viewProps.contactManagerProps.display).toBe(false);
  });

  it('modal ok function resets the selected contacts', async () => {
    setup({ field: 'test' });
    await act(
      () =>
        viewProps.contactManagerProps?.handleModalOk &&
        viewProps.contactManagerProps.handleModalOk(),
    );
    expect(viewProps.contactManagerProps.selectedRows).toHaveLength(0);
  });

  it('cancel sets field to undefined', async () => {
    setup({ field: 'test' });
    await act(async () => viewProps.onClear());
    expect(testFormikProps.values.test).toBe(null);
  });
});
