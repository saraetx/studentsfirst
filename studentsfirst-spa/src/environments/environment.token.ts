import { InjectionToken } from '@angular/core';
import { EnvironmentInterface } from './environment.interface';

export const ENVIRONMENT = new InjectionToken<EnvironmentInterface>('environment');
