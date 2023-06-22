import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'DataT'
})
export class DataTPipe implements PipeTransform {

  transform(value: any, args?: any): any {
    return null;
  }

}
