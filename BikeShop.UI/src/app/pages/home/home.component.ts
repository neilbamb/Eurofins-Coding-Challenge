import { Component, OnInit } from '@angular/core';
import { Bike, BikeService } from '../../bike.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  bikes: Bike[] = [];
  favourites: string[] = [];

  constructor(private bikeService: BikeService) { }

  ngOnInit(): void {
    const storedFavourites = this.readFavouriteRefs();
    this.bikeService.list().subscribe({
      next: (response) => {
        this.bikes = response;
        this.applyFavouritesToBikes(storedFavourites);
      },
      error: (error) => console.log('Error: ', error)
    });
  }

  private readFavouriteRefs(): string[] {
    const stored = localStorage.getItem('favouriteBikes');
    if (!stored) {
      return [];
    }

    try {
      const parsed = JSON.parse(stored);
      if (!Array.isArray(parsed)) {
        return [];
      }

      // Keep only string refs and dedupe
      return Array.from(new Set(parsed.filter((x: unknown) => typeof x === 'string')));
    } catch {
      return [];
    }
  }

  private applyFavouritesToBikes(favouriteRefs: string[]) {
    const favouriteSet = new Set(favouriteRefs);
    this.bikes.forEach((bike) => {
      if (favouriteSet.has(bike.ref)) {
        bike.inFavourites = true;
      }
    });
  }

  markBikeAsFavourite(bikeList: any[], favouriteBikeRef: string) {
    const bikeFound = bikeList.find((bike: any) => bike.ref === favouriteBikeRef);
    if (bikeFound) {
      bikeFound.inFavourites = true;
    }
  }

  addBikeToFavourites(ref: string) {
    const favouriteRefs = this.readFavouriteRefs();
    const favouriteSet = new Set(favouriteRefs);

    // Prevent duplicates (the UI can be re-clicked before state persists)
    if (favouriteSet.has(ref)) {
      return;
    }

    this.markBikeAsFavourite(this.bikes, ref);

    favouriteSet.add(ref);
    localStorage.setItem('favouriteBikes', JSON.stringify(Array.from(favouriteSet)));
  }
}
