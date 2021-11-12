import styled from 'styled-components';

interface ICenterProps {
  /** Set to true to use inline-flex instead of flex */
  inline?: boolean;
}

/**
 * Center is a wrapper around commonly used center pattern:
 *
 * ```
 * <div style={{ display: 'flex', alignItems: 'center', justifyContent: 'center' }}>
 *  Centered content
 * </div>
 * ```
 *
 * NOTE: To use center with `inline` elements set inline prop.
 */
export const Center = styled.div<ICenterProps>`
  display: ${props => (props.inline ? 'inline-flex' : 'flex')};
  align-items: center;
  justify-content: center;
`;
