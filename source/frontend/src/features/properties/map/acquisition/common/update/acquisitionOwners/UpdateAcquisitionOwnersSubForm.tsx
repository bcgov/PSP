import { LinkButton, RemoveButton } from 'components/common/buttons';
import { Input } from 'components/common/form';
import Address from 'features/contacts/contact/create/components/address/Address';
import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import { FieldArray, useFormikContext } from 'formik';
import { useState } from 'react';
import { Container, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { AcquisitionFormModal } from '../../../modals/AcquisitionFormModal';
import {
  AcquisitionOwnerFormModel,
  OwnerAddressFormModel,
  WithAcquisitionOwners,
} from '../../models';

interface IUpdateAcquisitionOwnersSubFormProps {}

const UpdateAcquisitionOwnersSubForm: React.FunctionComponent<
  React.PropsWithChildren<IUpdateAcquisitionOwnersSubFormProps>
> = () => {
  const { values } = useFormikContext<WithAcquisitionOwners>();
  const [removeIndex, setRemoveIndex] = useState<number>(-1);
  const [showRemoveModal, setShowRemoveModal] = useState<boolean>(false);

  return (
    <>
      <FieldArray
        name="owners"
        render={arrayHelpers => (
          <>
            {values.owners.map((owner, index) => (
              <Row key={`owner-parent-${index}`} className="py-3">
                <Container>
                  <ButtonDiv>
                    <RemoveButton
                      label={'Remove Owner'}
                      onRemove={() => {
                        setRemoveIndex(index);
                        setShowRemoveModal(true);
                      }}
                    >
                      Remove Owner
                    </RemoveButton>
                  </ButtonDiv>
                  <H3>Name</H3>
                  <SectionField label="Given names">
                    <Input
                      field={`owners[${index}].givenName`}
                      placeholder="First name Middle name (individuals only)"
                    />
                  </SectionField>
                  <SectionField label="Last name/Corporation name">
                    <Input
                      field={`owners[${index}].lastNameOrCorp1`}
                      placeholder="Individual's Last name / Corporation's name"
                    />
                  </SectionField>
                  <SectionField
                    label="Other name"
                    tooltip="Additional name for Individual (ex: alias or maiden name or space for long last name) Corporation (ex: Doing Business as) or placeholder for the Last name/Corporate name 2 field from Title"
                  >
                    <Input
                      field={`owners[${index}].lastNameOrCorp2`}
                      placeholder="Alias/Doing business as etc."
                    />
                  </SectionField>
                  <SectionField label="Incorporation number">
                    <Input
                      field={`owners[${index}].incorporationNumber`}
                      placeholder="Incorporation number"
                    />
                  </SectionField>
                  <StyledDiv>
                    <H3>Mailing Address</H3>
                    <Address
                      namespace={`owners[${index}].address`}
                      addressLines={OwnerAddressFormModel.addressLines(owner.address)}
                    />
                  </StyledDiv>
                </Container>
              </Row>
            ))}
            <LinkButton
              data-testid="add-file-owner"
              onClick={() => {
                const owner = new AcquisitionOwnerFormModel();
                arrayHelpers.push(owner);
              }}
            >
              + Add owner
            </LinkButton>

            <AcquisitionFormModal
              message="Are you sure you want to remove this Owner?"
              title="Remove Owner"
              display={showRemoveModal}
              handleOk={() => {
                setShowRemoveModal(false);
                arrayHelpers.remove(removeIndex);
                setRemoveIndex(-1);
              }}
              handleCancel={() => {
                setShowRemoveModal(false);
                setRemoveIndex(-1);
              }}
            ></AcquisitionFormModal>
          </>
        )}
      ></FieldArray>
    </>
  );
};

export default UpdateAcquisitionOwnersSubForm;

export const StyledDiv = styled.div`
  background-color: none;
`;

export const ButtonDiv = styled.div`
  display: flex;
  flex-direction: row;
  justify-content: end;
`;

export const H3 = styled.h3`
  font-size: 2rem;
  font-weight: 700;
  text-decoration: none solid rgb(33, 37, 41);
  line-height: 2rem;
  margin-bottom: 1rem;
`;
