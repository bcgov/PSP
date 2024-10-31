import ApiVersionInfo from '@/components/common/ApiVersionInfo';

import FooterStyled from './FooterStyled';

/**
 * Display an "empty" header bar with limited functionality as a placeholder
 */
const EmptyFooter = () => {
  return (
    <FooterStyled>
      <div className="main-footer">
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/disclaimer"
          className="nav-item"
        >
          Disclaimer
        </a>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/privacy"
          className="nav-item"
        >
          Privacy
        </a>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/accessible-government"
          className="nav-item"
        >
          Accessibility
        </a>
        <a
          target="_blank"
          rel="noopener noreferrer"
          href="http://www.gov.bc.ca/gov/content/home/copyright"
          className="nav-item"
        >
          Copyright
        </a>
        <ApiVersionInfo />
      </div>
    </FooterStyled>
  );
};

export default EmptyFooter;
