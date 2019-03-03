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

  houses: Array<House>;                         // Список домов
  editHouse: House = new House();               // Редактируемый дом
  houseWithMaxWaterValue: House = new House();  // Дом с максимальным потреблением воды
  error: string;                                // Ошибка

  constructor(private houseService: HouseService) { }

  ngOnInit() {
    this.getHouses();
    this.getHouseWithMaxWater();
  }

  // Начало режима редактирования дома
  startEditHouse(house: House){
    this.editHouse = house;
  }

  // Выход из режима редактирования дома
  cancelEdit(){
    this.editHouse = new House();
  }

  // Регистрация водяного счетчика
  addWaterMeter(waterMeter: WaterMeter){
    let house = this.houses.find(h => h.id == waterMeter.buildingId);
    house.waterMeter = waterMeter;
    this.editHouse = new House();
    this.getHouseWithMaxWater();
  }

  // Регистрация дома
  addHouse(house: House){
    this.houses.push(house);
  }

  // Получение списка домов
  getHouses(){
    this.houseService.getHouses()
      .then(houses => {
        this.houses = houses;
      })
      .catch(error => {
        this.error = error;
      });
  }

  // Получение дома с максимальным потреблением воды
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
