import { useKeycloak } from '@react-keycloak/web';
import { fireEvent } from '@testing-library/react';

export const mockKeycloak = (claims: string[], agencies: number[]) => {
  (useKeycloak as jest.Mock).mockReturnValue({
    keycloak: {
      userInfo: {
        agencies: agencies,
        roles: claims,
      },
      subject: 'test',
    },
  });
};

export const fillInput = async (
  container: HTMLElement,
  name: string,
  value: any,
  type: string = 'input',
) => {
  let input: Element | null = null;

  if (type === 'radio') {
    input = container.querySelector(`#input-${name}`);
  } else {
    if (type === 'typeahead' || type === 'datepicker') {
      input = container.querySelector(`input[name="${name}"]`);
    } else {
      input = container.querySelector(`${type}[name="${name}"]`);
    }
  }

  // abort early if no input field found
  if (!input) return { input };

  if (type === 'input') {
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
    fireEvent.focusOut(input);
  } else if (type === 'typeahead') {
    fireEvent.focus(input);
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
    const select = container.querySelector(`[aria-label="${value}"]`);
    fireEvent.click(select!);
    fireEvent.focusOut(input);
  } else if (type === 'datepicker') {
    fireEvent.mouseDown(input);
    fireEvent.change(input, {
      target: {
        value: value,
      },
    });
    fireEvent.keyPress(input, { key: 'Enter', code: 'Enter' });
  } else if (type === 'radio') {
    fireEvent.click(input);
    fireEvent.focusOut(input);
  } else {
    fireEvent.change(input!, {
      target: {
        value: value,
      },
    });
    fireEvent.focusOut(input);
  }
  fireEvent.blur(input!);

  return { input };
};

export const flushPromises = () => new Promise(setImmediate);
