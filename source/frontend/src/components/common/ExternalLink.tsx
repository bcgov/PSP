import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

export interface ExternalLinkProps {
  to: string;
  children: React.ReactNode;
  className?: string;
  target?: string;
  rel?: string;
}

export const ExternalLink: React.FC<ExternalLinkProps> = ({
  to,
  children,
  className,
  target = '_blank',
  rel = 'noopener noreferrer',
}) => {
  return (
    <StyledLink to={to} target={target} rel={rel} className={className}>
      {children}
      <FaExternalLinkAlt />
    </StyledLink>
  );
};

const StyledLink = styled(Link)`
  display: flex;
  align-items: center;
  gap: 8px;
  text-decoration: none;
  color: inherit;

  &:hover {
    text-decoration: underline;
  }
`;
