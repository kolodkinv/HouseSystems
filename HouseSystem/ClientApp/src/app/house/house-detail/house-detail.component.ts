import {Component, EventEmitter, OnInit, Output} from '@angular/core';
import {House} from "../house.models";
import {HouseService} from "../house.service";

@Component({
  selector: 'app-house-detail',
  templateUrl: './house-detail.component.html',
  providers: [HouseService]
})

export class HouseDetailComponent implements OnInit {

  @Output() onAdd = new EventEmitter<House>();  // Событие что дом зарегистрирован
  newHouse: House = new House();                // Регистрируемый дом
  error: string;                                // Сообщение об ошибке
  success: boolean = false;                     // Флаг успешности регистрации дома

  constructor(private houseService: HouseService) { }

  ngOnInit() {
  }

  // Добавление нового дома
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

  // Сброс ошибки добавления
  resetError(){
    this.error = '';
  }

  // Сброс сообщения об успешном добавлении
  resetSucces(){
    this.success = false;
  }
}
