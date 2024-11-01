import { Component, EventEmitter, Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent {
  // title: string;
  // list: any[] = [];
  // closeBtnName: string;
  @Input() udpateSelectedRoles = new EventEmitter();
  user: User;
  roles: any[];

  constructor(public bsModalRef: BsModalRef){}

  updateRoles() {
    this.udpateSelectedRoles.emit(this.roles);
    this.bsModalRef.hide();
  }

}
