import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {House} from "../house.models";
import {HouseService} from "../house.service";

@Component({
  selector: 'app-house-detail',
  templateUrl: './house-detail.component.html',
  providers: [HouseService]
})

export class HouseDetailComponent implements OnInit {
  @Output() onAdd = new EventEmitter<House>();
  newHouse: House = new House();
  error: string;
  success: boolean = false;

  constructor(private houseService: HouseService) { }

  ngOnInit() {
  }

  addHouse(house: House){
    this.houseService.addHouse(house)
      .then(house => {
        this.onAdd.emit(house);
        this.newHouse = new House();
        this.success = true;
      })
      .catch(error => {
        this.error = error.error.error;
      });
  }

  resetError(){
    this.error = '';
  }

  resetSucces(){
    this.success = false;
  }
}
