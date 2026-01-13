import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonEditComponent, ButtonNewComponent, ButtonSaveComponent, FormView } from '@framework';
import { FormGroupComponent, FormInputGroupComponent, FormService, GroupAccordionComponent, GroupScrollSpyComponent, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { IRolesUpdate } from '../../../../models';
import { RolesActionsChildListComponent } from '../child-lists/roles-actions-child-list/roles-actions-child-list';
import { RolesMenusChildListComponent } from '../child-lists/roles-menus-child-list/roles-menus-child-list';
import { RolesUsersChildListComponent } from '../child-lists/roles-users-child-list/roles-users-child-list';

@Component({
  selector: 'app-roles-form',
  templateUrl: './roles-form.html',
  styleUrls: ['./roles-form.scss'],
  imports: [
    ButtonEditComponent,
    ButtonNewComponent,
    ButtonSaveComponent,
    FormGroupComponent,
    FormInputGroupComponent,
    FormsModule,
    GroupAccordionComponent,
    GroupScrollSpyComponent,
    NgIf,
    ReactiveFormsModule,
    RibbonGroupComponent,
    RolesActionsChildListComponent,
    RolesMenusChildListComponent,
    RolesUsersChildListComponent,
    TranslatePipe,
  ],
  providers: [{ provide: FormService }],
})
export class RolesFormComponent extends FormView<IRolesUpdate> {
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
  protected formSetup(): FormGroup<any> {
    return this.formBuilder.group({
      name: [null, Validators.required],
    });
  }
  //#endregion
}