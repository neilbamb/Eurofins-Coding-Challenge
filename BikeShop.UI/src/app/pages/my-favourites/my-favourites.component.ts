import { BikeService } from '../../bike.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-my-favourites',
  templateUrl: './my-favourites.component.html',
  styleUrls: ['./my-favourites.component.scss']
})
export class MyFavouritesComponent implements OnInit {
  bikes: any[] = [];
  favouriteBikes: any[] = [];

  constructor(private bikeService: BikeService) { }

  ngOnInit(): void {
    const storedFavourites = this.readFavouriteRefs();
    // Clean up any legacy duplicates stored by the old implementation
    localStorage.setItem('favouriteBikes', JSON.stringify(storedFavourites));

    this.bikeService.list().subscribe({
      next: (response) => {
        this.bikes = response;
        this.updateFavourites(JSON.stringify(storedFavourites));
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
      return Array.from(new Set(parsed.filter((x: unknown) => typeof x === 'string')));
    } catch {
      return [];
    }
  }

  removeFromFavourites(ref: string) {
    const favourites = JSON.parse(localStorage.getItem('favouriteBikes') || '[]');
    const newFavourites = favourites.filter((bikeRef: string) => bikeRef !== ref);

    localStorage.setItem('favouriteBikes', JSON.stringify(newFavourites));
    this.updateFavourites(JSON.stringify(newFavourites));
  }

  updateFavourites(favourites: string) {
    this.favouriteBikes = [];
    let parsed: unknown;
    try {
      parsed = JSON.parse(favourites);
    } catch {
      parsed = [];
    }

    if (Array.isArray(parsed) && parsed.length) {
      // Dedupe refs to avoid showing the same bike multiple times
      const stringRefs = parsed.filter((x: unknown): x is string => typeof x === 'string');
      const uniqueRefs = Array.from(new Set(stringRefs));
      uniqueRefs.forEach((ref) => {
        const bikeInfo = this.bikes.find((bike: any) => bike.ref === ref);
        if (bikeInfo) {
          this.favouriteBikes.push(bikeInfo);
        }
      });
    }
  }
}
