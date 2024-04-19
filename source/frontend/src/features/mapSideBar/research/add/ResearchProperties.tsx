import { FieldArray, useFormikContext } from 'formik';
import { Col, Row } from 'react-bootstrap';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { useModalContext } from '@/hooks/useModalContext';

import { AddressForm, PropertyForm } from '../../shared/models';
import { ResearchForm } from './models';

export interface IResearchPropertiesProps {
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const ResearchProperties: React.FC<IResearchPropertiesProps> = ({ confirmBeforeAdd }) => {
  const { values } = useFormikContext<ResearchForm>();
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();
  const { setModalContent, setDisplayModal } = useModalContext();

  return (
    <Section header="Properties to include in this file:">
      <div className="py-2">
        Select one or more properties that you want to include in this research file. You can choose
        a location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ push, remove }) => (
          <>
            <LoadingBackdrop show={bcaLoading} />
            <Row className="py-3 no-gutters">
              <Col>
                <MapSelectorContainer
                  addSelectedProperties={(newProperties: IMapProperty[]) => {
                    newProperties.reduce(async (promise, property) => {
                      return promise.then(async () => {
                        const formProperty = PropertyForm.fromMapProperty(property);
                        if (property.pid) {
                          const bcaSummary = await getPrimaryAddressByPid(property.pid, 30000);
                          formProperty.address = bcaSummary?.address
                            ? AddressForm.fromBcaAddress(bcaSummary?.address)
                            : undefined;
                        }

                        if (await confirmBeforeAdd(formProperty)) {
                          // Require user confirmation before adding property to file
                          setModalContent({
                            variant: 'warning',
                            title: 'User Override Required',
                            message: (
                              <>
                                <p>
                                  This property has already been added to one or more research
                                  files.
                                </p>
                                <p>Do you want to acknowledge and proceed?</p>
                              </>
                            ),
                            okButtonText: 'Yes',
                            cancelButtonText: 'No',
                            handleOk: () => {
                              push(formProperty);
                              setDisplayModal(false);
                            },
                            handleCancel: () => setDisplayModal(false),
                          });
                          setDisplayModal(true);
                        } else {
                          // No confirmation needed - just add the property to the file
                          push(formProperty);
                        }
                      });
                    }, Promise.resolve());
                  }}
                  modifiedProperties={values.properties}
                />
              </Col>
            </Row>
            <Section header="Selected properties">
              <SelectedPropertyHeaderRow />
              {values.properties.map((property, index) => (
                <SelectedPropertyRow
                  key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                  onRemove={() => remove(index)}
                  nameSpace={`properties.${index}`}
                  index={index}
                  property={property.toMapProperty()}
                />
              ))}
              {values.properties.length === 0 && <span>No Properties selected</span>}
            </Section>
          </>
        )}
      </FieldArray>
    </Section>
  );
};

export default ResearchProperties;
