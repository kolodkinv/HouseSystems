import {HttpClient} from "@angular/common/http";
import {Injectable} from "@angular/core";

@Injectable()
export class HouseService {

  urlHouses: string = "api/Houses/";

  constructor(private httpClient: HttpClient) { }

  private static responseError(error: any): Promise<any> {
    return Promise.reject(error.json());
  }

  getHouses(){
    return this.httpClient.get(this.urlHouses)
      .toPromise()
      .then(response => response)
      .catch(HouseService.responseError);
  }
}
