import { BrowserAuthOptions } from '@azure/msal-browser';

export interface EnvironmentInterface {
  production: boolean,
  apiBase: string,
  apiAppUri: string,
  auth: BrowserAuthOptions
}
