import truncate from 'lodash/truncate';
import { FaCheck, FaTimesCircle } from 'react-icons/fa';
import styled, { useTheme } from 'styled-components';

import { SectionField } from '@/components/common/Section/SectionField';

import { UploadResponseModel } from './models';

export interface IShapeUploadResultViewProps {
  uploadResult: UploadResponseModel;
}

export const ShapeUploadResultView: React.FunctionComponent<IShapeUploadResultViewProps> = ({
  uploadResult,
}) => {
  const theme = useTheme();

  return (
    <>
      {uploadResult.isSuccess ? (
        <SectionField label="Shapefile uploaded successfully" labelWidth={{ xs: 12 }}>
          {truncate(uploadResult.fileName ?? '', { length: 100 })}
          <FaCheck
            data-testid="file-check-icon"
            className="ml-2"
            size="1.6rem"
            color={theme.css.iconsColorSuccess}
          />
        </SectionField>
      ) : (
        <SectionField label="Shapefile upload failed" labelWidth={{ xs: 12 }}>
          {truncate(uploadResult.fileName ?? '', { length: 100 })}
          <FaTimesCircle
            data-testid="file-check-icon"
            className="ml-2"
            size="1.6rem"
            color={theme.css.iconsColorDanger}
          />
          <StyledFailDiv>{uploadResult.errorMessage ?? ''}</StyledFailDiv>
        </SectionField>
      )}
    </>
  );
};

export default ShapeUploadResultView;

const StyledFailDiv = styled.div`
  color: ${props => props.theme.bcTokens.iconsColorDanger};
`;
