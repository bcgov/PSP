import { FieldArray, useFormikContext } from 'formik';
import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LinkButton, RemoveIconButton } from '@/components/common/buttons';
import { ContactInputContainer } from '@/components/common/form/ContactInput/ContactInputContainer';
import ContactInputView from '@/components/common/form/ContactInput/ContactInputView';
import { PrimaryContactSelector } from '@/components/common/form/PrimaryContactSelector/PrimaryContactSelector';
import { SectionField } from '@/components/common/Section/SectionField';
import {
  DispositionSalePurchaserContactModel,
  WithSalePurchasers,
} from '@/features/mapSideBar/disposition/models/DispositionSalePurchaserContactModel';
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
            (purchaser: DispositionSalePurchaserContactModel, index: number) => (
              <React.Fragment key={`purchaser-${purchaser?.id}`}>
                <Row className="py-2" data-testid={`purchaserRow[${index}]`} noGutters>
                  <Col xs="auto" xl="10">
                    <ContactInputContainer
                      field={`dispositionPurchasers.${index}.contact`}
                      View={ContactInputView}
                      displayErrorAsTooltip={false}
                    ></ContactInputContainer>
                  </Col>
                  <Col xs="auto" xl="2" className="pl-3 mt-2">
                    <RemoveIconButton
                      title="Remove Purchaser"
                      data-testId={`dispositionPurchasers.${index}.remove-button`}
                      onRemove={() => {
                        setModalContent({
                          ...getDeleteModalProps(),
                          title: 'Remove Purchaser',
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
                  <Row noGutters>
                    <Col xs="auto" xl="12">
                      <SectionField
                        label="Primary contact"
                        labelWidth={{ xs: 5 }}
                        contentWidth={{ xs: 6 }}
                      >
                        <PrimaryContactSelector
                          field={`dispositionPurchasers.${index}.primaryContactId`}
                          contactInfo={purchaser?.contact}
                        ></PrimaryContactSelector>
                      </SectionField>
                    </Col>
                  </Row>
                )}
              </React.Fragment>
            ),
          )}

          <LinkButton
            data-testid="add-purchaser-button"
            className="mb-3"
            onClick={() => {
              const purchaserContact = new DispositionSalePurchaserContactModel(
                null,
                dispositionSaleId,
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
