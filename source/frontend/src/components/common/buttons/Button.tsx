import classnames from 'classnames';
import { CSSProperties, forwardRef, MouseEventHandler } from 'react';
import BootstrapButton, { ButtonProps as BootstrapButtonProps } from 'react-bootstrap/Button';
import Spinner from 'react-bootstrap/Spinner';
import styled from 'styled-components';

export interface ButtonProps extends BootstrapButtonProps {
  /** Adds a custom class to the button element of the <Button> component */
  className?: string;
  /** Allow for direct style overrides */
  style?: CSSProperties;
  /** Sets an icon before the text. Can be any icon from Evergreen or a custom element. */
  icon?: React.ReactNode;
  /** When true, the button is disabled. isLoading also sets the button to disabled. */
  disabled?: boolean;
  /** Button click handler */
  onClick?: MouseEventHandler<any>;
  /** Display a spinner when the form is being submitted */
  showSubmitting?: boolean;
  /** if true and showSubmitting is true, display the spinner */
  isSubmitting?: boolean;
  /** default button text value */
  defaultValue?: string | number | string[];
  /** overwrite type from React.HTMLAttributes */
  'aria-relevant'?: 'text' | 'all' | 'additions' | 'additions text' | 'removals';
}

/**
 * Buttons allow users to take actions, and make choices, with a single tap.
 * Buttons are used primarily for actions, such as “Add”, “Close”, “Cancel”, or “Save”.
 * Plain buttons, which look similar to links, are used for less important or
 * less commonly used actions, such as "View more details".
 */
export const Button = forwardRef<typeof StyledButton, ButtonProps>((props, ref) => {
  const { showSubmitting, isSubmitting, disabled, icon, children, className, ...rest } = props;

  const classes = classnames({
    Button: true,
    'Button--disabled': disabled,
    'Button--icon-only': (children === null || children === undefined) && icon,
    // eslint-disable-next-line @typescript-eslint/no-non-null-assertion
    [className!]: className,
  });

  return (
    <StyledButton ref={ref} className={classes} disabled={disabled} {...rest}>
      {icon && <div className="Button__icon">{icon}</div>}
      {children && <div className="Button__value">{children}</div>}
      {showSubmitting && isSubmitting && (
        <Spinner
          animation="border"
          size="sm"
          role="status"
          as="span"
          style={{ marginLeft: '.8rem', padding: '.8rem' }}
        />
      )}
    </StyledButton>
  );
});

const StyledButton = styled(BootstrapButton)`
  &.btn {
    // common styling for all buttons
    display: flex;
    align-items: center;
    justify-content: center;
    padding: 0.4rem 1.2rem;
    border: 0.2rem solid transparent;
    border-radius: 0.4rem;
    text-align: center;
    text-decoration: none;
    font-size: 1.8rem;
    font-family: 'BCSans', 'Noto Sans', Verdana, Arial, sans-serif;
    font-weight: 700;
    letter-spacing: 0.1rem;
    cursor: pointer;
    .Button__value {
      width: max-content;
    }

    &:hover {
      text-decoration: underline;
      opacity: 0.8;
    }

    &:focus {
      outline-width: ${({ theme }) => theme.bcTokens.layoutBorderWidthMedium};
      outline-style: solid;
      outline-color: ${({ theme }) => theme.bcTokens.surfaceColorBorderActive};
      outline-offset: ${({ theme }) => theme.bcTokens.layoutMarginHair};
      box-shadow: none;
    }

    // PRIMARY buttons
    &.btn-primary {
      color: ${({ theme }) => theme.bcTokens.typographyColorPrimaryInvert};
      background-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
      &:hover,
      &:active,
      &:focus {
        background-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonHover};
      }
    }

    // SECONDARY buttons
    &.btn-secondary {
      color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
      background: none;
      border-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
        background-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDefault};
      }
    }

    // INFO buttons
    &.btn-info {
      color: ${({ theme }) => theme.bcTokens.themeGray70};
      border: none;
      background: none;
      padding-left: 0.6rem;
      padding-right: 0.6rem;
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.css.linkHoverColor};
        background: none;
      }
    }

    // LIGHT buttons
    &.btn-light {
      color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
      background-color: ${({ theme }) => theme.css.borderOutlineColor};
      border: none;
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
        background-color: ${({ theme }) => theme.css.borderOutlineColor};
      }
    }

    // DARK buttons
    &.btn-dark {
      color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
      background-color: ${({ theme }) => theme.bcTokens.typographyColorSecondary};
      border: none;
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
        background-color: ${({ theme }) => theme.bcTokens.typographyColorSecondary};
      }
    }

    // DANGER buttons
    &.btn-danger {
      color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
      background-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
        background-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryDangerButtonDefault};
      }
    }

    // WARNING buttons
    &.btn-warning {
      color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
      background-color: ${({ theme }) => theme.bcTokens.themeGold100};
      border-color: ${({ theme }) => theme.bcTokens.themeGold100};
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.bcTokens.surfaceColorFormsDefault};
        border-color: ${({ theme }) => theme.bcTokens.themeGold100};
        background-color: ${({ theme }) => theme.bcTokens.themeGold100};
      }
    }

    // LINK buttons
    &.btn-link {
      font-size: 1.6rem;
      font-weight: 400;
      color: ${({ theme }) => theme.css.linkColor};
      background: none;
      border: none;
      text-decoration: none;
      min-height: 2.5rem;
      line-height: 3rem;
      justify-content: left;
      letter-spacing: unset;
      text-align: left;
      padding: 0;
      text-decoration: underline;
      &:hover,
      &:active,
      &:focus {
        color: ${({ theme }) => theme.css.linkHoverColor};
        text-decoration: underline;
        border: none;
        background: none;
        box-shadow: none;
        outline: none;
      }

      &:disabled,
      &.disabled {
        color: ${({ theme }) => theme.bcTokens.iconsColorDisabled};
        background: none;
        pointer-events: none;
      }
    }

    // DISABLED buttons -- applies to all buttons (primary, secondary, etc)
    &:disabled,
    &:disabled:hover {
      color: ${({ theme }) => theme.bcTokens.typographyColorDisabled};
      background-color: ${({ theme }) => theme.bcTokens.surfaceColorPrimaryButtonDisabled};
      box-shadow: none;
      user-select: none;
      pointer-events: none;
      cursor: not-allowed;
    }
  }

  &.Button {
    .Button__icon {
      margin-right: 1.6rem;
    }
    &--icon-only {
      &:focus {
        outline: none;
      }
      .Button__icon {
        margin-right: 0;
      }
    }
  }
`;
