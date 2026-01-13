import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ButtonFiltersComponent } from '@framework';
import { CatalogSelectComponent, FormGroupComponent, FormInputGroupComponent } from '@library';
import { FiltersBase } from '@shared';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-menus-filter',
  templateUrl: './menus-filter.html',
  imports: [
    ButtonFiltersComponent,
    CatalogSelectComponent,
    FormGroupComponent,
    FormInputGroupComponent,
    FormsModule,
    ReactiveFormsModule,
  ]
})
export class MenusFilterComponent extends FiltersBase {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  protected readonly menusCatalogEndpoint: string = `${environment.apiUrl}/Menus/Catalog`;
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
      label: [null],
      parentMenuID: [null],
      url: [null],
    });
  }
  //#endregion
}