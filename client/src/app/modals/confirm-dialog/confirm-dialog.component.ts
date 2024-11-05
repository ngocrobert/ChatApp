import { Component } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-confirm-dialog',
  templateUrl: './confirm-dialog.component.html',
  styleUrls: ['./confirm-dialog.component.css']
})
export class ConfirmDialogComponent {
  title : string;
  message: string;
  btnOkText: string;
  btnCancelText: string;
  result: boolean;

  constructor(public bsModelRef: BsModalRef){}

  confirm(){
    this.result = true;
    this.bsModelRef.hide();
  }

  decline(){
    this.result = false;
    this.bsModelRef.hide();
  }

}
