import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export type Bike = {
  manufacturer: string;
  ref: string;
  model: string;
  category: string;
  price: string;
  colour: string;
  weight: string;
  img_url: string;
  inFavourites?: boolean;
};

export type CreateBikeRequest = {
  manufacturer: string;
  model: string;
  category: string;
  price: string;
  colour: string;
  weight: string;
  img_url: string;
};

@Injectable({
  providedIn: 'root'
})
export class BikeService {

  constructor(private http: HttpClient) { }

  // Challenge setup runs the API on a different port than Angular dev server.
  private readonly apiBaseUrl = 'http://localhost:5111/api/bikes';

  list(): Observable<Bike[]> {
    return this.http.get<Bike[]>(this.apiBaseUrl);
  }

  getByRef(ref: string): Observable<Bike> {
    return this.http.get<Bike>(`${this.apiBaseUrl}/${ref}`);
  }

  create(request: CreateBikeRequest): Observable<Bike> {
    return this.http.post<Bike>(this.apiBaseUrl, request);
  }
}
