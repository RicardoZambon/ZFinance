import { NgClass } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonEditComponent, ButtonNewComponent, ButtonSaveComponent, FormView } from '@framework';
import { CatalogSelectComponent, FormGroupComponent, FormInputComponent, FormInputGroupComponent, FormService, GroupAccordionComponent, GroupScrollSpyComponent, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { environment } from '../../../../../environments/environment';
import { IMenusDisplay } from '../../../../models';

@Component({
  selector: 'app-menus-form',
  templateUrl: './menus-form.html',
  styleUrls: ['./menus-form.scss'],
  imports: [
    ButtonEditComponent,
    ButtonNewComponent,
    ButtonSaveComponent,
    CatalogSelectComponent,
    FormGroupComponent,
    FormInputComponent,
    FormInputGroupComponent,
    FormsModule,
    GroupAccordionComponent,
    GroupScrollSpyComponent,
    NgClass,
    ReactiveFormsModule,
    RibbonGroupComponent,
    TranslatePipe,
  ],
  providers: [{ provide: FormService }]
})
export class MenusFormComponent extends FormView<IMenusDisplay> {
  //#region ViewChilds, Inputs, Outputs
  //#endregion

  //#region Variables
  protected readonly menusCatalogEndpoint: string = `${environment.apiUrl}/Menus/Catalog`;
  //#endregion

  //#region Properties
  protected get iconControlValue(): FormControl {
    return (<FormControl>this.dataForm.get('icon'))?.value ?? '';
  }
  //#endregion

  //#region Constructor and Angular life cycle methods
  //#endregion

  //#region Event handlers
  //#endregion

  //#region Public methods
  //#endregion

  //#region Private methods
  protected formSetup(): FormGroup<any> {
    return this.formBuilder.group({
      icon: [null],
      label: [null, Validators.required],
      order: [0, { validators: [Validators.min(0)], nonNullable: true }],
      parentMenuID: [null],
      url: [null],
    });
  }
  //#endregion
}