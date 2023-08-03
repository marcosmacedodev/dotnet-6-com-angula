import { Directive } from '@angular/core';
import { AbstractControl, NG_VALIDATORS, ValidationErrors, Validator, ValidatorFn } from '@angular/forms';

@Directive({
  selector: '[appConfirmPassword]',
  providers: [{provide: NG_VALIDATORS, useExisting: ConfirmPasswordDirective, multi: true}]
})
export class ConfirmPasswordDirective implements Validator{

  constructor() { }

  validate(control: AbstractControl<any, any>): ValidationErrors | null {
   const cp = control.get('confirm_password');
   const p = control.get('password');
   return cp && p && cp.value != p.value ? {confirm_password: {value: cp.value}}: null;
  }

}

export function ConfirmPasswordValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const cp = control.get('confirm_password');
    const p = control.get('password');
    return cp && p && cp.value != p.value ? {confirm_password: {value: cp.value}}: null;
  };
}
