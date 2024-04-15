import userEvent from '@testing-library/user-event';
import { createMemoryHistory } from 'history';

import { ContactTypes } from '@/features/contacts/interfaces';
import { act, render, RenderOptions } from '@/utils/test-utils';

import { ContactTypeSelector } from './ContactTypeSelector';

const history = createMemoryHistory();
const setContactTypeFn = vi.fn();

describe('ContactTypeSelector component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      contactType?: ContactTypes;
      setContactType?: (contactType: ContactTypes) => void;
    } = {},
  ) => {
    // render component under test
    const component = render(
      <ContactTypeSelector
        contactType={renderOptions.contactType ?? ContactTypes.INDIVIDUAL}
        setContactType={renderOptions.setContactType ?? setContactTypeFn}
      />,
      {
        ...renderOptions,
        history,
      },
    );

    return {
      component,
    };
  };
  beforeEach(() => {
    setContactTypeFn.mockReset();
  });
  it('renders as expected', () => {
    const { component } = setup();
    expect(component.asFragment()).toMatchSnapshot();
  });
  it('checks individual by default', () => {
    const {
      component: { getByLabelText },
    } = setup();
    expect(getByLabelText('Individual')).toBeChecked();
  });
  it('highlights individual if selected', () => {
    const {
      component: { getByLabelText },
    } = setup({ contactType: ContactTypes.INDIVIDUAL });
    expect(getByLabelText('Individual')).toBeChecked();
  });
  it('highlights organization if selected', () => {
    const {
      component: { getByLabelText },
    } = setup({ contactType: ContactTypes.ORGANIZATION });
    expect(getByLabelText('Organization')).toBeChecked();
  });
  it('calls setContactType correctly if individual is clicked', async () => {
    const {
      component: { getByLabelText },
    } = setup({ contactType: ContactTypes.ORGANIZATION });
    await act(async () => userEvent.click(getByLabelText('Individual')));
    expect(setContactTypeFn).toHaveBeenCalledWith(ContactTypes.INDIVIDUAL);
  });

  it('calls setContactType correctly if organization is clicked', async () => {
    const {
      component: { getByLabelText },
    } = setup({ contactType: ContactTypes.INDIVIDUAL });
    await act(async () => userEvent.click(getByLabelText('Organization')));
    expect(setContactTypeFn).toHaveBeenCalledWith(ContactTypes.ORGANIZATION);
  });
});
