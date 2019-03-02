import { Component, OnInit } from '@angular/core';
import {HouseService} from "../house.service";
import {House} from "../house.models";

@Component({
  selector: 'app-house-list',
  templateUrl: './house-list.component.html',
  providers: [HouseService]
})

export class HouseListComponent implements OnInit {

  houses: Array<House>;
  editHouse: House = new House();
  error: string;

  constructor(private houseService: HouseService) { }

  ngOnInit() {
    this.getHouses();
  }

  startEditHouse(house: House){
    this.editHouse = house;
  }

  onCancel(){
    this.editHouse = new House();
  }

  getHouses(){
    this.houseService.getHouses()
      .then(houses => {
        this.houses = houses;
      })
      .catch(error => {
        this.error = error;
      });
  }

}
