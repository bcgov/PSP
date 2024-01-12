import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveButton } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  DispositionSaleContactModel,
  WithSalePurchasers,
} from '@/features/mapSideBar/disposition/models/DispositionSaleContactModel';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';

export interface IDispositionSalePurchasersSubFormProps {}

const DispositionSalePurchaserSubForm: React.FunctionComponent<
  React.PropsWithChildren<IDispositionSalePurchasersSubFormProps>
> = () => {
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
                <Row className="py-3" data-testid={`purchaserRow[${index}]`}>
                  <Col xs="auto" xl="5" className="pl-0" data-testid="contact-input">
                    <ContactInputContainer
                      field={`dispositionPurchasers.${index}.contact`}
                      View={ContactInputView}
                      displayErrorAsTooltip={false}
                    ></ContactInputContainer>
                  </Col>
                  <Col xs="auto" xl="2" className="pl-0 mt-2">
                    <RemoveButton
                      dataTestId={`dispositionPurchasers.${index}.remove-button`}
                      onRemove={() => {
                        setModalContent({
                          ...getDeleteModalProps(),
                          title: 'RemovePurchaser',
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
                    />
                  </Col>
                </Row>
                {purchaser.contact?.organizationId && !purchaser.contact?.personId && (
                  <SectionField label="Primary contact" labelWidth="5" noGutters>
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
              const purchaserContact = new DispositionSaleContactModel();
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
