import { Component } from '@angular/core';
import { FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { ButtonEditComponent, ButtonNewComponent, ButtonSaveComponent, FormView } from '@framework';
import { FormGroupComponent, FormInputGroupComponent, FormService, GroupAccordionComponent, GroupScrollSpyComponent, RibbonGroupComponent } from '@library';
import { TranslatePipe } from '@ngx-translate/core';
import { IActionsUpdate } from '../../../../models';

@Component({
  selector: 'app-actions-form',
  templateUrl: './actions-form.html',
  styleUrls: ['./actions-form.scss'],
  imports: [
    ButtonEditComponent,
    ButtonNewComponent,
    ButtonSaveComponent,
    FormGroupComponent,
    FormInputGroupComponent,
    FormsModule,
    GroupAccordionComponent,
    GroupScrollSpyComponent,
    ReactiveFormsModule,
    RibbonGroupComponent,
    TranslatePipe,
  ],
  providers: [{ provide: FormService }]
})
export class ActionsFormComponent extends FormView<IActionsUpdate> {
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
      code: [null, Validators.required],
      description: [null, Validators.required],
      entity: [null, Validators.required],
      name: [null, Validators.required],
    });
  }
  //#endregion
}