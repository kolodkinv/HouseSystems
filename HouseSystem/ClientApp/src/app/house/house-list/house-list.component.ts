import { Component, OnInit } from '@angular/core';
import {HouseService} from "../house.service";
import {House} from "../house.models";
import {WaterMeter} from "../../water-meter/water-meter.models";

@Component({
  selector: 'app-house-list',
  templateUrl: './house-list.component.html',
  providers: [HouseService]
})

export class HouseListComponent implements OnInit {

  houses: Array<House>;
  editHouse: House = new House();
  houseWithMaxWaterValue: House = new House();
  error: string;

  constructor(private houseService: HouseService) { }

  ngOnInit() {
    this.getHouses();
    this.getHouseWithMaxWater();
  }

  startEditHouse(house: House){
    this.editHouse = house;
  }

  cancelEdit(){
    this.editHouse = new House();
  }

  addWaterMeter(waterMeter: WaterMeter){
    let house = this.houses.find(h => h.id == waterMeter.buildingId);
    house.waterMeter = waterMeter;
    this.editHouse = new House();
    this.getHouseWithMaxWater();
  }

  addHouse(house: House){
    this.houses.push(house);
  }

  getHouses(){
    this.houseService.getHouses()
      .then(houses => {
        this.houses = houses;
      })
      .catch(error => {
        debugger;
        this.error = error;
      });
  }

  getHouseWithMaxWater(){
    this.houseService.getHouseWithMaxWater()
      .then(house => {
        this.houseWithMaxWaterValue = house;
      })
      .catch(error => {
        this.error = error;
      })
  }

}
