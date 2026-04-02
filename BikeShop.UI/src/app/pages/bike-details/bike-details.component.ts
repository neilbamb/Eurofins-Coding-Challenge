import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { Bike, BikeService } from '../../bike.service';

@Component({
  selector: 'app-bike-details',
  templateUrl: './bike-details.component.html',
  styleUrls: ['./bike-details.component.scss']
})
export class BikeDetailsComponent implements OnInit {
  bike: Bike | null = null;
  error: string | null = null;
  isFavourite = false;

  constructor(
    private route: ActivatedRoute,
    private bikeService: BikeService,
    private router: Router
  ) { }

  ngOnInit(): void {
    const bikeRef = this.route.snapshot.paramMap.get('ref');
    if (!bikeRef) {
      this.error = 'Missing bike reference.';
      return;
    }

    this.bikeService.getByRef(bikeRef).subscribe({
      next: (b) => {
        this.bike = b;
        this.refreshFavouriteState();
      },
      error: () => {
        this.error = 'Bike not found.';
      }
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

  private writeFavouriteRefs(refs: string[]): void {
    localStorage.setItem('favouriteBikes', JSON.stringify(refs));
  }

  private refreshFavouriteState(): void {
    if (!this.bike) {
      this.isFavourite = false;
      return;
    }

    const favRefs = this.readFavouriteRefs();
    this.isFavourite = favRefs.includes(this.bike.ref);
    this.bike.inFavourites = this.isFavourite;
  }

  addToFavourites(): void {
    if (!this.bike) {
      return;
    }

    const favRefs = new Set(this.readFavouriteRefs());
    if (favRefs.has(this.bike.ref)) {
      return;
    }

    favRefs.add(this.bike.ref);
    this.writeFavouriteRefs(Array.from(favRefs));
    this.refreshFavouriteState();
  }

  removeFromFavourites(): void {
    if (!this.bike) {
      return;
    }

    const favRefs = new Set(this.readFavouriteRefs());
    favRefs.delete(this.bike.ref);
    this.writeFavouriteRefs(Array.from(favRefs));
    this.refreshFavouriteState();
  }
}

