// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

import { EnvironmentInterface } from './environment.interface';

export const environment: EnvironmentInterface = {
  production: false,
  apiBase: 'https://localhost:5001',
  auth: {
    clientId: '5cf30068-237e-439e-98c2-e06cf3c681f8',
    authority: 'https://login.microsoftonline.com/f5afb4ff-5542-4b7a-8881-ec35b51f2d75/',
    redirectUri: 'http://localhost:4200/'
  }
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
