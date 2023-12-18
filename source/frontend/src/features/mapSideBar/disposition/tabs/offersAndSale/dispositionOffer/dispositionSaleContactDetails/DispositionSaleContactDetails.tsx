import { FaExternalLinkAlt } from 'react-icons/fa';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

import { StyledLink } from '@/components/maps/leaflet/LayerPopup/styles';
import {
  Api_DispositionSalePurchaser,
  Api_DispositionSalePurchaserAgent,
  Api_DispositionSalePurchaserSolicitor,
} from '@/models/api/DispositionFile';
import { formatApiPersonNames } from '@/utils/personUtils';

export interface IDispositionSaleContactDetails {
  contactInformation:
    | Api_DispositionSalePurchaser
    | Api_DispositionSalePurchaserAgent
    | Api_DispositionSalePurchaserSolicitor;
  primaryContactLabel?: string | null;
}

const DispositionSaleContactDetails: React.FunctionComponent<IDispositionSaleContactDetails> = ({
  contactInformation,
  primaryContactLabel,
}) => {
  const labelValue = primaryContactLabel ? primaryContactLabel : 'Primary Contact';
  const primaryContact = contactInformation.primaryContact ?? null;

  return (
    <>
      <StyledLink
        target="_blank"
        rel="noopener noreferrer"
        to={
          contactInformation.personId
            ? `/contact/P${contactInformation.personId}`
            : `/contact/O${contactInformation.organizationId}`
        }
      >
        <span>
          {contactInformation.personId
            ? formatApiPersonNames(contactInformation.person)
            : contactInformation.organization?.name ?? ''}
        </span>
        <FaExternalLinkAlt className="ml-2" size="1rem" />
      </StyledLink>
      {primaryContact && (
        <>
          <StyledLabel>{labelValue}:</StyledLabel>
          <StyledSecondaryLink
            target="_blank"
            rel="noopener noreferrer"
            to={`/contact/P${contactInformation.primaryContactId}`}
          >
            <span>{formatApiPersonNames(primaryContact)}</span>
            <FaExternalLinkAlt className="ml-2" size="1rem" />
          </StyledSecondaryLink>
        </>
      )}
    </>
  );
};

export default DispositionSaleContactDetails;

const StyledLabel = styled.label`
  font-size: 16px;
  padding: 0 0.5rem;
  font-style: italic;
`;

export const StyledSecondaryLink = styled(Link)`
  padding: 0 0.5rem;
`;
