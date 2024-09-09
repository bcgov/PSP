// import original module declarations
import 'styled-components';

type BcTokensModule = typeof import('@bcgov/design-tokens/js/variables.js');

// and extend them!
declare module 'styled-components' {
  export interface DefaultTheme {
    tenant: ITenantConfig2;
    css: CSSModuleClasses;
    bcTokens: BcTokensModule;
  }
}
