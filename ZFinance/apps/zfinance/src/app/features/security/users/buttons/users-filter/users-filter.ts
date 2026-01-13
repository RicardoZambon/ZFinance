import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonFiltersComponent } from '@framework';
import { FormGroupComponent, FormInputGroupComponent } from '@library';
import { FiltersBase } from '@shared';

@Component({
  selector: 'app-users-filter',
  templateUrl: './users-filter.html',
  imports: [
    ButtonFiltersComponent,
    FormGroupComponent,
    FormInputGroupComponent,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class UsersFilterComponent extends FiltersBase {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  //#endregion

  //#region Properties
  //#endregion
  
  //#region Constructor and Angular life cycle methods
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  protected override formSetup(): FormGroup<any> {
    return this.formBuilder.group({
      email: [null],
      name: [null],
    });
  }
  //#endregion
}