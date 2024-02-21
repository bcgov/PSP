import { FieldArray, FieldArrayRenderProps, FormikProps, useFormikContext } from 'formik';
import React, { useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import styled from 'styled-components';

import { LinkButton, StyledRemoveLinkButton } from '@/components/common/buttons';
import GenericModal from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { useProductProvider } from '@/hooks/repositories/useProductProvider';
import { exists, isValidId } from '@/utils/utils';

import { ProductForm, ProjectForm } from '../models';
import ProductSubForm from './ProductSubForm';

export interface IProductsArrayFormProps {
  field: string;
  formikProps: FormikProps<ProjectForm>;
}

export const ProductsArrayForm: React.FunctionComponent<IProductsArrayFormProps> = ({
  field,
  formikProps,
}) => {
  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);
  const { values } = useFormikContext<ProjectForm>();

  const [showModal, setShowModal] = useState(false);
  const [showFileModal, setShowFileModal] = useState(false);
  const [rowToDelete, setRowToDelete] = useState<number | undefined>(undefined);

  const { retrieveProductFiles, retrieveProductFilesLoading } = useProductProvider();

  const products = values.products || [];

  const handleRemove = async (index: number) => {
    const productId = products[index].id;
    if (isValidId(productId)) {
      const files = await retrieveProductFiles(productId);
      if (exists(files) && files.length > 0) {
        setShowFileModal(true);
      } else {
        showDeleteWarning(index);
      }
    } else {
      showDeleteWarning(index);
    }
  };

  const showDeleteWarning = (index: number) => {
    setShowModal(true);
    setRowToDelete(index);
  };

  const confirmDelete = () => {
    if (rowToDelete !== undefined) {
      arrayHelpersRef.current?.remove(rowToDelete);
    }
    setShowModal(false);
  };

  return (
    <>
      <Section header="Associated products">
        <LoadingBackdrop show={retrieveProductFilesLoading}></LoadingBackdrop>
        <FieldArray
          name={field}
          render={arrayHelpers => {
            arrayHelpersRef.current = arrayHelpers;
            return (
              <>
                {products.map((product, index) => (
                  <div key={`product-create-${index}`}>
                    <Row className="align-items-end pb-4">
                      <Col />
                      <Col xs="auto">
                        <StyledRemoveLinkButton
                          title="Delete Project"
                          variant="light"
                          onClick={() => handleRemove(index)}
                        >
                          <FaTrash size="2rem" />
                        </StyledRemoveLinkButton>
                      </Col>
                    </Row>
                    <ProductSubForm
                      index={index}
                      nameSpace={`${field}.${index}`}
                      formikProps={formikProps}
                    />
                    {index !== products.length - 1 && <StyledSpacer className="my-5" />}
                  </div>
                ))}
                <LinkButton onClick={() => arrayHelpers.push(new ProductForm())}>
                  + Add another product
                </LinkButton>
              </>
            );
          }}
        ></FieldArray>
      </Section>

      <GenericModal
        variant="error"
        display={showFileModal}
        title="Remove Product with Files"
        message={
          'Important: you cannot delete this product unless you remove it from associated files first.'
        }
        okButtonText="Cancel"
        handleOk={() => {
          setShowFileModal(false);
        }}
      />
      <GenericModal
        variant="warning"
        display={showModal}
        title="Remove Product"
        message={
          'Deleting this product will remove it from all "Product" dropdowns. Are you certain you wish to proceed?'
        }
        okButtonText="Remove"
        cancelButtonText="Cancel"
        handleOk={() => confirmDelete()}
        handleCancel={() => setShowModal(false)}
      />
    </>
  );
};

export default ProductsArrayForm;

const StyledSpacer = styled.div`
  border-bottom: 0.1rem solid grey;
`;
