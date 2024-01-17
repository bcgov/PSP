import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton, StyledRemoveLinkButton } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  DispositionSaleContactModel,
  WithSalePurchasers,
} from '@/features/mapSideBar/disposition/models/DispositionSaleContactModel';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';

export interface IDispositionSalePurchasersSubFormProps {
  dispositionSaleId: number | null;
}

const DispositionSalePurchaserSubForm: React.FunctionComponent<
  React.PropsWithChildren<IDispositionSalePurchasersSubFormProps>
> = ({ dispositionSaleId }) => {
  const { values } = useFormikContext<WithSalePurchasers>();
  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <FieldArray
      name="dispositionPurchasers"
      render={arrayHelpers => (
        <>
          {values.dispositionPurchasers.map(
            (purchaser: DispositionSaleContactModel, index: number) => (
              <React.Fragment key={`purchaser-${index}`}>
                <StyledRow className="py-3" data-testid={`purchaserRow[${index}]`}>
                  <ContactInputContainer
                    field={`dispositionPurchasers.${index}.contact`}
                    View={ContactInputView}
                    displayErrorAsTooltip={false}
                  ></ContactInputContainer>
                  <StyledRemoveLinkButton
                    title="Remove Purchaser"
                    data-testid={`dispositionPurchasers.${index}.remove-button`}
                    variant="light"
                    onClick={() => {
                      setModalContent({
                        ...getDeleteModalProps(),
                        title: 'Remo vePurchaser',
                        message: 'Do you wish to remove this purchaser?',
                        okButtonText: 'Yes',
                        cancelButtonText: 'No',
                        handleOk: async () => {
                          arrayHelpers.remove(index);
                          setDisplayModal(false);
                        },
                        handleCancel: () => {
                          setDisplayModal(false);
                        },
                      });
                      setDisplayModal(true);
                    }}
                  >
                    <FaTrash size="2rem" />
                  </StyledRemoveLinkButton>
                </StyledRow>
                {purchaser.contact?.organizationId && !purchaser.contact?.personId && (
                  <SectionField label="Primary contact" labelWidth="5">
                    <PrimaryContactSelector
                      field={`dispositionPurchasers.${index}.primaryContactId`}
                      contactInfo={purchaser?.contact}
                    ></PrimaryContactSelector>
                  </SectionField>
                )}
              </React.Fragment>
            ),
          )}

          <LinkButton
            data-testid="add-purchaser-button"
            onClick={() => {
              const purchaserContact = new DispositionSaleContactModel(
                null,
                dispositionSaleId,
                0,
                null,
              );
              arrayHelpers.push(purchaserContact);
            }}
          >
            + Add another purchaser
          </LinkButton>
        </>
      )}
    />
  );
};

export default DispositionSalePurchaserSubForm;

const StyledRow = styled(Row)`
  flex-wrap: nowrap;
`;

// const StyledRemoveButtonCol = styled(Col)`
//   padding-right: 0;
// `;
