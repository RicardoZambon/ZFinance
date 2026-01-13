import { NgIf } from '@angular/common';
import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonEditComponent, ButtonNewComponent, ButtonSaveComponent, FormView } from '@framework';
import { FormGroupComponent, FormInputGroupComponent, FormService, GroupAccordionComponent, GroupScrollSpyComponent, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { IUsersDisplay } from '../../../../models';
import { UsersRolesChildListComponent } from '../child-lists';

@Component({
  selector: 'app-users-form',
  templateUrl: './users-form.html',
  styleUrls: ['./users-form.scss'],
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
    TranslatePipe,
    UsersRolesChildListComponent,
  ],
  providers: [{ provide: FormService }]
})
export class UsersFormComponent extends FormView<IUsersDisplay> {
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
      email: [null, Validators.required],
      name: [null, Validators.required],
    });
  }
  //#endregion
}