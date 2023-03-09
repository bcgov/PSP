import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Section } from 'features/mapSideBar/tabs/Section';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import styled from 'styled-components';

import { IAcquisitionOwnersSummaryViewProps } from './AcquisitionOwnersSummaryContainer';

const AcquisitionOwnersSummaryView: React.FC<IAcquisitionOwnersSummaryViewProps> = ({
  isLoading,
  ownersList,
}) => {
  if (isLoading) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <Section header="Owners">
      <StyledParagraph>
        Each property in this file should be owned by the owner(s) in this section
      </StyledParagraph>
      {ownersList?.map((owner, index) => {
        return (
          <span key={`owner-${index}-${owner.ownerName}`}>
            <SectionField label="Name">{owner.ownerName}</SectionField>
            <SectionField
              label="Other name"
              tooltip="Additional name for Individual (ex: alias or maiden name or space for long last name) Corporation (ex: Doing Business as) or placeholder for the Last name/Corporate name 2 field from Title"
            >
              {owner.ownerOtherName}
            </SectionField>
            <SectionField label="Mailing address">
              <StyledAddressWrapper>{owner.ownerDisplayAddress}</StyledAddressWrapper>
            </SectionField>
            {index < ownersList.length - 1 && <hr></hr>}
          </span>
        );
      })}
    </Section>
  );
};

export default AcquisitionOwnersSummaryView;

const StyledAddressWrapper = styled.div`
  white-space: pre-line;
`;

const StyledParagraph = styled.p`
  color: #494949;
  font-size: 1.6rem;
  text-decoration: none;
`;
