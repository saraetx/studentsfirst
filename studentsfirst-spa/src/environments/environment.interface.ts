import { BrowserAuthOptions } from '@azure/msal-browser';

export interface EnvironmentInterface {
  production: boolean,
  apiBase: string,
  auth: BrowserAuthOptions
}
